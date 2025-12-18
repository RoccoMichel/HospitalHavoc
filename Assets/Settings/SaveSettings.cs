using UnityEngine;
using System.IO;

[CreateAssetMenu]
public class SaveSettings : ScriptableObject
{
    public string folderName,
                  fileName;

    public bool isReadable = true;

    public string GetFullPath()
    {
        var folder = Path.Combine(Application.persistentDataPath, folderName);

        return Path.Combine(folder, fileName + ".json");
    }
}