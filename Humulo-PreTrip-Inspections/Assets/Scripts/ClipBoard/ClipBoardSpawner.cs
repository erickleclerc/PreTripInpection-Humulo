using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipBoardSpawner : MonoBehaviour
{
    public GameObject clipBoardPrefab;
    public StepManager stepManager;

    private bool isSpawned;
    private GameObject clipBoard;

    public Vector3 rotationOffset;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var step = stepManager.stepSequence[stepManager.currentStep];
        //if (step.interactionType == "ClipBoardCheck")
        if(transform.localEulerAngles.z < 110 && transform.localEulerAngles .z > 70)
        {
            if (!isSpawned) //clipboard is not spawned
            {
                Vector3 spawnPoint = transform.position + (transform.right * 0.1f);
                clipBoard = Instantiate(clipBoardPrefab, spawnPoint, transform.rotation);
                clipBoard.transform.Rotate(rotationOffset);
                isSpawned = true;
            }
            else //clipboard already exists
            {
                Vector3 spawnPoint = transform.position + (transform.right * 0.1f);
                clipBoard.transform.position = spawnPoint;
                clipBoard.transform.rotation = transform.rotation;
                clipBoard.transform.Rotate(rotationOffset);
            }
        }
        else if(clipBoard != null) //hand is out of rotation and clipboard is spawned
        {
            Destroy(clipBoard);
            isSpawned = false;
        }
    }
}
