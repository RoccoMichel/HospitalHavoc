using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpawnRoad : EditorWindow
{
    bool setSeed;
    int seed;

    bool spawnList;

    bool random;

    GameObjectList list;
    SerializedObject serializedData;
    SerializedProperty objectsProperty;

    bool showedList;

    Vector2Int lengt;
    UnityEngine.Object roadObject;
    bool spawnDuble = true;

    float spacing = 3;
    float ofset = 1.5f;

    UnityEngine.Object parent;
    
    Vector3 startPos;
    Vector3 rotation;

    string path = "Assets/Editor/ObjectList.asset";

    GameObject preview;

    bool showPreview;

    List<GameObject> previewList = new List<GameObject>();

    int index;

     [MenuItem("Tools/BoltsTools/Spawn Road")]
    public static void OpenWidow()
    {
        GetWindow(typeof(SpawnRoad));
    }

    private void OnGUI()
    {
        setSeed = EditorGUILayout.Toggle("Set Seed", setSeed);

        if (setSeed)
        {
            seed = EditorGUILayout.IntField("Seed", seed);
            UnityEngine.Random.InitState(seed);
        }
        else
            UnityEngine.Random.InitState(UnityEngine.Random.Range(1, 9999));

        spawnList = EditorGUILayout.Toggle("List", spawnList);

        if (spawnList)
        {
            ShowList();
            random = EditorGUILayout.Toggle("Random", random);
        }

        lengt = EditorGUILayout.Vector2IntField("Lengt", lengt);
        spawnDuble = EditorGUILayout.Toggle("Spawn Duble", spawnDuble);

        if(!spawnList)
            roadObject = EditorGUILayout.ObjectField("Prefab", roadObject, typeof(UnityEngine.Object), false);

        spacing = EditorGUILayout.FloatField("Spacing", spacing);

        ofset = EditorGUILayout.FloatField("Ofset", ofset);

        parent = EditorGUILayout.ObjectField("Parent Object", parent, typeof(UnityEngine.Object), true);

        startPos = EditorGUILayout.Vector3Field("Start Position", startPos);
        rotation = EditorGUILayout.Vector3Field("Rotation", rotation);

        if(!showPreview)
            if (GUILayout.Button("Preview"))
                PreviewSpawn();
        if (showPreview)
            if (GUILayout.Button("Remove Preview"))
                RemovePreview();

        if (GUILayout.Button("Spawn Road"))
            Spawn();
    }

    public void Spawn()
    {
        int timesDone = 0;
        
        while (timesDone < lengt.x + lengt.y)
        {
            Vector3 pos = Vector3.zero;
            Vector3 pos2 = Vector3.zero;
            if (timesDone < lengt.x)
            {
                pos = new Vector3(startPos.x + timesDone * spacing, startPos.y, startPos.z + ofset);
                pos2 = new Vector3(startPos.x + timesDone * spacing, startPos.y, startPos.z - ofset);
            }
            else if(timesDone - lengt.x < lengt.y)
            {
                pos = new Vector3(startPos.x + ofset, startPos.y, startPos.z + Mathf.Abs(timesDone - lengt.x) * spacing);
                pos2 = new Vector3(startPos.x - ofset, startPos.y, startPos.z + Mathf.Abs(timesDone - lengt.x) * spacing);
            }

            if (spawnList)
            {
                int index = timesDone % list.gameObjectList.Count;

                if (random)
                    index = UnityEngine.Random.Range(0, list.gameObjectList.Count - 1);
                
                SpawnGameObject(list.gameObjectList[index], pos);

                if(spawnDuble)
                    SpawnGameObject(list.gameObjectList[index], pos2, new Vector3(0, 180, 0));
            }
            else
            {
                SpawnGameObject(roadObject, pos);

                if (spawnDuble)
                    SpawnGameObject(roadObject, pos2, new Vector3(0, 180, 0));
            }

            timesDone++;
        }
    }

    public void SpawnGameObject(UnityEngine.Object objectToSpawn, Vector3 pos, Vector3 rot = new Vector3())
    {
        GameObject spawnedObject = PrefabUtility.InstantiatePrefab(objectToSpawn).GameObject();
        spawnedObject.transform.position = pos;
        spawnedObject.transform.rotation *= Quaternion.Euler(rotation + rot);

        if (parent != null)
            spawnedObject.transform.parent = parent.GameObject().transform;
    }

    public void PreviewSpawn()
    {
        int timesDone = 0;

        while (timesDone < lengt.x + lengt.y)
        {
            GameObject game = null;
            if (spawnList)
            {
                index = timesDone % list.gameObjectList.Count;
                if (random)
                    index = UnityEngine.Random.Range(0, list.gameObjectList.Count - 1);

                game = PrefabUtility.InstantiatePrefab(list.gameObjectList[index]).GameObject();
            }
            else
                game = PrefabUtility.InstantiatePrefab(roadObject).GameObject();


            Vector3 pos = Vector3.zero;
            Vector3 pos2 = Vector3.zero;

            if (timesDone < lengt.x)
            {
                pos = new Vector3(startPos.x + timesDone * spacing, startPos.y, startPos.z + ofset);
                pos2 = new Vector3(startPos.x + timesDone * spacing, startPos.y, startPos.z - ofset);
            }
            else if (timesDone - lengt.x < lengt.y)
            {
                pos = new Vector3(startPos.x + ofset, startPos.y, startPos.z + (timesDone - lengt.x) * spacing);
                pos2 = new Vector3(startPos.x - ofset, startPos.y, startPos.z + (timesDone - lengt.x) * spacing);
            }

            game.transform.position = pos;
            game.transform.rotation *= Quaternion.Euler(rotation);

            if (parent != null)
                game.transform.parent = parent.GameObject().transform;

            previewList.Add(game);

            if (spawnDuble)
            {
                Vector3 rot2 = rotation + new Vector3(0, 180, 0);
                GameObject game2 = null;

                if (spawnList)
                {
                    game2 = PrefabUtility.InstantiatePrefab(list.gameObjectList[index]).GameObject();
                }
                else
                    game2 = PrefabUtility.InstantiatePrefab(roadObject).GameObject();

                game2.transform.position = pos2;
                game2.transform.rotation *= Quaternion.Euler(rot2);

                if (parent != null)
                    game2.transform.parent = parent.GameObject().transform;

                previewList.Add(game2);
            }

            timesDone++;
        }

        showPreview = true;
    }

    public void RemovePreview()
    {
        int timesToDelete = 0;

        while (timesToDelete < previewList.Count)
        {
            DestroyImmediate(previewList[timesToDelete]);

            timesToDelete++;
        }

        showPreview = false;
    }

    public void ShowList()
    {
        if (!showedList)
        {
            GameObjectList makeList = CreateInstance<GameObjectList>();

            AssetDatabase.CreateAsset(makeList, path);

            list = AssetDatabase.LoadAssetAtPath<GameObjectList>(path);

            if (list != null)
            {
                serializedData = new SerializedObject(list);
                objectsProperty = serializedData.FindProperty("gameObjectList");
            }

            showedList = true;
        }

        serializedData.Update();

        EditorGUILayout.PropertyField(objectsProperty, true);

        serializedData.ApplyModifiedProperties();
    }

    private void Update()
    {
        if (preview == null)
            preview = new GameObject();
        else
        {
            preview.transform.position = startPos;
            preview.name = "Preview";

            var iconCotent = EditorGUIUtility.IconContent("sv_icon_dot11_pix16_gizmo");
            EditorGUIUtility.SetIconForObject(preview, (Texture2D)iconCotent.image);
        }
    }

    private void OnDestroy()
    {
        if (AssetDatabase.AssetPathExists(path))
            AssetDatabase.DeleteAsset(path);

        if (preview != null)
            DestroyImmediate(preview);

        if (showPreview)
        {
            int timesDone = 0;
            while (timesDone < previewList.Count)
            {
                DestroyImmediate(previewList[timesDone]);

                timesDone++;
            }
        }
    }
}

