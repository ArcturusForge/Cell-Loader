using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CellDatabase", menuName = "New CellDatabase")]
public class CellDatabase : ScriptableObject
{
    public List<Cell> cells;
}

[System.Serializable]
public class Cell
{
    public string id;
    public GameObject cellPrefab;
}
