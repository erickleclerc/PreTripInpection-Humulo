using UnityEngine;

public class FogLights : MonoBehaviour
{
    [SerializeField] private GameObject[] fogLights;
    [SerializeField] private Animator fogSwitchAnimator;
    private bool fogLightsOn = false;
    private bool switchCooldown = false;
    [SerializeField] private float switchCooldownDuration = 1.0f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("RightHand") || collision.collider.CompareTag("LeftHand"))
        {
            if (!switchCooldown)
            {
                if (fogLightsOn)
                {
                    SetFogLightsState(false, false);
                    fogLightsOn = false;
                    SetFogLightsOnState(false);
                }
                else
                {
                    SetFogLightsState(true, true);
                    fogLightsOn = true;
                    SetFogLightsOnState(true);
                }

                switchCooldown = true;
                Invoke("ResetSwitchCooldown", switchCooldownDuration);
            }
        }
    }

    private void SetFogLightsState(bool light1, bool light2)
    {
        fogLights[0].SetActive(light1);
        fogLights[1].SetActive(light2);
    }

    private void SetFogLightsOnState(bool state)
    {
        fogSwitchAnimator.SetBool("isFogLightsOn", state);
    }

    private void ResetSwitchCooldown()
    {
        switchCooldown = false;
    }
}
