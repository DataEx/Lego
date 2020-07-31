using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField]
    private BlockSpecs blockSpecification;
    private MaterialPropertyBlock mpb;
    private float materialAlpha = 1f;

    public void CreateBlockFromSpecification(BlockSpecs specification) {
        blockSpecification = specification;
        UpdateBlock();
    }

    [ContextMenu("Create Block")]
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
        SetColor();
    }

    private void Start()
    {
        UpdateBlock();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) {
            SetAlpha(0.5f);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetAlpha(1f);
        }
    }

    public void SetAlpha(float alpha)
    {
        materialAlpha = alpha;
        SetColor();
    }

    private void SetColor() {
        Color color = blockSpecification.color;
        color.a = materialAlpha;
        mpb.SetColor("_BaseColor", color);

        for (int i = 0; i < transform.childCount; i++)
        {
            Stub stub = transform.GetChild(i).GetComponent<Stub>();
            stub.SetMaterialBlock(mpb);
        }
    }


}
