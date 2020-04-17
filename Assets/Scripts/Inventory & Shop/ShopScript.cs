
using System.Collections.Generic;
using UnityEngine;

public class ShopScript : MonoBehaviour
{
    public GameObject HeroInShop;
    public GameObject ShopCustomize;
    public GameObject ShopWeapon;
    public GameObject ShopArmour;
    public GameObject ShopMagic;
    public GameObject BtnMagic;
    public GameObject BtnArmour;
    public GameObject BtnWeapon;
    public GameObject BtnLeave;
    public GameObject ShopBtn;
    public GameObject BtnCustomize;

    public List<GameObject> HideMe;

    // Start is called before the first frame update
    public void HideWeapAndArm() 
    {
        foreach (GameObject obj in HideMe) 
        {
            obj.SetActive(false);
        }

        ShopBtn.SetActive(false);
        ShopWeapon.SetActive(false);
        ShopArmour.SetActive(false);
        ShopCustomize.SetActive(false);
        ShopMagic.SetActive(false);
        BtnMagic.SetActive(true);
        BtnCustomize.SetActive(true);
        BtnArmour.SetActive(true);
        BtnWeapon.SetActive(true);
        BtnLeave.SetActive(true);
    }
    public void HideOrShowWeap()
    {
        foreach (GameObject obj in HideMe)
        {
            obj.SetActive(false);
        }

        ShopWeapon.SetActive(!ShopWeapon.activeSelf);
        BtnArmour.SetActive(!BtnArmour.activeSelf);
        BtnLeave.SetActive(!BtnLeave.activeSelf);
        BtnCustomize.SetActive(!BtnCustomize.activeSelf);
        BtnMagic.SetActive(!BtnMagic.activeSelf);
    }
    public void HideOrShowArmour()
    {
        foreach (GameObject obj in HideMe)
        {
            obj.SetActive(false);
        }

        ShopArmour.SetActive(!ShopArmour.activeSelf);
        BtnWeapon.SetActive(!BtnWeapon.activeSelf);
        BtnLeave.SetActive(!BtnLeave.activeSelf);
        BtnCustomize.SetActive(!BtnCustomize.activeSelf);
        BtnMagic.SetActive(!BtnMagic.activeSelf);
    }
    public void HideOrShowCustomize() 
    {
        foreach (GameObject obj in HideMe)
        {
            obj.SetActive(false);
        }

        ShopCustomize.SetActive(!ShopCustomize.activeSelf);
        BtnArmour.SetActive(!BtnArmour.activeSelf);
        BtnLeave.SetActive(!BtnLeave.activeSelf);
        BtnCustomize.SetActive(!BtnCustomize.activeSelf);
        BtnMagic.SetActive(!BtnMagic.activeSelf);
    }

    public void HideOrShowMagic()
    {
        foreach (GameObject obj in HideMe)
        {
            obj.SetActive(false);
        }

        HeroInShop.SetActive(false);
        ShopMagic.SetActive(!ShopMagic.activeSelf);
        BtnWeapon.SetActive(!BtnWeapon.activeSelf);
        BtnArmour.SetActive(!BtnArmour.activeSelf);
        BtnLeave.SetActive(!BtnLeave.activeSelf);
        BtnCustomize.SetActive(!BtnCustomize.activeSelf);
    }
}
