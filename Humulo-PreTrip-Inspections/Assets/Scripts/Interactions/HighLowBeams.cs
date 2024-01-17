using UnityEngine;

public class HighLowBeams : MonoBehaviour
{
    //High Beams, Low Beams
    [SerializeField] private GameObject[] lights;
    [SerializeField] private Animator lightSwitchAnimator;
    private bool lowBeamsOn = true;
    private bool switchCooldown = false;
    [SerializeField] private float switchCooldownDuration = 1.0f;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("RightHand") || collision.collider.CompareTag("LeftHand"))
        {
            if (!switchCooldown)
            {
                if (lowBeamsOn)
                {
                    Debug.Log("Switching to high beams");
                    // Turn off low beams, turn on high beams
                    SetLightsState(true, true, false, false);
                    lowBeamsOn = false;
                    SetLowBeamsOnState(false);
                    SetHighBeamsOnState(true);
                }
                else
                {
                    // Turn off high beams, turn on low beams
                    SetLightsState(false, false, true, true);
                    lowBeamsOn = true;
                    SetHighBeamsOnState(false);
                    SetLowBeamsOnState(true);
                }

                switchCooldown = true;
                Invoke("ResetSwitchCooldown", switchCooldownDuration);
            }
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("RightHand") || other.CompareTag("LeftHand"))
    //    {
    //        if (lowBeamsOn)
    //        {
    //            //turn off low beams, turn on high beams
    //            lights[0].SetActive(true);
    //            lights[1].SetActive(true);
    //            lights[2].SetActive(false);
    //            lights[3].SetActive(false);
    //            lowBeamsOn = false;

    //            // Set the animator bools
    //            SetLowBeamsOnState(false);
    //            SetHighBeamsOnState(true);
    //        }
    //        else if (!lowBeamsOn)
    //        {
    //            //turn off high beams, turn on low beams
    //            lights[0].SetActive(false);
    //            lights[1].SetActive(false);
    //            lights[2].SetActive(true);
    //            lights[3].SetActive(true);
    //            lowBeamsOn = true;

    //            // Set the animator bools
    //            SetHighBeamsOnState(false);
    //            SetLowBeamsOnState(true);
    //        }
    //    }
    //}

    private void SetLightsState(bool light1, bool light2, bool light3, bool light4)
    {
        lights[0].SetActive(light1);
        lights[1].SetActive(light2);
        lights[2].SetActive(light3);
        lights[3].SetActive(light4);
    }

    private void SetHighBeamsOnState(bool state)
    {
        lightSwitchAnimator.SetBool("isHighBeamsOn", state);
    }

    private void SetLowBeamsOnState(bool state)
    {
        lightSwitchAnimator.SetBool("isLowBeamsOn", state);
    }

    private void ResetSwitchCooldown()
    {
        switchCooldown = false;
    }
}
