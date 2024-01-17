using UnityEngine;

public class ObjectAnimatorController : MonoBehaviour
{
    public Animator animator;
    public enum ParameterOption { Horz, Vert }
    public ParameterOption selectedParameter;

    private int horzHash;
    private int vertHash;

    private void Start()
    {
        horzHash = Animator.StringToHash("Horz");
        vertHash = Animator.StringToHash("Vert");
    }

    private void Update()
    {
        switch (selectedParameter)
        {
            case ParameterOption.Horz:
                animator.SetTrigger(horzHash);
                break;
            case ParameterOption.Vert:
                animator.SetTrigger(vertHash);
                break;
            default:
                Debug.LogWarning("Invalid parameter option");
                break;
        }
    }
}
