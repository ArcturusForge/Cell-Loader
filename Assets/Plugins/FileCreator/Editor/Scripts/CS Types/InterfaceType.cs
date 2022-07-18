using System.IO;
using UnityEditor;

public class InterfaceType : CreatorProfile
{
    [MenuItem("Assets/Create/Scripts/InterfaceType", priority = 20)]
    public static void CreateMenu()
    {
        FileCreator.GenerateWindow<InterfaceType>();
    }

    public override void GenerateFile(string path, string scriptName)
    {
        using StreamWriter outfile = new StreamWriter(path);
        outfile.WriteLine("using System.Collections.Generic;");
        outfile.WriteLine("using UnityEngine;");
        outfile.WriteLine("");
        outfile.WriteLine($"public interface {scriptName}");
        outfile.WriteLine("{");
        outfile.WriteLine("");
        outfile.WriteLine("}");
    }
}
