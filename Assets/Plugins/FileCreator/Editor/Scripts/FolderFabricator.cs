using System.IO;
using UnityEditor;

public static class FolderFabricator
{
    public static void GenerateFolderPrefab<T>() where T : FolderProfile, new()
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

        var profile = new T();
        profile.GenerateFab(path);
        AssetDatabase.Refresh();
    }
}
