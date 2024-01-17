using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HonkHorn : MonoBehaviour
{
    private AudioSource horn;
    [SerializeField] AudioClip honkClip;

    // Start is called before the first frame update
    void Start()
    {
       horn = GetComponentInChildren<AudioSource>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("RightHand") || other.CompareTag("LeftHand"))
        {
                if (!horn.isPlaying)
            horn.PlayOneShot(honkClip);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RightHand") || other.CompareTag("LeftHand"))
        {
            horn.Stop();
        }
    }
}
