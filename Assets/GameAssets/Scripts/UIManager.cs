using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public bool isPaused = false;
    public GameObject pausePanel;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = Object.FindFirstObjectByType<GameManager>();
    }
    private void Start()
    {
        if(pausePanel)
        {
            pausePanel.SetActive(false);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenClosePausePanel();
        }
    }

    public void OpenClosePausePanel()
    {
        if(gameManager.gameOver)
        {
            return;
        }

        isPaused = !isPaused;

        if (pausePanel)
        {
            pausePanel.SetActive(isPaused);
            if(SoundManager.Instance)
            {
                SoundManager.Instance.PlaySFX(0);
                Time.timeScale = isPaused ? 0 : 1;
            }
        }
    }

    public void TryAgainButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
