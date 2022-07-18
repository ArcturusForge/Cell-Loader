using System.IO;
using UnityEditor;

public class AnimatorBehaviourType : CreatorProfile
{
   [MenuItem("Assets/Create/Scripts/AnimatorBehaviourType", priority = 50)]
    public static void CreateMenu()
    {
       FileCreator.GenerateWindow<AnimatorBehaviourType>();
    }

    public override void GenerateFile(string path, string scriptName)
    {
        using StreamWriter outfile = new StreamWriter(path);
        outfile.WriteLine("using UnityEngine;");
        outfile.WriteLine("");
        outfile.WriteLine("public class NewStateMachineBehaviour1 : StateMachineBehaviour");
        outfile.WriteLine("{");
        outfile.WriteLine("    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)");
        outfile.WriteLine("    {");
        outfile.WriteLine("    }");
        outfile.WriteLine("");
        outfile.WriteLine("    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)");
        outfile.WriteLine("    {");
        outfile.WriteLine("    }");
        outfile.WriteLine("");
        outfile.WriteLine("    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)");
        outfile.WriteLine("    {");
        outfile.WriteLine("    }");
        outfile.WriteLine("");
        outfile.WriteLine("    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)");
        outfile.WriteLine("    {");
        outfile.WriteLine("    }");
        outfile.WriteLine("");
        outfile.WriteLine("    public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)");
        outfile.WriteLine("    {");
        outfile.WriteLine("    }");
        outfile.WriteLine("}");
    }
}
