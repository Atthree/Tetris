using System.Security;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioClip[] musicClip;
    [SerializeField] private AudioSource musicSource;

    public bool playMusic = true;

    private AudioClip randomMusicClip;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        randomMusicClip = RandomClip(musicClip);
        BackgroundMusic(randomMusicClip);
    }

    AudioClip RandomClip(AudioClip[] clips)
    {
        AudioClip randomClip = clips[Random.Range(0, clips.Length)];
        return randomClip;
    }

    public void BackgroundMusic(AudioClip musicClip)
    {
        if (!musicClip || !musicSource || !playMusic)
        {
            return;
        }
        musicSource.clip = musicClip;
        musicSource.Play();
    }
    void UpdateMusic()
    {
        if(musicSource.isPlaying != playMusic)
        {
            if(playMusic)
            {
                randomMusicClip = RandomClip(musicClip);
                BackgroundMusic(randomMusicClip);
            }
            else
            {
                musicSource.Stop();
            }
        }
    }

    public void OnOffMusic()
    {
        playMusic = !playMusic;
        UpdateMusic();
    }
}
