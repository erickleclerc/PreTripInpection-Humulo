using UnityEngine;
using UnityEngine.UI;
using HurricaneVR.Framework.Core.Player;

public class UIAdvanceDebug : MonoBehaviour
{
    public GameObject objectToDisable; // The GameObject to disable
    public GameObject objectToEnable; // The GameObject to enable
    public Transform newPlayerLocation; // The new location for the player controller
    public GameObject XRRig; // Reference to the XRRig GameObject
    public float fadeDuration = 1f; // Duration of the fade effect

    private HVRCanvasFade canvasFade; // Reference to the HVRCanvasFade component

    private void Start()
    {
        
        // Get the HVRCanvasFade component
        canvasFade = FindObjectOfType<HVRCanvasFade>();
    }

    public void AdvanceStep()
    {
        
        // Enable the objectToEnable
        objectToEnable.SetActive(true);

        // Move the XRRig to the new location
        if (XRRig != null && newPlayerLocation != null)
        {
            StartCoroutine(TransitionCamera());
            XRRig.transform.position = newPlayerLocation.position;
        }
        else
        {
            Debug.LogWarning("XRRig or new player location not assigned.");
        }

        // Disable the objectToDisable
        objectToDisable.SetActive(false);

    }

    private System.Collections.IEnumerator TransitionCamera()
    {
        float elapsedTime = 0f;
        float startFade = 0f;
        float endFade = 1f;
        
        if (canvasFade == null)
        {
            Debug.Log("No canvas fade found");
        }

        // Fade out
        while (elapsedTime < fadeDuration)
        {
            // Calculate the current fade value based on the elapsed time and fade duration
            float currentFade = Mathf.Lerp(startFade, endFade, elapsedTime / fadeDuration);

            // Update the fade value using the HVRCanvasFade component
            if (canvasFade != null)
                canvasFade.UpdateFade(currentFade);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the fade reaches exactly the end value at the end of the fade duration
        if (canvasFade != null)
            canvasFade.UpdateFade(endFade);

        // Delay for a brief moment
        yield return new WaitForSeconds(0.5f);

        elapsedTime = 0f;
        startFade = 1f;
        endFade = 0f;

        // Fade in
        while (elapsedTime < fadeDuration)
        {
            // Calculate the current fade value based on the elapsed time and fade duration
            float currentFade = Mathf.Lerp(startFade, endFade, elapsedTime / fadeDuration);

            // Update the fade value using the HVRCanvasFade component
            if (canvasFade != null)
                canvasFade.UpdateFade(currentFade);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the fade reaches exactly the end value at the end of the fade duration
        if (canvasFade != null)
            canvasFade.UpdateFade(endFade);
    }
}
