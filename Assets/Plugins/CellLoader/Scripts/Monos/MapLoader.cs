using Arcturus.MapLoader.Internal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        public static int ArtificialLoadDelay { get { return I.loadDelay; } set { I.loadDelay = value; } }
        private int loadDelay = 100;

        public static Scene NativeScene => I._nativeScene;
        private Scene _nativeScene;

        // Every cell that exists in game.
        private Dictionary<string, Cell> cells;

        // Every cell that loads will have a base counter of 1, any additional cells that require that cell add to its counter.
        // Unloading cells only decrease the counter untill it has a counter of zero.
        private List<CellRef> loadedCells;

        // Internal Events
        public static Action<string, CellCoordinator> OnCellLoaded;

        // Codebased Events
        public static Action OnLoadingStart;
        public static Action OnLoadingEnd;
        public static Action<float> OnLoadingProgress;

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
            OnCellLoaded += I.RegisterCell;
        }

        private void OnDestroy()
        {
            // Unregister internal events.
            OnCellLoaded -= I.RegisterCell;
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
                        var currentCell = loadedCells[i];
                        currentCell.loadInfluence++;
                    }
                    else
                    {
                        // Untolerated cell.
                        var currentCell = loadedCells[i];
                        currentCell.loadInfluence--;

                        if (currentCell.loadInfluence <= 0)
                        {
                            // unload it.
                            Debug.Log($"Unloading a cell: {currentCell.cellID}");
                            UnloadCell(currentCell.cellID.name);
                        }
                    }
                }

                // Cleans any remaining zero influence cells.
                CleanScenes();

                // Add newly loaded cell.
                loadedCells.Add(new CellRef(matchingCell.id, coordinator, matchingCell.scene, 1));
            }

            // TODO: Review implementation..
            I.SendLoadEvents(LoadEventType.End);
        }

        /// <summary>
        /// Small follow up method that ensures any zero influence cells are unloaded.
        /// </summary>
        private void CleanScenes()
        {
            for (int c = 0; c < I.loadedCells.Count; c++)
            {
                if (I.loadedCells[c].loadInfluence <= 0)
                    UnloadCell(I.loadedCells[c].cellID.name);
            }
        }
        #endregion

        /// <summary>
        /// Will attempt to unload a cell.<br/>
        /// Reduces its load influence by 1 if unable to unload.
        /// </summary>
        /// <param name="cellId"></param>
        public static void UnloadCell(string cellId)
        {
            var cell = I.loadedCells.Find(X => X.cellID.name == cellId);
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
                SceneManager.UnloadSceneAsync(cell.scene.ScenePath);
            }
        }

        public static void LoadCell(Cell_ID cell_ID, LoadNoise loadNoise = LoadNoise.Loud)
        {
            LoadCell(cell_ID.name, loadNoise);
        }

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
    }
}

namespace Arcturus.MapLoader.Internal
{
    public enum LoadEventType { Start, End, Progress }
}