
using UnityEngine.UI;
using UnityEngine;
using GameSparksTutorials;

using System.Collections.Generic;
public class HairStylesScript : MonoBehaviour
{
    public GameObject HeroInShop;

    public GameObject HairInInfo;

    public GameObject HairInShop;

    public GameObject HairStylesParent;

    Image[] HairStylePics;

    public static List<string> HairList = new List<string> { "Bald", "Viking", "Ninja", "Wizard", "BigGuy", "Warrior", "Gladiator" };
    private void Start()
    {
        HairStylePics = HairStylesParent.GetComponentsInChildren<Image>();
       
        for(int i = 1; i <= HairList.Count; i++ )
        {
            HairStylePics[i].sprite = Resources.Load<Sprite>("HairStyles" + HairList[i - 1]);
        }

        HeroInShop.SetActive(true);
        HairInShop.GetComponent<Image>().sprite = Resources.Load<Sprite>("HairStyles" + HairList[DataController.GetValue<int>("HairStyle")]);
        HairInInfo.GetComponent<Image>().sprite = Resources.Load<Sprite>("HairStyles" + HairList[DataController.GetValue<int>("HairStyle")]); ;
        HeroInShop.SetActive(false);
    }

    public void SetHairStyle(int HairStylenumber) 
    {
        Debug.Log(HairStylenumber);
        DataController.SaveValue("HairStyle", HairStylenumber);
        HeroInShop.SetActive(true);
        HairInShop.GetComponent<Image>().sprite = Resources.Load<Sprite>("HairStyles" + HairList[DataController.GetValue<int>("HairStyle")]);
        HairInInfo.GetComponent<Image>().sprite = Resources.Load<Sprite>("HairStyles" + HairList[DataController.GetValue<int>("HairStyle")]);
    }
}
