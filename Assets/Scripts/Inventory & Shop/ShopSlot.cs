using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using GameSparksTutorials;

public class ShopSlot : MonoBehaviour
{
    public Image icon;
    public Image BoughtOrNot;
    Item item;
    public GameObject AffirmWindow;
    public Text WindowText;

    public GameObject WarningWindow;
    public Text WarningText;
    public Text Price;
    private bool allowToBuy = false;
    public GameObject ItemInfo;
    public Text TextItemInfo;
    private List<string> EquipmentModifiersList = Equipment.EquipmentModifiers;
    private string DisplayText;
    int ModifierTemp;
    public static List<string> name = new List<string>();

    GameObject[] shopParents;

    private void Update()
    {
        if (item != null)
        {
            if (item.name != null) 
            {
                if (name.Contains(item.name))
                {
                    Debug.Log("updating ye " + item.ammount);
                    name.Remove(item.name);
                    BoughtOrNot.color = Color.clear;
                }
            }
        }
    }
    public void DisplayItemInfo() 
    {
        shopParents = GameObject.FindGameObjectsWithTag("ShopParent");
        foreach (GameObject obj in shopParents)
        {
            foreach (ShopSlot shpSlt in obj.GetComponentsInChildren<ShopSlot>())
            {
                shpSlt.StopDisplayItemInfo();
            }
        }

        if (item != null) 
        {
            DisplayText = "";
            DisplayText += item.name + ": " + DataController.GetValue<string>(item.name + "equipSlot") + "\n";
            item.ammount = DataController.GetValue<int>(item.name + "ammount");
            ItemInfo.SetActive(true);
            foreach (string ModifierName in EquipmentModifiersList)
            {
                ModifierTemp = DataController.GetValue<int>(item.name + ModifierName);
                if (ModifierTemp > 0)
                {
                    DisplayText += ModifierName.Substring(8) + " : + " + ModifierTemp + " %" + "\n";
                }
            }
            TextItemInfo.text = DisplayText;
            if (item.ammount > 0)
            {
                BoughtOrNot.color = Color.white;
            }
            else
            {
                BoughtOrNot.color = Color.clear;
            }
        }
       
    }
    public void StopDisplayItemInfo()
    {
        ItemInfo.SetActive(false);
    }
    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = Resources.Load<Sprite>("Pic" + item.name);
        icon.enabled = true;
        Price.text = item.Price.ToString();
        if (item.ammount > 0)
        {
            BoughtOrNot.color = Color.white;
        }
        else 
        {
            BoughtOrNot.color = Color.clear;
        }
    }

    public void TestLook() 
    {
        EquipmentManager.instance.EquipInShop((Equipment)item);
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        BoughtOrNot.color = Color.clear;
        icon.enabled = false;
    }

    public void BuyItem()
    {
        DataController.SaveValue(item.name + "ammount", 0);
        ItemInfo.SetActive(false);
        AffirmWindow.SetActive(true);
        WindowText.text = "Do u want to buy " + item.name + " ?";        
    }

    public void IfYes() 
    {
        AffirmWindow.SetActive(false);
        if (DataController.GetValue<int>(item.name + "ammount") > 0)
        {
            WarningWindow.SetActive(true);
            WarningText.text = "You have already bought the " + item.name + "!";
            allowToBuy = false;
        }
        else if (DataController.GetValue<int>("StatsPowerMine") < 16 && (item.name.Contains("Daggers") || item.name.Contains("Hammer") || item.name.Contains("LongSword"))) 
        {
            WarningWindow.SetActive(true);
            WarningText.text = "Your Power have to be 16 or more to use double handed weapons!";
        }
        else
        {
            Debug.Log("Yeah, you dont have it well done");
            if (item.Price <= DataController.GetValue<int>("Bread"))
            {
                allowToBuy = true;
            }
            else 
            {
                WarningWindow.SetActive(true);
                WarningText.text = "Not enough Bread!";
            }

        }
        if (allowToBuy)
        {
            DataController.SaveValue("GSNotSynced" + DataController.GetValue<string>("username"), 1);
            DataController.SaveValue("Bread", (DataController.GetValue<int>("Bread") - item.Price));
            DataController.SaveValue(item.name + "ammount", 1);
            item.ammount = 1;
            item.icon = item.icon = Resources.Load<Sprite>("Pic" + item.name);  // HERE THE SAME AS INVENTORY
            Inventory.instance.Add(item);
            Debug.Log(item.name + " Is BOUGHT!!!!");
            BoughtOrNot.color = Color.white;
            GetBreadAmmount.Updated = false;
        }
        else
        {
            Debug.Log("NotBought" + item.name);
        }
        allowToBuy = false;
    }

    public void IfNo() 
    {
        AffirmWindow.SetActive(false);
    }

    public void CloseWarning() 
    {
        WarningWindow.SetActive(false);
    }
}
