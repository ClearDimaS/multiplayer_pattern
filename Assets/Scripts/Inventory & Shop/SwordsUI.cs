using UnityEngine;

public class SwordsUI : MonoBehaviour
{
    public Transform itemsParent;

    public GameObject inventoryUI;

    Swords inventory;

    ShopSlot[] slots;

    void Start()
    {
        inventory = Swords.instance;
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
