using UnityEditor;

public class ScriptSplitFab : FolderProfile
{
    [MenuItem("Assets/Create/FolderFabs/ScriptSplitFab", priority = 19)]
    public static void CreateMenu()
    {
        FolderFabricator.GenerateFolderPrefab<ScriptSplitFab>();
    }

    public override void GenerateFab(string path)
    {
        var gnFolder = BuildFolder(path, "Generics");
        var inFolder = BuildFolder(path, "Interfaces");
        var mnFolder = BuildFolder(path, "Monos");
        var soFolder = BuildFolder(path, "Scriptables");
        var edFolder = BuildFolder(path, "Editor");
    }
}
