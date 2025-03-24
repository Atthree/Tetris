using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Counters")]
    [Range(0.01f, 1f)]
    [SerializeField] private float spawnSuresi = 0.5f; 
    private float spawnSayac;
    private float spawnLevelCounter;

    [Header("Managers")]
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private SpawnerManager spawnerManager;
    private ShapeManager activeShape;
    private ScoreManager scoreManager;
    public IconOpenClose rotateIcon;
    private FollowShapeManager followShape;
    private ShapeManager eldekiSekil;

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

    [SerializeField] private GameObject gameOverPanel;
    public bool rightDirec = true;
    public bool gameOver = false;
    public Image eldekiSekilImg;
    private bool isChange = true;

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
        if (scoreManager == null)
        {
            scoreManager = Object.FindFirstObjectByType<ScoreManager>();
            if (scoreManager == null) Debug.LogError("GameManager: ScoreManager bulunamadı!");
            else Debug.Log("GameManager: ScoreManager bulundu!");
        }
        if (followShape == null)
        {
            followShape = Object.FindFirstObjectByType<FollowShapeManager>();
            if (followShape == null) Debug.LogError("GameManager: followShape bulunamadı!");
            else Debug.Log("GameManager: FollowShape bulundu!");
        }

        if (spawnerManager != null && activeShape == null)
        {
            SpawnNewShape();
        }
        if(spawnerManager && eldekiSekil == null)
        {
            eldekiSekil = spawnerManager.CreateOtherShape();

            if (eldekiSekil.name == activeShape.name)
            {
                Destroy(eldekiSekil.gameObject);
                eldekiSekil = spawnerManager.CreateOtherShape();

                eldekiSekilImg.sprite = eldekiSekil.shape;
                eldekiSekil.gameObject.SetActive(false);
            }  
            else
            {
                eldekiSekilImg.sprite = eldekiSekil.shape;
                eldekiSekil.gameObject.SetActive(false);
            }
        }
        if (gameOverPanel)
        {
            gameOverPanel.SetActive(false);
        }

        // Başlangıç değerlerini ayarla
        spawnLevelCounter = spawnSuresi; // Otomatik düşme hızı başlangıçta spawnSuresi ile aynı olsun
        spawnSayac = Time.time + spawnSuresi;
        inputCounter = Time.time;
        inputTurnCounter = Time.time;
        inputDownCounter = Time.time;
    }

    private void Update()
    {
        if (!boardManager || !spawnerManager || !activeShape || gameOver || !scoreManager)
        {
            if (gameOver) Debug.Log("GameManager: Oyun bitti!");
            return;
        }

        Control();
    }
    private void LateUpdate()
    {
        if (followShape)
        {
            followShape.CreateFollowShape(activeShape, boardManager);
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
                if (rotateIcon)
                {
                    rotateIcon.CurrentIcon(rightDirec);
                }
            }
        }
        // Düzenleme: Manuel aşağı hareket ve otomatik düşme ayrıldı
        else if (Input.GetKey("down") && Time.time > inputDownCounter)
        {
            inputDownCounter = Time.time + inputDownTimer; // Manuel hızlandırma için ayrı zamanlayıcı
            Settled();
        }
        // Düzenleme: Otomatik düşme için ayrı kontrol
        else if (Time.time > spawnSayac)
        {
            spawnSayac = Time.time + spawnLevelCounter; // Otomatik düşme hızı spawnLevelCounter ile kontrol edilir
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

           isChange = true;

            if (spawnerManager)
            {
                SpawnNewShape();

                eldekiSekil = spawnerManager.CreateOtherShape();

                if (eldekiSekil.name == activeShape.name)
                {
                    Destroy(eldekiSekil.gameObject);
                    eldekiSekil = spawnerManager.CreateOtherShape();

                    eldekiSekilImg.sprite = eldekiSekil.shape;
                    eldekiSekil.gameObject.SetActive(false);
                }
                else
                {
                    eldekiSekilImg.sprite = eldekiSekil.shape;
                    eldekiSekil.gameObject.SetActive(false);
                }
            }

            if (followShape)
            {
                followShape.ResetFc();
            }

            boardManager.ClearAllLines();

            if (boardManager.complatedLine > 0)
            {
                scoreManager.LineScore(boardManager.complatedLine);
                if (scoreManager.isLevelPassed)
                {
                    SoundManager.Instance.PlaySFX(6);
                    // Düzenleme: Seviye ilerledikçe düşme hızını daha dengeli artır
                    // Açık lama: spawnLevelCounter, spawnSuresi'nden küçük bir miktar azaltılarak hız artırılır
                    spawnLevelCounter = Mathf.Max(spawnSuresi - ((int)scoreManager.level - 1) * 0.05f, 0.05f);
                    Debug.Log("New spawnLevelCounter: " + spawnLevelCounter);
                }
                else
                {
                    if (boardManager.complatedLine > 1)
                    {
                        SoundManager.Instance.VocalSounds();
                    }
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

        if (!boardManager.CurrentPosition(activeShape))
        {
            activeShape.CanTurnRight(!rightDirec);
            SoundManager.Instance.PlaySFX(0);
        }
        else
        {
            if (rotateIcon)
            {
                rotateIcon.CurrentIcon(rightDirec);
            }
            SoundManager.Instance.PlaySFX(0);
        }
    }

    public void ChangeHandleShape()
    {
        if (isChange)
        {
            isChange = false;
            activeShape.gameObject.SetActive(false);
            eldekiSekil.gameObject.SetActive(true);

            eldekiSekil.transform.position = activeShape.transform.position;

            activeShape = eldekiSekil;
        }
        if (followShape)
        {
            followShape.ResetFc();
        }

    }
}