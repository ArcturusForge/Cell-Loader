using Arcturus.MapLoader;
using System.Collections.Generic;
using UnityEngine;

public class PortalInteractable : TransitionController, IPlayerInteract
{
    [Header("Map Loading Data")]
    public List<Cell_ID> additionalCells = new List<Cell_ID>();

    public override void Awake()
    {
        base.Awake();
        CellLoader.OnLoadedCellsChanged += ToggleState;
    }

    private void OnDestroy()
    {
        CellLoader.OnLoadedCellsChanged -= ToggleState;
    }

    private void ToggleState(List<Cell_ID> activeCells)
    {
        // If the cell that this instance of the portal leads to is already active then
        // disable the portal since it is not needed.
        // Otherwise enable the portal.
        foreach (Cell_ID activeCell in activeCells)
        {
            if (activeCell == targetCellID)
            {
                gameObject.SetActive(false);
                return;
            }
        }

        gameObject.SetActive(true);
    }

    public void PingCanInteract()
    {
        if (loadNoise == LoadNoise.Loud)
            PlayerCont.Player.ToggleInteractUI(true);
        else // Just for demo purposes.
            CellLoader.LoadMultipleCells(targetCellID, additionalCells, loadNoise);
    }

    public void PingEndInteract()
    {
        PlayerCont.Player.ToggleInteractUI(false);
    }

    public bool PlayerInteract(GameObject playerObject, out object data)
    {
        data = new List<object> { loadNoise, targetGatewayID };
        CellLoader.LoadMultipleCells(targetCellID, additionalCells, loadNoise);
        return true;
    }
}
