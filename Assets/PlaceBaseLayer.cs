using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBaseLayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Block baseLayer = GetComponent<Block>();
        WorldGrid.Place(baseLayer, Vector3Int.zero);
    }
}
