using HurricaneVR.Framework.Core.UI;
using HurricaneVR.Framework.Shared;
using Nova;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

public class NovaInteractionSender : MonoBehaviour
{
    // Reference to the EventSystem in your scene.
    public EventSystem eventSystem;

    private HVRInputModule inputModule;

    private void Start()
    {
        inputModule = GetComponent<HVRInputModule>();
        if (inputModule == null)
        {
            Debug.LogError("HVRInputModule not found in the scene.");
        }
    }

    private void Update()
    {
        if (eventSystem == null || inputModule == null)
        {
            Debug.LogWarning("Missing references to EventSystem or HVRInputModule in NovaInteractionSender.");
            return;
        }

        // Check if the primary button on the XR controller is pressed.
        bool isButtonPressed = GetPressedButtonState();

        // Get the world-space position and direction of the XR controller.
        Ray pointerRay = GetPointerRay();

        // Create an Interaction.Update from the pointer ray and button state.
        Interaction.Update update = new Interaction.Update(pointerRay, controlID: 0); // Set an appropriate control ID.

        // Pass the Interaction.Update to Nova's Interaction API.
        Interaction.Point(update, isButtonPressed);
    }

    private bool GetPressedButtonState()
    {
        HVRButtons pressButton = inputModule.PressButton;
        bool isButtonPressed;

        // Check if the InputModule's press button is pressed on the left or right XR controller.
        if (HVRController.GetButtonState(HVRHandSide.Right, pressButton).Active)
        {
            isButtonPressed = true;
        }
        else
        {
            isButtonPressed = HVRController.GetButtonState(HVRHandSide.Left, pressButton).Active;
        }

        return isButtonPressed;
    }

    private Ray GetPointerRay()
    {

        // Check if the instance pointers length is 0.
        if (HVRInputModule.Instance.Pointers.Count == 0)
        {
            return new Ray();
        }

        // If the instance pointers are null break out of the function.
        if (HVRInputModule.Instance.Pointers[0] == null)
        {
            return new Ray();
        }
        
        HVRUIPointer pointer = HVRInputModule.Instance.Pointers[0];
        if (pointer != null)
        {
            Vector3 position = pointer.Camera.transform.position;
            Vector3 direction = pointer.Camera.transform.forward;
            return new Ray(position, direction);
        }

        // Return a fallback ray if XR input is not available.
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }
}
