using UnityEditor;

public class InheritedClassFab : FolderProfile
{
    [MenuItem("Assets/Create/FolderFabs/InheritedClassFab", priority = 20)]
    public static void CreateMenu()
    {
        FolderFabricator.GenerateFolderPrefab<InheritedClassFab>();
    }

    public override void GenerateFab(string path)
    {
        var exFolder = BuildFolder(path, "RenameFolder");
        BuildInsideFolder(exFolder, "Base");
    }
}
