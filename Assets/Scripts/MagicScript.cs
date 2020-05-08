using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparksTutorials;
using UnityEngine.UI;

public class MagicScript : MonoBehaviour
{

    #region Singleton
    public static MagicScript instance;

    public void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }
        instance = this;
    }

    #endregion

    public delegate void OnItemChanhged();
    public OnItemChanhged onItemChanhgedCallback;

    public int space = 20;

    public List<Item> items = new List<Item>();

    public GameObject MagicDesription;
    public GameObject LookAtBtn;
    public GameObject HeroInShop;
    
    public GameObject IceAnim;
    public GameObject CurseAnim;
    public GameObject FireAnim;
    public GameObject BoltAnim;

    public Text MagicDescription0;
    public Text MagicDescription1;
    public Text MagicDescription2;
    public Text MagicDescription3;
    
    GameObject MagicAnim;
    Animator animator;

    List<string> MagicNames = new List<string> { "Ice", "Curse", "Fire", "Bolt" };

    List<GameObject> AnimationList;

    List<string> MagicDescriptions = new List<string> { "ice_description", "curse_description",
        "fire_description",
        "bolt_description" };

    private void Start()
    {
        AnimationList = new List<GameObject> { IceAnim, CurseAnim, FireAnim, BoltAnim };
    }

    public void SetMagic(int i) 
    {
        MagicDesription.SetActive(true);
        LookAtBtn.SetActive(true);
        DataController.SaveValue("EquippedMagicMine", MagicNames[i]);
        Debug.Log(MagicNames[i]);

        MagicDescription0.text = LocalisationSystem.GetLocalisedValue(MagicNames[i]) + LocalisationSystem.GetLocalisedValue("magic_text1") + " \n";
        MagicDescription1.text = LocalisationSystem.GetLocalisedValue(MagicNames[i]) + LocalisationSystem.GetLocalisedValue("magic_text1") + " \n";
        MagicDescription2.text = LocalisationSystem.GetLocalisedValue(MagicDescriptions[i]) + " \n" + " \n";
        MagicDescription3.text = LocalisationSystem.GetLocalisedValue("magic_text2") + " \n" + //The basic magic damage is calculated as a 1.59 times of Sleep stat.
           LocalisationSystem.GetLocalisedValue("magic_text3") + " \n" + LocalisationSystem.GetLocalisedValue("magic_text4"); // Maximux chance of a debuff is 24 %.         " You can use Magic only if your Sleep stat is higher than 16."
        Debug.Log("Equipped magic: " + MagicNames[i] +  "(" + DataController.GetValue<string>("EquippedMagicMine") + ")" );
    }

    public void LookAtMagic()
    {
        if (MagicAnim != null)
        {
            animator.SetBool("Magic" + DataController.GetValue<string>("EquippedMagicMine"), false);
            Destroy(MagicAnim);
            HeroInShop.SetActive(false);
        }
        else 
        {
            HeroInShop.SetActive(true);
            animator = HeroInShop.GetComponent<Animator>();
            Debug.Log(DataController.GetValue<string>("EquippedMagicMine"));
            animator.SetBool("Magic" + DataController.GetValue<string>("EquippedMagicMine"), true);
            if (DataController.GetValue<string>("EquippedMagicMine") == MagicNames[0])
            {
                MagicAnim = Instantiate(AnimationList[0]) as GameObject;
            } else if (DataController.GetValue<string>("EquippedMagicMine") == MagicNames[1])
            {
                MagicAnim = Instantiate(AnimationList[1]) as GameObject;
            } else if (DataController.GetValue<string>("EquippedMagicMine") == MagicNames[2])
            {
                MagicAnim = Instantiate(AnimationList[2]) as GameObject;
            } else if (DataController.GetValue<string>("EquippedMagicMine") == MagicNames[3]) 
            {
                MagicAnim = Instantiate(AnimationList[3]) as GameObject;
            }

            MagicAnim.transform.position = HeroInShop.transform.position + new Vector3 (-3, 5, 0);

            MagicAnim.GetComponentsInChildren<RectTransform>()[1].localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            MagicAnim.GetComponentsInChildren<RectTransform>()[1].localScale = new Vector3(1f, 1f, 1f);

            Invoke("EndAnim", 0.15f);

            Invoke("EndMagic", 1.2f);
        }
    }

    public void BeforGoBack() 
    {
        LookAtBtn.SetActive(false);
        MagicDescription0.text = "";
        MagicDescription1.text = "";
        MagicDescription2.text = "";
        MagicDescription3.text = "";
        MagicDesription.SetActive(false);
        if (animator != null) 
        {
            animator.SetBool("Magic" + DataController.GetValue<string>("EquippedMagicMine"), false);
        }

        Destroy(MagicAnim);
        HeroInShop.SetActive(false);
    }

    public void HideAnimation() 
    {
        HeroInShop.SetActive(false);
    }

    void EndAnim() 
    {
        if (animator != null) 
        {
            for (int i = 0; i < MagicNames.Count; i++)
            {
                animator.SetBool("Magic" + MagicNames[i], false);
            }
        }
    }

    void EndMagic() 
    {
        if (MagicAnim != null) 
        {
            Destroy(MagicAnim);
        }
    }
}
