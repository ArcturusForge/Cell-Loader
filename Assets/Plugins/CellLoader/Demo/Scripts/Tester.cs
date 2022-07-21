using Arcturus.MapLoader;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public Cell_ID managerScene;
    public Cell_ID addScene1;
    public Cell_ID addScene2;
    public Cell_ID addScene3;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            MapLoader.LoadCell(addScene1, LoadNoise.Silent);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            MapLoader.LoadCell(addScene2, LoadNoise.Silent);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            MapLoader.LoadCell(addScene3, LoadNoise.Silent);
    }
}
