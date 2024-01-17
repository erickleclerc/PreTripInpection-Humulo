using UnityEngine;
using System.Collections.Generic;
using Nova;

public class ProgressBarObject : MonoBehaviour
{
    private UIBlock2D loadingBar;
    private AudioSource audioSource;
    private float lastProgress = 0;

    public enum ProgressType
    {
        HorizontalFill,
        SpriteSwap
    }

    public ProgressType progressType; // The type of progress indicator

    public AudioClip loadingSound;
    public AudioClip loadedSound;
    public UIBlock2D reticle;
    public int reticleRotationSpeed = 1;

    public List<Sprite> progressSprites; // List of sprites for sprite swapping progress

    private void Awake()
    {
        loadingBar = GetComponent<UIBlock2D>();
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        if (reticle != null && progressType == ProgressType.SpriteSwap)
        {
            reticle.transform.Rotate(new Vector3(0, 0, 1), reticleRotationSpeed * Time.deltaTime);
        }
    }


    public void UpdateProgress(float progress)
    {
        Debug.Log("Progress: " + progress);
        Show();

        if (progress < lastProgress || lastProgress == 0) // This is the beginning of a new scan
        {
            if (loadingSound != null)
            {
                PlayProgressLoading();
            }
        }
        else if (progress >= 1) // Scan is complete
        {
            if (loadedSound != null)
            {
                PlayProgressComplete();
            }
            Hide();
        }

        if (loadingBar != null && progress < 1) // Scan is ongoing
        {
            if (progressType == ProgressType.HorizontalFill)
            {
                loadingBar.Size = Length3.Percentage(progress, 1, 1);
            }
            else if (progressType == ProgressType.SpriteSwap)
            {
                int spriteIndex = Mathf.FloorToInt(progress * progressSprites.Count);
                if( reticle != null )
                {
                    reticle.SetImage(progressSprites[spriteIndex]);
                }
            }
        }

        lastProgress = progress;
    }

    public void PlayProgressLoading()
    {
        if (loadingSound != null)
        {
            audioSource.PlayOneShot(loadingSound, 0.1f);
        }
    }

    public void PlayProgressComplete()
    {
        audioSource.PlayOneShot(loadedSound);
    }

    // Hide this object
    public void Hide()
    {
        lastProgress = 0;
        
        if (loadingBar != null)
        {
            loadingBar.BodyEnabled = false;
        }
    }

    // Show this object
    public void Show()
    {
        if (loadingBar != null)
        {
            loadingBar.BodyEnabled = true;
        }
    }
}
