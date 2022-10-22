using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// Manage load level from level data (both ingame and editor),
/// parse and store level from gameobject (editor).
/// </summary>
public class LevelManager : MonoBehaviour
{
    [Serializable]
    public class LevelPrefabs
    {
        public GameObject playerPrefab;
        public GameObject boxPrefab;
        public GameObject targetPrefab;
        public GameObject wallPrefab;
    }

    [Serializable]
    private class LevelData
    {
        public Vector2Int[] players;
        public Vector2Int[] boxes;
        public Vector2Int[] targets;
        public Vector2Int[] walls;
    }

    public enum Level
    {
        [StringValue("Level0")]
        Level0,
    }

    [SerializeField] private LevelPrefabs levelPrefabs;
    private readonly Dictionary<Level, AsyncOperationHandle<TextAsset>> levelHandles = new();

    /// <summary>
    /// Blocking call to load immediately the level.
    /// Should only be called by GameManager.
    /// </summary>
    /// <param name="level">LevelManager.Level</param>
    public void LoadLevelImmediate(Level level)
    {
        LoadLevelDataFileAsync(level);
        TextAsset ta = UtilAddressable.WaitForCompletion(levelHandles[level]);
        if (ta != default)
        {
            InstantiateLevel(ta, level);
        }
    }

    /// <summary>
    /// Should only be called by GameManager in game, or call by Editor
    /// </summary>
    public void CleanGeneratedLevels()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag(Constants.TagLevelRoot);
        for (int i = 0; i < gos.Length; i++)
        {
#if UNITY_EDITOR
            DestroyImmediate(gos[i]);
#else
            Destroy(gos[i]);
            gos[i].SetActive(false);
#endif
        }
    }

    private void LoadLevelDataFileAsync(Level level)
    {
        if (!levelHandles.ContainsKey(level))
        {
            string levelname = Util.GetEnumStringValue(level);
            levelHandles[level] = UtilAddressable.LoadAssetAsync<TextAsset>("Assets/Levels/" + levelname + ".json");
        }
    }

    /// <summary>
    /// Instantiate a Level with corresponding asset
    /// </summary>
    /// <param name="textAsset">JSON file containing LevelData</param>
    /// <param name="level">Level name will be used on generated node</param>
    private void InstantiateLevel(TextAsset textAsset, Level level = Level.Level0)
    {
        if (textAsset != null)
        {
            LevelData data = JsonUtility.FromJson<LevelData>(textAsset.ToString());
            GameObject levelNode = new("_gen" + Util.GetEnumStringValue(level));
            levelNode.tag = Constants.TagLevelRoot;
            InstantiateObject(levelPrefabs.playerPrefab, data.players, Constants.TagPlayer, levelNode);
            InstantiateObject(levelPrefabs.boxPrefab, data.boxes, Constants.TagBox, levelNode);
            InstantiateObject(levelPrefabs.targetPrefab, data.targets, Constants.TagTarget, levelNode);
            InstantiateObject(levelPrefabs.wallPrefab, data.walls, Constants.TagWall, levelNode);
        }
    }

    private void InstantiateObject(GameObject prefab, Vector2Int[] positions, string nodeName, GameObject parent)
    {
        GameObject node = new(nodeName);
        node.transform.parent = parent.transform;
        for (int i = 0; i < positions.Length; i++)
        {
#if UNITY_EDITOR
            GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab, node.transform);
#else
            GameObject obj = (GameObject)Instantiate(prefab, node.transform);
#endif
            obj.transform.localPosition = Util.Vector2InttoVector3XZ(positions[i], prefab.transform.position.y);
        }
    }

    #region REGION_UNITY_EDITOR
#if UNITY_EDITOR
    [SerializeField] private GameObject parseRoot;
    [SerializeField] private TextAsset levelDataFile;

    /// <summary>
    /// In editor, parse ParseRoot and find level elements such as Player and Boxes
    /// </summary>
    public void ParseLevelInEditor()
    {
        // Find in hierarchy
        PlayerController[] players = parseRoot.GetComponentsInChildren<PlayerController>();
        BoxController[] boxes = parseRoot.GetComponentsInChildren<BoxController>();
        TargetController[] targets = parseRoot.GetComponentsInChildren<TargetController>();
        WallController[] walls = parseRoot.GetComponentsInChildren<WallController>();
        // Analyze
        LevelData levelData = new()
        {
            players = ParseObjectsPositionOnLevel(players),
            boxes = ParseObjectsPositionOnLevel(boxes),
            targets = ParseObjectsPositionOnLevel(targets),
            walls = ParseObjectsPositionOnLevel(walls),
        };

        string path = GetLevelFilePathAndBackup(parseRoot.name);
        File.WriteAllText(path, JsonUtility.ToJson(levelData));
        Debug.Log($"Write data to {path} \n" +
            $"Parsing: {parseRoot}; player count={players.Length}; boxes count={boxes.Length}; " +
            $"targets count={targets.Length}; objects count={walls.Length}");
    }

    /// <summary>
    /// In editor, load level from TextAsset indicated by levelDataFile
    /// </summary>
    public void LoadLevelInEditor()
    {
        InstantiateLevel(levelDataFile);
    }

    /// <summary>
    /// Get level file storage path with ".json" extension,
    /// if the file exist, backup old file with -bak-datetime
    /// </summary>
    /// <param name="levelname"></param>
    /// <returns></returns>
    private string GetLevelFilePathAndBackup(string levelname)
    {
        string path = Application.dataPath + "/Levels/" + levelname + ".json";
        string pathbak = Application.dataPath + "/Levels/" + levelname + "-bak" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".json";
        if (File.Exists(path))
        {
            File.Move(path, pathbak);
        }
        return path;
    }

    private Vector2Int[] ParseObjectsPositionOnLevel(MonoBehaviour[] objects)
    {
        Vector2Int[] data = new Vector2Int[objects.Length];
        for (int i = 0; i < objects.Length; i++)
        {
            data[i] = Util.Vector3toVector2IntXZ(objects[i].transform.localPosition);
        }
        return data;
    }
#endif
    #endregion

}
