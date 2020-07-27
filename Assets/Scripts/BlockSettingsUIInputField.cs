using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BlockSettingsUIInputField : MonoBehaviour
{
    public abstract void SetField(T value);
    public abstract void AddOnFieldChangeHandler(UnityAction<T> action);

}
