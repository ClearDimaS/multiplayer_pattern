
using System;
using System.Collections.Generic;
using System.Linq;
using GameSparksTutorials;
using UnityEngine;
using UnityEngine.UI;

public class GetFightResults : MonoBehaviour
{
    private List<Text> TxtOutPut;
    public GameObject loadingscreen;

    public static bool invokeOnce;
    void Start()
    {
        Invoke("Changes", 1.0f);
    }
    private void Changes()
    {
        TxtOutPut = GetComponents<Text>().ToList();
        foreach (Text text in TxtOutPut)
        {
            if (text.name == "ExpGained")
            {
                text.text = LocalisationSystem.GetLocalisedValue("exp") + ": +" + DataController.GetValue<int>("expAdded");
            }

            if (text.name == "BreadGained")
            {
                text.text = LocalisationSystem.GetLocalisedValue("bread") + ": +" + DataController.GetValue<int>("breadAdded");
                if (loadingscreen != null)
                {
                    loadingscreen.SetActive(false);
                }
            }

            string SymbolPlusOrMinus;

            if (DataController.GetValue<int>("RatingChange") > 0)
            {
                SymbolPlusOrMinus = "+";
            }
            else 
            {
                SymbolPlusOrMinus = "-";
            }

            if (text.name == "RatingChange")
            {
                text.text = LocalisationSystem.GetLocalisedValue("rating") + ": " + SymbolPlusOrMinus + " " + Math.Abs(DataController.GetValue<int>("RatingChange"));
            }
        }
    }
}
