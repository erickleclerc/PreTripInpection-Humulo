using Nova;
using UnityEngine;
using TMPro;

public class VerticalBillboard : MonoBehaviour
{
    public Transform CameraToTarget;
    public float initialHorizontalOffset = -0.5f;
    public float moveSpeed = 1f;
    public float smoothTime = 0.1f;
    public float minDistanceThreshold = 2f;
    public float maxDistanceThreshold = 10f;

    public TextBlock textBlockToScale;
    public float minTextSize = 0.3f;
    public float maxTextSize = 1f;

    private Vector3 initialLocalPosition;
    private Vector3 currentVelocity;
    private Vector3 initialHorizontalDirection;

    private void Start()
    {
        initialLocalPosition = transform.localPosition;

        if (CameraToTarget == null)
        {
            CameraToTarget = Camera.main.transform;
        }

        if (CameraToTarget != null)
        {
            initialHorizontalDirection = directionToCamera(transform);
        }
    }

    private void Update()
    {
        if (CameraToTarget != null)
        {
            Vector3 targetPosition = new Vector3(CameraToTarget.position.x, transform.position.y, CameraToTarget.position.z);
            transform.LookAt(targetPosition, Vector3.up);

            float distanceToCamera = Vector3.Distance(CameraToTarget.position, transform.position);
            float normalizedDistance = Mathf.Clamp01((distanceToCamera - maxDistanceThreshold) / (minDistanceThreshold - maxDistanceThreshold));
            float adjustedOffset = Mathf.Lerp(initialHorizontalOffset, 0f, normalizedDistance);
            // Lerp the text size
            float adjustedTextSize = Mathf.Lerp(maxTextSize, minTextSize, normalizedDistance);

            Vector3 horizontalDirection = directionToCamera(transform);
            Vector3 offset = horizontalDirection * adjustedOffset;
            // Set the font size of the text block
            textBlockToScale.TMP.fontSize = adjustedTextSize;

            Vector3 targetLocalPosition = initialLocalPosition + offset;
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetLocalPosition, ref currentVelocity, smoothTime);
        }
    }

    private Vector3 directionToCamera(Transform origin)
    {
        Vector3 direction = CameraToTarget.position - origin.position;
        Vector3 horizontalDirection = new Vector3(direction.x, 0f, direction.z).normalized;
        return -horizontalDirection;
    }
}
