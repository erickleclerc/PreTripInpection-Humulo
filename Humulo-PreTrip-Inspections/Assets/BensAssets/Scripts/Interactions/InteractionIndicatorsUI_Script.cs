using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nova;
using static StepManager;

public class InteractionIndicatorsUI_Script : MonoBehaviour
{
    public Camera cam;
    public float cameraCastRadius = 1f;
    public float cameraCastDistance = 10f;
    public float smooth;
    public float minDistanceToObject;

    public GameObject sightIndicator, grabIndicator;
    public RectTransform canvasRectTransform;

    public LayerMask excludedLayers;

    public StepManager stepManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cam.transform.position);
        transform.Rotate(new Vector3(0, 180, 0));

        // Create a layer mask that includes all layers except the excluded ones
        LayerMask includedLayers = ~excludedLayers;


        // Show Aim indicator if the current step is a sight interaction
        if (stepManager != null)
        {
            if (stepManager.currentStep < stepManager.stepSequence.Length) // Check if the current step is within the bounds of the step sequence
            {
                if (stepManager.stepSequence[stepManager.currentStep].interactionType == Interactions.sight) // Check if the current step is a sight interaction
                {
                    sightIndicator.SetActive(true);
                }
                else
                {
                    sightIndicator.SetActive(false);
                }
            }
            else
            {
                Debug.LogWarning("Current step is out of bounds");
            }
        }
        else
        {
            Debug.LogError("Step Manager not found");
        }



        RaycastHit hit;
        // Does the ray intersect any objects  on the grabbable layer
        if(Physics.SphereCast(cam.transform.position, cameraCastRadius, cam.transform.forward, out hit, cameraCastDistance, includedLayers))
        {
            if (hit.collider.gameObject.layer == 20 && hit.collider.gameObject.GetComponent<InteractableObject>() != null) // Assuming layer 20 is the grabbable layer
            {
                //Debug.Log("Grabbable on sight");


                Vector3 newPos = hit.point - (cam.transform.forward * hit.collider.bounds.size.magnitude) + (hit.transform.position - hit.point); //puts the world canvas on top of the object on sight

                if(Vector3.Distance(cam.transform.position, hit.collider.transform.position) < minDistanceToObject)
                {
                    newPos = hit.point - (cam.transform.forward * 0.5f);
                }

                float lerpTime = 0;
                lerpTime += Time.deltaTime;
                float lerpDuration = lerpTime / smooth;
                transform.position = Vector3.Lerp(transform.position, newPos, lerpDuration);

                // Convert the hit point from world space to screen space
                Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(cam, hit.point);

                // Convert the screen point to canvas local position
                Vector2 canvasLocalPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPoint, cam, out canvasLocalPoint);

                // Convert the canvas local position back to world space
                Vector3 worldPoint;
                RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRectTransform, screenPoint, cam, out worldPoint);

                var interactableObj = hit.collider.gameObject.GetComponent<InteractableObject>();
                if ((interactableObj.interactionType == "grab" || interactableObj.interactionType == "placeObject") && interactableObj.shouldBeInteractedWith)
                {
                    // Set the position of the grab indicator in world space
                    //grabIndicator.SetActive(true);
                    //grabIndicator.transform.position = worldPoint;
                }
                else if(interactableObj.interactionType == "sight" && interactableObj.shouldBeInteractedWith)
                {
                    interactableObj.OnSight();
                    float sightProgress = interactableObj.sightProgress;
                    //Debug.Log(sightProgress);
                    if(sightProgress > 0)
                    {
                        //// update the progress bar to match sight progress
                        //progressBarObject.UpdateProgress(sightProgress);
                    }

                }
                else
                {
                    
                    grabIndicator.SetActive(false);
                }

            }
            else
            {
                
                grabIndicator.SetActive(false);
            }
        }
        else
        {
            
            grabIndicator.SetActive(false);
        }
    }

    //function to visualize capsuleDraw
    public static void DrawWireCapsule(Vector3 p1, Vector3 p2, float radius)
    {
#if UNITY_EDITOR
        // Special case when both points are in the same position
        if (p1 == p2)
        {
            // DrawWireSphere works only in gizmo methods
            Gizmos.DrawWireSphere(p1, radius);
            return;
        }
        using (new UnityEditor.Handles.DrawingScope(Gizmos.color, Gizmos.matrix))
        {
            Quaternion p1Rotation = Quaternion.LookRotation(p1 - p2);
            Quaternion p2Rotation = Quaternion.LookRotation(p2 - p1);
            // Check if capsule direction is collinear to Vector.up
            float c = Vector3.Dot((p1 - p2).normalized, Vector3.up);
            if (c == 1f || c == -1f)
            {
                // Fix rotation
                p2Rotation = Quaternion.Euler(p2Rotation.eulerAngles.x, p2Rotation.eulerAngles.y + 180f, p2Rotation.eulerAngles.z);
            }
            // First side
            UnityEditor.Handles.DrawWireArc(p1, p1Rotation * Vector3.left, p1Rotation * Vector3.down, 180f, radius);
            UnityEditor.Handles.DrawWireArc(p1, p1Rotation * Vector3.up, p1Rotation * Vector3.left, 180f, radius);
            UnityEditor.Handles.DrawWireDisc(p1, (p2 - p1).normalized, radius);
            // Second side
            UnityEditor.Handles.DrawWireArc(p2, p2Rotation * Vector3.left, p2Rotation * Vector3.down, 180f, radius);
            UnityEditor.Handles.DrawWireArc(p2, p2Rotation * Vector3.up, p2Rotation * Vector3.left, 180f, radius);
            UnityEditor.Handles.DrawWireDisc(p2, (p1 - p2).normalized, radius);
            // Lines
            UnityEditor.Handles.DrawLine(p1 + p1Rotation * Vector3.down * radius, p2 + p2Rotation * Vector3.down * radius);
            UnityEditor.Handles.DrawLine(p1 + p1Rotation * Vector3.left * radius, p2 + p2Rotation * Vector3.right * radius);
            UnityEditor.Handles.DrawLine(p1 + p1Rotation * Vector3.up * radius, p2 + p2Rotation * Vector3.up * radius);
            UnityEditor.Handles.DrawLine(p1 + p1Rotation * Vector3.right * radius, p2 + p2Rotation * Vector3.left * radius);
        }
#endif
    }

    private void OnDrawGizmos()
    {
        DrawWireCapsule(cam.transform.position, cam.transform.position + (cam.transform.forward * cameraCastDistance), cameraCastRadius);
        Gizmos.DrawWireSphere(transform.position, cameraCastRadius);
    }
}
