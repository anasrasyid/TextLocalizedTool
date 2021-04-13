using UnityEngine;
using TMPro;
using personaltools.textlocalizedtool;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TMPTextLocaliser : MonoBehaviour
{
    TextMeshProUGUI textField;

    public LocalisedString localisedString;

    void Start()
    {
        textField = GetComponent<TextMeshProUGUI>();
        ShowText();
        LocalizationManager.Instance.onChangeLanguage += ShowText;
    }

    public void ShowText()
    {
        textField.text = localisedString.Value;
    }

    private void OnDestroy()
    {
        LocalizationManager.Instance.onChangeLanguage -= ShowText;
    }
}
