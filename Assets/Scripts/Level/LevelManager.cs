#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
#endif
using UnityEngine;

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

    [System.Serializable]
    private class LevelData
    {
        public Vector2Int[] players;
        public Vector2Int[] boxes;
        public Vector2Int[] targets;
        public Vector2Int[] walls;
    }

    [SerializeField] private LevelPrefabs levelPrefabs;
    public static readonly string[] LevelNames = { "Level0", "Level" };

    public void LoadLevel(TextAsset jsonTextAsset)
    {
        if (jsonTextAsset != null)
        {
            LevelData data = JsonUtility.FromJson<LevelData>(jsonTextAsset.ToString());
            GameObject levelNode = new("_gen" + jsonTextAsset.name);
            levelNode.tag = Constants.TagLevelRoot;
            InstantiateObject(levelPrefabs.playerPrefab, data.players, Constants.TagPlayer, levelNode);
            InstantiateObject(levelPrefabs.boxPrefab, data.boxes, Constants.TagBox, levelNode);
            InstantiateObject(levelPrefabs.targetPrefab, data.targets, Constants.TagTarget, levelNode);
            InstantiateObject(levelPrefabs.wallPrefab, data.walls, Constants.TagWall, levelNode);
        }
    }

    public void LoadLevel(string levelname)
    {
        LoadLevel((TextAsset)AssetDatabase.LoadAssetAtPath("Assets/Levels/" + levelname + ".json", typeof(TextAsset)));
    }

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

    private void InstantiateObject(GameObject prefab, Vector2Int[] positions, string nodeName, GameObject parent)
    {
        GameObject node = new(nodeName);
        node.transform.parent = parent.transform;
        for (int i = 0; i < positions.Length; i++)
        {
            GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab, node.transform);
            obj.transform.localPosition = Util.Vector2InttoVector3XZ(positions[i], prefab.transform.position.y);
        }
    }

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
        Debug.Log("Write data to" + path + "\n" +
            "Parsing:" + parseRoot +
            "; player count=" + players.Length +
            "; boxes count=" + boxes.Length +
            "; targets count=" + targets.Length +
            "; objects count=" + walls.Length);
    }

    /// <summary>
    /// In editor, load level from TextAsset indicated by levelDataFile
    /// </summary>
    public void LoadLevelInEditor()
    {
        LoadLevel(levelDataFile);
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
}
