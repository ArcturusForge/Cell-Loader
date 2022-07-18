using System.IO;
using UnityEditor;

public class AssetListType : CreatorProfile
{
    [MenuItem("Assets/Create/Text Files/AssetListType", priority = 31)]
    public static void CreateMenu()
    {
        FileCreator.GenerateWindow<AssetListType>("txt");
    }

    public override void GenerateFile(string path, string scriptName)
    {
        using StreamWriter outfile = new StreamWriter(path);
        outfile.WriteLine("Key:");
        outfile.WriteLine("-  <= Not Received.");
        outfile.WriteLine("-* <= Not Implemented.");
        outfile.WriteLine("-/ <= Implemented.");
        outfile.WriteLine("^  <= Explanation for above.");
        outfile.WriteLine("");
        outfile.WriteLine("Assets:");
        outfile.WriteLine("");
        outfile.WriteLine("General =>");
        outfile.WriteLine("-");
    }
}
