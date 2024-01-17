using Nova;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    public int currentTrackIndex = 0;
    public bool isPlaying = false;
    public UIBlock2D playButton;
    public UIBlock2D pauseButton;

    void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Start()
    {
        if (isPlaying)
        {
            PlayCurrentTrack();
        }
        
    }

    void Update()
    {
        if (!audioSource.isPlaying && isPlaying)
        {
            PlayCurrentTrack();
        }
        
    }

    public void TogglePlayPause()
    {
        if (audioSource.isPlaying)
        {
            Pause();
        }
        else
        {
            Play();
        }
    }

    public void Play()
    {
        isPlaying = true;
        audioSource.UnPause();
        playButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);
    }

    public void Pause()
    {
        isPlaying = false;
        audioSource.Pause();
        playButton.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);
    }

    public void PlayNextTrack()
    {
        currentTrackIndex = (currentTrackIndex + 1) % audioClips.Length;
        PlayCurrentTrack();
    }

    public void PlayPreviousTrack()
    {
        currentTrackIndex = (currentTrackIndex - 1 + audioClips.Length) % audioClips.Length;
        PlayCurrentTrack();
    }

    private void PlayCurrentTrack()
    {
        
        audioSource.Stop();
        audioSource.clip = audioClips[currentTrackIndex];
        audioSource.Play();
    }
}
