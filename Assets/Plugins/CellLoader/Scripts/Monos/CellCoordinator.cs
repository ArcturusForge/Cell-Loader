using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Arcturus.MapLoader
{
    public class CellCoordinator : MonoBehaviour
    {
        [SerializeField] private GameObject backupSpawn;
        private List<TransitionController> gateways = new List<TransitionController>();

        public static Dictionary<string, CellCoordinator> ActiveCoordinators = new Dictionary<string, CellCoordinator>();
        public static Action OnRegisterGateways;

        private void Awake()
        {
            var sceneName = Path.GetFileNameWithoutExtension(gameObject.scene.path);
            ActiveCoordinators[sceneName] = this;
        }

        private void Start()
        {
            OnRegisterGateways?.Invoke();

            var sceneName = Path.GetFileNameWithoutExtension(gameObject.scene.path);
            CellLoader.OnCellLoaded?.Invoke(sceneName, this);
            CellLoader.BroadcastLoadedCells();
        }

        private void OnDestroy()
        {
            var sceneName = Path.GetFileNameWithoutExtension(gameObject.scene.path);
            ActiveCoordinators.Remove(sceneName);
        }

        public void AddGateway(TransitionController transitionController)
        {
            if (gateways.Contains(transitionController))
                return;

            gateways.Add(transitionController);
        }

        public GameObject GetGateway(string gatewayID)
        {
            if (gateways.Exists(X => X.MyID.name == gatewayID))
                return gateways.Find(X => X.MyID.name == gatewayID).GetTeleportPoint();
            return backupSpawn;
        }

        public GameObject GetGateway(Gateway_ID gatewayID)
        {
            if (gateways.Exists(X => X.MyID == gatewayID))
                return gateways.Find(X => X.MyID == gatewayID).GetTeleportPoint();
            return backupSpawn;
        }
    }
}
