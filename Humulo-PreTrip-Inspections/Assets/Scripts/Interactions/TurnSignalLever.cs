using UnityEngine;

public class TurnSignalLever : MonoBehaviour
{
    [SerializeField] private StepManager stepManager;
    [SerializeField] private Animator turnSignalAnimator;
    [SerializeField] private GameObject[] turnSignalLights;

    private void OnTriggerEnter(Collider other)
    {
        if (stepManager.currentStep == 4)
        {
            Debug.Log(other.gameObject.name);
            if (other.gameObject.CompareTag("DownLever"))
            {
                turnSignalLights[0].SetActive(true);

                //set animator to flick down
                turnSignalAnimator.SetBool("flickDown", true);
            }
            else if (other.gameObject.CompareTag("UpLever"))
            {
                turnSignalLights[1].SetActive(true);

                //set animator to flick up
                turnSignalAnimator.SetBool("flickUp", true);
            }
        }

        if (stepManager.currentStep == 5)
        {
            if (other.gameObject.CompareTag("DownLever"))
            {
                if (turnSignalLights[1].activeInHierarchy)
                {
                    turnSignalLights[1].SetActive(false);
                }
            }
            else if (other.gameObject.CompareTag("UpLever"))
            {
                if (turnSignalLights[0].activeInHierarchy)
                {
                    turnSignalLights[0].SetActive(false);
                }
            }

            //set animator back to idle
            turnSignalAnimator.SetBool("flickDown", false);
            turnSignalAnimator.SetBool("flickUp", false);
            turnSignalAnimator.SetBool("idle", true);
        }
    }
}
    

