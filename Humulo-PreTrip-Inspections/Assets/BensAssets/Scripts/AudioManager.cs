using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource; // Reference to the AudioSource component

    public AudioClip sightScanStartClip; // AudioClip for the sight scan start sound
    public AudioClip sightScanCompleteClip; // AudioClip for the sight scan complete sound

    public void Start()
    {
        // create a new audio source component and set it to the audioSource variable
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayScanStartAudio()
    {
        if (audioSource != null && sightScanStartClip != null)
        {
            audioSource.PlayOneShot(sightScanStartClip, 0.2f);
        }
    }

    public void PlayScanCompleteAudio()
    {
        if (audioSource != null && sightScanCompleteClip != null)
        {
            audioSource.PlayOneShot(sightScanCompleteClip, 0.7f);
        }
    }
}
