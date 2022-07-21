using System.IO;
using UnityEngine;

namespace Arcturus.MapLoader
{
    public class CellCoordinator : MonoBehaviour
    {
        private void Awake()
        {
            MapLoader.OnCellLoaded?.Invoke(Path.GetFileNameWithoutExtension(gameObject.scene.path), this);
        }

        // TODO:
        public GameObject GetGateway(string gatewayID)
        {
            // TODO:
            return gameObject;
        }
    }
}
