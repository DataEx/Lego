using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public class BlockSpecsMenuCreator : MonoBehaviour
{
    [SerializeField]
    private BlockCreator blockCreator;

    [SerializeField]
    private TextUI inputFieldPrefab = default;

    [SerializeField]
    private ColorPickerUI colorFieldPrefab = default;

    private Dictionary<Type, BlockSettingsUI> menuCreatorDictionary;

    private void Awake()
    {
        menuCreatorDictionary = new Dictionary<Type, BlockSettingsUI>();
        menuCreatorDictionary[typeof(int)] = inputFieldPrefab;
        menuCreatorDictionary[typeof(float)] = inputFieldPrefab;
        menuCreatorDictionary[typeof(string)] = inputFieldPrefab;
        menuCreatorDictionary[typeof(Color)] = colorFieldPrefab;
    }

    void Start()
    {
        CreateFields();
    }

    private void CreateFields() {
        // For each parameter of BlockSpecs, create a field to enter data 
        var structType = typeof(BlockSpecs);
        FieldInfo[] fields = structType.GetFields();
        for (int i = 0; i < fields.Length; i++)
        {
            Type type = fields[i].FieldType;
            print($"{fields[i].Name} is a {fields[i].FieldType}");
            CreateField(fields[i]);
        }
    }

    private void CreateField(FieldInfo fieldInfo) {
        Type type = fieldInfo.FieldType;

        if (!menuCreatorDictionary.ContainsKey(type)){
            return;
        }
        BlockSettingsUI ui = Instantiate(menuCreatorDictionary[type]);
        ui.transform.SetParent(transform);
        ui.SetFieldInfo(fieldInfo);
        ui.SetDefaultValue(fieldInfo.GetValue(blockCreator.BlockSpecs));
        ui.AddOnSettingValueChangedListener(OnFieldChange);
    }

    public void OnFieldChange(BlockSettingsUI ui, object value, FieldInfo field)
    {
        var blockSpecs = blockCreator.BlockSpecs;
        if(blockSpecs == null) { return; }

        var convertedValue = Convert.ChangeType(value, field.FieldType);
        field.SetValue(blockSpecs, convertedValue);
        blockSpecs.OnValidate();
        ui.SetValue(field.GetValue(blockSpecs));
    }

    public void ResetFields() {
        BlockSpecs specs = blockCreator.BlockSpecs;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<BlockSettingsUI>().ResetToDefault();
        }
    }
}