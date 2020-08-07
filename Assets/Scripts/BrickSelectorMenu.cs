using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEditor;

public class BrickSelectorMenu : MonoBehaviour
{
    [SerializeField, Tooltip("Folder under /Assets")]
    string folderPath;

    [SerializeField]
    BrickDisplayer brickDisplayerPrefab = default;

    private void OnValidate()
    {
        if (folderPath.Length > 0 && folderPath[0] == '/') {
            folderPath = folderPath.Substring(1);
        }
    }

    void Start()
    {
        string path = Application.dataPath + "/" + folderPath;
        var info = new DirectoryInfo(path);
        string localPath = "Assets/" + folderPath;

        var fileInfo = info.GetFiles().Where(x => x.Name.EndsWith(".asset"));
        foreach (var file in fileInfo) {
            string filePath = localPath + "/" + file.Name;
            print(filePath);
            var obj = AssetDatabase.LoadAssetAtPath(filePath, typeof(BlockSpecs));
            print(obj);
        }
    }
}
