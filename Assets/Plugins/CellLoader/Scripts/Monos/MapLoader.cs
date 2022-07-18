using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcturus.MapLoader
{
    public class MapLoader : MonoBehaviour
    {
        private static MapLoader I;

        [Header("Setup Data")]
        public CellDatabase cellDatabase;

        [Header("Transition Events")]
        public OnLoadStart OnLoadStart;
        public OnLoadEnd OnLoadEnd;
        public OnRelocatePlayer OnRelocatePlayer;

        private Dictionary<string, GameObject> cells;
        private Dictionary<string, CellCoordinator> spawnedCells;

        private void Awake()
        {
            #region Static Referencing
            if (I == null) I = this;
            else if (I != this) Destroy(gameObject);
            #endregion

            cells = new Dictionary<string, GameObject>();
            foreach (var cell in cellDatabase.cells)
                cells[cell.id] = cell.cellPrefab;

            spawnedCells = new Dictionary<string, CellCoordinator>();
        }

        public static bool AddCell(string id, GameObject cellPrefab, bool replaceConflict = false)
        {
            if (I.cells.ContainsKey(id) && !replaceConflict)
            {
                Debug.LogWarning($"Cell Conflict: cell of id {id} already exists! Replacement has not occured..");
                return false;
            }
            else if (I.cells.ContainsKey(id) && replaceConflict)
                Debug.LogWarning($"Cell Replacement: cell of id {id} already exists! Replacement has occured..");

            I.cells[id] = cellPrefab;
            return true;
        }

        public static bool LoadCell(string cellId, string gatewayId, LoadNoise loadNoise = LoadNoise.Loud)
        {
            if (!I.cells.ContainsKey(cellId))
            {
                Debug.LogWarning($"Missing Cell: cell of id {cellId} does not exist!");
                return false;
            }

            //if () // Check if gateway exists
            //{
            //    Debug.LogWarning($"Missing Gateway: Gateway of id {gatewayId} does not exist!");
            //    return false;
            //}

            I.StartCoroutine(I.GenerateCell(cellId, gatewayId, loadNoise));

            return true;
        }

        private IEnumerator GenerateCell(string cellId, string gatewayId, LoadNoise loadNoise = LoadNoise.Loud)
        {
            if (loadNoise == LoadNoise.Loud)
                I.OnLoadStart?.Invoke();

            yield return new WaitForEndOfFrame();

            var obj = Instantiate(I.cells[cellId]);
            var coordinator = obj.GetComponent<CellCoordinator>();
            // TODO:

            yield return new WaitForEndOfFrame();

            if (loadNoise == LoadNoise.Loud)
                I.OnLoadStart?.Invoke();
        }
    }
}
