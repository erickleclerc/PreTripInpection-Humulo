using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Nova;
using UnityEngine.UI;

public class SceneMenuDebug : MonoBehaviour
{

    public InputActionReference toggleSceneLoaderAction;
    public InputActionReference toggleMusicPlayerAction;
    
    public GameObject SceneMenuDebugObject;
    public GameObject MusicPlayerObject;
    public GameObject NextStepObject;
    public GameObject PreviousStepObject;

    public TextBlock CurrentStepTextBlock;

    public StepManager stepManager;

    private int lastStep = -1;

    private void Start()
    {
        // Find object in scene tagged StepManager
        stepManager = GameObject.FindWithTag("StepManager").GetComponent<StepManager>();
        if (stepManager == null)
        {
            Debug.Log("StepManager not found in scene");
        }
        else if(stepManager != null)
        {
            Debug.Log("StepManager found in scene");

        }

        // Enable the action when the script starts
        toggleSceneLoaderAction.action.Enable();
        toggleMusicPlayerAction.action.Enable();

    }

    // Update is called once per frame
    void Update()
    {
        // Check if the action was triggered and toggle the SceneMenuDebugObject
        if (toggleSceneLoaderAction.action.triggered)
        {
            SceneMenuDebugObject.SetActive(!SceneMenuDebugObject.activeSelf);
        }
        if(toggleMusicPlayerAction.action.triggered)
        {
            MusicPlayerObject.SetActive(!MusicPlayerObject.activeSelf);
        }

        if(stepManager.currentStep != lastStep)
        {
            CurrentStepTextBlock.Text = "Current Step: " + stepManager.currentStep;
            lastStep = stepManager.currentStep;
        }
    }

    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    // Method to handle the next step button click event
    public void NextStepButtonClicked()
    {
        if (stepManager != null)
        {
            stepManager.NextStep();
        }
    }

    // Method to handle the previous step button click event
    public void PrevStepButtonClicked()
    {
        if (stepManager != null)
        {
            stepManager.PrevStep();
        }
        
    }
}
