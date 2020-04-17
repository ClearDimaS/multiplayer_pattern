using UnityEngine;

public class DaggersUI : MonoBehaviour
{
    public Transform itemsParent;

    public GameObject inventoryUI;

    Daggers inventory;

    ShopSlot[] slots;

    void Start()
    {
        inventory = Daggers.instance;
        inventory.onItemChanhgedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<ShopSlot>();
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
