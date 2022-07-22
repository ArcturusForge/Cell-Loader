using UnityEngine;

public interface IPlayerInteract
{
    /// <summary>
    /// Is pinged by the player to enable interaction logic/ui.
    /// </summary>
    /// <returns></returns>
    public abstract void PingCanInteract();

    /// <summary>
    /// Is called by the player to attempt an interaction with this object.
    /// </summary>
    /// <returns></returns>
    public abstract bool PlayerInteract(GameObject playerObject);
}
