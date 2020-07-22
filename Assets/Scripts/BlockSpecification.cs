using UnityEngine;

[System.Serializable]
public struct BlockSpecification
{
    public int width;
    public int length;
    public Color color;

    public void OnValidate() {
        width = Mathf.Max(1, width);
        length = Mathf.Max(1, length);
        color.a = 1f;
    }
}
