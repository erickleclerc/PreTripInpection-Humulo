using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageUIController : MonoBehaviour
{
    public GameObject checkMark, currentStepText;
    public TMP_Text stepsMarkerText;
    public int stepsNumber, currentStepNum;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateStage(int stepsNum)
    {
        currentStepText.SetActive(true);
        stepsNumber = stepsNum;
        stepsMarkerText.text = $"- {currentStepNum}/{stepsNumber}";
    }

    public void StepCompleted(int currentStep)
    {
        currentStepNum = currentStep;
        stepsMarkerText.text = $"- {currentStepNum}/{stepsNumber}";
    }


    public void StageCompleted()
    {
        currentStepText.SetActive(false);
        stepsMarkerText.text = "";
        checkMark.SetActive(true);
    }
}
