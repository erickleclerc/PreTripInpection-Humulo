using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabKinematic : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Grabbed()
    {
        rb.isKinematic = false;
    }

    public void Realeased()
    {
        rb.isKinematic = true;
    }
}
