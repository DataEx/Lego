using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stub : MonoBehaviour
{
    [SerializeField]
    private Nub nub;

    private Renderer[] renderers;
    private MaterialPropertyBlock mpb;

    private void Awake()
    {
        InitializeVariables();
    }

    public void SetMaterialBlock(MaterialPropertyBlock mpb)
    {
        InitializeVariables();
        foreach (var r in renderers)
        {
            r.SetPropertyBlock(mpb);
        }
    }

    private void InitializeVariables() {
        if (renderers == null)
        {
            renderers = GetComponentsInChildren<Renderer>();
        }
    }
}
