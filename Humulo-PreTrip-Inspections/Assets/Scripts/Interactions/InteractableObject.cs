using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class InteractableObject : MonoBehaviour
{
    public string interactionType;

    public bool wasGrabbed, wasTouched, onSight;

    public float timeOnSight;

    public float timeToCompleteSight = 3f;

    public bool sigthed; //on sight for X seconds uninterrupted

    public bool shouldBeInteractedWith; //so we know if this obj is part of the current step

    public float sightProgress = 0;

    public int targetLayer = 20;

    private GameObject scanObject;

    private Material scanMaterial; //is assigned in awake

    private void Awake()
    {
        scanMaterial = Resources.Load<Material>("Materials/M_Scan");
        if (scanMaterial == null)
        {
            Debug.Log("Failed to load scan material");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = targetLayer;
    }

    // Update is called once per frame
    void Update()
    {
        if(interactionType == "sight" && onSight)
        {
            if (sigthed) return;

            if(timeOnSight == 0) 
            {
                ActivateScanShader();
            }

            timeOnSight += Time.deltaTime;

            sightProgress = (timeOnSight / timeToCompleteSight);
            scanObject.GetComponent<ScanShaderCommunicator>().partialView = (timeOnSight / timeToCompleteSight);

            if (timeOnSight > timeToCompleteSight)
            {
                sigthed = true;
                Destroy(scanObject);

            }
        }
        else
        {
            timeOnSight = 0;
            Destroy(scanObject);
        }

        onSight = false;

        if (!shouldBeInteractedWith)
        {
            gameObject.layer = 19; //if the object was interacted with, take it out of the grabbable state
        }
        //else gameObject.layer = 20;
    }

    public void Grabbed()
    {
        wasGrabbed = true;
    }

    public void Released()
    {
        wasGrabbed = false;
    }

    public void Touched()
    {
            wasTouched = true;
    }

    public void LetGo()
    {
         wasTouched = false;
    }

    public void OnSight()
    {
        onSight = true;
    }

    private void ActivateScanShader()
    {
        scanObject = new GameObject("Scan");
        scanObject.transform.parent = this.transform;
        scanObject.transform.localPosition = Vector3.zero;

        // Set the scale and rotation to default to match parent scale and rotation
        scanObject.transform.localScale = Vector3.one;
        scanObject.transform.localRotation = Quaternion.identity;

        MeshFilter objFilter = scanObject.AddComponent<MeshFilter>();
        objFilter.mesh = GetComponent<MeshFilter>().sharedMesh;

        Material[] newMaterials = new Material[GetComponent<MeshRenderer>().materials.Length];

        for (int j = 0; j < newMaterials.Length; j++)
        {
            newMaterials[j] = scanMaterial;
        }

        Renderer render = scanObject.AddComponent<MeshRenderer>();
        render.materials = newMaterials;

        scanObject.AddComponent<ScanShaderCommunicator>();
    }


}
