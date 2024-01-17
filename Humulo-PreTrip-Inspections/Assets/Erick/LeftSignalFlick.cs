using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;

public class LeftSignalFlick : MonoBehaviour
{
    [SerializeField] private StepManager stepManager;
    [SerializeField] private Animator turnSignalAnimator;
    [SerializeField] private GameObject[] turnSignalLights;

    private bool mayCancel = false;

    private void OnTriggerEnter(Collider other)
    {
        if (stepManager.currentStep == 4)
        {
            Debug.Log(other.gameObject.name);
            if (other.gameObject.CompareTag("LeftHand"))
            {
                Debug.Log("Left Hand Touched LEFT SIGNAL");
                turnSignalLights[0].SetActive(true);
                turnSignalAnimator.SetBool("flickDown", true);
            }
        }

        StartCoroutine(DelayFlick());

        if (mayCancel)
        {
            if (stepManager.currentStep == 5)
            {
                if (other.gameObject.CompareTag("LeftHand"))
                {
                    if (turnSignalLights[0].activeInHierarchy)
                    {
                        turnSignalLights[0].SetActive(false);
                    }
                }
                turnSignalAnimator.SetBool("flickDown", false);
                //turnSignalAnimator.SetBool("flickUp", false);

                turnSignalAnimator.SetBool("idle", true);
            }
        }
    }


    IEnumerator DelayFlick()
    {
        yield return new WaitForSeconds(3);

        mayCancel = true;
    }
}
