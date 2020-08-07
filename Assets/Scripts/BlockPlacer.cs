using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlacer : MonoBehaviour
{
    [SerializeField]
    private BlockSpecs[] blockPrefabs = default;
    private int prefabIndex = 0;

    private Block previewBlock;
    private Camera activeCamera;
    private LayerMask layerMask;

    //GameObject g;

    private void Awake()
    {
        activeCamera = Camera.main;
        layerMask = ~LayerMask.GetMask("Ignore Raycast");
        /*
        g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        g.transform.localScale = 0.4f * Vector3.one;
        g.GetComponent<Collider>().enabled = false;
        */
    }

    private void Start()
    {
        CreatePreviewBlock();
    }

    private void CreatePreviewBlock()
    {
        if(previewBlock != null) { Destroy(previewBlock.gameObject); }

        previewBlock = CreateBlock();
        previewBlock.SetAlpha(0.5f);
        previewBlock.SetLayer(LayerMask.NameToLayer("Ignore Raycast"));
    }

    private Block CreateBlock()
    {
        GameObject go = new GameObject("Preview");
        Block block = go.AddComponent<Block>();
        block.CreateBlockFromSpecification(blockPrefabs[prefabIndex]);
        return block;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            prefabIndex = (prefabIndex + 1) % blockPrefabs.Length;
            CreatePreviewBlock();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            prefabIndex = (prefabIndex - 1);
            if(prefabIndex < 0) { prefabIndex = blockPrefabs.Length - 1; }
            CreatePreviewBlock();
        }


        if (Input.GetKeyDown(KeyCode.R)) {
            RotateClockwise();
        }

        BlockPlacementPermission canPlace = PreviewPlacement(out Vector3Int coordinateToPlace);
        if (canPlace == BlockPlacementPermission.Valid)
        {
            previewBlock.gameObject.SetActive(true);
            if (Input.GetMouseButtonDown(0))
            {
                PlaceBlockAt(coordinateToPlace);
            }
        }
        else {
            previewBlock.gameObject.SetActive(false);

        }
    }

    private void RotateClockwise() {
        previewBlock.Rotate(previewBlock.BlockOrientation.Next());
    }

    private void RotateCounterClockwise()
    {
        previewBlock.Rotate(previewBlock.BlockOrientation.Previous());
    }


    private BlockPlacementPermission PreviewPlacement(out Vector3Int coordinateToPlace) {
        coordinateToPlace = Vector3Int.zero;

        if (UIHelper.IsMouseOverUI()) { return BlockPlacementPermission.OutOfRange; }

        Ray ray = activeCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, layerMask))
        {
            Stub stub = hit.transform.GetComponent<Stub>();
            if (stub == null) { return BlockPlacementPermission.OutOfRange; }
            //Debug.Log("hit stub: " + stub.name);
            //g.transform.position = hit.point;
            coordinateToPlace = stub.BlockCoordinate + Vector3Int.up;
            BlockPlacementPermission canPlace = WorldGrid.CanPlace(previewBlock, coordinateToPlace);
            //Debug.Log("try : " + coordinateToPlace + ": " + canPlace.ToString(), stub);
            while (canPlace == BlockPlacementPermission.Occupied) {
                coordinateToPlace += Vector3Int.up;
                //Debug.Log("try : " + coordinateToPlace, stub);
                canPlace = WorldGrid.CanPlace(previewBlock, coordinateToPlace);
            }
            if (canPlace == BlockPlacementPermission.Valid) {
                //Debug.Log("can : " + coordinateToPlace, stub);
                Vector3 previewPosition = WorldGrid.CoordinateToWorldPosition(coordinateToPlace);
                previewBlock.transform.position = previewPosition;
            }
            return canPlace;
        }
        return BlockPlacementPermission.OutOfRange;
    }

    private void PlaceBlockAt(Vector3Int coordinate) {
        Block block = CreateBlock();
        block.Rotate(previewBlock.BlockOrientation);
        block.name = "Block";
        WorldGrid.Place(block, coordinate);
    }
}
