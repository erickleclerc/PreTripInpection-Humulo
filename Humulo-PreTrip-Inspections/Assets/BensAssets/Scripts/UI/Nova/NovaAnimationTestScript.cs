using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nova;
using Unity.VisualScripting;

public class NovaAnimationTestScript : MonoBehaviour
{
    public UIBlock2D UIBlock2D;

    public InteractableObject interactableObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(interactableObject != null)
        {
            if (interactableObject.sightProgress > 0)
            {
                UIBlock2D.Size.X = interactableObject.sightProgress;

            } 
        }

    }
}
