using System.IO;
using UnityEditor;
using UnityEngine;

public static class FileCreator
{
    public static void GenerateWindow<T>(string fileType = "cs") where T : CreatorProfile, new()
    {
        string path;
        var obj = Selection.activeObject;
        if (obj == null) path = "Assets";
        else
        {
            path = AssetDatabase.GetAssetPath(obj.GetInstanceID());
            
            if (path.Contains("."))
                path = path.Replace("/" + Path.GetFileName(path), "");
        }

        Prompt.Show("Name File", "Exclude blank space!", (string value) => { GenerateScript<T>(path, value, fileType); }, null, CheckInput);
    }

    static void GenerateScript<T>(string path, string scriptName, string fileType) where T : CreatorProfile, new()
    {
        string copyPath = $"{path}/{scriptName}.{fileType}";
        Debug.Log($"Creating {scriptName}.{fileType} at: {copyPath}");
        if (File.Exists(copyPath) == false)
        {
            var profile = new T();
            profile.GenerateFile(copyPath, scriptName);
        }
        AssetDatabase.Refresh();
    }

    static bool CheckInput(string input)
    {
        return !input.Contains(" ");
    }
}
