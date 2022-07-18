using System.IO;
using UnityEngine;

public abstract class FolderProfile
{
    public abstract void GenerateFab(string path);

    protected DirectoryInfo BuildFolder(string path, string folderName)
    {
        try
        {
            var folderPath = $"{path}/{folderName}";
            var dirInfo = Directory.CreateDirectory(folderPath);
            return dirInfo;
        }
        catch (System.Exception)
        {
            Debug.LogError("Folder Build Failed!");
            throw;
        }
    }

    protected DirectoryInfo BuildInsideFolder(DirectoryInfo parentFolder, string folderName)
    {
        try
        {
            return BuildFolder(parentFolder.FullName, folderName);
        }
        catch (System.Exception)
        {
            Debug.LogError("Folder Build Failed!");
            throw;
        }
    }
}
