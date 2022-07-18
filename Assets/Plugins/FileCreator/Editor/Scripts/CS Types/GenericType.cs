using System.IO;
using UnityEditor;

public class GenericType : CreatorProfile
{
    [MenuItem("Assets/Create/Scripts/GenericType", priority = 20)]
    public static void CreateMenu()
    {
        FileCreator.GenerateWindow<GenericType>();
    }

    public override void GenerateFile(string path, string scriptName)
    {
        using StreamWriter outfile = new StreamWriter(path);
        outfile.WriteLine($"public class {scriptName}");
        outfile.WriteLine("{");
        outfile.WriteLine("");
        outfile.WriteLine("}");
    }
}
