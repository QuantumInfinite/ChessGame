using UnityEngine;
using System.Collections;

public class Example : MonoBehaviour
{
    public UnityEngine.UI.InputField mainInputField;
    private string inputValue;

    public void Start()
    {
        inputValue = mainInputField.text;
        mainInputField.onValueChanged.AddListener(OnInputValueChanged);
    }

    // Invoked when the value of the text field changes.
    public void OnInputValueChanged(string newValue)
    {
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && (Input.GetKey(KeyCode.V) || Input.GetKeyUp(KeyCode.V)))
        {
            Debug.LogError("Not allowed");
            mainInputField.onValueChanged.RemoveListener(OnInputValueChanged);
            mainInputField.text = inputValue;
            mainInputField.onValueChanged.AddListener(OnInputValueChanged);
        }
        else
        {
            inputValue = mainInputField.text;
        }
    }
}