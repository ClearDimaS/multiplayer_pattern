
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSparksTutorials;
public class ItemInfoScript : MonoBehaviour
{
    public Text TextItemInfo;
    public GameObject ItemInfo;
    public static int currentEquipSlotNum;
    public int EquipSlotNum;
    string DisplayText;
    private List<string> EquipmentModifiersList = Equipment.EquipmentModifiers;
    List<int> ModifierTemp = new List<int>();

    public void DisplayItemInfo()
    {
        Debug.Log(Random.Range(0, 120));

        currentEquipSlotNum = EquipSlotNum;

        if (EquipmentManager.instance.currentEquipment[EquipSlotNum] != null)
        {
            DisplayText = "";
            DisplayText += EquipmentManager.instance.currentEquipment[EquipSlotNum].name + ": " + DataController.GetValue<string>(EquipmentManager.instance.currentEquipment[EquipSlotNum].name + "equipSlot") + "\n";
            //Debug.Log(EquipmentManager.instance.currentEquipment[EquipSlotNum].ModifierBashChance);
            ModifierTemp.Add(EquipmentManager.instance.currentEquipment[EquipSlotNum].ModifierArmor);
            ModifierTemp.Add(EquipmentManager.instance.currentEquipment[EquipSlotNum].ModifierDamage);
            ModifierTemp.Add(EquipmentManager.instance.currentEquipment[EquipSlotNum].ModifierMissChance);
            ModifierTemp.Add(EquipmentManager.instance.currentEquipment[EquipSlotNum].ModifierCriticalChance);
            ModifierTemp.Add(EquipmentManager.instance.currentEquipment[EquipSlotNum].ModifierBashChance);
            ModifierTemp.Add(EquipmentManager.instance.currentEquipment[EquipSlotNum].ModifierStunChance);
            ModifierTemp.Add(EquipmentManager.instance.currentEquipment[EquipSlotNum].ModifierBlockChance);
            ModifierTemp.Add(EquipmentManager.instance.currentEquipment[EquipSlotNum].ModifierMagic);
            int i = 0;
            ItemInfo.SetActive(true);
            foreach (string ModifierName in EquipmentModifiersList)
            {
                //Debug.Log(ModifierTemp[i]);
                //Debug.Log(i);
                if (ModifierTemp[i] > 0)
                {
                    DisplayText += ModifierName.Substring(8) + " : + " + ModifierTemp[i] + " %" + "\n";
                }
                i++;
            }
            TextItemInfo.text = DisplayText;
        }
        else 
        {
            TextItemInfo.text = "Nothing equipped";
        }
    }
    public void StopDisplayItemInfo()
    {
        ItemInfo.SetActive(false);
    }

    public void Unequip()
    {
        EquipmentManager.instance.Unequip(currentEquipSlotNum);
        StopDisplayItemInfo();
    }
}
