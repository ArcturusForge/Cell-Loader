using System.IO;
using UnityEditor;

public class CreatorType : CreatorProfile
{
    [MenuItem("Assets/Create/Scripts/CreatorType", priority = 70)]
    public static void CreateMenu()
    {
        FileCreator.GenerateWindow<CreatorType>();
    }

    public override void GenerateFile(string path, string scriptName)
    {
        using StreamWriter outfile = new StreamWriter(path);
        outfile.WriteLine("using System.IO;");
        outfile.WriteLine("using UnityEditor;");
        outfile.WriteLine("");
        outfile.WriteLine($"public class {scriptName} : CreatorProfile");
        outfile.WriteLine("{");
        outfile.WriteLine($"    [MenuItem(\"Assets/Create/Scripts/{scriptName}\", priority = 20)]");
        outfile.WriteLine("    public static void CreateMenu()");
        outfile.WriteLine("    {");
        outfile.WriteLine($"        FileCreator.GenerateWindow<{scriptName}>();");
        outfile.WriteLine("    }");
        outfile.WriteLine("");
        outfile.WriteLine("    public override void GenerateFile(string path, string scriptName)");
        outfile.WriteLine("    {");
        outfile.WriteLine("        using StreamWriter outfile = new StreamWriter(path);");
        outfile.WriteLine("        outfile.WriteLine(\"\");");
        outfile.WriteLine("    }");
        outfile.WriteLine("}");
    }
}
