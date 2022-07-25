using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Arcturus.MapLoader
{
    public class CellCoordinator : MonoBehaviour
    {
        [SerializeField] private GameObject backupSpawn;
        [SerializeField] private List<GatewayData> gateways = new List<GatewayData>();

        private void Awake()
        {
            MapLoader.OnCellLoaded?.Invoke(Path.GetFileNameWithoutExtension(gameObject.scene.path), this);
        }

        // TODO:
        public GameObject GetGateway(string gatewayID)
        {
            // TODO:
            if (gateways.Exists(X => X.id.name == gatewayID))
                return gateways.Find(X => X.id.name == gatewayID).spawnPoint;
            return backupSpawn;
        }

        public GameObject GetGateway(Gateway_ID gatewayID)
        {
            if (gateways.Exists(X => X.id == gatewayID))
                return gateways.Find(X => X.id == gatewayID).spawnPoint;
            return backupSpawn;
        }
    }
}
