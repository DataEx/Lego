using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCreator : MonoBehaviour
{
    [SerializeField]
    private BlockSpecs blockSpecs;

    // Start is called before the first frame update
    void Start()
    {
        blockSpecs = ScriptableObject.CreateInstance<BlockSpecs>();
        blockSpecs.OnValidate();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) {
            Save();
        }
    }

    private void Save() {
        UnityEditor.AssetDatabase.CreateAsset(blockSpecs, $"Assets/Prefabs/Blocks/{blockSpecs.displayName}.asset");
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
    }
}
