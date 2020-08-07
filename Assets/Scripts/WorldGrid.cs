using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGrid : MonoBehaviour
{
    [SerializeField]
    private Vector3Int dimensions;
    private static Vector3Int gridDimensions;

    private static bool[] grid;
    private static Vector3 origin;
    private static float stubSize = 1f;
    private static Vector3 OriginOffset => origin + Vector3.one * stubSize * 0.5f;

    private void Awake()
    {
        grid = new bool[dimensions.x * dimensions.y * dimensions.z];
        origin = transform.position;
        gridDimensions = dimensions;
    }

    public static Vector3 CoordinateToWorldPosition(Vector3Int coordinate) {
        Vector3 position = OriginOffset + (Vector3)coordinate * stubSize;
        return position;
    }

    public static Vector3Int WorldPositionToCoordinate(Vector3 worldPosition) {
//        Something wrong...
        worldPosition -= OriginOffset;
        Vector3Int coordinate = new Vector3Int((int)worldPosition.x, (int)worldPosition.y, (int)worldPosition.z);
        return coordinate;
    }


    private static bool IsOccupied(Vector3Int coordinate) {
        int index = CoordinateToIndex(coordinate);
        return grid[index];
    }

    private static void SetCoordinate(Vector3Int coordinate, bool enabled)
    {
        int index = CoordinateToIndex(coordinate);
        grid[index] = enabled;
    }

    private static int CoordinateToIndex(Vector3Int coordinate)
    {
        int index = coordinate.x + gridDimensions.x * (coordinate.y + gridDimensions.y * coordinate.z);
        return index;
    } 

    public static BlockPlacementPermission CanPlace(Block block, Vector3Int baseCoordinate)
    {
        Stub[] stubs = block.GetStubs();
        for (int i = 0; i < stubs.Length; i++)
        {
            Vector3Int localCoordinate = stubs[i].GetLocalBlockCoordinate();
            Vector3Int worldCoordinate = baseCoordinate + 
                BlockOrientationHelper.TransformCoordinate(localCoordinate, block.BlockOrientation);
            if (!IsValidCoordinate(worldCoordinate)) {
                //Debug.LogError($"Out of range: At {baseCoordinate}, cannot place local: ({localCoordinate} | {stubs[i].name}) {i} at {worldCoordinate}");
                return BlockPlacementPermission.OutOfRange;
            }
            if (IsOccupied(worldCoordinate)) {
                //Debug.LogError($"Occupied: At {baseCoordinate}, cannot place local:{localCoordinate} {i} at {worldCoordinate}");
                return BlockPlacementPermission.Occupied;
            }
        }
        return BlockPlacementPermission.Valid;
    }

    public static void Place(Block block, Vector3Int baseCoordinate)//, Orientation)
    {
        if(CanPlace(block, baseCoordinate) != BlockPlacementPermission.Valid) { return; }

        Stub[] stubs = block.GetStubs();
        for (int i = 0; i < stubs.Length; i++)
        {
            Vector3Int localCoordinate = stubs[i].GetLocalBlockCoordinate();
            Vector3Int worldCoordinate = baseCoordinate + 
                BlockOrientationHelper.TransformCoordinate(localCoordinate, block.BlockOrientation);
            SetCoordinate(worldCoordinate, true);
            stubs[i].name = worldCoordinate.ToString();
            stubs[i].BlockCoordinate = worldCoordinate;
        }
        block.transform.position = CoordinateToWorldPosition(baseCoordinate);
    }

    private static bool IsValidCoordinate(Vector3Int coordinate) {
        if(coordinate.x < 0 || coordinate.y < 0 || coordinate.z < 0) { return false; }
        if(coordinate.x >= gridDimensions.x || coordinate.y >= gridDimensions.y || coordinate.z >= gridDimensions.z) { return false; }
        return true;
    }

    IEnumerator FillGrid() {
        Vector3Int coordinate = Vector3Int.zero;
        for (int y = 0; y < dimensions.y; y++)
        {
            coordinate.y = y;
            for (int x = 0; x < dimensions.x; x++)
            {
                coordinate.x = x;
                for (int z = 0; z < dimensions.z; z++)
                {
                    coordinate.z = z;
                    Vector3 spawnPosition = CoordinateToWorldPosition(coordinate);
                    GameObject obj = Instantiate(PrefabSpawner.StubPrefab.gameObject, spawnPosition, Quaternion.identity);
                    yield return null;
                }
            }
        }

    }
}
