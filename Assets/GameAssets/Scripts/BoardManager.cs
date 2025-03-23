using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Header("Board Settings")]
    [SerializeField] private Transform tilePrefab;
    [SerializeField] public int height = 22;
    [SerializeField] public int width = 10;

    private Transform[,] grid;

    private void Awake()
    {
        grid = new Transform[width, height];
    }

    private void Start()
    {
        CreateEmptySquares();
    }

    private bool IsWithinBoard(int x, int y)
    {

        return x >= 0 && x < width && y >= 0; 
    }

    private bool IsSquareOccupied(int x, int y, ShapeManager shape)
    {
        return grid[x, y] != null && grid[x, y].parent != shape.transform;
    }

    public bool CurrentPosition(ShapeManager shape)
    {
        if (!shape) return false;

        foreach (Transform child in shape.transform)
        {
            Vector2 pos = RoundToInt(child.position);

            if (!IsWithinBoard((int)pos.x, (int)pos.y))
            {
                return false;
            }

            if (pos.y < height && IsSquareOccupied((int)pos.x, (int)pos.y, shape))
            {
                return false;
            }
        }
        return true;
    }

    private void CreateEmptySquares()
    {
        if (tilePrefab == null)
        {
            Debug.LogError("BoardManager: Tile Prefab atanmamış!");
            return;
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Transform tile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity, transform);
                tile.name = $"Tile_{x}_{y}"; // Hata ayıklama için isimlendirme
            }
        }
    }

    public void PutShapeInGrid(ShapeManager shape)
    {
        if (!shape) return;

        foreach (Transform child in shape.transform)
        {
            Vector2 pos = RoundToInt(child.position);
            int x = (int)pos.x;
            int y = (int)pos.y;

            if (IsWithinBoard(x, y))
            {
                grid[x, y] = child;
            }
            else
            {
                Debug.LogWarning($"BoardManager: Şekil tahta dışında ({x}, {y}) - Atlanıyor!");
            }
        }
    }

    private bool LineCompleted(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }
        return true;
    }

    private void ClearLine(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] != null)
            {
                Destroy(grid[x, y].gameObject);
                grid[x, y] = null;
            }
        }
    }

    private void DownOneLine(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].position = new Vector3(x, y - 1, 0); // Pozisyonu güncelle
            }
        }
    }

    private void DownAllLines(int startY)
    {
        for (int y = startY; y < height; y++)
        {
            DownOneLine(y);
        }
    }

    public void ClearAllLines()
    {
        for (int y = 0; y < height; y++)
        {
            if (LineCompleted(y))
            {
                ClearLine(y);
                DownAllLines(y + 1);
                y--; // Aynı satırı tekrar kontrol et
            }
        }
    }

    public bool SpillOut(ShapeManager shape)
    {
        if (!shape) return false;

        foreach (Transform child in shape.transform)
        {
            if (child.transform.position.y >= height-1) // Tahtanın üst sınırını aştıysa
            {
                return true;
            }
        }
        return false;
    }

    private Vector2 RoundToInt(Vector2 vector)
    {
        return new Vector2(math.round(vector.x), math.round(vector.y)); // Unity.Mathematics kullanıyoruz
    }

    // Hafızayı temizlemek için (isteğe bağlı)
    private void OnDestroy()
    {
        if (grid != null)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (grid[x, y] != null)
                    {
                        Destroy(grid[x, y].gameObject);
                    }
                }
            }
        }
    }
}