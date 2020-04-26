
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    GameObject InventoryObj;

    #region Singleton
    public static Inventory instance;

    public void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one instance of Inventory found!");
            return;
        }
        instance = this;
    }

    #endregion

    public delegate void OnItemChanhged();
    public OnItemChanhged onItemChanhgedCallback;

    public int space = 40;

    public List<Item> items = new List<Item>();

    void Start()
    {
        InventoryObj = GameObject.FindGameObjectsWithTag("Inventory")[0];
    }

    public bool Add(Item item)
    {
        if (!item.isDefaultItem)
        {
            if (items.Count >= space)
            {
                Debug.Log("Not enough room.");
                return false;
            }

            items.Add(item);
            if (onItemChanhgedCallback != null)
            {
                onItemChanhgedCallback.Invoke();
            }
        }

        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);

        if (onItemChanhgedCallback != null)
        {
            onItemChanhgedCallback.Invoke();
        }
    }

    public void MoveOut() 
    {
        InventoryObj.transform.position = new Vector3(-2, -35, 0);
    }

    public void MoveIn(string InOrOut)
    {
        //CharacterInfo = GameObject.FindGameObjectsWithTag("PlayerInfo")[0];
        if (InventoryObj.transform.position.y < 0)
        {
            InventoryObj.transform.position = new Vector3(0, 1, 0);
        }
        else
        {
            InventoryObj.transform.position = new Vector3(-2, -35, 0);
        }
    }
}
