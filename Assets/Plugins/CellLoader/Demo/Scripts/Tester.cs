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
        MapLoader.ArtificialLoadDelay = 500;
        MapLoader.OnLoadingEnd += OnLoaded;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            MapLoader.LoadCell(addScene1);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            MapLoader.LoadCell(addScene2);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            MapLoader.LoadCell(addScene3);
    }

    public void StartDemo()
    {
        Debug.Log("Starting Demo...");
        MapLoader.LoadCell(addScene1);
    }

    private void OnLoaded()
    {
        playerObj.SetActive(true);
    }
}
