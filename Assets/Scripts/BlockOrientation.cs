using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockOrientation { // Clockwise
    Normal, QuarterTurn, HalfTurn, ThreeQuaterTurn
}

public static class BlockOrientationHelper {
    public static Vector3Int TransformCoordinate(Vector3Int coordinate, BlockOrientation orientation) {
        Vector3Int transformedCoordinate = Vector3Int.zero;
        switch (orientation)
        {
            case BlockOrientation.Normal:
                transformedCoordinate = coordinate;
                break;
            case BlockOrientation.QuarterTurn:
                transformedCoordinate = new Vector3Int(coordinate.z, coordinate.y, -coordinate.x);
                break;
            case BlockOrientation.HalfTurn:
                transformedCoordinate = new Vector3Int(-coordinate.x, coordinate.y, -coordinate.z);
                break;
            case BlockOrientation.ThreeQuaterTurn:
                transformedCoordinate = new Vector3Int(-coordinate.z, coordinate.y, coordinate.x);
                break;
        }
        return transformedCoordinate;
    }

    public static BlockOrientation Next(this BlockOrientation orientation) {
        int enumLength = System.Enum.GetNames(typeof(BlockOrientation)).Length; ;
        int index = (int)orientation + 1;
        if(index >= enumLength) { index = 0; }
        return (BlockOrientation)index;
    }

    public static BlockOrientation Previous(this BlockOrientation orientation)
    {
        int enumLength = System.Enum.GetNames(typeof(BlockOrientation)).Length; ;
        int index = (int)orientation - 1;
        if (index < 0) { index = enumLength - 1; }
        return (BlockOrientation)index;
    }

    public static Vector3 ToRotation(this BlockOrientation orientation) {
        Vector3 rotation = Vector3.zero;
        switch (orientation)
        {
            case BlockOrientation.Normal:
                rotation = Vector3.zero;
                break;
            case BlockOrientation.QuarterTurn:
                rotation = new Vector3(0f, 90f, 0f);
                break;
            case BlockOrientation.HalfTurn:
                rotation = new Vector3(0f, 180f, 0f);
                break;
            case BlockOrientation.ThreeQuaterTurn:
                rotation = new Vector3(0f, 270f, 0f);
                break;
        }
        return rotation;
    }
}