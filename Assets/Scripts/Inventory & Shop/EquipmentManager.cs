using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using GameSparksTutorials;

public class EquipmentManager : MonoBehaviour
{
    #region Singleton
    public static EquipmentManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion
    public List<Button> ListOfIcons;
    public List<Image> ListOfImages;
    public List<Image> ListOfImagesShop;
    public GameObject ItemsPreview;
    List<string> ForEquipmentImgsLoad = new List<string> { "Head", "Chest", "Arms", "Legs", "LeftHand", "RightHand", "Feet", "LeftHand" };
    bool ObjectsLoaded;
    List<string> ModifiersList = Equipment.EquipmentModifiers;
    UnityEngine.Color color;


    public Equipment[] currentEquipment;
    public Equipment[] currentEquipmentInShop;

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    Inventory inventory;

    void Start()
    {
        inventory = Inventory.instance;
        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];
        currentEquipmentInShop = new Equipment[numSlots];

        DataController.SaveValue("TotalModifierArmorMine", 0);
        DataController.SaveValue("TotalModifierDamageMine", 0);
        DataController.SaveValue("TotalModifierMissChanceMine", 0);
        DataController.SaveValue("TotalModifierCriticalChanceMine", 0);
        DataController.SaveValue("TotalModifierBashChanceMine", 0);
        DataController.SaveValue("TotalModifierStunChanceMine", 0);
        DataController.SaveValue("TotalModifierBlockChanceMine", 0);
        DataController.SaveValue("TotalModifierMagicMine", 0);
    }

    public void Equip(Equipment newItem)
    {
        Debug.Log(currentEquipment[7]);
        ShowPlayerStats.WasChanged = true;
        //Debug.Log("Trying to equip" + "" + newItem.name);

        int slotIndex = (int)newItem.equipSlot;

        DataController.SaveValue("TotalModifierArmorMine", DataController.GetValue<int>("TotalModifierArmorMine") + newItem.ModifierArmor);
        DataController.SaveValue("TotalModifierDamageMine", DataController.GetValue<int>("TotalModifierDamageMine") + newItem.ModifierDamage);
        DataController.SaveValue("TotalModifierMissChanceMine", DataController.GetValue<int>("TotalModifierMissChanceMine") + newItem.ModifierMissChance);
        DataController.SaveValue("TotalModifierCriticalChanceMine", DataController.GetValue<int>("TotalModifierCriticalChanceMine") + newItem.ModifierCriticalChance);
        DataController.SaveValue("TotalModifierBashChanceMine", DataController.GetValue<int>("TotalModifierBashChanceMine") + newItem.ModifierBashChance);
        DataController.SaveValue("TotalModifierStunChanceMine", DataController.GetValue<int>("TotalModifierStunChanceMine") + newItem.ModifierStunChance);
        DataController.SaveValue("TotalModifierBlockChanceMine", DataController.GetValue<int>("TotalModifierBlockChanceMine") + newItem.ModifierBlockChance);
        DataController.SaveValue("TotalModifierMagicMine", DataController.GetValue<int>("TotalModifierMagicMine") + newItem.ModifierMagic);
        DataController.SaveValue(newItem.name + "Equipped", 2);
        Equipment oldItem = null;

        DataController.SaveValue("Equipped" + ForEquipmentImgsLoad[(int)newItem.equipSlot] + "Mine", newItem.name);

        if (slotIndex == 7)
        {
            if (DataController.GetValue<int>("StatsPowerMine") >= 16) 
            {
                if (currentEquipment[slotIndex] != null)
                {
                    currentEquipment[slotIndex] = null;
                }
                if (currentEquipment[4] != null)
                {
                    Unequip(4);
                }
                if (currentEquipment[5] != null)
                {
                    Unequip(5);
                }
                currentEquipment[5] = newItem;
                currentEquipment[slotIndex] = newItem;
                ListOfIcons[5].image.sprite = newItem.icon;
                ListOfIcons[5].image.color = Color.white;
                ListOfImages[5].sprite = Resources.Load<Sprite>(newItem.name);
                ListOfImages[5].color = Color.white;
            }
        }
        else 
        {
            if ((slotIndex == 4 || slotIndex == 5))
            {
                if (currentEquipment[5] != null) 
                {
                    if ((int)currentEquipment[5].equipSlot == 7)
                    {
                        Unequip(5);
                    }
                }
            }

            if (currentEquipment[slotIndex] != null)
            {
                Unequip(slotIndex);
            }
            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(newItem, oldItem);
            }
            currentEquipment[slotIndex] = newItem;

            ListOfIcons[slotIndex].image.sprite = newItem.icon;
            ListOfIcons[slotIndex].image.color = Color.white;

            ListOfImages[slotIndex].sprite = Resources.Load<Sprite>(newItem.name);
            ListOfImages[slotIndex].color = Color.white;
        }
    }
    public void EquipInShop(Equipment newItem)
    {
        int slotIndexShop = (int)newItem.equipSlot;


        if (currentEquipmentInShop[slotIndexShop] != null) 
        {
            if (slotIndexShop != 7)
            {
                if (currentEquipmentInShop[slotIndexShop].name == newItem.name)
                {
                    ListOfImagesShop[slotIndexShop].color = Color.clear;
                    currentEquipmentInShop[slotIndexShop] = null;

                    Debug.Log("SettingCOlorTo transperent");

                    return;
                }
            }
            else 
            {
                if (currentEquipmentInShop[5].name == newItem.name)
                {
                    ListOfImagesShop[5].color = Color.clear;

                    currentEquipmentInShop[5] = null;

                    Debug.Log("SettingCOlorTo transperent");

                    return;
                }
            }
        }


        ItemsPreview.SetActive(true);
        Debug.Log(newItem.name + newItem.equipSlot);
        Debug.Log(slotIndexShop);
        if (slotIndexShop != 7)
        {
            if (currentEquipmentInShop[4] != null)
            {
                if (currentEquipmentInShop[4].equipSlot == EquipmentSlot.BothHands)
                {
                    ListOfImagesShop[4].color = Color.clear;
                    currentEquipmentInShop[(int)currentEquipmentInShop[4].equipSlot] = null;
                }
            }
            ListOfImagesShop[slotIndexShop].sprite = Resources.Load<Sprite>(newItem.name);
            ListOfImagesShop[slotIndexShop].color = Color.white;
            currentEquipmentInShop[slotIndexShop] = null;
            currentEquipmentInShop[slotIndexShop] = newItem;
        }
        else
        {
            currentEquipmentInShop[4] = null;
            ListOfImagesShop[4].color = Color.clear;
            currentEquipmentInShop[5] = newItem;
            ListOfImagesShop[5].color = Color.white;
            ListOfImagesShop[5].sprite = Resources.Load<Sprite>(newItem.name);
        }
    }

    public void UnEquipAllInShop()
    {
        foreach (Equipment eq in currentEquipmentInShop) 
        {
            if (eq != null) 
            {
                if (eq.equipSlot != EquipmentSlot.BothHands)
                {
                    if (ListOfImagesShop[(int)eq.equipSlot] != null)
                    {
                        currentEquipmentInShop[(int)eq.equipSlot] = null;
                        ListOfImagesShop[(int)eq.equipSlot].color = Color.clear;
                    }
                }
                else 
                {
                    currentEquipmentInShop[(int)eq.equipSlot] = null;
                    ListOfImagesShop[4].color = Color.clear;
                }

            }
        }
        ItemsPreview.SetActive(false);
    }

    public void Unequip(int slotIndex) 
    {
        if (currentEquipment[slotIndex] != null) 
        {
            DataController.DeleteValue("Equipped" + ForEquipmentImgsLoad[(int)currentEquipment[slotIndex].equipSlot] + "Mine");
            DataController.DeleteValue(currentEquipment[slotIndex].name + "Equipped");

            ShowPlayerStats.WasChanged = true;
            if (currentEquipment[slotIndex] != null)
            {
                Equipment oldItem = currentEquipment[slotIndex];
                inventory.Add(oldItem);
                DataController.SaveValue("TotalModifierArmorMine", DataController.GetValue<int>("TotalModifierArmorMine") - oldItem.ModifierArmor);
                DataController.SaveValue("TotalModifierDamageMine", DataController.GetValue<int>("TotalModifierDamageMine") - oldItem.ModifierDamage);
                DataController.SaveValue("TotalModifierMissChanceMine", DataController.GetValue<int>("TotalModifierMissChanceMine") - oldItem.ModifierMissChance);
                DataController.SaveValue("TotalModifierCriticalChanceMine", DataController.GetValue<int>("TotalModifierCriticalChanceMine") - oldItem.ModifierCriticalChance);
                DataController.SaveValue("TotalModifierBashChanceMine", DataController.GetValue<int>("TotalModifierBashChanceMine") - oldItem.ModifierBashChance);
                DataController.SaveValue("TotalModifierStunChanceMine", DataController.GetValue<int>("TotalModifierStunChanceMine") - oldItem.ModifierStunChance);
                DataController.SaveValue("TotalModifierBlockChanceMine", DataController.GetValue<int>("TotalModifierBlockChanceMine") - oldItem.ModifierBlockChance);
                DataController.SaveValue("TotalModifierMagicMine", DataController.GetValue<int>("TotalModifierMagicMine") - oldItem.ModifierMagic);

                currentEquipment[slotIndex] = null;

                if (onEquipmentChanged != null)
                {
                    onEquipmentChanged.Invoke(null, oldItem);
                }
                if (slotIndex != 7)
                {
                    ListOfIcons[slotIndex].image.color = Color.clear;
                    ListOfIcons[slotIndex].image.type = Image.Type.Simple;
                    ListOfIcons[slotIndex].image.useSpriteMesh = true;
                    ListOfImages[slotIndex].color = Color.clear;
                    ListOfImages[slotIndex].type = Image.Type.Simple;
                    ListOfImages[slotIndex].useSpriteMesh = true;
                }
            }
        }      
    }

    public void UnequipAll() 
    {
        for (int i = 0; i < 7; i++) 
        {
            if (currentEquipment[i] != null)          
            {
                Unequip(i);
            }
        }
        currentEquipment[7] = null;
    }

    
}
