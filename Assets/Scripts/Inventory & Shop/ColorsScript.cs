
using UnityEngine.UI;
using UnityEngine;
using GameSparksTutorials;
using System.Collections.Generic;

public class ColorsScript : MonoBehaviour
{
    public GameObject HeroInInfo;
    public GameObject HeroInShop;
    public GameObject HairInInfo;
    public GameObject HairInShop;
    public GameObject BeardInInfo;
    public GameObject BeardInShop;
    public static List<string> ColorsList = new List<string>{"White", "Yellow", "Red", "Purple", "Blue", "Green", "Cyan", "Gray", "Pink", "Brown"};
    private void Start()
    {
        HeroInInfo.GetComponent<Image>().color = GetColor();
        BeardInShop.GetComponent<Image>().color = GetHairColor(); ;
        HairInShop.GetComponent<Image>().color = GetHairColor(); ;
        HeroInShop.SetActive(true);
        BeardInInfo.GetComponent<Image>().color = GetHairColor(); ;
        HairInInfo.GetComponent<Image>().color = GetHairColor(); ;
        HeroInShop.GetComponent<Image>().color = GetColor();
        HeroInShop.SetActive(false);
    }
    public void SetBodyColor(string color) 
    {
        if (color == "White") 
        {
            DataController.SaveValue("BodyColor", 0);
        } else if(color == "Yellow")
        {
            DataController.SaveValue("BodyColor", 1);
        } else if (color == "Red")
        {
            DataController.SaveValue("BodyColor", 2);
        } else if (color == "Purple")
        {
            DataController.SaveValue("BodyColor", 3);
        } else if (color == "Blue")
        {
            DataController.SaveValue("BodyColor", 4);
        } else if (color == "Green")
        {
            DataController.SaveValue("BodyColor", 5);
        } else if (color == "Cyan")
        {
            DataController.SaveValue("BodyColor", 6);
        } else if (color == "Gray")
        {
            DataController.SaveValue("BodyColor", 7);
        }
        else if (color == "Pink")
        {
            DataController.SaveValue("BodyColor", 8);
        }
        else if (color == "Brown")
        {
            DataController.SaveValue("BodyColor", 9);
        }
        HeroInShop.SetActive(true);
        HeroInInfo.GetComponent<Image>().color = GetColor();
        HeroInShop.GetComponent<Image>().color = GetColor();
    }

    public void SetHairColor(string color)
    {
        if (color == "White")
        {
            DataController.SaveValue("HairColor", 0);
        }
        else if (color == "Yellow")
        {
            DataController.SaveValue("HairColor", 1);
        }
        else if (color == "Red")
        {
            DataController.SaveValue("HairColor", 2);
        }
        else if (color == "Purple")
        {
            DataController.SaveValue("HairColor", 3);
        }
        else if (color == "Blue")
        {
            DataController.SaveValue("HairColor", 4);
        }
        else if (color == "Green")
        {
            DataController.SaveValue("HairColor", 5);
        }
        else if (color == "Cyan")
        {
            DataController.SaveValue("HairColor", 6);
        }
        else if (color == "Gray")
        {
            DataController.SaveValue("HairColor", 7);
        }
        else if (color == "Pink")
        {
            DataController.SaveValue("HairColor", 8);
        }
        else if (color == "Brown")
        {
            DataController.SaveValue("HairColor", 9);
        }
        HeroInShop.SetActive(true);
        HairInInfo.GetComponent<Image>().color = GetHairColor(); 
        HairInShop.GetComponent<Image>().color = GetHairColor(); 
        BeardInInfo.GetComponent<Image>().color = GetHairColor(); 
        BeardInShop.GetComponent<Image>().color = GetHairColor(); 
    }

    public Color GetColor() 
    {
        if (DataController.GetValue<int>("BodyColor") == 0)
        {
            return Color.white;
        }
        else if (DataController.GetValue<int>("BodyColor") == 1)
        {
            return Color.yellow;
        }
        else if (DataController.GetValue<int>("BodyColor") == 2)
        {
            return Color.red;
        }
        else if (DataController.GetValue<int>("BodyColor") == 3)
        {
            return Color.magenta;
        }
        else if (DataController.GetValue<int>("BodyColor") == 4)
        {
            return Color.blue;
        }
        else if (DataController.GetValue<int>("BodyColor") == 5)
        {
            return Color.green;
        }
        else if (DataController.GetValue<int>("BodyColor") == 6)
        {
            return Color.cyan;
        }
        else if (DataController.GetValue<int>("BodyColor") == 7)
        {
            return Color.gray;
        }
        else if (DataController.GetValue<int>("BodyColor") == 8)
        {
            return Color.Lerp(Color.yellow, Color.white, 0.81f); 
        } else if (DataController.GetValue<int>("BodyColor") == 9)
        {
            return Color.Lerp(Color.red, Color.black, 0.6f);
        }
        else 
        {
            return Color.white;
        }
    }
    public Color GetHairColor()
    {
        if (DataController.GetValue<int>("HairColor") == 0)
        {
            return Color.white;
        }
        else if (DataController.GetValue<int>("HairColor") == 1)
        {
            return Color.yellow;
        }
        else if (DataController.GetValue<int>("HairColor") == 2)
        {
            return Color.red;
        }
        else if (DataController.GetValue<int>("HairColor") == 3)
        {
            return Color.magenta;
        }
        else if (DataController.GetValue<int>("HairColor") == 4)
        {
            return Color.blue;
        }
        else if (DataController.GetValue<int>("HairColor") == 5)
        {
            return Color.green;
        }
        else if (DataController.GetValue<int>("HairColor") == 6)
        {
            return Color.cyan;
        }
        else if (DataController.GetValue<int>("HairColor") == 7)
        {
            return Color.gray;
        }
        else if (DataController.GetValue<int>("HairColor") == 8)
        {
            return Color.Lerp(Color.yellow, Color.white, 0.81f);
        }
        else if (DataController.GetValue<int>("HairColor") == 9)
        {
            return Color.Lerp(Color.red, Color.black, 0.6f);
        }
        else
        {
            return Color.white;
        }
    }

    public static Color GetColorForScript(int ColorNumber)
    {
        if (ColorNumber == 0)
        {
            return Color.white;
        }
        else if (ColorNumber == 1)
        {
            return Color.yellow;
        }
        else if (ColorNumber == 2)
        {
            return Color.red;
        }
        else if (ColorNumber == 3)
        {
            return Color.magenta;
        }
        else if (ColorNumber == 4)
        {
            return Color.blue;
        }
        else if (ColorNumber == 5)
        {
            return Color.green;
        }
        else if (ColorNumber == 6)
        {
            return Color.cyan;
        }
        else if (ColorNumber == 7)
        {
            return Color.gray;
        }
        else if (ColorNumber == 8)
        {
            return Color.Lerp(Color.yellow, Color.white, 0.81f);
        }
        else if (ColorNumber == 9)
        {
            return Color.Lerp(Color.red, Color.black, 0.6f);
        }
        else
        {
            return Color.white;
        }
    }
}