public class GameObjectList : ScriptableObject
{
    public List<UnityEngine.Object> gameObjectList;
}

public class SettPositionOfGameObject : EditorWindow
{
    UnityEngine.Object theObject;

    Vector3 dir = new Vector3(1, 0, 0);

      [MenuItem("Tools/BoltsTools/Sett Position Of Game Object")]
    public static void OpenWidow()
    {
        GetWindow(typeof(SettPositionOfGameObject));
    }

    private void OnGUI()
    {
        theObject = EditorGUILayout.ObjectField("objcet", theObject, typeof(UnityEngine.Object), true);

        dir = EditorGUILayout.Vector3Field("Diraction", dir);

        if (GUILayout.Button("Move Object"))
            MoveObject();
    }

    public void MoveObject()
    {
        GameObject objectToMove = theObject.GameObject();

        RaycastHit hit;
        if(Physics.Raycast(objectToMove.transform.position, dir, out hit, 2))
        {
            if(objectToMove.GetComponent<Collider>() != null)
            {
                Vector3 objectCenter = objectToMove.GetComponent<Collider>().bounds.center;
                Vector3 closest = hit.collider.ClosestPoint(objectCenter);

                Vector3 away = (objectCenter - closest).normalized;

                float myHalfExtent = Vector3.Dot(objectToMove.GetComponent<Collider>().bounds.extents, away);
                objectToMove.transform.position = closest + away * myHalfExtent;
            }
        }
    }

