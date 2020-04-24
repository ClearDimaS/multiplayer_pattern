using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSparksTutorials;
public class RewardChestScript : MonoBehaviour
{
    public Text[] WinsForChestTexts;

    public GameObject Chest;

    public GameObject ChestPanel;

    public GameObject OkButton;

    public GameObject ChestInfoBtn;

    public GameObject Shining;

    public SpriteRenderer PicOfReward;

    private void Start()
    {
        firstTime = true;

        DataController.SaveValue("WinsForChest", 22);

        if (DataController.GetValue<int>("WinsForChest") >= 3) 
        {
            ChestInfoBtn.GetComponentInChildren<Animator>().Play("ChestInfoBtnEnough");
        }

    }


    bool firstTime;

    bool BtnInteractable;

    public void UpdTextOfWinQuantity() 
    {
        BtnInteractable = true;

        ChestPanel.SetActive(true);

        if (DataController.GetValue<int>("WinsForChest") >= 3 && firstTime)
        {
            MoveShiningBack();

            foreach (Text txt in WinsForChestTexts)
            {
                txt.text = (DataController.GetValue<int>("WinsForChest") - 1).ToString();
            }


            Shining.SetActive(true);

            Invoke("UpdateWinAmmountWithAnimation", 0.5f);
        }
        else 
        {
            if (DataController.GetValue<int>("WinsForChest") < 3) 
            {
                Shining.SetActive(false);
            }

            Chest.GetComponentInChildren<Animator>().Play("NotShine");

            foreach (Text txt in WinsForChestTexts)
            {
                txt.text = (DataController.GetValue<int>("WinsForChest")).ToString();
            }

            OkButton.SetActive(true);
        }
    }


    void UpdateWinAmmountWithAnimation() 
    {
        foreach (Text txt in WinsForChestTexts)
        {
            txt.text = (DataController.GetValue<int>("WinsForChest")).ToString();
        }

        WinsForChestTexts[0].GetComponentInChildren<Animator>().Play("AmmountUpd");

        Chest.SetActive(true);
    }


    Equipment rewardEquipment;

    public void GetChestReward()
    {
        if (BtnInteractable) 
        {
            if (DataController.GetValue<int>("WinsForChest") >= 3)
            {
                ChestInfoBtn.GetComponentInChildren<Animator>().Play("ChestInfoBtnNotEnough");

                DataController.SaveValue("WinsForChest", DataController.GetValue<int>("WinsForChest") - 3);

                firstTime = false;

                Chest.GetComponentInChildren<Animator>().Play("ChestOpen");

                if (DataController.GetValue<int>("WinsForChest") >= 3)
                {
                    Invoke("UpdTextOfWinQuantity", 2.0f);
                }
                else
                {
                    UpdTextOfWinQuantity();
                }

                rewardEquipment = GetRandomEquipment();

                PicOfReward.sprite = item.icon;

                Invoke("MoveShiningBack", 1.0f);

                Invoke("SaveRewardPic", 1.8f);

                Shining.GetComponentInChildren<SpriteRenderer>().sortingOrder = 4;

               ItemPickup.PickUpToInv(rewardEquipment);
            }
            BtnInteractable = false;
        }

        // Suggest to get the second chest for an add here

        OkButton.SetActive(true);
    }

    void MoveShiningBack() 
    {
        Shining.GetComponentInChildren<SpriteRenderer>().sortingOrder = 1;
    }

    void SaveRewardPic() 
    {
        PicOfReward.color = Color.white;

        PicOfReward.transform.localScale = new Vector3(6.0f, 4.8f, 1.0f);
    }

    private List<string> EquipmentNames;

    List<string> NewNamesGS = new List<string>();

    Equipment item;

    Equipment GetRandomEquipment()
    {
        foreach (string TypeItem in Equipment.ForInvLoad)
        {
            foreach (string Name in ForEZEdit.EquipmentNames)
            {
                NewNamesGS.Add(Name + TypeItem);
            }
        }

        string AName = NewNamesGS[Random.Range(0, NewNamesGS.Count)];

        while (DataController.GetValue<int>(AName + "ammount") != 0) 
        {
            AName = NewNamesGS[Random.Range(0, NewNamesGS.Count)];
        }

        // DELETE THE NEXT LINE!

        item = (Equipment)ScriptableObject.CreateInstance(typeof(Equipment));
        item.name = AName;
        item.icon = Resources.Load<Sprite>("Pic" + AName); // add sprites in the Resources folder with names same as NewName variables here 
        item.Price = DataController.GetValue<int>(AName + "Price");
        item.SellPrice = DataController.GetValue<int>(AName + "SellPrice");
        item.ModifierArmor = DataController.GetValue<int>(AName + "ModifierArmor");
        item.ModifierDamage = DataController.GetValue<int>(AName + "ModifierDamage");
        item.ModifierMissChance = DataController.GetValue<int>(AName + "ModifierMissChance");
        item.ModifierCriticalChance = DataController.GetValue<int>(AName + "ModifierCriticalChance");
        item.ModifierBashChance = DataController.GetValue<int>(AName + "ModifierBashChance");
        item.ModifierStunChance = DataController.GetValue<int>(AName + "ModifierStunChance");
        item.ModifierBlockChance = DataController.GetValue<int>(AName + "ModifierBlockChance");
        item.ModifierMagic = DataController.GetValue<int>(AName + "ModifierMagic");

        #region EqipSlotPart
        if (AName.Contains("LongSword"))
        {
            item.equipSlot = EquipmentSlot.BothHands;
        }
        else
            if (AName.Contains("Sword"))
        {
            item.equipSlot = EquipmentSlot.RightHand;
        }
        else
                if (AName.Contains("Axe"))
        {
            item.equipSlot = EquipmentSlot.RightHand;
        }
        else
                if (AName.Contains("Spear"))
        {
            item.equipSlot = EquipmentSlot.RightHand;
        }
        else
                if (AName.Contains("Hammer"))
        {
            item.equipSlot = EquipmentSlot.BothHands;
        }
        else
                if (AName.Contains("Daggers"))
        {
            item.equipSlot = EquipmentSlot.BothHands;
        }

        if (AName.Contains("Helmet"))
        {
            item.equipSlot = EquipmentSlot.Head;
        }
        else
        if (AName.Contains("BreastPlate"))
        {
            item.equipSlot = EquipmentSlot.Chest;
        }
        else
        if (AName.Contains("Sleeves"))
        {
            item.equipSlot = EquipmentSlot.Arms;
        }
        else
        if (AName.Contains("Pants"))
        {
            item.equipSlot = EquipmentSlot.Legs;
        }
        else
        if (AName.Contains("Boots"))
        {
            item.equipSlot = EquipmentSlot.Feet;
        }
        else
        if (AName.Contains("Shield"))
        {
            item.equipSlot = EquipmentSlot.LeftHand;
        }
        #endregion

        item.ammount = 1;

        DataController.SaveValue(AName + "ammount", 1);

        return item;
    }
}
