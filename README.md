# Simple Text Localized
## Built In
![Version](https://badgen.net/badge/Release/v1.0.0/blue) ![Languages](https://badgen.net/badge/Languages/C-Sharp/purple)
![Engine](https://badgen.net/badge/Engine/Unity%202019.4.13f1/gray) ![License](https://badgen.net/github/license/micromatch/micromatch)

## Installation
Please check documentation below
> https://docs.unity3d.com/2019.3/Documentation/Manual/upm-ui-giturl.html

## What is it?
Simple Text Localized is a tool package that help developer to make localization in their game. This tools have custom window editor to help editing dictionary key localization. Text Localized use csv file to write localization data and have 2 mode (online mode and offline mode) based on csv file location.

For details information and example, download sample project in folder Samples.

## Feature
- local and cloud csv file location.
- add, edit and remove value by key in localised file (offline mode only).
- add and remove language in localised file (offline mode only).
- create new localised file.
- only show active language in csv file (hide language dont use) in localiser window and localization manager.
- easy to use in another project.
- have 100 languages support.

## Getting Started
### Window Localiser
To open localiser click Window > Localiser.

![image](.\images\localiser-start.JPG)

Localiser window editor have two mode :
- load  : to load current localization file
    ![image](.\images\localiser-load.JPG)
- create : to create new localization file
    ![image](.\images\localiser-create.JPG)

### Localiser Manager
![image](.\images\localiser-manager.JPG)

Localization Manager class / script, control all activity in this tools. including change mode, change current csv file, change active language in csv file and cloud location for csv file. 

note : please make sure, csv url is csv file has published by url and set permission to everyone.

### Example Code use this tools
Don't forget to use LocalisedString Class to access this tools.
```C#
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
```

## Languages Support
Please check link below, to know languages support and their key code
> https://cloud.google.com/translate/docs/languages

## Contributor
[Anas Rasyid](https://github.com/anasrasyid) 

## License
MIT