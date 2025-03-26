using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Transform mainMenu, settingsMenu;
    [SerializeField] AudioSource musicSource;
    [SerializeField] private Slider musicBackSlider, fxSlider;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1f);
            musicBackSlider.value = PlayerPrefs.GetFloat("musicVolume");
        }
        else
        {
            musicBackSlider.value = PlayerPrefs.GetFloat("musicVolume");
        }
        fxSlider.value = 1f;
    }

    public void SettingsMenuAc()
    {
        mainMenu.GetComponent<RectTransform>().DOLocalMoveX(-1200, .5f);
        settingsMenu.GetComponent<RectTransform>().DOLocalMoveX(0f, .5f);
    }
    public void SettingsMenuKapat()
    {
        mainMenu.GetComponent<RectTransform>().DOLocalMoveX(0f, .5f);
        settingsMenu.GetComponent<RectTransform>().DOLocalMoveX(1200f, .5f);
    }
    public void GamePlayFnc()
    {
        SceneManager.LoadScene("GamePlay");
    }
    public void BackgrondVolumeChange()
    {
        musicSource.volume = musicBackSlider.value;
        PlayerPrefs.SetFloat("musicVolume", musicBackSlider.value);
    }

    public void FXVolumeDegistir()
    {
        PlayerPrefs.SetFloat("FxVolume", fxSlider.value);
    }
}
