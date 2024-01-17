using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiperSwitch1 : MonoBehaviour
{


    public Animator WiperSwitchAnimator;


    public bool Wiper1On = false;

    public void TurnWipersOn()
    {
        if(Wiper1On == false)
        {
            Wiper1On = true;
            SetWiper1State(true);
        }
        else
        {
            Wiper1On = false;
            SetWiper1State(false);
        }
    }

    private void SetWiper1State(bool state)
    {
        WiperSwitchAnimator.SetBool("isWiperOn", state);
    }
}
