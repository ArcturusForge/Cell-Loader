using DevLocker.Utils;

namespace Arcturus.MapLoader.Internal
{
    [System.Serializable]
    public class CellRef
    {
        public Cell_ID cellID;
        public CellCoordinator coordinator;
        public SceneReference scene;
        public int loadInfluence;

        public CellRef(Cell_ID cellID, CellCoordinator coordinator, SceneReference scene, int loadWeight)
        {
            this.cellID = cellID;
            this.coordinator = coordinator;
            this.scene = scene;
            this.loadInfluence = loadWeight;
        }
    }
}

