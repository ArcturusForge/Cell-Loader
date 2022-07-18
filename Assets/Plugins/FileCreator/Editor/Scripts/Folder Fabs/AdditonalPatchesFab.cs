using UnityEditor;

public class AdditonalPatchesFab : FolderProfile
{
    [MenuItem("Assets/Create/FolderFabs/AdditonalPatchesFab", priority = 20)]
    public static void CreateMenu()
    {
        FolderFabricator.GenerateFolderPrefab<AdditonalPatchesFab>();
    }

    public override void GenerateFab(string path)
    {
        var apFolder = BuildFolder(path, "Additional_Patches");
        var fcFolder = BuildInsideFolder(apFolder, "FileCreator");
        BuildInsideFolder(fcFolder, "Editor");
    }
}
