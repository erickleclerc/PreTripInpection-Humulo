using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Nova;

public enum UIObjectType
{
    UIFirstStep,
    UILastStep,
    Tooltip,
    NoUI
}

public enum SectionName
{
    Safety_Precautions,
    Tractor_Cab,
    Under_The_Hood,
    Tires,
    Trailer_Connections,
    Trailer_Side,
    Trailer_Rear,
    Trailer_Side_2,
    Inside_of_Cab
}

public enum ImageAspectRatio
{
    Square,
    Wide,
}

public class UIObject : MonoBehaviour
{
   // Sets the type of UI object this is, will be used to determine how to display the object
    public UIObjectType ObjectType;

    //Public Variables to contain the data that will be used to populate the UI
    public SectionName SectionName;
    public int SectionProgressMax = 1;
    public int SectionProgressIndex = 0;
    public string BodyText;
    public ImageAspectRatio imageAspectRatio;
    public List<Sprite> RefImageSprites;
    public string RefImageCaption;
    public string ButtonLabel;
    public Color green  = new Color(40, 205, 65);

    public Transform toolTipOriginPoint;
    public string ToolTipText;
    public bool inCabInteraction;

    // Reference to the button that will be used to advance the step
    public GameObject buttonObject;

    // Internal components for UI with button
    public GameObject UIInteractiveParent;
    public TextBlock SectionNameTextBlock;
    public GameObject SectionProgressParent;
    public GameObject RefImageParent;
    public GameObject BodyFirstStepParent;
    public GameObject BodyLastStepParent;
    public TextBlock BodyTextBlock;
    public TextBlock CaptionTextBlock;
    public TextBlock ButtonLabelTextBlock;
    // Tool tip components
    public GameObject ToolTipParent;
    // Point attached to the focused object
    public GameObject ToolTipPointA; 
    // Point attached to the tool tip UI2DBlock object. 
    public GameObject ToolTipPointB; // Right now this isn't used, because the offset is automatically controlled by VerticalBillboard.cs
    public TextBlock ToolTipTextBlock;

    // Prefabs to instantiate as needed
    public UIBlock2D RefImagePrefab;
    public UIBlock2D RefImageWidePrefab;
    public UIBlock2D ProgressIndexPrefab;
    

    // Debug Tools
    public bool UpdateNow = false;

    private void Start()
    {
        UpdateUIComponents();
    }

    private void Update()
    {
        if(UpdateNow) // Toggle this bool in editor to force update
        {
            UpdateNow = false;
            UpdateUIComponents();
        }
    }

    //// Called in the editor whenever a value is changed
    //private void OnValidate()
    //{
    //    UpdateUIComponents();
    //}

