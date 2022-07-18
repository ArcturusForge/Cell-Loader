using System.IO;
using UnityEditor;

public class BlankTextType : CreatorProfile
{
    [MenuItem("Assets/Create/Text Files/BlankTextType", priority = 20)]
    public static void CreateMenu()
    {
        FileCreator.GenerateWindow<BlankTextType>("txt");
    }

    public override void GenerateFile(string path, string scriptName)
    {
        using StreamWriter outfile = new StreamWriter(path);
        outfile.WriteLine("");
    }
}
