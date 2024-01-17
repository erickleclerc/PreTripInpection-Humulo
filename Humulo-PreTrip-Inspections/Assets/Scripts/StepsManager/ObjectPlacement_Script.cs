using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacement_Script : MonoBehaviour
{
    public bool objectPlaced;

    private void Awake()
    {

    }


    private void OnTriggerEnter(Collider other)
    {

    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Trigger Entered");
        if (other.gameObject.CompareTag("ObjectToBePlaced") && !other.gameObject.GetComponent<InteractableObject>().wasGrabbed)
        {
            objectPlaced = true;
            Debug.Log(other.gameObject.name +  " placed");
            StartCoroutine(ResetBool());
        }
    }

    private IEnumerator ResetBool()
    {
        yield return new WaitForSeconds(2);
        objectPlaced = false; //reset bool to other steps
    }
}
