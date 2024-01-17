using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiperSwitch2 : MonoBehaviour
{


    public Animator Wiper2SwitchAnimator;


    public bool Wiper2On = false;

    public void TurnWipersOn()
    {
        if(Wiper2On == false)
        {
            Wiper2On = true;
            SetWiper2State(true);
        }
        else
        {
            Wiper2On = false;
            SetWiper2State(false);
        }
    }

    private void SetWiper2State(bool state)
    {
        Wiper2SwitchAnimator.SetBool("isWiper2On", state);
    }
}
