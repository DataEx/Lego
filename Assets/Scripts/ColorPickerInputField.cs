using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColorPickerInputField : BlockSettingsUIInputField
{
    public override Type InputType => typeof(Color);

    public override void SetValue(object value)
    {
    }
}