    private void Update()
    {
        if(theObject != null)
        {
            GameObject objectToMove = theObject.GameObject();

            Debug.DrawRay(objectToMove.transform.position, dir * 2, Color.green);
        }
    }
}

public class SelectAllChildrenWithComponent : EditorWindow
{
    GameObject target;
    List<Type> componets = new List<Type>();
    string[] componetsName = new string[0];
    int slectedIndex = 0;

      [MenuItem("Tools/BoltsTools/Select All Children With Component")]
    public static void OpenWindow()
    {
        GetWindow(typeof(SelectAllChildrenWithComponent));
    }

    private void OnGUI()
    {
        var newTarget = (GameObject)EditorGUILayout.ObjectField("Target", target, typeof(GameObject), true);

        if(newTarget != target)
        {
            target = newTarget;
            RefreshComponentList();
        }

        DrawEnum();
    }

    public void DrawEnum()
    {
        if (target != null && componets.Count > 0)
        {
            slectedIndex = EditorGUILayout.Popup(
                new GUIContent("Component Type"),
                slectedIndex,
                componetsName
                );

            if (GUILayout.Button("Select Children"))
                SelectChildrenWithComponent(componets[slectedIndex]);
        }
        else if (target != null)
            EditorGUILayout.HelpBox("No Components Found :(", MessageType.Error);
    }

    void RefreshComponentList()
    {
        componets.Clear();

        var comps = target.GetComponentsInChildren<Component>();

        componets = comps
            .Select(c => c.GetType())
            .Distinct()
            .Where(t => t != typeof(Transform))
            .ToList();

        componetsName = componets
            .Select(t => t.Name)
            .ToArray();

        slectedIndex = 0;
    }

    void SelectChildrenWithComponent(Type componentType)
    {
        var foundComps = target.GetComponentsInChildren(componentType, true);

        var gos = new HashSet<GameObject>(
            foundComps.Cast<Component>().Select(c => c.gameObject));

        Selection.objects = gos.ToArray();
    }
}

public class SelectAllWithComponentOrName : EditorWindow
{
    List<Type> componets = new List<Type>();
    string[] componetsName = new string[0];
    int selectedIndex = 0;

    bool selectName;
    string objectName = "";

      [MenuItem("Tools/BoltsTools/Select All Objects With Component")]
    public static void OpenWidow()
    {
        GetWindow(typeof(SelectAllWithComponentOrName));
    }

    private void OnEnable()
    {
        RefreshComponentList();
        EditorApplication.hierarchyChanged += RefreshComponentList;
    }

    private void OnDisable()
    {
        EditorApplication.hierarchyChanged -= RefreshComponentList;
    }

