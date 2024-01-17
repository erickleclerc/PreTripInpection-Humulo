using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StepManager : MonoBehaviour
{
    private int previousStep;
    private int interactionsCompleted = 0; //variable to check if all objects of step were interacted with
    public int currentStep;

    public Step[] stepSequence;

    public AudioSource audioSource; //should be audio source on the VRRig

    public Material highLightMaterial;
    public enum Interactions { grab, sight, placeObject, button, clipboardCheck, touch };

    public Transform playerRig;

    public CameraFade cameraFade;

    private GameObject previousUIParent;
    private GameObject currentUIParent;

    public bool drawGizmos = true;

    private GameObject[] highLightOverlays;

    public GameObject UIObject;
    public GameObject AIPrefab;

    public GameObject truckExteriorPrefab;
    public GameObject truckInteriorPrefab;

    public AudioManager audioManager;
    public UIManager uIManager;
    public bool shouldAnimateAI;

    private Coroutine newStepCoroutine;
    public bool debugIgnoreAI = false; // Debug bool to hide the AI error messages

    [System.Serializable]
    public class Step
    {
        public string stepName;
        public bool hideOnClipboard; //check this true if the step shouldn't appear as a Step in the user's clipboard

        public SectionName SectionName;
        public int SectionProgressMax = 5;
        public int SectionProgressIndex = 0;

        public GameObject[] focusedObjs;

        public Interactions interactionType;
        public bool inCabInteraction;

        public AudioClip instructionMP3;

        public UIObjectType uIObjectType;

        [TextArea(3, 10)]
        public string instructionText;

        public ImageAspectRatio imageAspectRatio;
        public List<Sprite> RefImageSprites;
        [TextArea(1, 3)]
        public string RefImageCaption;
        public string ButtonLabel;
        
        public string ToolTipText;
        public Transform toolTipOriginPoint;

        public Transform teleportLocation;

        [Tooltip("Use this if the interaction type is `Place Object`")]
        public GameObject positionToPlaceObject;

        [HideInInspector]
        public string defaultTag; //used to remember the default tag of the object that will be placed somewhere

        public UIConfiguration UIConfig;
        public AIPresence aiPresence;

        public bool swapTruckPrefabs;
        

        

        [System.Serializable]
        public class UIConfiguration
        {
            public GameObject UIParent;
            public Vector3 UIPosition;
            public Quaternion UIRotaiton;
            public bool shouldSpawnUI;
        }

        [System.Serializable]
        public class AIPresence
        {
            public Vector3 AIPosition;
            public Quaternion AIRotation;
            public bool shouldAnimante;
        }
    }


    public void Awake()
    {
        
        if (uIManager == null)
        {
            Debug.LogError("UIManager not found on StepManager");
        }
    }

    void Start()
    {
        cameraFade = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFade>();

        previousStep = currentStep;
        newStepCoroutine = StartCoroutine(NewStep());

    }

    void Update()
    {
        DebugStepNavigation();

        //if audiosource is playing, ai should be animating
        if (audioSource.isPlaying)
        {
            shouldAnimateAI = true;
        }
        else
        {
            shouldAnimateAI = false;
        }

        // If we're on a new step, then start a new step, unless we're done the steps
        // Ensure a new step coroutine isn't already running
        if (previousStep != currentStep && currentStep < stepSequence.Length)
        {
            newStepCoroutine = StartCoroutine(NewStep());
        }

        previousStep = currentStep;

        // Stop checking for step completion if we're done the steps
        if (currentStep < stepSequence.Length)
        {
            if (stepSequence[currentStep].focusedObjs.Length > 0)
            {
                CheckStepCompletion();
            }
            else
            {
                //Debug.Log($"No focused objects in step: {currentStep}");
            }
        }
    }

    private IEnumerator NewStep()
    {

        //RemoveHighLightFromLastStep();
        // debug print the values of the current step
        //Debug.Log("Current Step: " + currentStep);
        //Debug.Log($"INTERACTION TYPE {step.interactionType}");

        interactionsCompleted = 0;

        Step step = stepSequence[currentStep];


        


        if (step.UIConfig.shouldSpawnUI && UIObject != null)
        {
            UIObject.SetActive(true);
            UIObject.transform.position = step.UIConfig.UIPosition;
            UIObject.transform.rotation = step.UIConfig.UIRotaiton;
        }
        else if ( UIObject != null)
        {
            UIObject.SetActive(false);
        } 

        // Teleportation
        if (step.teleportLocation != null)
        {
            // Hide UI, teleport, show UI
            uIManager.HideUI();
            if (uIManager != null)
            {
                uIManager.NewStepUpdateUI(currentStep);
            }
            else
            {
                Debug.Log("UI Manager not found on StepManager");
            }
            // Fade out, teleport, fade in
            cameraFade.FadeOut();
            yield return new WaitForSeconds(cameraFade.fadeDuration);

            // reset player rig transform to 0,0,0
            playerRig.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

            // assign position of teleport location to player rig
            playerRig.position = step.teleportLocation.position;
            //Debug.Log("teleported");
            // assign y rotation of teleport location to player rig
            Vector3 newRotation = playerRig.rotation.eulerAngles;
            newRotation.y = step.teleportLocation.rotation.eulerAngles.y;
            playerRig.rotation = Quaternion.Euler(newRotation);
            
            yield return new WaitForSeconds(cameraFade.fadeDuration);
            uIManager.ShowUI();
            cameraFade.FadeIn();
        }
        else // Non Teleporation, update UI
        {
            if (uIManager != null)
            {
                uIManager.NewStepUpdateUI(currentStep);
            }
            else
            {
                Debug.Log("UI Manager not found on StepManager");
            }
        }

        if (step.UIConfig.UIParent != null)
        {
            HidePreviousUIParent();
            ShowCurrentUIParent(step.UIConfig.UIParent);
        }

        //A.I. Placement in the scene
        if (AIPrefab != null)
        {
            AIPrefab.transform.SetPositionAndRotation(step.aiPresence.AIPosition, step.aiPresence.AIRotation);
        }
        else if (!debugIgnoreAI) 
        {
            Debug.LogWarning("AI Prefab is not assigned to the game object.");
        }

        // If AudioSource is already playing a clip, stop it before playing the next one
        if (audioSource.isPlaying) audioSource.Stop();

        yield return new WaitForSeconds(1);
        if (step.instructionMP3 != null) audioSource.PlayOneShot(step.instructionMP3); //plays instruction
        //Debug.Log(step.instructionText);

        // Don't highlight Buttons
        if (step.interactionType != Interactions.button && step.interactionType != Interactions.clipboardCheck)
        {

            //create highlights
            highLightOverlays = new GameObject[step.focusedObjs.Length];
            for (int i = 0; i < step.focusedObjs.Length; i++)
            {
                highLightOverlays[i] = new GameObject("Highlight");
                highLightOverlays[i].transform.parent = step.focusedObjs[i].transform;
                highLightOverlays[i].transform.localPosition = Vector3.zero;

                // Set the scale and rotation to default to match parent scale and rotation
                highLightOverlays[i].transform.localScale = Vector3.one;
                highLightOverlays[i].transform.localRotation = Quaternion.identity;

                MeshFilter objFilter = highLightOverlays[i].AddComponent<MeshFilter>();
                objFilter.mesh = step.focusedObjs[i].GetComponent<MeshFilter>().sharedMesh;

                Material[] newMaterials = new Material[step.focusedObjs[i].GetComponent<MeshRenderer>().materials.Length];

                for (int j = 0; j < newMaterials.Length; j++)
                {
                    newMaterials[j] = highLightMaterial;
                }

                Renderer render = highLightOverlays[i].AddComponent<MeshRenderer>();
                render.materials = newMaterials;

                var interactableObj = step.focusedObjs[i].GetComponent<InteractableObject>();

                interactableObj.interactionType = step.interactionType.ToString();
                interactableObj.shouldBeInteractedWith = true;
                interactableObj.timeOnSight = 0;
                interactableObj.sigthed = false;

                step.focusedObjs[i].layer = 20;

                highLightOverlays[i].AddComponent<Outline>();
                highLightOverlays[i].GetComponent<Outline>().OutlineWidth = 8;
            }
        }

        // If current step requires prefab swap, disable current prefab and enable new prefab
        if (step.swapTruckPrefabs == true)
        {
            SwapTruckPrefabs();
        }   
    }

    private void CheckStepCompletion()
    {
        var step = stepSequence[currentStep];

        if (step.interactionType == Interactions.grab)
        {
            foreach (GameObject focusedObj in step.focusedObjs)
            {
                CustomVibrate vibrateComponent = focusedObj.GetComponent<CustomVibrate>();
                if (vibrateComponent != null )
                    vibrateComponent.enabled = true;


                if (focusedObj.GetComponent<InteractableObject>().wasGrabbed && focusedObj.GetComponent<InteractableObject>().shouldBeInteractedWith)
                {
                    interactionsCompleted += 1;
                    RemoveHighLight(focusedObj);
                    focusedObj.GetComponent<InteractableObject>().shouldBeInteractedWith = false;
                    if (vibrateComponent != null)
                    {
                        vibrateComponent.enabled = false;
                    }
                    
                }
            }
        }
        else if (step.interactionType == Interactions.touch)
        {
            foreach (GameObject focusedObj in step.focusedObjs)
            {
                CustomVibrate vibrateComponent = focusedObj.GetComponent<CustomVibrate>();
                if (vibrateComponent != null)
                    vibrateComponent.enabled = true;


                if (focusedObj.GetComponent<InteractableObject>().wasTouched && focusedObj.GetComponent<InteractableObject>().shouldBeInteractedWith)
                {
                    interactionsCompleted += 1;
                    RemoveHighLight(focusedObj);
                    focusedObj.GetComponent<InteractableObject>().shouldBeInteractedWith = false;
                    if (vibrateComponent != null)
                    {
                        vibrateComponent.enabled = false;
                    }

                }
            }
        }
        else if (step.interactionType == Interactions.sight)
        {
            foreach (GameObject focusedObj in step.focusedObjs)
            {
                var interactableObject = focusedObj.GetComponent<InteractableObject>();
                if (interactableObject == null) Debug.LogError("InteractableObject script not found on " + focusedObj.name);
                else if (interactableObject.sigthed && interactableObject.shouldBeInteractedWith)
                {
                    interactionsCompleted += 1;
                    //Play one shot scan complete sound
                    audioManager.PlayScanCompleteAudio();
                    RemoveHighLight(focusedObj);
                    interactableObject.shouldBeInteractedWith = false;
                }
            }
        }
        else if (step.interactionType == Interactions.placeObject)
        {
            if (step.focusedObjs.Length > 1) Debug.Log("Step interaction is `Place Object`, you can have just 1 focused Object");

            GameObject obgToPlace = step.focusedObjs[0];

            if (obgToPlace.GetComponent<InteractableObject>().wasGrabbed && obgToPlace.GetComponent<InteractableObject>().shouldBeInteractedWith)
            {
                RemoveHighLight(obgToPlace);
                MeshFilter meshFilter = obgToPlace.GetComponent<MeshFilter>();
                obgToPlace.GetComponent<InteractableObject>().shouldBeInteractedWith = false;

                step.defaultTag = obgToPlace.tag;
                obgToPlace.tag = "ObjectToBePlaced";

                step.positionToPlaceObject.GetComponent<MeshFilter>().mesh = meshFilter.mesh;
                step.positionToPlaceObject.AddComponent<Outline>();
                step.positionToPlaceObject.GetComponent<Outline>().OutlineWidth = 8;
            }

            if (step.positionToPlaceObject.GetComponent<ObjectPlacement_Script>().objectPlaced) //object isnt grabbed and entered area of placement
            {
                if (step.defaultTag != null) obgToPlace.tag = step.defaultTag;

                step.positionToPlaceObject.GetComponent<MeshFilter>().mesh = null;
                Destroy(step.positionToPlaceObject.GetComponent<Outline>());

                obgToPlace.transform.position = step.positionToPlaceObject.transform.position;
                obgToPlace.transform.rotation = step.positionToPlaceObject.transform.rotation;

                interactionsCompleted += 1;
            }
        }
        else if (step.interactionType == Interactions.button)
        {
            foreach (GameObject focusedObj in step.focusedObjs)
            {
                if (focusedObj != null)
                {
                    var interactableButton = focusedObj.GetComponent<InteractableButton>();
                    if (interactableButton.wasPressed)
                    {
                        interactionsCompleted += 1;
                        //Debug.Log("Button was pressed");
                        interactableButton.wasPressed = false;
                    }
                }
                else
                {
                    // Debug Error messages explaining this current step has a null focused obj
                    Debug.LogError("Null focused object in step " + currentStep);
                }
            }
        }

        if (interactionsCompleted == step.focusedObjs.Length)
        {
            Debug.Log($"Step {currentStep} Completed");
            if( currentStep < stepSequence.Length - 1)
            {
                currentStep += 1;
            }
            else
            {
                Debug.Log("All steps completed");
                NextSection();
            }
        }
    }

    private void RemoveHighLight(GameObject objectToRemove)
    {
        if(objectToRemove.transform.Find("Highlight") != null)
        {
            Destroy(objectToRemove.transform.Find("Highlight").gameObject);
        }
        
    }

    private void HidePreviousUIParent()
    {
        if (previousUIParent != null)
            previousUIParent.SetActive(false);
    }

    private void ShowCurrentUIParent(GameObject currentUIParent)
    {
        if (currentUIParent != null)
        {
            currentUIParent.SetActive(true);
            previousUIParent = currentUIParent;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (drawGizmos)
        {
          if (stepSequence[currentStep] == null) return; // return if there is no step selected   
                var step = stepSequence[currentStep];

                //if (!step.aiPresence.shouldSpawnUI) return; // return if step doesn't has AI

                // Save the current Gizmos matrix
                Matrix4x4 gizmoMatrix = Gizmos.matrix;

                // Calculate the position relative to the world
                Vector3 worldPosition = step.aiPresence.AIPosition;

                // Apply the desired rotation to the Gizmo matrix
                Gizmos.matrix = Matrix4x4.TRS(worldPosition, step.aiPresence.AIRotation, Vector3.one);

                Vector3 size = new Vector3(1, 1, 1);
                if ( AIPrefab != null)
                {
                    if (AIPrefab.transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>() != null)
                    {
                        SkinnedMeshRenderer meshRender = AIPrefab.transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>();

                        // Calculate the size of the cube based on the RectTransform size
                        size.x = meshRender.bounds.size.x;
                        size.y = meshRender.bounds.size.y*2;
                        size.z = meshRender.bounds.size.z;
                        //size = rectTransform.rect.size;
                    }
                }

                //set color of gizmos
                Color gizmoColor = Color.red;
                gizmoColor.a = 0.5f; 
                Gizmos.color = gizmoColor;


                // Draw the Gizmo cube centered at the origin
                Gizmos.DrawCube(Vector3.zero + new Vector3(0,1,0), new Vector3(size.x, size.y, size.z));

                // Calculate the position of the second cube (on the front face)
                Vector3 secondCubePosition = new Vector3(0, 0f, size.z / 2f);

                // Set color for the second cube
                Color secondCubeColor = Color.blue;
                secondCubeColor.a = 0.5f;
                Gizmos.color = secondCubeColor;

                // Draw the second cube
                Gizmos.DrawCube(secondCubePosition, new Vector3(size.x / 2f, size.y / 2f, size.z / 2f));

                // Restore the original Gizmos matrix
                Gizmos.matrix = gizmoMatrix;
        }
    }

    public void NextStep()
    {
        if (currentStep < stepSequence.Length - 1)
        {
            currentStep++;
            ResetStep();
        }
        else if (currentStep >= stepSequence.Length - 1)
        {
            NextSection();
        }
    }


    private void NextSection()
    {
        Debug.Log("Moving onto Next Scene");
        SceneManagement sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManagement>();

        // Get the current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Calculate the index of the next scene
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if the next scene index is within the range of available scenes
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Load the next scene
            sceneManager.LoadNextScene();
        }
        else
        {
            // There is no next scene (end of the scenes)
            Debug.Log("No next scene available. You've reached the end.");
        }
    }


    public void PrevStep()
    {
        if (currentStep > 0)
        {
            currentStep--;
            ResetStep();
        }
    }

    private void ResetStep()
    {
        StopAllCoroutines();
        //SceneView.RepaintAll();
    }

    private void DebugStepNavigation()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextStep();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PrevStep();
        }
    }

    private void SwapTruckPrefabs()
    {
        if (truckExteriorPrefab.activeSelf)
        {
            truckExteriorPrefab.SetActive(false);
            truckInteriorPrefab.SetActive(true);
        }
        else if (truckInteriorPrefab.activeSelf)
        {
            truckExteriorPrefab.SetActive(true);
            truckInteriorPrefab.SetActive(false);
        }
    }
}
