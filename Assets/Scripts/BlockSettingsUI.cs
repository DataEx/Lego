using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class BlockSettingsUI : MonoBehaviour
{
    [SerializeField]
    private Text parameterNameLabel = default;

    [SerializeField]
    private BlockSettingsUIInputField inputField = default;

    protected FieldInfo fieldInfo;
    protected Type fieldType;
    protected object defaultValue;

    protected class SettingValueChanged : UnityEvent<BlockSettingsUI, object, FieldInfo> { }
    protected SettingValueChanged OnSettingValueChanged = new SettingValueChanged();

    public void SetFieldInfo(FieldInfo fieldInfo) {
        this.fieldInfo = fieldInfo;
        fieldType = fieldInfo.FieldType;
        SetParameterName(fieldInfo.Name);
    }

    public void SetDefaultValue(object value)
    {
        defaultValue = value;
        SetValue(value);
    }

    public void SetValue(object value) {
        inputField.SetValue(value);
    }

    public void ResetToDefault() {
        SetValue(defaultValue);
    }

    public void AddOnSettingValueChangedListener(UnityAction<BlockSettingsUI, object, FieldInfo> action) {
        OnSettingValueChanged.AddListener(action);
    }

    private void Start()
    {
        inputField.OnFieldChangedEvent.AddListener(OnFieldChanged);
    }

    private void OnFieldChanged(object value) {
        OnSettingValueChanged.Invoke(this, value, fieldInfo);
    }

    private void SetParameterName(string parameterName) {
        // Convert parameter name to something readable (assuming camelcase as naming convention)
        StringBuilder sb = new StringBuilder();       
        for (int i = 0; i < parameterName.Length; i++)
        {
            if (i == 0)
            {
                sb.Append(parameterName[i].ToString().ToUpper());
            }
            else {
                char c = parameterName[i];
                // Add space between words
                if (char.IsUpper(c)) {
                    sb.Append(" ");
                }
                sb.Append(c.ToString());
            }
        }

        parameterNameLabel.text = sb.ToString();
    }
}
