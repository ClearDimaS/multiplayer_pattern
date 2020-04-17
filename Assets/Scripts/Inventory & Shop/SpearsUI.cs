using UnityEngine;

public class SpearsUI : MonoBehaviour
{
    public Transform itemsParent;

    public GameObject inventoryUI;

    Spears inventory;

    ShopSlot[] slots;

    void Start()
    {
        inventory = Spears.instance;
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
