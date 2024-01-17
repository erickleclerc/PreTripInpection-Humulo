using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR.Framework.Shared;
using HurricaneVR.Framework.Core.UI;
using JetBrains.Annotations;
using System.Net.Sockets;

public class UIManager : MonoBehaviour
{
    public GameObject UIObjectPrefab;

    public StepManager stepManager;
    public GameObject uIObject;
    private UIObject uIObjectScript;
    public SmoothUIObject smoothUIObjectScript;
    private HVRInputModule HVRInputModule;
    private bool UIHidden = false;
    public Transform UICabLocation; // Assigns both the location and rotation of the UI object when it is in the cab


    // Start is called before the first frame update
    void Awake()
    {
        
        if (stepManager == null)
        {
            Debug.LogError("UIManager: Step Manger not assigned");
        }
        HVRInputModule = GetComponent<HVRInputModule>();
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    // Update the UI object with the new step data
    public void NewStepUpdateUI(int step)
    {
        CreateUIObject();

        // Hide the UI object if there is no UI for this step
        if (stepManager.stepSequence[step].uIObjectType == UIObjectType.NoUI)
        {
            //Debug.Log("No UI Object for this step");
            uIObject.SetActive(false);
            return;
        }
        else if (!UIHidden)
        {
            uIObject.SetActive(true);
        }
        

        if( uIObjectScript != null && stepManager != null)
        {
            //Debug.Log("Updating UI Object with step " + step.ToString());
            uIObjectScript.ObjectType = stepManager.stepSequence[step].uIObjectType;
            uIObjectScript.SectionName = stepManager.stepSequence[step].SectionName;
            uIObjectScript.SectionProgressMax = stepManager.stepSequence[step].SectionProgressMax;
            uIObjectScript.SectionProgressIndex = stepManager.stepSequence[step].SectionProgressIndex;
            uIObjectScript.BodyText = stepManager.stepSequence[step].instructionText;
            uIObjectScript.imageAspectRatio = stepManager.stepSequence[step].imageAspectRatio;
            uIObjectScript.RefImageSprites = stepManager.stepSequence[step].RefImageSprites;
            uIObjectScript.RefImageCaption = stepManager.stepSequence[step].RefImageCaption;
            uIObjectScript.ButtonLabel = stepManager.stepSequence[step].ButtonLabel;
            uIObjectScript.toolTipOriginPoint = stepManager.stepSequence[step].toolTipOriginPoint;
            uIObjectScript.ToolTipText = stepManager.stepSequence[step].ToolTipText;
            uIObjectScript.inCabInteraction = stepManager.stepSequence[step].inCabInteraction;
            uIObjectScript.UpdateUIComponents();
        }
        
        // If the step is not a tooltip, then move the UI object to the correct position
        // And assign the UIButton as the focused object
        if (stepManager.stepSequence[step].uIObjectType != UIObjectType.Tooltip && stepManager.stepSequence[step].interactionType == StepManager.Interactions.button)
        {
            if (stepManager.stepSequence[step].inCabInteraction)
            {
                if (UICabLocation == null)
                {
                    Debug.LogError("UIManager: UICabLocation not assigned");
                }
                else
                {
                    //Debug.Log("Moving UI Object to Cab");
                    // Assign uIObject as child of UICabLocation, and zero out the position and rotation
                    // Freeze the smoothUIObjectScript
                    uIObject.transform.SetParent(UICabLocation);
                    uIObject.transform.localPosition = Vector3.zero;
                    uIObject.transform.localRotation = Quaternion.identity;
                    smoothUIObjectScript.SetFreezeLocation(true);
                }
                
            }
            else // If this is not an in Cab interaction, then turn on the smoothUIObjectScript
            {
                //Debug.Log("Moving UI Object to ToolTip");
                // UnFreeze the smoothUIObjectScript
                uIObject.transform.SetParent(this.transform);
                smoothUIObjectScript.SetFreezeLocation(false);
                smoothUIObjectScript.smoothNow = true;
            }
            
            // Assign uIObjecct as the focused object for the current step
            if (uIObjectScript.buttonObject != null)
            {
                // Clear the focused object for the current step
                stepManager.stepSequence[step].focusedObjs = new GameObject[1]; 
                stepManager.stepSequence[step].focusedObjs[0] = uIObjectScript.buttonObject;
            }
            else
            {
                Debug.LogError("UIManager: Button Object not assigned");
            }
        }
    }

    public void CreateUIObject()
    {
        if (uIObject != null)
        {
            return;
        }
        
        if (UIObjectPrefab == null)
        {
            Debug.LogError("UIManager: UIObjectPrefab not set");
            return;
        }
        else
        {
            //// Create the UI object
            //Debug.Log("Creating UI Object");
            uIObject = Instantiate(UIObjectPrefab, transform);

            // Add uIObject to the HVRInputModule UICanvas list
            Canvas canvas = uIObject.GetComponent<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("UIManager: Canvas not found on UIObjectPrefab");
            }
            else
            {
                HVRInputModule.UICanvases.Add(uIObject.GetComponent<Canvas>());
            }

            uIObjectScript = uIObject.GetComponent<UIObject>();
            if (uIObjectScript == null)
            {
                Debug.LogError("UIManager: UIObject script not found on UIObjectPrefab");
            }
            

            // Get the SmoothUIObject script from the UI object
            smoothUIObjectScript = uIObject.GetComponent<SmoothUIObject>();
            if (smoothUIObjectScript == null)
            {
                Debug.LogError("UIManager: SmoothUIObject script not found on UIObjectPrefab");
            }
        }
    }
    
    public void HideUI()
    {
        UIHidden = true;
        if(uIObject != null)
        {
            uIObject.SetActive(false);
        }
    }

    public void ShowUI()
    {
        UIHidden = false;
        if (uIObject != null)
        {
            uIObject.SetActive(true);
            if (smoothUIObjectScript != null)
            {
                smoothUIObjectScript.MoveUI();
                smoothUIObjectScript.smoothNow = true;
            }
        }
    }
}
