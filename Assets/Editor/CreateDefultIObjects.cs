using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ObjectsToSpawn : ScriptableObject
{
    public List<GameObject> toSpawn;
}

public class SpawnObjects : EditorWindow
{
    ObjectsToSpawn scriptableObject;
    SerializedObject serializedData;
    SerializedProperty objectsProperty;

    bool showList,
         showingList;

    string path = "Assets/Editor/ObjectsNeeded.asset";

    bool listExsists = false;

    [MenuItem("Tools/BoltsTools/Spawn Needed Objects")]
    public static void OpenWindow()
    {
        GetWindow(typeof(SpawnObjects), true, "Spawn Objects Needed");
    }

    void OnGUI()
    {
        if(!listExsists)
            CheckIfScriptableObjectExsists();

        showList = EditorGUILayout.Toggle("Show List", showList);

        if(showList)
            ShowList();

        if(GUILayout.Button("Spawn Objects"))
            SpawnTheObjects();
    }

    void SpawnTheObjects()
    {
        int timesDone = 0;

        int undoGroup = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName("Spawn Needed Objects");

        while (timesDone < scriptableObject.toSpawn.Count)
        {
            GameObject newOjbect = PrefabUtility.InstantiatePrefab(scriptableObject.toSpawn[timesDone]).GameObject();

            timesDone++;

            Undo.RegisterCreatedObjectUndo(newOjbect, "Spawned Prefab");
        }

        Undo.CollapseUndoOperations(undoGroup);
    }

    void ShowList()
    {
        if (!showingList)
        {
            if (!File.Exists(path))
            {
                ObjectsToSpawn newList = CreateInstance<ObjectsToSpawn>();

                AssetDatabase.CreateAsset(newList, path);

                scriptableObject = AssetDatabase.LoadAssetAtPath<ObjectsToSpawn>(path);
            }
            else
                scriptableObject = AssetDatabase.LoadAssetAtPath<ObjectsToSpawn>(path);

            if (scriptableObject != false)
            {
                serializedData = new SerializedObject(scriptableObject);
                objectsProperty = serializedData.FindProperty("toSpawn");
            }

            showingList = true;
        }

        serializedData.Update();
        EditorGUILayout.PropertyField(objectsProperty, true);

        serializedData.ApplyModifiedProperties();
    }

    void CheckIfScriptableObjectExsists()
    {


        listExsists = true;
    }
}