using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public abstract class BlockSettingsUIInputField : MonoBehaviour
{
    public abstract Type InputType { get;}
    public abstract void SetValue(object value);

    public class OnFieldChanged : UnityEvent<object> { }
    public OnFieldChanged OnFieldChangedEvent = new OnFieldChanged();

    protected virtual void OnFieldChangedCallback(object value)
    {
        OnFieldChangedEvent.Invoke(value);
    }
}
