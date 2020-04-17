using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparksTutorials;
using UnityEngine.UI;

public class CelebrationAnimation : MonoBehaviour
{
    public GameObject SetCelebrationBtn;
    public GameObject HeroInShop;
    public GameObject TheListObj;

    Animator animator;

    public List<GameObject> ObjctsToHideWhileHere;

    public List<GameObject> Animations;

    public GameObject AffirmWindow;

    public GameObject Warning;

    public static List<int> Prices = new List<int> { 100, 200, 300 };
    private void Start()
    {
        UpdState();
    }

    void UpdState() 
    {
        int temp = 0;

        foreach (GameObject animation in Animations)
        {
            animation.GetComponentInChildren<Text>().text = Prices[temp].ToString();

            // DataController.SaveValue("WinAnimNumberMine" + temp + "ammount", 0);

            if (DataController.GetValue<int>("WinAnimNumberMine" + temp + "ammount") > 0)
            {
                //Debug.Log("You have it");
                animation.GetComponentsInChildren<Image>()[1].color = Color.white;
            }
            else
            {
                Debug.Log("You dont have it");
                animation.GetComponentsInChildren<Image>()[1].color = Color.clear;
            }

            temp += 1;
        }
    }

    public void SetCelebration()
    {
        Debug.Log(DataController.GetValue<int>("WinAnimNumberMine" + DataController.GetValue<int>("WinAnimNumberMinePreLook") + "ammount"));
        if (DataController.GetValue<int>("WinAnimNumberMine" + DataController.GetValue<int>("WinAnimNumberMinePreLook") + "ammount") > 0)
        {
            DataController.SaveValue("WinAnimNumberMine", DataController.GetValue<int>("WinAnimNumberMinePreLook"));

            Debug.Log("Victory Animation is set: " + DataController.GetValue<int>("WinAnimNumberMine"));
        }
        else 
        {
            AffirmWindow.SetActive(true);

            AffirmWindow.GetComponentInChildren<Text>().text = "Do u really want to buy this animation for " + Prices[DataController.GetValue<int>("WinAnimNumberMinePreLook")] + " of Bread?";
        }
    }

    public void YesBuyAnimation() 
    {
        if (DataController.GetValue<int>("Bread") > Prices[DataController.GetValue<int>("WinAnimNumberMinePreLook")])
        {
            DataController.SaveValue("WinAnimNumberMine" + DataController.GetValue<int>("WinAnimNumberMinePreLook") + "ammount", 1);

            DataController.SaveValue("WinAnimNumberMine", DataController.GetValue<int>("WinAnimNumberMinePreLook"));

            DataController.SaveValue("Bread", DataController.GetValue<int>("Bread") - Prices[DataController.GetValue<int>("WinAnimNumberMinePreLook")]);

            Debug.Log("Victory Animation is set: " + DataController.GetValue<int>("WinAnimNumberMine"));

            AffirmWindow.SetActive(false);

            DataController.SaveValue("GSNotSynced" + DataController.GetValue<string>("username"), 1);

            UpdState();
        }
        else 
        {
            AffirmWindow.SetActive(false);

            Warning.SetActive(true);
        }
    }

    public void NoDontBuyAnimation()
    {
        AffirmWindow.SetActive(false);
    }

    public void LookAtCelebration(int i)
    {
        SetCelebrationBtn.SetActive(true);

        HeroInShop.SetActive(true);
        animator = HeroInShop.GetComponentInChildren<Animator>();
        DataController.SaveValue("WinAnimNumberMinePreLook", i);
        animator.SetBool("PlayerVictory" + DataController.GetValue<int>("WinAnimNumberMinePreLook").ToString(), true);

        Invoke("EndAnim", 0.15f);

        AffirmWindow.SetActive(false);
    }

    public void BeforGoBack()
    {
        if (!TheListObj.activeSelf)
        {
            foreach (GameObject obj in ObjctsToHideWhileHere)
            {
                obj.SetActive(true);
            }
        }

        SetCelebrationBtn.SetActive(false);
        if (animator != null)
        {
            animator.SetBool("PlayerVictory" + DataController.GetValue<int>("WinAnimNumberMinePreLook").ToString(), false);
        }

        HeroInShop.SetActive(false);
    }

    public void BackBtn() 
    {
        foreach (GameObject obj in ObjctsToHideWhileHere)
        {
            obj.SetActive(true);
        }

        TheListObj.SetActive(false);
    }

    void EndAnim()
    {
        if (animator != null)
        {
            animator.SetBool("PlayerVictory" + DataController.GetValue<int>("WinAnimNumberMinePreLook").ToString(), false);
        }
    }
}
