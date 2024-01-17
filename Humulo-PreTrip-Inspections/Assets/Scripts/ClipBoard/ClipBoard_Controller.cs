using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class ClipBoard_Controller : MonoBehaviour
{
    private int currentStageNum, currentStepNum, stepsNum;
    private StepManager stepManager;

    public GameObject stepTextPrefab;

    public GameObject stepsParent; //steps text go as child of this
    public GameObject checkMark;

    public TMP_Text stageTitle, pageNumber;

    private List<TMP_Text> stepsList;

    private void Awake()
    {
        //get data from stepManager
        stepManager = GameObject.FindGameObjectWithTag("StepManager").GetComponent<StepManager>();
        currentStageNum = (int)stepManager.stepSequence[stepManager.currentStep].SectionName;
        stepsNum = stepManager.stepSequence[stepManager.currentStep].SectionProgressMax;
        currentStepNum = stepManager.stepSequence[stepManager.currentStep].SectionProgressIndex;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateSteps();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateSteps()
    {
        pageNumber.text = $"p. {(int)stepManager.stepSequence[stepManager.currentStep].SectionName}/9";
        /*if(stepsList != null)
        {
            foreach (var text in stepsList)
            {
                Destroy(text.transform.parent.gameObject);
            }
        }*/

        string currentStage = stepManager.stepSequence[stepManager.currentStep].SectionName.ToString();
        stageTitle.text = currentStage; //apply stage title

        stepsList = new List<TMP_Text>();

        //create steps texts
        int stepNum = 1;
        for (int i = 0; i < stepManager.stepSequence.Length; i++)
        {
            //if step doens't belong to the stage or shouldn't appear on clipboard 
            if (stepManager.stepSequence[i].SectionName.ToString() != currentStage || stepManager.stepSequence[i].hideOnClipboard == true)
            {
                continue; 
            }

            var textObj = Instantiate(stepTextPrefab, stepsParent.transform);

            var text = textObj.GetComponentInChildren<TMP_Text>();

            string originalString = stepManager.stepSequence[i].stepName;
            string extractedText = "";

            int startIndex = originalString.IndexOf('"');
            int endIndex = originalString.LastIndexOf('"');

            if (startIndex != -1 && endIndex != -1 && endIndex > startIndex) //foun quotation marks in the name of the step
            {
                extractedText = originalString.Substring(startIndex + 1, endIndex - startIndex - 1);

                text.text = $"{stepNum}. {extractedText}";
            }
            else //didn't find quotation marks on step
            {
                text.text = $"{stepNum}. {stepManager.stepSequence[i].stepName}";
                Debug.Log($"step {i} doesn't have quotation marks on it");
            }

            if (i < stepManager.currentStep) //is a step that was already completed
            {
                text.text = $"<s>{text.text}</s>";
            }

            if (i < stepManager.currentStep) //is a step that was already completed
            {
                text.text = $"<s>{text.text}</s>";
            }

          

            stepsList.Add(text);

            stepNum += 1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 21) //if is on layer "hand"
        {
            if(stepManager.stepSequence[stepManager.currentStep].interactionType.ToString() == "clipboardCheck") //if is check step
            {
                stepManager.NextStep();
                checkMark.SetActive(true);
                var text = stepsList[stepsList.Count - 1];
                text.text = $"<s>{text.text}</s>";

                //UpdateSteps();
            }
        }
    }
}
