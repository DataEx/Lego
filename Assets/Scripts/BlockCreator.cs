using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class BlockCreator : MonoBehaviour
{
    [SerializeField]
    private BlockSpecs blockSpecs;
    public BlockSpecs BlockSpecs { get; private set; }

    [Space(15)]
    [SerializeField]
    private UnityEvent onCreateNewBlock;

    private void Awake()
    {
        CreateNewBlock();
    }

    public void CreateNewBlock() {
        UnsubscribeBlockSpecs();
        blockSpecs = ScriptableObject.CreateInstance<BlockSpecs>();
        BlockSpecs = blockSpecs;
        SubscribeBlockSpecs();
        BlockSpecs.OnValidate();
        onCreateNewBlock.Invoke();
    }

    public void SaveBlock()
    {
        UnityEditor.AssetDatabase.CreateAsset(BlockSpecs, $"Assets/Prefabs/Blocks/{blockSpecs.displayName}.asset");
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
        print("make me a block with " + BlockSpecs.length);
    }

    private void CheckForSpecsFileChange()
    {
        if (BlockSpecs != blockSpecs)
        {
            UnsubscribeBlockSpecs();
            BlockSpecs = blockSpecs;
            SubscribeBlockSpecs();
        }
    }

    private void UnsubscribeBlockSpecs()
    {
        if (BlockSpecs != null)
        {
            BlockSpecs.OnSpecsUpdate -= OnSpecsUpdate;
        }
    }

    private void SubscribeBlockSpecs()
    {
        if (BlockSpecs != null)
        {
            BlockSpecs.OnSpecsUpdate += OnSpecsUpdate;
        }
    }

    private void OnDestroy()
    {
        if (BlockSpecs != null) {
            BlockSpecs.OnSpecsUpdate -= OnSpecsUpdate;
        }
    }
}
