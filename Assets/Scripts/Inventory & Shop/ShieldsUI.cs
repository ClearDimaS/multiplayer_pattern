using UnityEngine;

public class ShieldsUI : MonoBehaviour
{
    public Transform itemsParent;

    public GameObject inventoryUI;

    Shields inventory;

    ShopSlot[] slots;

    void Start()
    {
        inventory = Shields.instance;
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
