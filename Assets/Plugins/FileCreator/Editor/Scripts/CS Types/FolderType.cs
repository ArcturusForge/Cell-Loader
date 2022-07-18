using System.IO;
using UnityEditor;

public class FolderType : CreatorProfile
{
    [MenuItem("Assets/Create/Scripts/FolderType", priority = 70)]
    public static void CreateMenu()
    {
        FileCreator.GenerateWindow<FolderType>();
    }

    public override void GenerateFile(string path, string scriptName)
    {
        using StreamWriter outfile = new StreamWriter(path);
        outfile.WriteLine("using UnityEditor;");
        outfile.WriteLine("");
        outfile.WriteLine($"public class {scriptName} : FolderProfile");
        outfile.WriteLine("{");
        outfile.WriteLine($"    [MenuItem(\"Assets/Create/FolderFabs/{scriptName}\", priority = 20)]");
        outfile.WriteLine("    public static void CreateMenu()");
        outfile.WriteLine("    {");
        outfile.WriteLine($"        FolderFabricator.GenerateFolderPrefab<{scriptName}>();");
        outfile.WriteLine("    }");
        outfile.WriteLine("");
        outfile.WriteLine("    public override void GenerateFab(string path)");
        outfile.WriteLine("    {");
        outfile.WriteLine("        var exFolder = BuildFolder(path, \"FolderName\");");
        outfile.WriteLine("    }");
        outfile.WriteLine("}");
    }
}
