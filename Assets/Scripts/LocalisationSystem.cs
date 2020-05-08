using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LocalisationSystem
{

    public enum Language 
    {
        English,
        Russian,
        Chinese,
        French,
        Deutch
    }

    public static Language language = Language.English;

    private static Dictionary<string, string> localisedEN;
    private static Dictionary<string, string> localisedRU;
    private static Dictionary<string, string> localisedCH;
    private static Dictionary<string, string> localisedDE;
    private static Dictionary<string, string> localisedFR;

    public static bool isInit;

    public static void Init() 
    {
        CSVLoader csvLoader = new CSVLoader();
        csvLoader.LoadCSV();

        localisedEN = csvLoader.GetDictionaryValues("en");
        localisedRU = csvLoader.GetDictionaryValues("ru");
        localisedCH = csvLoader.GetDictionaryValues("ch");
        localisedDE = csvLoader.GetDictionaryValues("de");
        localisedFR = csvLoader.GetDictionaryValues("fr");

        isInit = true;
    }

    public static string GetLocalisedValue(string key) 
    {
        if (!isInit) { Init(); }

        string value = key;

        //string val;
        //localisedRU.TryGetValue(key, out val);
        //Debug.Log(val);
        //localisedEN.TryGetValue(key, out val);
        //Debug.Log(val);
        //localisedCH.TryGetValue(key, out val);

        switch (language) 
        {
            case Language.English:
                localisedEN.TryGetValue(key, out value);
                break;
            case Language.Russian:
                localisedRU.TryGetValue(key, out value);
                break;
            case Language.Chinese:
                localisedCH.TryGetValue(key, out value);
                break;
            case Language.French:
                localisedFR.TryGetValue(key, out value);
                break;
            case Language.Deutch:
                localisedDE.TryGetValue(key, out value);
                break;
        }

        return value;
    }
}
