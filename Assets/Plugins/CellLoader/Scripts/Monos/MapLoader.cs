using Arcturus.MapLoader.Internal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arcturus.MapLoader.Internal
{
    public enum LoadEventType { Start, End, Progress }
}

namespace Arcturus.MapLoader
{
    public class MapLoader : MonoBehaviour
    {
        private static MapLoader I;

        [Header("Setup Data")]
        public CellDatabase cellDatabase;

        [Header("Transition Events")]
        [SerializeField] private OnLoadStart OnLoadStart;
        [SerializeField] private OnLoadEnd OnLoadEnd;
        [SerializeField] private OnLoadProgress OnLoadProgress;

        // Public Accessors
        public static CellCoordinator LatestCoordinator => I.latestCoordinator;
        private CellCoordinator latestCoordinator;

        public static int ArtificialLoadDelay { get { return I.loadDelay; } set { I.loadDelay = value; } }
        private int loadDelay = 100;

        public static Scene NativeScene => I._nativeScene;
        private Scene _nativeScene;

        // Every cell that exists in game.
        private Dictionary<string, Cell> cells;

        // Every cell that loads will have a base counter of 1, any additional cells that require that cell add to its counter.
        // Unloading cells only decrease the counter untill it has a counter of zero.
        private List<CellRef> loadedCells;

        // Load Event Blocker
        private static bool allowRegisterEvents = true;

        // Internal Events
        public static Action<string, CellCoordinator> OnCellLoaded;

        // Codebased Events
        public static Action OnLoadingStart;
        public static Action OnLoadingEnd;
        public static Action<float> OnLoadingProgress;
        public static Action<List<Cell_ID>> OnLoadedCellsChanged;

        private void Awake()
        {
            #region Static Referencing
            if (I == null) I = this;
            else if (I != this) Destroy(gameObject);
            #endregion

            _nativeScene = SceneManager.GetActiveScene();

            cells = new Dictionary<string, Cell>();
            foreach (var cell in cellDatabase.cells)
                cells[cell.id.name] = cell;

            loadedCells = new List<CellRef>();

            // Register internal events.
            OnCellLoaded += RegisterCell;
        }

        private void OnDestroy()
        {
            // Unregister internal events.
            OnCellLoaded -= RegisterCell;
        }

        #region Helper Funcs
        private void SendLoadEvents(LoadEventType eventType, float val = 0f)
        {
            switch (eventType)
            {
                case LoadEventType.Start:
                    OnLoadStart?.Invoke();
                    OnLoadingStart?.Invoke();
                    break;
                case LoadEventType.End:
                    OnLoadEnd?.Invoke();
                    OnLoadingEnd?.Invoke();
                    break;
                case LoadEventType.Progress:
                    OnLoadProgress?.Invoke(val);
                    OnLoadingProgress?.Invoke(val);
                    break;
                default:
                    break;
            }
        }

        private void RegisterCell(string sceneName, CellCoordinator coordinator)
        {
            if (!loadedCells.Exists(X => X.scene.SceneName == sceneName))
            {
                Cell matchingCell = I.cellDatabase.cells.Find(X => X.scene.SceneName == sceneName);

                // Unload incompatible scenes.
                for (int i = loadedCells.Count - 1; i >= 0; i--)
                {
                    if (matchingCell.toleratedCells.Contains(loadedCells[i].cellID))
                    {
                        // Tolerated cell.
                        loadedCells[i].loadInfluence++;
                    }
                    else
                    {
                        // Untolerated cell.
                        var currentCell = loadedCells[i];
                        currentCell.loadInfluence -= 2;

                        if (currentCell.loadInfluence <= 0)
                        {
                            // unload it.
                            UnloadCell(currentCell.cellID.name);
                        }
                    }
                }

                // Cleans any remaining zero influence cells.
                CleanScenes();

                // Add newly loaded cell.
                loadedCells.Add(new CellRef(matchingCell.id, coordinator, matchingCell.scene, 1));

                // Sends event with list of loaded cells.
                BroadcastLoadedCells();
            }

            // TODO: Review implementation..
            if (allowRegisterEvents)
            {
                I.latestCoordinator = coordinator;
                I.SendLoadEvents(LoadEventType.End);
            }
        }

        /// <summary>
        /// Small follow up method that ensures any zero influence cells are unloaded.
        /// </summary>
        private static void CleanScenes()
        {
            for (int c = 0; c < I.loadedCells.Count; c++)
            {
                if (I.loadedCells[c].loadInfluence <= 0)
                    UnloadCell(I.loadedCells[c].cellID.name);
            }
        }

        public static void BroadcastLoadedCells()
        {
            var loadedList = new List<Cell_ID>();

            //Debug.Log("Cell list:");
            foreach (var cell in I.loadedCells)
            {
                loadedList.Add(cell.cellID);
                //Debug.Log($"Cell: {cell.cellID}");
            }

            OnLoadedCellsChanged?.Invoke(loadedList);
        }
        #endregion

