using System.IO;
using UnityEditor;

public class ScriptableType : CreatorProfile
{
    [MenuItem("Assets/Create/Scripts/ScriptableType", priority = 20)]
     public static void CreateMenu()
     {
        FileCreator.GenerateWindow<ScriptableType>();
     }

     public override void GenerateFile(string path, string scriptName)
     {
        using StreamWriter outfile = new StreamWriter(path);
        outfile.WriteLine("using System.Collections.Generic;");
        outfile.WriteLine("using UnityEngine;");
        outfile.WriteLine("");
        outfile.WriteLine($"[CreateAssetMenu(fileName = \"{scriptName}\", menuName = \"New {scriptName}\")]");
        outfile.WriteLine($"public class {scriptName} : ScriptableObject");
        outfile.WriteLine("{");
        outfile.WriteLine("");
        outfile.WriteLine("}");
    }
}
