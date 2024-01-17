using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nova;

public class XRHandInput : MonoBehaviour
{
    private const uint RightFingerTipControlID = 1;
    private const uint LeftFingerTipControlID = 2;

    /// <summary>
    /// The radius of interaction for the finger tip.
    /// I.e. how close the finger must be to be considered as "interacting" with
    /// Nova content. Conceptually similar to a SphereCollider's radius.
    /// </summary>
    public float FingerTipCollisionRadius = .01f;

    // / <summary>
    // / The position of the right and left finger tip in world space.
    // / </summary>
    public GameObject RightFingerTipObject;
    public GameObject LeftFingerTipObject;

    private Vector3 RightFingerTipWorldPosition;
    private Vector3 LeftFingerTipWorldPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(RightFingerTipObject != null )
        {
            RightFingerTipWorldPosition = RightFingerTipObject.transform.position;
            // Create a Nova.Sphere from the right index finger tip position
            Sphere rightFingerTipSphere = new Sphere(RightFingerTipWorldPosition, FingerTipCollisionRadius);

            // Call Point with the newly created sphere and the right finger control ID.
            Interaction.Point(rightFingerTipSphere, RightFingerTipControlID);
        }
        
        if (LeftFingerTipObject != null)
        {
            LeftFingerTipWorldPosition = LeftFingerTipObject.transform.position;
            // Create a Nova.Sphere from the left index finger tip position
            Sphere leftFingerTipSphere = new Sphere(LeftFingerTipWorldPosition, FingerTipCollisionRadius);

            // Call Point with the newly created sphere and the left finger control ID.
            Interaction.Point(leftFingerTipSphere, LeftFingerTipControlID);
        }
    }
}
