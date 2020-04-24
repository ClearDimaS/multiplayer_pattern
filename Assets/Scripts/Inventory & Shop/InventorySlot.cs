using UnityEngine;
using UnityEngine.UI;
using GameSparksTutorials;
using System.Collections.Generic;
public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button removeButton;
    public GameObject ConfirmWindow;
    public Text ConfirmSell;
    public Text ConfirmSell2;
    public long? SellPrice = 0;
    public GameObject ItemInfo;
    public Text TextItemInfo;
    private List<string> EquipmentModifiersList = Equipment.EquipmentModifiers;
    private string DisplayText;
    int ModifierTemp;
    string NameTemp;

    Item item;

    GameObject InventoryParent;


    private void Start()
    {
        InventoryParent = GameObject.FindGameObjectWithTag("InventoryParent");
    }
    public void DisplayItemInfo()
    {
        foreach (InventorySlot obj in InventoryParent.GetComponentsInChildren<InventorySlot>()) 
        {
            obj.StopDisplayItemInfo();
        }

        if (item != null) 
        {
            DisplayText = "";
            DisplayText += item.name + ": " + DataController.GetValue<string>(item.name + "equipSlot") + "\n";

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
        }
    }
    public void StopDisplayItemInfo()
    {
        ItemInfo.SetActive(false);
    }
    public void AddItem(Item newItem) 
    {
        item = newItem;
        item.ammount = 1;
        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public void ClearSlot() 
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnRemoveButton() 
    {
        ConfirmWindow.SetActive(true);
        SellPrice = - DataController.GetValue<int>(item.name + "SellPrice");
        ConfirmSell2.text = "Do you really want to sell\n" + item.name + "\n" + "for " + SellPrice + " Bread?";
        ConfirmSell.text = "\n" + "\n"+ item.name + "\n"+ "        " + SellPrice + "     ";

    }

    public void YesSellPls() 
    {
        DataController.SaveValue("GSNotSynced" + DataController.GetValue<string>("username"), 1);
        Debug.Log("Trying to remove " + item.name);
        NameTemp = item.name;
        ConfirmWindow.SetActive(false);

        DataController.SaveValue("GSNotSynced" + DataController.GetValue<string>("username"), 1);
        DataController.SaveValue(item.name + "ammount", 0);
        DataController.SaveValue("Bread", (DataController.GetValue<int>("Bread") + (int)SellPrice));

        Inventory.instance.Remove(item);
        GetBreadAmmount.Updated = false;
    }
    public void NoPlsNo() 
    {
        ConfirmWindow.SetActive(false);
    }

    public void UseItem() 
    {
        if (item != null) 
        {
            item.Use();
        }
    }
}
