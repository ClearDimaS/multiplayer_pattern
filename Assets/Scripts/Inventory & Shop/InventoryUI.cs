using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;

    public GameObject inventoryUI;

    Inventory inventory;

    InventorySlot[] slots;

    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChanhgedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    public void HideOrShow() 
    {
        if (inventoryUI.transform.position.z != 0)
        {
            inventoryUI.transform.position = new Vector3(1, 2, 0);
        }
        else
        {
            inventoryUI.transform.position = new Vector3(-1, 20, 1);
        }
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