    private void OnGUI()
    {
        selectName = EditorGUILayout.Toggle("Select With Name", selectName);

        EditorGUILayout.Space();
        if(componets.Count == 0)
        {
            EditorGUILayout.HelpBox("No Componets Founs", MessageType.Error);
            if (GUILayout.Button("Re-scan Scene")) RefreshComponentList();
            return;
        }

        if (selectName)
        {
            objectName = EditorGUILayout.TextField("Name", objectName);

            if (GUILayout.Button("Select All Objects"))
                SelectAllWithName();
        }
        else
        {
            selectedIndex = EditorGUILayout.Popup("Component Type", selectedIndex, componetsName);

            if (GUILayout.Button("Select All Objects"))
                SelectAllWith(componets[selectedIndex]);

            if (GUILayout.Button("Re-scan Scene"))
                RefreshComponentList();
        }
    }

    public void RefreshComponentList()
    {
        componets.Clear();

        var roots = SceneManager.GetActiveScene().GetRootGameObjects();

        var allComps = roots
            .SelectMany(r => r.GetComponentsInChildren<Component>(true))
            .Where(c => c != null);

        componets = allComps
            .Select(c => c.GetType())
            .Where(t => t != typeof(Transform))
            .Distinct()
            .OrderBy(t => t.Name)
            .ToList();

        componetsName = componets
            .Select(t => t.Name)
            .ToArray();

        selectedIndex = 0;
    }

    public void SelectAllWith(Type type)
    {
        var roots = SceneManager.GetActiveScene().GetRootGameObjects();
        var comps = roots
            .SelectMany(r => r.GetComponentsInChildren(type, true))
            .Cast<Component>();

        var gos = comps
            .Select(c => c.gameObject)
            .Distinct()
            .ToArray();

        Selection.objects = gos;
    }

    void SelectAllWithName()
    {
        var allGameobjects = SceneManager.GetActiveScene().GetRootGameObjects();

        var getAllNames = allGameobjects
            .SelectMany(r => r.GetComponentsInChildren<Transform>(true))
            .Select(t => t.gameObject);

        var matches = getAllNames
            .Where(go => go.name.ToLower().Contains(objectName.ToLower()))
            .Distinct()
            .ToArray();

        Selection.objects = matches;
    }
}

// Change The Value Of A Given PlayerPrefs
public class SetNewPlayerPrefs : EditorWindow
{
    string playerPrefs;
    string newPlayerPrefs;

    string[] type = new string[] { "int", "float", "string" };
    int selectionIndex = 0;

    // Adds It To The Tools Menu
      [MenuItem("Tools/BoltsTools/PlayerPref/Set New Value On PlayerPrefs")]
    public static void ShowWidow()
    {
        // Creats A Custom Window
        GetWindow(typeof(SetNewPlayerPrefs));
    }

    private void OnGUI()
    {
        selectionIndex = EditorGUILayout.Popup("PlayerPref Type", selectionIndex, type);

        // Adds A Label
        GUILayout.Label("Set New Value On PlayerPrefs", EditorStyles.boldLabel);

        // Adds A Editable Text Field That The You Input The PlayerPrefs You Want To Change
        playerPrefs = EditorGUILayout.TextField("PlayerPrefs To Cnange", playerPrefs);
        // Adds A Editable Text Field That The New Value For The PlayerPrefs Is Going To Be
        newPlayerPrefs = EditorGUILayout.TextField("Set New PlayerPrefs Value", newPlayerPrefs);

        // Adds A Button That When Pressed Calls The ChangePlayerPres Function
        if (GUILayout.Button("Change PlayerPres"))
            ChangePlayerPres();
    }

    public void ChangePlayerPres()
    {
        if (selectionIndex == 0)
            PlayerPrefs.SetInt(playerPrefs, int.Parse(newPlayerPrefs));
        else if (selectionIndex == 1)
            PlayerPrefs.SetFloat(playerPrefs, float.Parse(newPlayerPrefs));
        else
            PlayerPrefs.SetString(playerPrefs, newPlayerPrefs);

    }
}



// You Can Check A Value Of A PlayerPfres
public class SeePlayerPrefs : EditorWindow
{
    string playerPfrefs;

