using Arcturus.MapLoader;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class TransitionController : MonoBehaviour
{
    [Header("Transition Data")]
    [SerializeField]
    protected LoadNoise loadNoise = LoadNoise.Loud;

    [Header("Inbound Data")]
    [SerializeField]
    protected Gateway_ID myGatewayID;

    [SerializeField]
    protected GameObject incomingTeleportPoint;

    [Header("Outbound Data")]
    [SerializeField]
    protected Cell_ID targetCellID;

    [SerializeField]
    protected Gateway_ID targetGatewayID;

    public Gateway_ID MyID => myGatewayID;

    public virtual void Awake()
    {
        CellCoordinator.OnRegisterGateways += RegisterSelf;
    }

    private void RegisterSelf()
    {
        CellCoordinator.OnRegisterGateways -= RegisterSelf;

        var sceneName = Path.GetFileNameWithoutExtension(gameObject.scene.path);
        if (CellCoordinator.ActiveCoordinators.ContainsKey(sceneName))
            CellCoordinator.ActiveCoordinators[sceneName].AddGateway(this);
    }

    /// <summary>
    /// Function that returns the point where the player will be standing when the loading screen fades.
    /// </summary>
    /// <returns></returns>
    public GameObject GetTeleportPoint()
    {
        return incomingTeleportPoint;
    }

    /// <summary>
    /// Autofill function that will load the target scene.
    /// </summary>
    public void LoadTargetCell()
    {
        CellLoader.LoadCell(targetCellID, loadNoise);
    }

    /// <summary>
    /// Autofill function that sends through the necessary data to load the target scene alongside multiple other scenes.<br/>
    /// The additional scenes are always loaded silently and thus do not trigger transition events unlike the main scene.
    /// </summary>
    /// <param name="additionalCells"></param>
    public void LoadMultipleCells(List<Cell_ID> additionalCells)
    {
        CellLoader.LoadMultipleCells(targetCellID, additionalCells, loadNoise);
    }
}
