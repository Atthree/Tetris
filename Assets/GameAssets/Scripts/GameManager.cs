using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Counters")]
    [Range(0.01f, 1f)]
    [SerializeField] private float moveTime = 0.5f;
    private float moveCounter;

    [Header("Managers")]
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private SpawnerManager spawnerManager;
    private ShapeManager activeShape;

    [Header("Input Timers")]
    [Range(0.02f, 1f)]
    [SerializeField] private float inputTimer = 0.25f;
    private float inputCounter;
    [Range(0.02f, 1f)]
    [SerializeField] private float inputTurnTimer = 0.25f;
    private float inputTurnCounter;
    [Range(0.02f, 1f)]
    [SerializeField] private float inputDownTimer = 0.25f;
    private float inputDownCounter;

    private bool gameOver = false;

    private void Start()
    {
        if (boardManager == null)
        {
            boardManager = Object.FindFirstObjectByType<BoardManager>();
            if (boardManager == null) Debug.LogError("GameManager: BoardManager bulunamadı!");
            else Debug.Log("GameManager: BoardManager bulundu!");
        }

        if (spawnerManager == null)
        {
            spawnerManager = Object.FindFirstObjectByType<SpawnerManager>();
            if (spawnerManager == null) Debug.LogError("GameManager: SpawnerManager bulunamadı!");
            else Debug.Log("GameManager: SpawnerManager bulundu!");
        }

        if (spawnerManager != null && activeShape == null)
        {
            SpawnNewShape();
        }

        moveCounter = Time.time + moveTime;
        inputCounter = Time.time;
        inputTurnCounter = Time.time;
        inputDownCounter = Time.time;
    }

    private void Update()
    {
        if (!boardManager || !spawnerManager || !activeShape || gameOver)
        {
            if (gameOver) Debug.Log("GameManager: Oyun bitti!");
            return;
        }

        Control();

        if (Time.time >= moveCounter)
        {
            moveCounter = Time.time + moveTime;
            Settled();
        }
    }

    private void Control()
    {
        if ((Input.GetKey("right") && Time.time > inputCounter) || Input.GetKeyDown("right"))
        {
            activeShape.MoveRight();
            inputCounter = Time.time + inputTimer;
            if (!boardManager.CurrentPosition(activeShape))
            {
                activeShape.MoveLeft();
            }
        }
        else if ((Input.GetKey("left") && Time.time > inputCounter) || Input.GetKeyDown("left"))
        {
            activeShape.MoveLeft();
            inputCounter = Time.time + inputTimer;
            if (!boardManager.CurrentPosition(activeShape))
            {
                activeShape.MoveRight();
            }
        }
        else if (Input.GetKeyDown("up") && Time.time > inputTurnCounter)
        {
            activeShape.TurnRight();
            inputTurnCounter = Time.time + inputTurnTimer;
            if (!boardManager.CurrentPosition(activeShape))
            {
                activeShape.TurnLeft();
            }
        }
        else if (Input.GetKey("down") && Time.time > inputDownCounter)
        {
            moveCounter = Time.time + moveTime;
            inputDownCounter = Time.time + inputDownTimer;
            Settled();
        }
    }

    private void Settled()
    {
        activeShape.MoveDown();

        if (!boardManager.CurrentPosition(activeShape))
        {
            activeShape.MoveUp();
            boardManager.PutShapeInGrid(activeShape);
            boardManager.ClearAllLines();

            SpawnNewShape();

            // Oyun sonu kontrolü
            if (activeShape != null && boardManager.SpillOut(activeShape))
            {
                gameOver = true;
                Debug.Log("GameManager: Şekil tahtayı aştı, oyun bitti!");
            }
        }
    }

    private void SpawnNewShape()
    {
        activeShape = spawnerManager.CreateShape();
        if (activeShape != null)
        {
            // Tahtanın üstünden biraz aşağıda spawn et
            Vector2 spawnPos = new Vector2(boardManager.width / 2f, boardManager.height - 2);
            activeShape.transform.position = RoundToInt(spawnPos);
            Debug.Log("GameManager: Yeni şekil spawn edildi: " + activeShape.name + " at " + activeShape.transform.position);

            // Şeklin pozisyonu başlangıçta geçersizse uyarı ver ama oyun bitmesin
            if (!boardManager.CurrentPosition(activeShape))
            {
                Debug.LogWarning("GameManager: Yeni şekil tahtada geçersiz bir pozisyonda spawn edildi!");
            }
        }
        else
        {
            Debug.LogWarning("GameManager: Yeni şekil oluşturulamadı!");
        }
    }

    private Vector2 RoundToInt(Vector2 vector)
    {
        return new Vector2(Mathf.Round(vector.x), Mathf.Round(vector.y));
    }
}