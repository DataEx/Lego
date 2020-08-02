using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField]
    private BlockSpecs blockSpecification;
    private MaterialPropertyBlock mpb;
    private float materialAlpha = 1f;
    private Stub[] stubs = null;
    private Vector3Int[] localBlockCoordinates = null;
    public BlockOrientation BlockOrientation { get; private set; } = BlockOrientation.Normal;

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

        stubs = new Stub[blockSpecification.width * blockSpecification.length];
        localBlockCoordinates = new Vector3Int[stubs.Length];
        print("create stubs");
        Vector3Int blockCoordinate = Vector3Int.zero;
        int index = 0;
        for (int i = 0; i < blockSpecification.length; i++)
        {
            blockCoordinate.z = i;
            for (int j = 0; j < blockSpecification.width; j++)
            {
                blockCoordinate.x = j;

                Stub stub = Instantiate(stubPrefab);
                stub.transform.parent = stubParent;
                Vector3 stubPosition = new Vector3(stubBounds.size.x * j, 0f, stubBounds.size.z * i);
                stub.transform.localPosition = stubPosition;
                stub.transform.localRotation = Quaternion.identity;
                stub.Block = this;
                stub.SetMaterialBlock(mpb);
                stub.SetLocalBlockCoordinate(blockCoordinate);

                stubs[index] = stub;
                localBlockCoordinates[index] = blockCoordinate;
                index++;
            }
        }
        SetColor();
    }

    private void Awake()
    {
        if (blockSpecification != null) {
            UpdateBlock();
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

        for (int i = 0; i < stubs.Length; i++)
        {
            stubs[i].SetMaterialBlock(mpb);
        }
    }

    /*
    public Vector3Int[] GetLocalBlockCoordinates() {
        return localBlockCoordinates;
    }
    */
    public Stub[] GetStubs() {
        return stubs;
    }

    public void Rotate(BlockOrientation orientation) {
        BlockOrientation = orientation;
        transform.eulerAngles = orientation.ToRotation();
    }
}
