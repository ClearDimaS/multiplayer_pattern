using UnityEngine;

using System.Collections.Generic;
using GameSparksTutorials;
public class ItemPickup : Interactable
{
    private List<string> EquipmentTypes;
    private Equipment item;


    void Start()
    {
        EquipmentTypes = Equipment.ForInvLoad;
        item = (Equipment)ScriptableObject.CreateInstance(typeof(Equipment));
        RefreshTheList();
    }

    private void RefreshTheList() 
    {
     
    }

    public override void Interact() 
    {
        //base.Interact();

        PickUp();
    }

    void PickUp() 
    {

    }

    public static void PickUpToInv(Equipment TheItem) 
    {
        if (DataController.GetValue<int>(TheItem.name + "Equipped") == 0)
        {
            Inventory.instance.Add(TheItem);
        }
        else
        {
            EquipmentManager.instance.Equip(TheItem);
        }
    }
}
