using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    [SerializeField]
    Stub stubPrefab = default;

    public static Stub StubPrefab {
        get {
            if (Instance == null)
            {
                return FindObjectOfType<PrefabSpawner>().stubPrefab;
            }
            else {
                return Instance.stubPrefab;
            }
        }
    } 

    public static PrefabSpawner Instance;

    private void Awake()
    {
        Instance = this;
    }
}
