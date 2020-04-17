
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equimpent")]
public class Equipment : Item
{
    public EquipmentSlot equipSlot;
    public string BoughtOrNot;

    public static List<string> EquipmentModifiers = new List<string> { "ModifierArmor", "ModifierDamage", "ModifierMissChance", "ModifierCriticalChance", "ModifierBashChance", "ModifierStunChance", "ModifierBlockChance", "ModifierMagic" };
    public static List<string> PossibleSlots = new List<string> { "Head", "Chest", "Arms", "Legs", "LeftHand", "RightHand", "Feet", "BothHands" };  // This List have to be exaclty the same as EquipmentSlot down this script, and the names in GS have to start with it in order to load stuff
    public static List<string> ForInvLoad = new List<string> { "Helmet", "BreastPlate", "Sleeves", "Pants", "Boots", "Sword", "Axe", "Spear", "Shield", "LongSword", "Hammer", "Daggers"  };

    public override void Use()
    {
        base.Use();
        EquipmentManager.instance.Equip(this);
        RemoveFromInventory();
    }
}

public enum EquipmentSlot{ Head, Chest, Arms, Legs, LeftHand, RightHand, Feet, BothHands }

