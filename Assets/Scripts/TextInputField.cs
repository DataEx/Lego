using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TextInputField : BlockSettingsUIInputField
{
    public override Type InputType => typeof(string);

    private InputField inputField;

    private void Awake()
    {
        inputField = GetComponent<InputField>();
        inputField.onEndEdit.AddListener(OnEditEnd);
    }

    public override void SetValue(object value)
    {
        inputField.text = value.ToString();
    }

    private void OnEditEnd(string s) {
        OnFieldChangedCallback(s);
    }
}
