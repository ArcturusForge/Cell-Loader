using Arcturus.MapLoader;
using System.Collections.Generic;
using UnityEngine;

public class PortalInteractable : MonoBehaviour, IPlayerInteract
{
    [Header("Map Loading Data")]
    public Cell_ID cell;
    public Gateway_ID gateway;
    public LoadNoise noise = LoadNoise.Loud;

    public void PingCanInteract()
    {
        if (noise == LoadNoise.Loud)
            PlayerCont.Player.ToggleInteractUI(true);
        else // Just for demo purposes.
            MapLoader.LoadCell(cell, noise);
    }

    public void PingEndInteract()
    {
        PlayerCont.Player.ToggleInteractUI(false);
    }

    public bool PlayerInteract(GameObject playerObject, out object data)
    {
        MapLoader.LoadCell(cell, noise);
        data = new List<object> { noise, gateway };
        return true;
    }
}
