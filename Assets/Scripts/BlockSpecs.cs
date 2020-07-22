using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpecs : ScriptableObject
{
    public string displayName = "New Block";
    public int width;
    public int length;
    public Color color = Color.yellow;

    public void OnValidate()
    {
        width = Mathf.Max(1, width);
        length = Mathf.Max(1, length);
        color.a = 1f;
    }
}
