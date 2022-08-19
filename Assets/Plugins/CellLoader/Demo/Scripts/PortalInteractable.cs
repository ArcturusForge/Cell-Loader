using Arcturus.MapLoader;
using System.Collections.Generic;
using UnityEngine;

public class PortalInteractable : MonoBehaviour, IPlayerInteract
{
    [Header("Map Loading Data")]
    public Cell_ID cell;
    public List<Cell_ID> additionalCells = new List<Cell_ID>();
    public Gateway_ID gateway;
    public LoadNoise noise = LoadNoise.Loud;

    public void Awake()
    {
        MapLoader.OnLoadedCellsChanged += ToggleState;
    }

    private void OnDestroy()
    {
        MapLoader.OnLoadedCellsChanged -= ToggleState;
    }

    private void ToggleState(List<Cell_ID> activeCells)
    {
        // If the cell that this instance of the portal leads to is already active then
        // disable the portal since it is not needed.
        // Otherwise enable the portal.
        foreach (Cell_ID activeCell in activeCells)
        {
            if (activeCell == cell)
            {
                gameObject.SetActive(false);
                return;
            }
        }

        gameObject.SetActive(true);
    }

    public void PingCanInteract()
    {
        if (noise == LoadNoise.Loud)
            PlayerCont.Player.ToggleInteractUI(true);
        else // Just for demo purposes.
            MapLoader.LoadMultipleCells(cell, additionalCells, noise);
    }

    public void PingEndInteract()
    {
        PlayerCont.Player.ToggleInteractUI(false);
    }

    public bool PlayerInteract(GameObject playerObject, out object data)
    {
        data = new List<object> { noise, gateway };
        MapLoader.LoadMultipleCells(cell, additionalCells, noise);
        return true;
    }
}
