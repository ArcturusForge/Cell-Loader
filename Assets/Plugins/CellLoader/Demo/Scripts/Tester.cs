using Arcturus.MapLoader;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public GameObject playerObj;

    public Cell_ID managerScene;
    public Cell_ID addScene1;
    public Cell_ID addScene2;
    public Cell_ID addScene3;

    private void Start()
    {
        // Delay works in milliseconds.
        // This signifies the minimum load time any cell load will take.
        CellLoader.ArtificialLoadDelay = 500;
        CellLoader.OnLoadingEnd += OnLoaded;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            CellLoader.LoadCell(addScene1);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            CellLoader.LoadCell(addScene2);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            CellLoader.LoadCell(addScene3);

        if (Input.GetKeyDown(KeyCode.U))
            CellLoader.UnloadAll();
    }

    public void StartDemo()
    {
        Debug.Log("Starting Demo...");
        CellLoader.LoadCell(addScene1);
    }

    private void OnLoaded()
    {
        playerObj.SetActive(true);
    }
}
