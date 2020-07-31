using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Blockk : MonoBehaviour
{
    private BlockSpecs blockSpecification;
    private MaterialPropertyBlock mpb;

    public void CreateBlockFromSpecification(BlockSpecs specification) {
        blockSpecification = specification;
        UpdateBlock();
    }

    public void UpdateBlock() {
        Transform stubParent = transform;
        Stub stubPrefab = FindObjectOfType<PrefabSpawner>().StubPrefab;
        Bounds stubBounds = stubPrefab.GetComponent<Renderer>().bounds;
        if(mpb == null) {
            mpb = new MaterialPropertyBlock();
        }

        for (int i = stubParent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(stubParent.GetChild(i).gameObject);
        }

        mpb.SetColor("_BaseColor", blockSpecification.color);
        for (int i = 0; i < blockSpecification.length; i++)
        {
            for (int j = 0; j < blockSpecification.width; j++)
            {
                Stub stub = Instantiate(stubPrefab.gameObject).GetComponent<Stub>();
                stub.transform.parent = stubParent;
                Vector3 stubPosition = new Vector3(stubBounds.size.x * j, 0f, stubBounds.size.z * i);
                stub.transform.localPosition = stubPosition;
                stub.transform.localRotation = Quaternion.identity;
                stub.SetMaterialBlock(mpb);
            }
        }
    }
}
