using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftTurnLever : MonoBehaviour
{
    [SerializeField] private StepManager stepManager;
    [SerializeField] private Animator turnSignalAnimator;
    [SerializeField] private GameObject turnSignalLight;

    private bool mayCancel = false;

    public void ChangeSignal()
    {
        if (mayCancel== false)
        {
            turnSignalLight.SetActive(true);
            turnSignalAnimator.SetBool("flickDown", true);
            mayCancel = true;
        }
        else
        {
            turnSignalLight.SetActive(false);
            turnSignalAnimator.SetBool("flickDown", false);
            turnSignalAnimator.SetBool("idle", true);
            mayCancel = false;
        }


        //if (stepManager.currentStep == 4)
        //{

            
        //}

        //if (stepManager.currentStep == 5)
        //{
        //    turnSignalLight.SetActive(false);
        //    turnSignalAnimator.SetBool("flickDown", false);
        //    turnSignalAnimator.SetBool("idle", true);
        //}

    }

   

   
}


