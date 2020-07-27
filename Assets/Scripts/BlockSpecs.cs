using UnityEngine;

public class BlockSpecs : ScriptableObject
{
    public string displayName = "New Block";
    public int width;
    public int length;
    public Color color = Color.yellow;

    public delegate void OnSpecsUpdateDelegate();
    public OnSpecsUpdateDelegate OnSpecsUpdate = delegate { };

    public void OnValidate()
    {
        width = Mathf.Max(1, width);
        length = Mathf.Max(1, length);
        color.a = 1f;
        if (Application.isPlaying) {
            OnSpecsUpdate.Invoke();
        }
    }


}