        /// <summary>
        /// Will attempt to unload a cell.<br/>
        /// Reduces its load influence by 1 if unable to unload.
        /// </summary>
        /// <param name="cellID"></param>
        public static void UnloadCell(string cellID)
        {
            var cell = I.loadedCells.Find(X => X.cellID.name == cellID);
            cell.loadInfluence--;

            if (cell.loadInfluence <= 0)
            {
                I.loadedCells.Remove(cell);
                foreach (var tolCell in I.cellDatabase.cells.Find(X => X.id == cell.cellID).toleratedCells)
                {
                    for (int c = 0; c < I.loadedCells.Count; c++)
                    {
                        if (I.loadedCells[c].cellID == tolCell)
                        {
                            var cellDat = I.loadedCells[c];
                            cellDat.loadInfluence--;
                        }
                    }
                }

                // Unload scene.
                Debug.Log($"Unloading a cell: {cellID}");
                SceneManager.UnloadSceneAsync(cell.scene.ScenePath);
            }
        }

        /// <summary>
        /// Attempts to load a cell.<br/>
        /// LoadNoise determines load cell events (Loud = Use events || Silent = Don't use events).
        /// </summary>
        /// <param name="cell_ID"></param>
        /// <param name="loadNoise"></param>
        public static void LoadCell(Cell_ID cell_ID, LoadNoise loadNoise = LoadNoise.Loud)
        {
            LoadCell(cell_ID.name, loadNoise);
        }

        /// <summary>
        /// Attempts to load a cell.<br/>
        /// LoadNoise determines load cell events (Loud = Use events || Silent = Don't use events).
        /// </summary>
        /// <param name="cellId"></param>
        /// <param name="loadNoise"></param>
        public static async void LoadCell(string cellId, LoadNoise loadNoise = LoadNoise.Loud)
        {
            if (!I.cells.ContainsKey(cellId))
            {
                Debug.LogWarning($"Missing Cell: cell of id {cellId} does not exist!");
                return;
            }

            var cell = I.cells[cellId];

            // Run check to see if scene already loaded.
            if (I.loadedCells.Exists(X => X.cellID == cell.id))
            {
                Debug.LogWarning($"Load Ignored: {cell.id} is already loaded");

                // Temporarily unsubscribe from cell load event. 
                OnCellLoaded -= I.RegisterCell;

                // Simulate a successful scene load.
                OnCellLoaded?.Invoke(cell.scene.SceneName, I.loadedCells.Find(X => X.cellID == cell.id).coordinator);

                // Resubscribe to cell load event.
                OnCellLoaded += I.RegisterCell;

                I.OnLoadEnd?.Invoke();
                BroadcastLoadedCells();
                return;
            }

            var sceneAsync = SceneManager.LoadSceneAsync(cell.scene.ToString(), LoadSceneMode.Additive);

            // Load scene 
            if (loadNoise == LoadNoise.Silent)
                sceneAsync.allowSceneActivation = true;
            else
            {
                I.SendLoadEvents(LoadEventType.Start);
                sceneAsync.allowSceneActivation = false;
            }

            // Update scene loading progress bar.
            if (loadNoise == LoadNoise.Loud)
            {
                do
                {
                    // Artificial delay of 1 second so the load screen doesn't disappear too fast.
                    await Task.Delay(I.loadDelay);

                    // For loading bars..
                    I.SendLoadEvents(LoadEventType.Progress, sceneAsync.progress);

                } while (sceneAsync.progress < 0.9f);

                sceneAsync.allowSceneActivation = true;
            }
        }

        /// <summary>
        /// Attempts to load the main cell alongside the additional cells that are loaded silently.<br/>
        /// LoadNoise only affects the main cell loading routine.
        /// </summary>
        /// <param name="mainCell"></param>
        /// <param name="additionalCells"></param>
        /// <param name="loadNoise"></param>
        public static void LoadMultipleCells(Cell_ID mainCell, List<Cell_ID> additionalCells, LoadNoise loadNoise = LoadNoise.Loud)
        {
            var ids = new List<string>();
            foreach (var cell in additionalCells)
                ids.Add(cell.name);

            LoadMultipleCells(mainCell.name, ids, loadNoise);
        }

        /// <summary>
        /// Attempts to load the main cell alongside the additional cells that are loaded silently.<br/>
        /// LoadNoise only affects the main cell loading routine.
        /// </summary>
        /// <param name="mainCellID"></param>
        /// <param name="additionalCellIDs"></param>
        /// <param name="loadNoise"></param>
        public static void LoadMultipleCells(string mainCellID, List<string> additionalCellIDs, LoadNoise loadNoise = LoadNoise.Loud)
        {
            LoadCell(mainCellID, loadNoise);

            allowRegisterEvents = false;
            foreach (var cellID in additionalCellIDs)
            {
                var cell = I.cells[cellID];
                var sceneAsync = SceneManager.LoadSceneAsync(cell.scene.ToString(), LoadSceneMode.Additive);
                sceneAsync.allowSceneActivation = true;
            }
            allowRegisterEvents = true;
        }
    }
}