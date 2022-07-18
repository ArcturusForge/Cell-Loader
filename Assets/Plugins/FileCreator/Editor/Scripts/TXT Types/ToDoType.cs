using System.IO;
using UnityEditor;

public class ToDoType : CreatorProfile
{
    [MenuItem("Assets/Create/Text Files/ToDoType", priority = 31)]
    public static void CreateMenu()
    {
        FileCreator.GenerateWindow<ToDoType>("txt");
    }

    public override void GenerateFile(string path, string scriptName)
    {
        using StreamWriter outfile = new StreamWriter(path);
        outfile.WriteLine("Key:");
        outfile.WriteLine("-  <= Incomplete.");
        outfile.WriteLine("-/ <= Complete.");
        outfile.WriteLine("^  <= Explanation for above.");
        outfile.WriteLine("");
        outfile.WriteLine("ToDo:");
        outfile.WriteLine("");
        outfile.WriteLine("General =>");
        outfile.WriteLine("-");
    }
}
