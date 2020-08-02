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

    private void Awake()
    {
        activeCamera = Camera.main;
        layerMask = ~LayerMask.GetMask("Ignore Raycast");
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

        /*
        Vector3 halfExtents = (Vector3)previewBlock.Extents * 0.5f;
        if (Physics.BoxCast(ray.origin, halfExtents, Vector3.down, out RaycastHit hit,
            Quaternion.Euler(previewBlock.BlockOrientation.ToRotation()), 100f, layerMask)) { 
        */
            
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, layerMask))
        {
            Stub stub = hit.transform.GetComponent<Stub>();
            if (stub == null) { return BlockPlacementPermission.OutOfRange; }
            coordinateToPlace = WorldGrid.WorldPositionToCoordinate(stub.transform.position) + Vector3Int.up;
            Debug.Log("try : " + coordinateToPlace, stub);
            BlockPlacementPermission canPlace = WorldGrid.CanPlace(previewBlock, coordinateToPlace);
            while (canPlace == BlockPlacementPermission.Occupied) {
                coordinateToPlace += Vector3Int.up;
                Debug.Log("try : " + coordinateToPlace, stub);
                canPlace = WorldGrid.CanPlace(previewBlock, coordinateToPlace);
            }
            if (canPlace == BlockPlacementPermission.Valid) {
                Debug.Log("can : " + coordinateToPlace, stub);
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
