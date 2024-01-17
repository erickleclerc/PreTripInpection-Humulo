using UnityEngine;
using UnityEngine.UI;

public class ButtonDebug : MonoBehaviour
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
        else
        {
            Debug.LogError("Button component not found on the GameObject.");
        }
    }

    private void OnButtonClick()
    {
        Debug.Log("Button clicked!");
    }
}
