using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorPickerInputField : BlockSettingsUIInputField, IPointerClickHandler
{
    public override Type InputType => typeof(Color);
    private Texture2D tex;
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        tex = GetComponent<Image>().mainTexture as Texture2D;
    }

    public override void SetValue(object value)
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Math relies on pivot being (0, 0). If need to alter pivot, cursorPosition will need to be adjusted accordingly
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, eventData.position,
            eventData.pressEventCamera, out Vector2 cursorPosition);

        cursorPosition.x = cursorPosition.x / rect.rect.width * tex.width;
        cursorPosition.y = cursorPosition.y / rect.rect.height * tex.height;
        Color color = tex.GetPixel((int)cursorPosition.x, (int)cursorPosition.y);
        if (color.a == 0f) { return; }
        OnFieldChangedCallback(color);
        
    }

}
