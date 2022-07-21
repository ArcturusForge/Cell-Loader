using DevLocker.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Arcturus.MapLoader.Internal
{
    [CreateAssetMenu(fileName = "CellDatabase", menuName = "New CellDatabase")]
    public class CellDatabase : ScriptableObject
    {
        public List<Cell> cells;
    }

    [System.Serializable]
    public class Cell
    {
        public Cell_ID id;
        public SceneReference scene;
        public List<Cell_ID> toleratedCells;
    }
}
