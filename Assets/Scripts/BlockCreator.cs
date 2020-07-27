using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCreator : MonoBehaviour
{
    [SerializeField]
    private BlockSpecs blockSpecs;
    private BlockSpecs currentBlockSpecs;


    public void CreateNewBlock() {
        UnsubscribeBlockSpecs();
        blockSpecs = ScriptableObject.CreateInstance<BlockSpecs>();
        currentBlockSpecs = blockSpecs;
        SubscribeBlockSpecs();
        currentBlockSpecs.OnValidate();
    }

    public void SaveBlock()
    {
        UnityEditor.AssetDatabase.CreateAsset(currentBlockSpecs, $"Assets/Prefabs/Blocks/{blockSpecs.displayName}.asset");
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
    }

    private void Update()
    {
        CheckForSpecsFileChange();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            // TODO: Create save function callable with button or menu option?
            SaveBlock();
        }

        // TODO allow user to rotate the block

        // TODO Maybe don't let them touch the file directly but create menu options that can be modified and then saved to file?
    }

    private void OnSpecsUpdate() {
        // TODO: Create block
        print("make me a block with " + currentBlockSpecs.length);
    }

    private void CheckForSpecsFileChange()
    {
        if (currentBlockSpecs != blockSpecs)
        {
            UnsubscribeBlockSpecs();
            currentBlockSpecs = blockSpecs;
            SubscribeBlockSpecs();
        }
    }

    private void UnsubscribeBlockSpecs()
    {
        if (currentBlockSpecs != null)
        {
            currentBlockSpecs.OnSpecsUpdate -= OnSpecsUpdate;
        }
    }

    private void SubscribeBlockSpecs()
    {
        if (currentBlockSpecs != null)
        {
            currentBlockSpecs.OnSpecsUpdate += OnSpecsUpdate;
        }
    }

    private void OnDestroy()
    {
        if (currentBlockSpecs != null) {
            currentBlockSpecs.OnSpecsUpdate -= OnSpecsUpdate;
        }
    }
}
