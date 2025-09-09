using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [Header("Grid")]
    public float cellSize = 1f;
    public Vector2 origin = Vector2.zero;

    private readonly HashSet<Vector2Int> occupied = new HashSet<Vector2Int>();

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    // World <-> Cell
    public Vector2Int WorldToCell(Vector3 world)
    {
        Vector2 p = (Vector2)world - origin;
        int x = Mathf.RoundToInt(p.x / cellSize);
        int y = Mathf.RoundToInt(p.y / cellSize);
        return new Vector2Int(x, y);
    }

    public Vector3 CellToWorld(Vector2Int cell)
    {
        Vector2 w = origin + new Vector2(cell.x * cellSize, cell.y * cellSize);
        return new Vector3(w.x, w.y, 0f);
    }

    // Occupancy
    public bool IsFree(Vector2Int cell) => !occupied.Contains(cell);

    // Reserve tr??c khi di chuy?n ?? không 2 th?ng cùng chui vào 1 ô trong cùng 1 step
    public bool TryReserve(Vector2Int cell)
    {
        if (occupied.Contains(cell)) return false;
        occupied.Add(cell);
        return true;
    }

    public void Release(Vector2Int cell)
    {
        occupied.Remove(cell);
    }
}
