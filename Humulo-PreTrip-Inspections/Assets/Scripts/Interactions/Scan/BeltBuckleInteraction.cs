using HurricaneVR.Framework.Core.Utils;
using UnityEngine;

public class BeltBuckleInteraction : MonoBehaviour
{
    // Reference to the belt clip object
    public Transform clipSnapPosition; // This is the position where the buckle will snap to in the clip
    public float snapSpeed = 10f;

    // Flag to check if the buckle is currently in the clip
    private bool isBuckleInClip = false;

    // Reference to the belt clip object
    public GameObject beltClip;
    public Vector3 pos;
    public Quaternion rot;

    // Reference to the rigidbody of the belt buckle
    private Rigidbody beltBuckleRigidbody;
    private InteractableObject interactableObject;

    private void Start()
    {
        // Cache the rigidbody of the belt buckle for performance
        beltBuckleRigidbody = GetComponent<Rigidbody>();
        interactableObject = GetComponent<InteractableObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the buckle has entered the clip's trigger area
        if (other.CompareTag("BeltClip"))
        {
            //Debug.Log("Buckle entered clip trigger area");
            // If the buckle is placed correctly, lock it into the clip.
            isBuckleInClip = true;

            // Make the belt buckle kinematic so it stops being affected by physics
            beltBuckleRigidbody.isKinematic = true;

            // Move the buckle to the snap position immediately when it enters the clip's trigger area
            transform.position = pos;
            transform.rotation = rot;

            interactableObject.wasGrabbed = true;
            
        }
    }
}