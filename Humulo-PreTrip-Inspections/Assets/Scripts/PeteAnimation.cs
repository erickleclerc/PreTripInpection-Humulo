using UnityEngine;

public class PeteAnimation : MonoBehaviour
{
    public StepManager stepManager;
    [SerializeField] private AudioSource audioSource;

    private int randomAnimation = 0;
    private bool canTalk = true;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (audioSource.isPlaying && canTalk)
        {
            canTalk = false;
            SelectAnimation();
        }
        else if (audioSource.isPlaying && !canTalk)
        {
            canTalk = false;
        }
        else
        { 
        randomAnimation = 0;
            animator.SetInteger("talkingInt", randomAnimation);
            canTalk = true;
        }

       if(stepManager.stepSequence[stepManager.currentStep].interactionType == StepManager.Interactions.clipboardCheck)
        {
            //animator.SetBool("isTalking", false);
            animator.SetBool("isUsingClipboard", true);
        }
        else
        {
            animator.SetBool("isUsingClipboard", false);
        }
    }

    private void SelectAnimation()
    {
        randomAnimation = Random.Range(1, 4);
        animator.SetInteger("talkingInt", randomAnimation);
    }
}
