using UnityEngine;
using UnityEngine.Events;

public class ButtonFunction : MonoBehaviour
{
    public UnityEvent ButtonPressed = new UnityEvent();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonPressedCall()
    {
        ButtonPressed.Invoke();
    }

}
