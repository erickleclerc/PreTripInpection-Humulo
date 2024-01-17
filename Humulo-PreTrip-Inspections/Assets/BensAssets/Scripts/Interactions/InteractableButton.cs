using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableButton : MonoBehaviour
{
    public bool shouldBeInteractedWith; //so we know if this obj is part of the current step
    
    public bool wasPressed;

    public void Start()
    {
        shouldBeInteractedWith = true;
        wasPressed = false;
    }

    public void Pressed()
    {
        wasPressed = true;
    }

    public void Reset()
    {
        wasPressed = false;
        shouldBeInteractedWith = true;
    }
}
