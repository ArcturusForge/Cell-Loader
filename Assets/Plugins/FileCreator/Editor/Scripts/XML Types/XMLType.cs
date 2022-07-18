using System.IO;
using UnityEditor;

public class XMLType : CreatorProfile
{
    [MenuItem("Assets/Create/Scripts/XMLType", priority = 20)]
    public static void CreateMenu()
    {
        FileCreator.GenerateWindow<XMLType>("xml");
    }

    public override void GenerateFile(string path, string scriptName)
    {
        using StreamWriter outfile = new StreamWriter(path);
        outfile.WriteLine("<?xml version=\"1.0\" encoding=\"utf-16\"?>");
        outfile.WriteLine("");
    }
}
