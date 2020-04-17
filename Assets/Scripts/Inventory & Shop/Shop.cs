using UnityEngine;
using System.Collections.Generic;

public class Shop : MonoBehaviour
{
    private List<string> EquipmentNames;
    public GameObject ShopObj;
    public GameObject AnyObject;
    public GameObject AnyObject2;
    List<string> NewNamesGS = new List<string>();
    List<string> ListOfEquipmentModifiers = Equipment.EquipmentModifiers;
    List<Equipment> EquipList;
    public GameObject ShopBtn;
    public List<GameObject> ObjectsToHide;

    private void Start()
    {

    }
    public void ShowOrHideShop(string where)
    {
        if (where == "in")
        {
            ShopObj.transform.position = new Vector3((float)-1.7, (float)1, 0);
        }
        else
        {
            ShopObj.transform.position = new Vector3((float)0, (float)20, 0);
            ShopBtn.SetActive(true);
        }
    }

    public void ShowOrHideAnything() 
    {
        AnyObject.SetActive(!AnyObject.activeSelf);
    }

    public void HideAnything()
    {
        foreach (GameObject Object in ObjectsToHide) 
        {
            Object.SetActive(false);
        }
    }

}
