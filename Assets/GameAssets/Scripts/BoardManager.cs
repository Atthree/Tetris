using Unity.Mathematics;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private Transform tilePrefab;
    public int height = 22;
    public int width = 10;

    private Transform[,] grid;
    private void Awake()
    {
        grid = new Transform[width, height];
    }

    private void Start()
    {
        CreateEmptySquares();
    }
    bool InTheBoard(int x, int y)
    {
        return (x >= 0 && x < width && y >= 0);
    }
    bool KareDolumu(int x, int y, ShapeManager shape)
    {
        return (grid[x, y] != null && grid[x, y].parent != shape.transform);
    }
    public bool CurrentPosition(ShapeManager shape)
    {
        foreach (Transform child in shape.transform)
        {
            Vector2 pos = MakeInt(child.position);

            if (!InTheBoard((int)pos.x, (int)pos.y))
            {
                return false;
            }
            if (pos.y < height)
            {
                if (KareDolumu((int)pos.x, (int)pos.y, shape))
                {
                    return false;
                }
            }
        }
        return true;
    }
    private void CreateEmptySquares()
    {
        if (tilePrefab != null)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Transform tile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                    tile.parent = this.transform;
                }
            }
        }
        else
        {
            Debug.Log("Tile Prefab null");
        }

    }
    public void PutShapeInGrid(ShapeManager shape)
    {
        if (!shape) { return; }

        foreach (Transform child in shape.transform)
        {
            Vector2 pos = MakeInt(child.position);
            grid[(int)pos.x, (int)pos.y] = child;
        }
    }
    Vector2 MakeInt(Vector2 vector)
    {
        return new Vector2(Mathf.Round(vector.x), Mathf.Round(vector.y));
    }
}
