using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelManager))]
[CanEditMultipleObjects]
public class LevelManagerEditor : Editor
{
    private SerializedProperty levelPrefabs;
    private SerializedProperty levelDataFile;
    private SerializedProperty parseRoot;

    private void OnEnable()
    {
        levelPrefabs = serializedObject.FindProperty("levelPrefabs");
        parseRoot = serializedObject.FindProperty("parseRoot");
        levelDataFile = serializedObject.FindProperty("levelDataFile");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.PropertyField(levelPrefabs);

        EditorGUILayout.LabelField("Store to Assets/Levels/");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(parseRoot);
        if (GUILayout.Button("Parse & Store"))
        {
            ((LevelManager)target).ParseLevelInEditor();
            PrefabUtility.RevertPropertyOverride(parseRoot, InteractionMode.UserAction);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.LabelField("Load from Assets/Levels/");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(levelDataFile);
        if (GUILayout.Button("Load"))
        {
            ((LevelManager)target).LoadLevelInEditor();
            PrefabUtility.RevertPropertyOverride(levelDataFile, InteractionMode.UserAction);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.LabelField("Clean all level generated (mark with tag:" + Constants.TagLevelRoot + ")");
        if (GUILayout.Button("Clean Levels"))
        {
            ((LevelManager)target).CleanGeneratedLevels();
        }
        serializedObject.ApplyModifiedProperties();
    }
}
