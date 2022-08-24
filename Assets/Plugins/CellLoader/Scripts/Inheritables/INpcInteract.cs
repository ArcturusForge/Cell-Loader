using UnityEngine;

public interface INpcInteract
{
    /// <summary>
    /// Is called by an npc to attempt an interaction with this object.
    /// </summary>
    /// <param name="npcObject"></param>
    /// <returns></returns>
    public abstract bool NpcInteract(GameObject npcObject);
}
