using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlacer : MonoBehaviour
{
    [SerializeField]
    private BlockSpecs blockToPlace = default;
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
        previewBlock = CreatePreviewBlock();

        foreach (Transform t in previewBlock.transform)
        {
            t.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }

    private Block CreatePreviewBlock()
    {
        GameObject go = new GameObject("Preview");
        Block block = go.AddComponent<Block>();
        block.CreateBlockFromSpecification(blockToPlace);
        return block;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            RotateClockwise();
        }

        BlockPlacementPermission canPlace = PreviewPlacement(out Vector3Int coordinateToPlace);
        if (canPlace == BlockPlacementPermission.Valid && Input.GetMouseButtonDown(0))
        {
            PlaceBlockAt(coordinateToPlace);
        }
    }

    private void RotateClockwise() {
        previewBlock.Rotate(previewBlock.BlockOrientation.Next());
    }

    private void RotateCounterClockwise()
    {

    }


    private BlockPlacementPermission PreviewPlacement(out Vector3Int coordinateToPlace) {
        coordinateToPlace = Vector3Int.zero;
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
        Block block = CreatePreviewBlock();
        block.Rotate(previewBlock.BlockOrientation);
        block.name = "Block";
        WorldGrid.Place(block, coordinate);
    }
}
