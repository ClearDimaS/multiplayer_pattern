using UnityEngine;

public class AxesUI : MonoBehaviour
{
    public Transform itemsParent;

    public GameObject inventoryUI;

    Axes inventory;

    ShopSlot[] slots;

    void Start()
    {
        inventory = Axes.instance;
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
