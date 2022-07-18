using UnityEditor;

public class ProjectComplexFab : FolderProfile
{
    [MenuItem("Assets/Create/FolderFabs/ProjectComplexFab", priority = 21)]
    public static void CreateMenu()
    {
        FolderFabricator.GenerateFolderPrefab<ProjectComplexFab>();
    }

    public override void GenerateFab(string path)
    {
        new AdditonalPatchesFab().GenerateFab(path);
        var grFolder = BuildFolder(path, "Graphics");
        var scFolder = BuildFolder(path, "Scripts");
        new ScriptSplitFab().GenerateFab(scFolder.FullName);
        var dmFolder = BuildFolder(path, "Demo");
        var etFolder = BuildFolder(path, "Editor");
    }
}
