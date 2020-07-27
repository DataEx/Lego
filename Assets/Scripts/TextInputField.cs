using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TextInputField : BlockSettingsUIInputField<string>
{
    private InputField inputField;

    public override void AddOnFieldChangeHandler(UnityAction<string> action)
    {
        inputField.onEndEdit.AddListener(action);
    }

    public override void SetField(string value)
    {
        inputField.text = value;
    }

    private void Awake()
    {
        inputField = GetComponent<InputField>();
    }
}
