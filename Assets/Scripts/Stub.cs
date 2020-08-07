using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stub : MonoBehaviour
{
    [SerializeField]
    private Nub nub = default;

    private Renderer[] renderers;
    private MaterialPropertyBlock mpb;
    public Block Block { get; set; }
    private Vector3Int localBlockCoordinate;
    public Vector3Int BlockCoordinate { get; set; }

    private void Awake()
    {
        InitializeVariables();
    }

    public void SetLocalBlockCoordinate(Vector3Int coordinate)
    {
        localBlockCoordinate = coordinate;
        name = coordinate.ToString();
    }

    public Vector3Int GetLocalBlockCoordinate() {
        return localBlockCoordinate;
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

    public void SetLayer(int layer) {
        gameObject.layer = layer;
        nub.gameObject.layer = layer;
    }
}