    string newValue = "";
    bool showText = false;
    bool exist;

    // Adds It To The Tools Menu
      [MenuItem("Tools/BoltsTools/PlayerPref/See PlayerPrefs")]
    public static void ShowWidow()
    {
        // Creats A Custom Window
        GetWindow(typeof(SeePlayerPrefs));
    }

    private void OnGUI()
    {
        // Adds A Editable Text Field That The You Input The PlayerPrefs You Want To Check
        playerPfrefs = EditorGUILayout.TextField("PlayerPfres", playerPfrefs);

        // Adds A Button That When Pressed Calls The Check Function
        if (GUILayout.Button("Check"))
            Check();

        // If showText Is True Add A Label With The PlayerPfres Value
        if (showText)
        {
            if (!exist)
            {
                GUIContent errorIcon = EditorGUIUtility.IconContent("console.errorIcon");
                GUILayout.Label(errorIcon, GUILayout.Width(50), GUILayout.Height(50));
            }

            GUIStyle wrapStyl = new GUIStyle(EditorStyles.boldLabel)
            {
                wordWrap = true,
                fontSize = 25
            };
            GUILayout.Label(newValue, wrapStyl);
        }
    }

    public void Check()
    {
        if (PlayerPrefs.HasKey(playerPfrefs))
        {
            newValue = (PlayerPrefs.GetInt(playerPfrefs).ToString() + PlayerPrefs.GetFloat(playerPfrefs) + PlayerPrefs.GetString(playerPfrefs)).ToString();
            if (newValue == "00")
                newValue = "0";
            else
                newValue = newValue.Replace("0", "");

            exist = true;
        }
        else
        {
            newValue = playerPfrefs + " Does Not Exist";
            exist = false;
        }

        showText = true;
    }
}

public class ResetPlayerPrefs : EditorWindow
{
    string playerPrefs;
    string textToShow;
    bool showText;
    bool exits;

      [MenuItem("Tools/BoltsTools/PlayerPref/Reset PlayerPrefs")]
    public static void ShowWidow()
    {
        GetWindow(typeof(ResetPlayerPrefs));
    }

    private void OnGUI()
    {
        playerPrefs = EditorGUILayout.TextField("PlayerPrefs To Reset", playerPrefs);

        if (GUILayout.Button("Reset"))
            Check();

        if (showText)
        {
            if (!exits)
            {
                GUIContent errorIcon = EditorGUIUtility.IconContent("console.errorIcon");
                GUILayout.Label(errorIcon, GUILayout.Width(50), GUILayout.Height(50));
            }

            GUILayout.Label(textToShow, EditorStyles.boldLabel);
        }
    }

    public void Check()
    {
        if (PlayerPrefs.HasKey(playerPrefs))
        {
            exits = true;
            textToShow = playerPrefs + " Has Been Reseted";

            PlayerPrefs.DeleteKey(playerPrefs);
        }
        else
        {
            exits = false;
            textToShow = playerPrefs + " Does Not Exit";
        }

        showText = true;
    }
}

public class ResetAllPlayerPrefs : EditorWindow
{
    bool showText;

      [MenuItem("Tools/BoltsTools/PlayerPref/Reset All PlayerPrefs")]
    public static void ShowWidow()
    {
        GetWindow(typeof(ResetAllPlayerPrefs));
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Reset All"))
            Check();

        if (showText)
        {
            GUILayout.Label("All PlayerPreds Has Ben Reseted", EditorStyles.boldLabel);
        }
    }

    public void Check()
    {
        PlayerPrefs.DeleteAll();
    }
}

public class TakeScreanshot : EditorWindow
{
    string path = "Assets/Name";

      [MenuItem("Tools/BoltsTools/Screanshot")]
    public static void OpenWindow()
    {
        GetWindow(typeof(TakeScreanshot));
    }

    private void OnGUI()
    {
        path = EditorGUILayout.TextField("File Name", path);

        if (GUILayout.Button("Take Screenshot"))
            TakeIt();
    }

    void TakeIt()
    {
        ScreenCapture.CaptureScreenshot(path + ".png");
    }
}