using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    public int ammount;
    public bool isDefaultItem = false;
    public int Price;
    public int SellPrice;
    public int ModifierArmor = 0;  //  + % of a player incoming damage reduction
    public int ModifierDamage = 0;  //  + % of a player damage increase
    public int ModifierMissChance = 0;  // + % of a chanse an opponent misses an attack
    public int ModifierCriticalChance = 0;  // + % of a chanse an opponent gets critical damage
    public int ModifierBashChance = 0;  // + % of a chanse an opponent skips next turn
    public int ModifierStunChance = 0;  // + % of a chanse an opponent will only attack next turn
    public int ModifierBlockChance = 0;  // + % of a player block chance
    public int ModifierMagic = 0;  // + % magic

    public virtual void Use() 
    {
        ///Use the item
        ///Something might happen
        ///
        Debug.Log("Using " + name) ;
    }

    public void RemoveFromInventory() 
    {
        Inventory.instance.Remove(this);
    }
}
