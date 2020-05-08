using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparksTutorials;
using UnityEngine.UI;
using System.IO;

public class LocalisationScript : MonoBehaviour
{
    List<string> Languages = new List<string> { "Eng", "Rus", "Chin", "Deutch", "French"  };

    void SetTexts() 
    {
        textField = gameObject.GetComponent<Text>();

        string value = LocalisationSystem.GetLocalisedValue(key);
        textField.text = value;

        textField.resizeTextForBestFit = true;

        textField.resizeTextForBestFit = false;

        textField.fontSize -= 10;
    }

    public Image Flag;

    public void SetLanguage(int i) 
    {
        DataController.SaveValue("Language", i);

        switch (i) 
        {
            case 0:
                LocalisationSystem.language = LocalisationSystem.Language.English;
                break;
            case 1:
                LocalisationSystem.language = LocalisationSystem.Language.Russian;
                break;
            case 2:
                LocalisationSystem.language = LocalisationSystem.Language.Chinese;
                break;
            case 3:
                LocalisationSystem.language = LocalisationSystem.Language.Deutch;
                break;
            case 4:
                LocalisationSystem.language = LocalisationSystem.Language.French;
                break;
        }

        if (Flag != null)
        {
            Flag.sprite = Resources.Load<Sprite>("Localisation" + Languages[i]);

            SetTexts();
        }

    }

    Text textField;

    public string key;
    
    void Start()
    {
        SetLanguage(DataController.GetValue<int>("Language"));

        SetTexts();
    }

}
