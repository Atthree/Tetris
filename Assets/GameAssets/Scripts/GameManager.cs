using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Counters")]
    [Range(0.01f,1f)]
    [SerializeField] private float spawnTime = .5f;
    private float spawnCounter;
    BoardManager boardManager;
    SpawnerManager spawnerManager;

    private ShapeManager activeShape;

    private void Start()
    {
        boardManager = GameObject.FindAnyObjectByType<BoardManager>();
        spawnerManager = GameObject.FindAnyObjectByType<SpawnerManager>();

        if (spawnerManager)
        {
            if (!activeShape)
            {
                activeShape = spawnerManager.CreateShape();
                activeShape.transform.position = MakeInt(activeShape.transform.position);
            }
        }
    }
    private void Update()
    {
        if (!boardManager || !spawnerManager)
        {
            return;
        }
        if (Time.time > spawnCounter)
        {
            spawnCounter = Time.time + spawnTime;
            if (activeShape)
            {
                activeShape.MoveDown();

                if(!boardManager.CurrentPosition(activeShape))
                {
                    activeShape.MoveUp();
                    boardManager.PutShapeInGrid(activeShape);
                    if(spawnerManager)
                    {
                        activeShape = spawnerManager.CreateShape();
                    }
                }
            }

        }

    }

    Vector2 MakeInt(Vector2 vector)
    {
        return new Vector2(Mathf.Round(vector.x), Mathf.Round(vector.y));
    }
}