    public void UpdateUIComponents()
    {
        // Hide and show the correct components based on the type of UI object this is using switch statement
        switch (ObjectType)
        {
            case UIObjectType.UIFirstStep:
                UIInteractiveParent.SetActive(true);
                BodyFirstStepParent.SetActive(true);
                BodyLastStepParent.SetActive(false);
                ToolTipParent.SetActive(false);
                break;
            case UIObjectType.UILastStep:
                UIInteractiveParent.SetActive(true);
                BodyFirstStepParent.SetActive(false);
                BodyLastStepParent.SetActive(true);
                ToolTipParent.SetActive(false);
                break;
            case UIObjectType.Tooltip:
                UIInteractiveParent.SetActive(false);
                ToolTipParent.SetActive(true);
                break;
        }   
        
        // Set Section Name, unless tooltip
        if (ObjectType != UIObjectType.Tooltip) 
        {
            string sectionNameParsed = SectionName.ToString().Replace("_", " "); // Replace underscores with spaces
            SectionNameTextBlock.Text = sectionNameParsed;
        }
        else if (ObjectType != UIObjectType.Tooltip)
        {
            Debug.Log("Section Block Text Not Set");
        }

        // Set Section Progress, unless tooltip
        if (SectionProgressParent != null && ObjectType != UIObjectType.Tooltip)
        {
            // Clear the section progress parent
            foreach (Transform child in SectionProgressParent.transform)
            {
                Destroy(child.gameObject);
            }

            // Instantiate the correct number of progress blocks
            for (int i = 0; i < SectionProgressMax; i++)
            {
                // Create a progress block for each index
                UIBlock2D progressBlock = Instantiate(ProgressIndexPrefab, SectionProgressParent.transform); 
                // Fill compelted steps based on SectionProgressIndex
                if (i < SectionProgressIndex)
                {
                    progressBlock.BodyEnabled = true;
                    progressBlock.Color = green;
                }
                else // Fill the rest of the progress blocks with empty blocks
                {
                    progressBlock.BodyEnabled = false;
                }
            }
        }
        else if (ObjectType != UIObjectType.Tooltip)
        {
            Debug.Log("Section Progress Parent Not Set");
        }

        // Set the Referene images if UILastStep
        if (RefImageParent != null && ObjectType == UIObjectType.UILastStep)
        {
            // Clear the ref image parent
            foreach (Transform child in RefImageParent.transform)
            {
                Destroy(child.gameObject);
            }

            // Instantiate the correct number of ref image blocks and assign images
            for (int i = 0; i < RefImageSprites.Count; i++)
            {
                UIBlock2D refImageBlock;
                if (imageAspectRatio == ImageAspectRatio.Square)
                {
                    refImageBlock = Instantiate(RefImagePrefab, RefImageParent.transform); // Create a ref image block for each sprite
                }
                else
                {
                    refImageBlock = Instantiate(RefImageWidePrefab, RefImageParent.transform);
                }

                
                refImageBlock.SetImage(RefImageSprites[i]);
            }
        }

        if (RefImageCaption != null && ObjectType == UIObjectType.UILastStep)
        {
            CaptionTextBlock.Text = RefImageCaption;
        }
        else if (RefImageCaption == null && ObjectType == UIObjectType.UILastStep)
        {
            Debug.Log("Ref Image Caption Not Set");
        }

        // Set the Instruction body text or the tooltip
        if (BodyText != null && ObjectType == UIObjectType.UIFirstStep)
        {
            BodyTextBlock.Text = BodyText;
        }
        else if (ToolTipText != null && ObjectType == UIObjectType.Tooltip)
        {
            ToolTipTextBlock.Text = ToolTipText;
        }
        else if (BodyText == null && ObjectType != UIObjectType.UILastStep)
        {
            Debug.Log("Body Text Not Set");
        }
        

        // Set the button text
        if (ButtonLabel != null && ObjectType != UIObjectType.Tooltip)
        {
            if(ButtonLabel == null)
            {
                buttonObject.SetActive(false);
            }
            else
            {
                buttonObject.SetActive(true);
                ButtonLabelTextBlock.Text = ButtonLabel;
            }
            ButtonLabelTextBlock.Text = ButtonLabel;
        }
        else if (ButtonLabel == null && ObjectType == UIObjectType.Tooltip)
        {
            Debug.Log("Button Label Text Not Set");
        }

        // Position the tooltip. Rotation doesn't matter, will always face the camera
        if (ObjectType == UIObjectType.Tooltip && toolTipOriginPoint != null)
        {
           ToolTipParent.transform.position = toolTipOriginPoint.position;
            // Set the tooltip to be a child of the focused object
            ToolTipParent.transform.parent = toolTipOriginPoint.transform;
        }

        
    }

    // coroutine that disables the button for a set amount of time
    public IEnumerator DisableButton(float time)
    {
        buttonObject.GetComponent<Button>().interactable = false;
        yield return new WaitForSeconds(time);
        buttonObject.GetComponent<Button>().interactable = true;
    }

    public void SetToolTipParent(GameObject gameObject)
    {
        // set tooltip to be a child of gameObject
        ToolTipParent.transform.parent = gameObject.transform;
    }
  
}


