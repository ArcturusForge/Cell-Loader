using System.IO;
using UnityEditor;

public class ScriptListType : CreatorProfile
{
    [MenuItem("Assets/Create/Text Files/ScriptListType", priority = 31)]
    public static void CreateMenu()
    {
        FileCreator.GenerateWindow<ScriptListType>("txt");
    }

    public override void GenerateFile(string path, string scriptName)
    {
        using StreamWriter outfile = new StreamWriter(path);
        outfile.WriteLine("---All Scripts---");
        outfile.WriteLine("");
        outfile.WriteLine("Name: ");
        outfile.WriteLine("Purpose:");
        outfile.WriteLine("-");
        outfile.WriteLine("Variables:");
        outfile.WriteLine("-");
        outfile.WriteLine("");
        outfile.WriteLine("");
        outfile.WriteLine("Name: ");
        outfile.WriteLine("Purpose:");
        outfile.WriteLine("-");
        outfile.WriteLine("Variables:");
        outfile.WriteLine("-");
        outfile.WriteLine("");
        outfile.WriteLine("");
        outfile.WriteLine("Name: ");
        outfile.WriteLine("Purpose:");
        outfile.WriteLine("-");
        outfile.WriteLine("Variables:");
        outfile.WriteLine("-");
        outfile.WriteLine("");
        outfile.WriteLine("");
    }
}
