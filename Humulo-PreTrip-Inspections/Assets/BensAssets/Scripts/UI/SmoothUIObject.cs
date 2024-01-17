using UnityEngine;
using System.Collections;

public class SmoothUIObject : MonoBehaviour
{
    public Transform playerHead;
    public float UIZDistance = 0.6f;
    public float verticalOffset = 0.5f;
    public float horizontalOffset = 0f;
    public float smoothSpeed = 10f;
    public float dampIntensity = 0.1f;
    public float rotationOffset = 0f;
    public bool lockZRotation = false;
    public float UIPositionThreshold = 0.001f;
    public float rotationThreshold = 0.001f;
    public bool smoothNow = true;
    public bool freezeLocation;

    private Vector3 currentVelocity;

    private Quaternion lastSmoothedRotation;
    private Quaternion initialRotation;
    private Coroutine smoothCoroutine;

    private void Awake()
    {
        // assign player head to the main camera in the scene
        if (playerHead == null)
        {
            playerHead = Camera.main.transform;
        }
        initialRotation = playerHead.rotation;
        lastSmoothedRotation = initialRotation;
        MoveUI();
       
    }

    private void Update()
    {
        // If the UI object is frozen in place, don't update it
        if (freezeLocation)
        {
            return;
        }
        
        // Check if the camera has moved or rotated beyond the threshold
        if (HasCameraMovedBeyondThreshold())
        {
            // Restart the smooth coroutine if it's already running
            if (smoothCoroutine != null)
            {
                StopCoroutine(smoothCoroutine);
            }

            StartSmoothCoRoutine();
        }
        else if (smoothNow) //  Debug feature to allow for triggering of the smooth coroutine
        {
            smoothNow = false;
            StartCoroutine(SmoothUI());
        }
      
    }

    public void StartSmoothCoRoutine()
    {
        smoothCoroutine = StartCoroutine(SmoothUI());
    }

    // Move this UI object to the correct position with respect to the player's head using smoothing and dampening
    private IEnumerator SmoothUI()
    {
        // do nothing if freeze location is true
        if (freezeLocation)
        {
            yield break;
        }
        
        while (true)
        {
            // Calculate the target position for the UI object
            Vector3 targetPosition = GetUIMovePosition();

            // Apply smoothing to the UI object position
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, dampIntensity);

            // Move the UI object towards the smoothed position
            transform.position = Vector3.MoveTowards(transform.position, smoothedPosition, Time.deltaTime * smoothSpeed);

            // Calculate the target rotation for the UI object
            Quaternion targetRotation = GetUIMoveRotation();

            // Apply smoothing to the UI object rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);

            // Update the last smoothed rotation
            lastSmoothedRotation = transform.rotation;

            // Check if the UI object has reached the target position
            if (Vector3.Distance(transform.position, targetPosition) < UIPositionThreshold)
            {
                yield break; // Exit the coroutine
            }

            yield return null;
        }
    }

    // Instant version of SmoothUI - no smoothing
    public void MoveUI()
    {
        if (!freezeLocation)
        {
            Vector3 targetPosition = GetUIMovePosition();
            Quaternion targetRotation = GetUIMoveRotation();
            transform.position = targetPosition;
            transform.rotation = targetRotation;
        }
    }

    private Quaternion GetUIMoveRotation()
    {
        Quaternion targetRotation = Quaternion.LookRotation(playerHead.position - transform.position, playerHead.up) *
                Quaternion.Euler(0f, rotationOffset, 0f);
        // Lock Z rotation if required
        if (lockZRotation)
        {
            targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, initialRotation.z);
        }
        return targetRotation;
    }

    private Vector3 GetUIMovePosition()
    {
        Vector3 targetPosition = playerHead.position + playerHead.forward * UIZDistance
                + playerHead.up * verticalOffset + playerHead.right * horizontalOffset;
        return targetPosition;
    }


    private bool HasCameraMovedBeyondThreshold()
    {
        // Calculate the rotation difference between the current and last smoothed rotation
        Quaternion rotationDifference = Quaternion.Inverse(lastSmoothedRotation) * playerHead.rotation;

        // Check if the rotation difference exceeds the threshold
        bool hasRotated = Quaternion.Angle(rotationDifference, Quaternion.identity) > rotationThreshold;

        return hasRotated;
    }

    public void SetFreezeLocation(bool freeze)
    {
        freezeLocation = freeze;
    }   

}
