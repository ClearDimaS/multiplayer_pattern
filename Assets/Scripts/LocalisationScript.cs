using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparksTutorials;
using UnityEngine.UI;
using System.IO;

public class LocalisationScript : MonoBehaviour
{
    List<string> Languages = new List<string> { "Eng", "Rus", "Chin" };

    void SetTexts() 
    {
        textField = gameObject.GetComponent<Text>();

        string value = LocalisationSystem.GetLocalisedValue(key);
        textField.text = value;
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
