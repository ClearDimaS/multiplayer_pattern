
using UnityEngine.UI;
using UnityEngine;
using GameSparksTutorials;

using System.Collections.Generic;

public class BeardsScript : MonoBehaviour
{
    public GameObject HeroInShop;
    public GameObject BeardInInfo;
    public GameObject BeardInShop;
    public static List<string> BeardsList = new List<string> { "Nothing", "VirginMustache" };
    private void Start()
    {
        HeroInShop.SetActive(true);
        BeardInShop.GetComponent<Image>().sprite = Resources.Load<Sprite>("Beards" + BeardsList[DataController.GetValue<int>("Beard")]);
        BeardInInfo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Beards" + BeardsList[DataController.GetValue<int>("Beard")]);
        HeroInShop.SetActive(false);
    }

    public void SetBeard(int Beardsnumber)
    {
        Debug.Log(Beardsnumber);
        DataController.SaveValue("Beard", Beardsnumber);
        HeroInShop.SetActive(true);
        BeardInShop.GetComponent<Image>().sprite = Resources.Load<Sprite>("Beards" + BeardsList[DataController.GetValue<int>("Beard")]);
        BeardInInfo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Beards" + BeardsList[DataController.GetValue<int>("Beard")]);
    }
}
