using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nova;
using static Nova.Gesture;
using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem;

public class EventHandler : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip hoverSound;
    public AudioClip clickSound;
    public UIBlock Root = null;
    public float volumePercentage = 0.8f;


    // Start is called before the first frame update
    void Start()
    {
        // create audiosource
        audioSource = gameObject.AddComponent<AudioSource>();

        if(Root == null)
        {
            Debug.Log("UIBlock is null");
        }

        Root.AddGestureHandler<Gesture.OnHover>(Hover);
        Root.AddGestureHandler<Gesture.OnClick>(Click);


    }

    private void Click(Gesture.OnClick click)
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
        // do haptics
        Vibrate(InputSystem.GetDevice<XRController>(CommonUsages.RightHand));
    }

    private void Hover(Gesture.OnHover hover)
    {
        if (hoverSound != null)
        {
            // play one shot at 80% volume

            
            audioSource.PlayOneShot(hoverSound);
        }
        // do haptics
        Vibrate(InputSystem.GetDevice<XRController>(CommonUsages.RightHand));
    }

    private static void Vibrate(XRController device)
    {
        var command = UnityEngine.InputSystem.XR.Haptics.SendHapticImpulseCommand.Create(0, 0.1f, 0.1f);
        device.ExecuteCommand(ref command);
        //Debug.Log(device.name);
    }
}
