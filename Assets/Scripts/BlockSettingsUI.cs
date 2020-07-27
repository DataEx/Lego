using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public abstract class BlockSettingsUI : MonoBehaviour
{
    [SerializeField]
    private Text parameterNameLabel = default;

    [SerializeField]
    private BlockSettingsUIInputField inputField = default;

    public void SetParameterName(string parameterName) {
        // Convert parameter name to something readable (assuming camelcase as naming convention)
        StringBuilder sb = new StringBuilder();       
        for (int i = 0; i < parameterName.Length; i++)
        {
            if (i == 0)
            {
                sb.Append(parameterName[i].ToString().ToUpper());
            }
            else {
                char c = parameterName[i];
                // Add space between words
                if (char.IsUpper(c)) {
                    sb.Append(" ");
                }
                sb.Append(c.ToString());
            }
        }

        parameterNameLabel.text = sb.ToString();
    }
}
