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

        bool canPlace = PreviewPlacement(out Vector3Int coordinateToPlace);
        if (canPlace && Input.GetMouseButtonDown(0))
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


    private bool PreviewPlacement(out Vector3Int coordinateToPlace) {
        coordinateToPlace = Vector3Int.zero;
        Ray ray = activeCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, layerMask))
        {
            Stub stub = hit.transform.GetComponent<Stub>();
            if (stub == null) { return false; }
            Block block = stub.Block;
            coordinateToPlace = WorldGrid.WorldPositionToCoordinate(stub.transform.position) + Vector3Int.up;
            Debug.Log(coordinateToPlace, stub);
            bool canPlace = WorldGrid.CanPlace(previewBlock, coordinateToPlace);
            if (canPlace) {
                Vector3 previewPosition = WorldGrid.CoordinateToWorldPosition(coordinateToPlace);
                previewBlock.transform.position = previewPosition;
            }
            return canPlace;
        }
        return false;
    }

    private void PlaceBlockAt(Vector3Int coordinate) {
        Block block = CreatePreviewBlock();
        block.Rotate(previewBlock.BlockOrientation);
        block.name = "Block";
        WorldGrid.Place(block, coordinate);
    }
}
