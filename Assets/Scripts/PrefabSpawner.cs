using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    [SerializeField]
    Stub stubPrefab = default;
    public Stub StubPrefab => stubPrefab;    

}
