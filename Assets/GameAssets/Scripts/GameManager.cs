using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Counters")]
    [Range(0.01f, 1f)]
    [SerializeField] private float spawnSuresi = 0.5f;
    private float spawnSayac;

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

    public IconOpenClose rotateIcon;
    [SerializeField] private GameObject gameOverPanel;
    public bool rightDirec = true;
    public bool gameOver = false;

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
        if (gameOverPanel)
        {
            gameOverPanel.SetActive(false);
        }

        spawnSayac = Time.time + spawnSuresi;
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

        if (Time.time > spawnSayac)
        {
            spawnSayac = Time.time + spawnSuresi;
            if(activeShape)
            {
                Settled();
            }
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
                SoundManager.Instance.PlaySFX(1);
                activeShape.MoveLeft();
            }
            else
            {
                SoundManager.Instance.PlaySFX(2);
            }
        }
        else if ((Input.GetKey("left") && Time.time > inputCounter) || Input.GetKeyDown("left"))
        {
            activeShape.MoveLeft();
            inputCounter = Time.time + inputTimer;
            if (!boardManager.CurrentPosition(activeShape))
            {
                SoundManager.Instance.PlaySFX(1);
                activeShape.MoveRight();
            }
            else
            {
                SoundManager.Instance.PlaySFX(2);
            }
        }
        else if (Input.GetKeyDown("up") && Time.time > inputTurnCounter)
        {
            activeShape.TurnRight();
            inputTurnCounter = Time.time + inputTurnTimer;
            if (!boardManager.CurrentPosition(activeShape))
            {
                SoundManager.Instance.PlaySFX(0);
                activeShape.TurnLeft();
            }
            else
            {
                rightDirec = !rightDirec;
                SoundManager.Instance.PlaySFX(0);
                if(rotateIcon)
                {
                    rotateIcon.CurrentIcon(rightDirec);
                }
            }
        }
        else if (Input.GetKey("down") && Time.time > inputDownCounter) 
        {
            spawnSayac = Time.time + spawnSuresi;
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
            SoundManager.Instance.PlaySFX(4);
            if (spawnerManager)
            {
                SpawnNewShape();
            }
            boardManager.ClearAllLines();
            
            if(boardManager.complatedLine > 0)
            {                
                if(boardManager.complatedLine > 1)
                {
                    SoundManager.Instance.VocalSounds();
                }
                SoundManager.Instance.PlaySFX(3);
            }

            if (activeShape != null)
            {                
                activeShape.MoveDown();
                if (!boardManager.CurrentPosition(activeShape))
                {
                    activeShape.MoveUp();
                    if (boardManager.SpillOut(activeShape))
                    {
                        Debug.Log("Game Over");
                        SoundManager.Instance.PlaySFX(5);
                        gameOver = true;
                        if (gameOverPanel)
                        {
                            gameOverPanel.SetActive(true);
                            SoundManager.Instance.PlaySFX(5);
                        }
                        Debug.Log("GameManager: Şekil tahtayı aştı, oyun bitti!");
                    }
                }
                else
                {
                    activeShape.MoveUp(); 
                }
            }
        }
    }

    private void SpawnNewShape()
    {
        activeShape = spawnerManager.CreateShape();
        if (activeShape != null)
        {
            Vector2 spawnPos = new Vector2(boardManager.width / 2f, boardManager.height - 1);
            activeShape.transform.position = RoundToInt(spawnPos);
            Debug.Log("GameManager: Yeni şekil spawn edildi: " + activeShape.name + " at " + activeShape.transform.position);

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

    public void RotationIcon()
    {
        rightDirec = !rightDirec;

        activeShape.CanTurnRight(rightDirec);

        if(!boardManager.CurrentPosition(activeShape))
        {
            activeShape.CanTurnRight(!rightDirec);
            SoundManager.Instance.PlaySFX(0);
        }
        else
        {
            if(rotateIcon)
            {
                rotateIcon.CurrentIcon(rightDirec);
            }
            SoundManager.Instance.PlaySFX(0);
        }
    }

}