using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColorPickerInputField : BlockSettingsUIInputField<Color>
{
    public override void AddOnFieldChangeHandler(UnityAction<Color> action)
    {
        throw new System.NotImplementedException();
    }

    public override void SetField(Color value)
    {

    }


}
