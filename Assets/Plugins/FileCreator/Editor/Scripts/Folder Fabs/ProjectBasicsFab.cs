using UnityEditor;

public class ProjectBasicsFab : FolderProfile
{
    [MenuItem("Assets/Create/FolderFabs/ProjectBasicsFab", priority = 20)]
    public static void CreateMenu()
    {
        FolderFabricator.GenerateFolderPrefab<ProjectBasicsFab>();
    }

    public override void GenerateFab(string path)
    {
        new AdditonalPatchesFab().GenerateFab(path);
        var scFolder = BuildFolder(path, "Scripts");
        new ScriptSplitFab().GenerateFab(scFolder.FullName);
        var dmFolder = BuildFolder(path, "Demo");
        var gpFolder = BuildFolder(path, "Graphics");
        var pfFolder = BuildFolder(path, "Prefabs");
    }
}
