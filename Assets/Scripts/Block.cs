using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField]
    private BlockSpecification blockSpecification;

    private MaterialPropertyBlock mpb;

    private void OnValidate()
    {
        blockSpecification.OnValidate();
        if(Application.isPlaying) { return; }
        EditorApplication.delayCall += () =>
        {
            if (this == null) { return; }
            CreateBlockFromSpecification();
        };
    }

    private void Start()
    {
        CreateBlockFromSpecification();
    }


    public void CreateBlockFromSpecification(BlockSpecification specification) {
        blockSpecification = specification;
        CreateBlockFromSpecification();
    }

    private void CreateBlockFromSpecification() {
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
                stub.SetMaterialBlock(mpb);
            }
        }
        mpb.SetColor("_BaseColor", blockSpecification.color);
    }


    [MenuItem("GameObject/Block", false, 10)]
    static void CreateCustomGameObject(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject go = new GameObject("Block");
        go.AddComponent<Block>();

        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
}
