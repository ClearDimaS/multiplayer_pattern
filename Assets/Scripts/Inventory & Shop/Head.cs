
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    public GameObject InventoryObj;

    #region Singleton
    public static Head instance;

    public void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }
        instance = this;
    }

    #endregion

    public delegate void OnItemChanhged();
    public OnItemChanhged onItemChanhgedCallback;

    public int space = 20;

    public List<Item> items = new List<Item>();

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
        Head.instance.Add(item);

        if (onItemChanhgedCallback != null)
        {
            onItemChanhgedCallback.Invoke();
        }
    }
}