using UnityEngine;

public class UILineRenderer : MonoBehaviour
{
    public Transform startPoint; // The starting point object
    public Transform endPoint; // The ending point object
    public Color lineColor = Color.white; // Color of the line

    private LineRenderer lineRenderer;

    private void Start()
    {
        // Create and configure the LineRenderer component
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // Set the positions of the line renderer
        Vector3[] positions = { startPoint.position, endPoint.position };
        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(positions);
    }
}
