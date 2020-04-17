using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparksTutorials;
using UnityEngine.UI;

public class EnemyChooser : MonoBehaviour
{

    public GameObject EquipmentParent;

    public GameObject StatListParent;

    public Image BossOrNotLoadImage;

    public List<Text> BossesInfoTextList;

    public void SetLoadMode(int loadModeNumber) 
    {
        DataController.SaveValue("LoadMode", loadModeNumber);

        loadEnemy.LoadAnEnemy();

        if (loadModeNumber == 1)
        {
            BossOrNotLoadImage.sprite = Resources.Load<Sprite>("RandomFightBtn");
        }
        else 
        {
            BossOrNotLoadImage.sprite = Resources.Load<Sprite>("BossFightBtn");
        }
    }

    SingleEnemyHandler loadEnemy = new SingleEnemyHandler();

    public void SetBossText() 
    {
        foreach (Text txt in BossesInfoTextList) 
        {
            txt.text = "Next Legend\nlevel is:\n " + loadEnemy.BossesLvlList[DataController.GetValue<int>("CurrentBossNumber")] +
                "\n\n" + DataController.GetValue<int>("CurrentBossNumber") + " / " + 10 + " Legends complete";
        }
    }

    public void UpdEnemyInfo() 
    {
        List<string> LocalNamesOther = new List<string> { "AttackOther", "AgilityOther", "PowerOther", "StrengthOther", "EnduranceOther", "SpeedOther", "SleepOther", "RegenOther" };

        int index = 0;
        
        foreach (Text text in StatListParent.GetComponentsInChildren<Text>()) 
        {
            text.text = DataController.GetValue<int>("Stats" + LocalNamesOther[index]).ToString();
            index++;
        }

        Image[] imgList = EquipmentParent.GetComponentsInChildren<Image>();

        List<string> ForEquipmentImgsLoad = new List<string> { "Head", "Chest", "Arms", "Legs", "RightHand", "Feet", "LeftHand" };

        Debug.Log(DataController.GetValue<int>("BodyColor" + "Other") + "   " + imgList[0]);
        imgList[0].color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor" + "Other"));

        imgList[1].sprite = Resources.Load<Sprite>("HairStyles" + HairStylesScript.HairList[DataController.GetValue<int>("HairStyle" + "Other")]);
        imgList[1].color = ColorsScript.GetColorForScript(DataController.GetValue<int>("HairColor" + "Other"));

        imgList[3].sprite = Resources.Load<Sprite>("Beards" + BeardsScript.BeardsList[DataController.GetValue<int>("Beard" + "Other")]);
        imgList[3].color = ColorsScript.GetColorForScript(DataController.GetValue<int>("HairColor" + "Other"));

        string tempEqName = "";
        
        for (int i = 4; i < 11; i++) 
        {
            tempEqName = DataController.GetValue<string>("Equipped" + ForEquipmentImgsLoad[i - 4] + "Other");

            if (tempEqName == "")
            {
                imgList[i].color = Color.clear;
            }
            else 
            {
                imgList[i].sprite = Resources.Load<Sprite>(tempEqName);
                imgList[i].color = Color.white;
            }

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
