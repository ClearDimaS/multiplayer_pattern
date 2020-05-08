
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DispSpecials : MonoBehaviour
{
    #region Singleton
    public static DispSpecials instance;

    public void Awake()
    {
        if (instance != null)
        {
            //Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }
        instance = this;
    }

    #endregion


    static GameObject SpecialsMenu;
    public Text Title;

    Text[] Texts = new Text[6];

    public static List<string> AttackTexts = new List<string>();
    public static List<string> AgilityTexts = new List<string>();
    public static List<string> PowerTexts = new List<string>();
    public static List<string> StrengthTexts = new List<string>();

    public static List<string> EnduranceTexts = new List<string>();
    public static List<string> SpeedTexts = new List<string>();
    public static List<string> SleepTexts = new List<string>();
    public static List<string> RegenTexts = new List<string>();

    List<string> StatsList = new List<string> { "Attack", "Agility", "Power", "Strength", "Endurance", "Speed", "Sleep", "Regen" };

    List<string> StatsList2 = new List<string> { "attack", "agility", "power", "strength", "endurance", "speed", "sleep", "regen" };
    List<List<string>> AllTexts = new List<List<string>>();
    // Start is called before the first frame update
    int specialsCounter;
    int StatNum;

    Mechanics.Specials specials = new Mechanics.Specials();
    void Start()
    {
        specials.SetTexts();
        AttackTexts = specials.AttackTexts;
        AgilityTexts = specials.AgilityTexts;
        PowerTexts = specials.PowerTexts;
        StrengthTexts = specials.StrengthTexts;

        EnduranceTexts = specials.EnduranceTexts;
        SpeedTexts = specials.SpeedTexts;
        SleepTexts = specials.SleepTexts;
        RegenTexts = specials.RegenTexts;
        GameObject tempMenu;

        tempMenu = GameObject.FindGameObjectWithTag("SpecialsPanel");
        if (tempMenu != null) 
        {
            SpecialsMenu = tempMenu;
        }

        AllTexts.Add(AttackTexts);
        AllTexts.Add(AgilityTexts);
        AllTexts.Add(PowerTexts);
        AllTexts.Add(StrengthTexts);
        AllTexts.Add(EnduranceTexts);
        AllTexts.Add(SpeedTexts);
        AllTexts.Add(SleepTexts);
        AllTexts.Add(RegenTexts);
    }


    public void ShowOrHide()
    {
        specialsCounter = 0;
        StatNum = 0;
        SpecialsMenu.SetActive(!SpecialsMenu.activeSelf);
        Texts = SpecialsMenu.GetComponentsInChildren<Text>();
        if (SpecialsMenu.activeSelf)
        {
            foreach (string StatName in StatsList)
            {
                if (Title.name.Contains(StatName))
                {
                    Texts[5].text = LocalisationSystem.GetLocalisedValue("specials") + " " + LocalisationSystem.GetLocalisedValue(StatName.ToLower()) + " :";
                    Texts[6].text = LocalisationSystem.GetLocalisedValue("specials") + " " + LocalisationSystem.GetLocalisedValue(StatName.ToLower()) + " :";
                    foreach (Text text in Texts)
                    {
                        if (specialsCounter < 5)
                        {
                            text.text = LocalisationSystem.GetLocalisedValue("tier") + (specialsCounter + 1) + " : " + AllTexts[StatNum][specialsCounter];
                        }
                        specialsCounter++;
                    }
                    break;
                }
                StatNum++; ;
            }
        }
    }
}
