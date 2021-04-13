using UnityEngine;
using UnityEngine.UI;
using personaltools.textlocalizedtool;

public class TextLocaliser : MonoBehaviour
{
    Text textField;

    public LocalisedString localisedString;

    void Start()
    {
        textField = GetComponent<Text>();
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