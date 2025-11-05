using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectsToSpawn : ScriptableObject
{
    public List<GameObject> toSpawn;

    public Vector3 camPos = new Vector3(0, 6.25f, -10),
                   camRot = new Vector3(30, 0, 0);
}

public class SpawnObjects : EditorWindow
{
    ObjectsToSpawn scriptableObject;
    SerializedObject serializedData;
    SerializedProperty listProperty;

    bool showingList;

    string path = "Assets/Editor/ObjectsNeeded.asset";

    [MenuItem("Tools/BoltsTools/Spawn Needed Objects")]
    public static void OpenWindow()
    {
        GetWindow(typeof(SpawnObjects), true, "Spawn Objects Needed");
    }

    void OnGUI()
    {
        ShowList();

        if (showingList)
        {
            scriptableObject.camPos = EditorGUILayout.Vector3Field("Camera Position", scriptableObject.camPos);
            scriptableObject.camRot = EditorGUILayout.Vector3Field("Camera Rotation", scriptableObject.camRot);
        }

        if(GUILayout.Button("Spawn Objects"))
            SpawnTheObjects();
    }

    void SpawnTheObjects()
    {
        int timesDone = 0;

        int undoGroup = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName("Spawn Needed Objects");

        Transform cam = Camera.main.transform;

        Undo.RecordObject(cam, "Setting The Camera Position And Rotation");

        cam.position = scriptableObject.camPos;
        cam.rotation = Quaternion.Euler(scriptableObject.camRot);

        Scene currentScene = SceneManager.GetActiveScene();
        string scenePath = currentScene.path;
        string sceneName = Path.GetFileNameWithoutExtension(scenePath);
        string folderName = Path.GetDirectoryName(scenePath);

        string folderPath = Path.Combine(folderName, sceneName);

        if(!AssetDatabase.IsValidFolder(folderPath))
            AssetDatabase.CreateFolder(folderName, sceneName);

        LevelDataObject newDataObject = CreateInstance<LevelDataObject>();

        if (File.Exists(folderPath + "/" + sceneName + "Data.asset"))
            AssetDatabase.DeleteAsset(folderPath + "/" + sceneName + "Data.asset");

        AssetDatabase.CreateAsset(newDataObject, folderPath + "/" + sceneName + "Data.asset");

        newDataObject = AssetDatabase.LoadAssetAtPath<LevelDataObject>(folderPath + "/" + sceneName + "Data.asset");

        while (timesDone < scriptableObject.toSpawn.Count)
        {
            GameObject newOjbect = PrefabUtility.InstantiatePrefab(scriptableObject.toSpawn[timesDone]).GameObject();

            timesDone++;

            Undo.RegisterCreatedObjectUndo(newOjbect, "Spawned Prefab");
        }

        GameController controller = GameObject.FindFirstObjectByType<GameController>();
        controller.levelData = newDataObject;

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
                listProperty = serializedData.FindProperty("toSpawn");
            }

            showingList = true;
        }

        serializedData.Update();
        EditorGUILayout.PropertyField(listProperty, true);

        serializedData.ApplyModifiedProperties();
    }
}