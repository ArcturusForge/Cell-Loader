using System.IO;
using UnityEditor;

public class LuaType : CreatorProfile
{
    [MenuItem("Assets/Create/Scripts/LuaType", priority = 20)]
    public static void CreateMenu()
    {
        FileCreator.GenerateWindow<LuaType>("lua");
    }

    public override void GenerateFile(string path, string scriptName)
    {
        using StreamWriter outfile = new StreamWriter(path);
        outfile.WriteLine("");
    }
}
