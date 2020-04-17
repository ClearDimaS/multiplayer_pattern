using UnityEngine;
using System.Collections.Generic;
using GameSparksTutorials;

public class LoadShop : Interactable
{
    private List<string> EquipmentNames;
    public Equipment item;
    List<string> NewNamesGS = new List<string>();
    List<string> MagicNamesGS = new List<string>();
    public string WeaponsOrArmour;


    public void Start()
    {
        EquipmentNames = ForEZEdit.EquipmentNames; //// Here should be names from GS to load, ok?!!!!!!!!!!!!!!!!!
        MagicNamesGS = ForEZEdit.MagicNames;
        item = (Equipment)ScriptableObject.CreateInstance(typeof(Equipment));
        foreach (string TypeItem in Equipment.ForInvLoad)
        {
            foreach (string Name in EquipmentNames)
            {
                NewNamesGS.Add(Name + TypeItem);
            }
        }
        Invoke("RefreshTheList", 0.5f);
    }

    public void RefreshTheList()
    {
        foreach (string AName in MagicNamesGS) 
        {
            Item item = ScriptableObject.CreateInstance<Item>(); //new Item();
            item.Price = DataController.GetValue<int>(AName + "Price");
            if (DataController.GetValue<int>(AName + "ammount") == 0)
            {
                item.ammount = 0;
            }
            else
            {
                item.ammount = 1;
            }
        }
        foreach (string AName in NewNamesGS)
        {
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

            if (DataController.GetValue<int>(AName + "ammount") == 0)
            {
                item.ammount = 0;
            }
            else
            {
                item.ammount = 1;
            }

            if (AName.Contains("LongSword"))
            {
                item.equipSlot = EquipmentSlot.BothHands;
                LongSwords.instance.Add(item);
            }
            else
            if (AName.Contains("Sword"))
            {
                item.equipSlot = EquipmentSlot.RightHand;
                Swords.instance.Add(item);
            }
            else
                if (AName.Contains("Axe"))
            {
                item.equipSlot = EquipmentSlot.RightHand;
                Axes.instance.Add(item);
            }
            else
                if (AName.Contains("Spear"))
            {
                item.equipSlot = EquipmentSlot.RightHand;
                Spears.instance.Add(item);
            }
            else
                if (AName.Contains("Hammer"))
            {
                item.equipSlot = EquipmentSlot.BothHands;
                Hammers.instance.Add(item);
            }
            else
                if (AName.Contains("Daggers"))
            {
                item.equipSlot = EquipmentSlot.BothHands;
                Daggers.instance.Add(item);
            }

            if (AName.Contains("Helmet"))
            {
                item.equipSlot = EquipmentSlot.Head;
                Head.instance.Add(item);
            }
            else
            if (AName.Contains("BreastPlate"))
            {
                item.equipSlot = EquipmentSlot.Chest;
                Chest.instance.Add(item);
            }
            else
            if (AName.Contains("Sleeves"))
            {
                item.equipSlot = EquipmentSlot.Arms;
                Arms.instance.Add(item);
            }
            else
            if (AName.Contains("Pants"))
            {
                item.equipSlot = EquipmentSlot.Legs;
                Legs.instance.Add(item);
            }
            else
            if (AName.Contains("Boots"))
            {
                item.equipSlot = EquipmentSlot.Feet;
                Feet.instance.Add(item);
            }
            else
            if (AName.Contains("Shield"))
            {
                item.equipSlot = EquipmentSlot.LeftHand;
                Shields.instance.Add(item);
            }
            DataController.SaveValue(item.name + "equipSlot", Equipment.PossibleSlots[(int)item.equipSlot]);
            if (item.ammount > 0)
            {
                ItemPickup.PickUpToInv(item);
            }
        }
    }
}
