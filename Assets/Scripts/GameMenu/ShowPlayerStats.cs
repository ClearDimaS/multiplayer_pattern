
using DataController = GameSparksTutorials.DataController;
using System.Collections.Generic;
using MonoBehaviour = UnityEngine.MonoBehaviour;
using GameObject = UnityEngine.GameObject;
using Color = UnityEngine.Color;
using Vector3 = UnityEngine.Vector3;
using Image = UnityEngine.UI.Image;
using Text = UnityEngine.UI.Text;
using UnityEngine;
using System;

public class ShowPlayerStats : MonoBehaviour
{
    int Price;
    int count;
    int CurStat;
    int CurIncrStat;
    string StatsName;
    public GameObject StatsBars;
    UnityEngine.UI.Image[] SpecialsList;
    Text title;
    public static bool WasChanged = false;
    private long? a;
    public GameObject CharacterInfo;
    private UnityEngine.UI.Text[] Texts;
    private List<string> ServerNames = new List<string> { "StatsAttack", "StatsAgility", "StatsPower", "StatsStrength", "StatsEndurance", "StatsSpeed", "StatsSleep", "StatsRegen" };
    private List<string> LocalNamesMine = new List<string> { "attackMine", "agilityMine", "powerMine", "strengthMine", "enduranceMine", "speedMine", "sleepMine", "regenMine" };
    public List<Image> BarsList;
    GameObject SpecialsMenu;

    private void Start()
    {
        //DataController.SaveValue("SkillPoints", 10000);
        //DataController.SaveValue("Bread", 10000);
        //DataController.SaveValue("Exp", 10000);

        SpecialsMenu = GameObject.FindGameObjectWithTag("SpecialsPanel");
        Texts = CharacterInfo.GetComponentsInChildren<Text>();
        //Debug.Log(Texts.Length);
        SpecialsMenu.SetActive(false);
        StatsBars.SetActive(false);
    }


    public void ShowOrHideBars()
    {
        foreach (string StatsName in ServerNames)
        {
            DataController.DeleteValue(StatsName + "Incr");
            DataController.DeleteValue("AllPts");
        }
        WasChanged = true;
        StatsBars.SetActive(!StatsBars.activeSelf);
    }

    public void GetSkillPoints() 
    {
        ResetSkillPoints();
        DataController.SaveValue("SkillPoints", 240);
        DataController.SaveValue("Bread", 2400);
    }
    public void MoveOut()
    {
        CharacterInfo.transform.position = new Vector3(-1000, -1000, 0);
    }


    public void MoveIn(string InOrOut)
    {

        //Debug.Log(DataController.GetValue<int>("SkillPoints") + "   " + DataController.GetValue<int>("SpentPoints") + "   " + DataController.GetValue<int>("AllPts"));
        //CharacterInfo = GameObject.FindGameObjectsWithTag("PlayerInfo")[0];
        if (InOrOut == "In")
        {
            DataController.SaveValue("SpentPoints", 0);

            int temp;

            foreach (string StatsName in ServerNames)
            {
                temp = (DataController.GetValue<int>(StatsName + "Incr") + DataController.GetValue<int>(StatsName + "Mine"));

                if (temp >= 8)
                {
                    DataController.SaveValue("SpentPoints", (DataController.GetValue<int>("SpentPoints")) + 8);
                    if (temp >= 16)
                    {
                        DataController.SaveValue("SpentPoints", (DataController.GetValue<int>("SpentPoints")) + 16);
                        if (temp >= 24)
                        {
                            DataController.SaveValue("SpentPoints", (DataController.GetValue<int>("SpentPoints")) + 24);
                            if (temp >= 32)
                            {
                                DataController.SaveValue("SpentPoints", (DataController.GetValue<int>("SpentPoints")) + 32);

                                DataController.SaveValue("SpentPoints", (DataController.GetValue<int>("SpentPoints")) + (temp - 32) * 5);
                            }
                            else
                            {
                                DataController.SaveValue("SpentPoints", (DataController.GetValue<int>("SpentPoints")) + (temp - 24) * 4);
                            }
                        }
                        else
                        {
                            DataController.SaveValue("SpentPoints", (DataController.GetValue<int>("SpentPoints")) + (temp - 16) * 3);
                        }
                    }
                    else
                    {
                        DataController.SaveValue("SpentPoints", (DataController.GetValue<int>("SpentPoints")) + (temp - 8) * 2);
                    }
                }
                else
                {
                    DataController.SaveValue("SpentPoints", (DataController.GetValue<int>("SpentPoints")) + temp);
                }
            }

            foreach (string StatsName in ServerNames)
            {
                DataController.DeleteValue(StatsName + "Incr");
                DataController.DeleteValue("AllPts");
            }
            WasChanged = true;
            if (CharacterInfo.transform.position.y > 0)
            {
                CharacterInfo.transform.position = new Vector3(-1000, -1000, 0);
            }
            else
            {
                CharacterInfo.transform.position = new Vector3(2, 1.7f, -2);
            }
        }
        else
        {
            if (DataController.GetValue<int>("StatsPowerMine") < 16 && EquipmentManager.instance.currentEquipment[7] != null)
            {
                EquipmentManager.instance.currentEquipment[4] = null;
                EquipmentManager.instance.currentEquipment[5] = null;
                EquipmentManager.instance.Unequip(7);
            }
            foreach (string StatsName in ServerNames)
            {
                DataController.SaveValue(StatsName + "Mine", DataController.GetValue<int>(StatsName + "Incr") + DataController.GetValue<int>(StatsName + "Mine"));
                DataController.DeleteValue(StatsName + "Incr");
            }
            
            DataController.SaveValue("SpentPoints", DataController.GetValue<int>("SpentPoints") + DataController.GetValue<int>("AllPts"));
            
            DataController.SaveValue("SkillPoints", DataController.GetValue<int>("SkillPoints") - DataController.GetValue<int>("AllPts"));
            
            DataController.DeleteValue("AllPts");
        }

        Debug.Log(DataController.GetValue<int>("SkillPoints") + "   " + DataController.GetValue<int>("SpentPoints") + "   " + DataController.GetValue<int>("AllPts"));
    }


    public void SpendSkillPoints(Text Title)
    {
        DataController.SaveValue("GSNotSynced" + DataController.GetValue<string>("username"), 1);
        if (Title.text.StartsWith("Attack"))
        {
            StatsName = "StatsAttack";
        }
        else if (Title.text.StartsWith("Agility"))
        {
            StatsName = "StatsAgility";
        }
        else if (Title.text.StartsWith("Power"))
        {
            StatsName = "StatsPower";
        }
        else if (Title.text.StartsWith("Strength"))
        {
            StatsName = "StatsStrength";
        }
        else if (Title.text.StartsWith("Endurance"))
        {
            StatsName = "StatsEndurance";
        }
        else if (Title.text.StartsWith("Speed"))
        {
            StatsName = "StatsSpeed";
        }
        else if (Title.text.StartsWith("Sleep"))
        {
            StatsName = "StatsSleep";
        }
        else if (Title.text.StartsWith("Regen"))
        {
            StatsName = "StatsRegen";
        }
        if (DataController.GetValue<int>("SkillPoints") - (DataController.GetValue<int>("AllPts") + (int)((DataController.GetValue<int>(StatsName + "Incr") + DataController.GetValue<int>(StatsName + "Mine")) * 0.125 + 1)) + 0.01 > 0)
        {
            if ((int)((DataController.GetValue<int>(StatsName + "Incr") + DataController.GetValue<int>(StatsName  + "Mine"))) < 40)
            {
                foreach (string name in ServerNames)
                {
                    if (name == StatsName)
                    {
                        //DataController.SaveValue("SpentPoints", (DataController.GetValue<int>("SpentPoints") + (int)((DataController.GetValue<int>(StatsName + "Incr") + DataController.GetValue<int>(StatsName + "Mine")) * 0.125 + 1 - 0.01)));
                        DataController.SaveValue(StatsName + "Incr", 1 + DataController.GetValue<int>(StatsName + "Incr"));
                        DataController.SaveValue("AllPts", (DataController.GetValue<int>("AllPts") + (int)((DataController.GetValue<int>(StatsName + "Incr") + DataController.GetValue<int>(StatsName + "Mine")) * 0.125 + 1 - 0.01)));
                        WasChanged = true;
                        break;
                    }
                };
            }

        }
    }


    public void ReturnSpentSkillPoints(Text Title)
    {
        if (Title.text.StartsWith("Attack"))
        {
            StatsName = "StatsAttack";
        }
        else if (Title.text.StartsWith("Agility"))
        {
            StatsName = "StatsAgility";
        }
        else if (Title.text.StartsWith("Power"))
        {
            StatsName = "StatsPower";
        }
        else if (Title.text.StartsWith("Strength"))
        {
            StatsName = "StatsStrength";
        }
        else if (Title.text.StartsWith("Endurance"))
        {
            StatsName = "StatsEndurance";
        }
        else if (Title.text.StartsWith("Speed"))
        {
            StatsName = "StatsSpeed";
        }
        else if (Title.text.StartsWith("Sleep"))
        {
            StatsName = "StatsSleep";
        }
        else if (Title.text.StartsWith("Regen"))
        {
            StatsName = "StatsRegen";
        }
        foreach (string name in ServerNames)
        {
            if (name == StatsName)
            {
                if (DataController.GetValue<int>(StatsName + "Incr") > 0)
                {
                    DataController.SaveValue("AllPts", (DataController.GetValue<int>("AllPts") - (int)((DataController.GetValue<int>(StatsName + "Incr") + DataController.GetValue<int>(StatsName + "Mine")) * 0.125 + 1 - 0.01)));
                    DataController.SaveValue(StatsName + "Incr", -1 + DataController.GetValue<int>(StatsName + "Incr"));
                    WasChanged = true;
                }
                break;
            }
        };
    }


    public void ResetSkillPoints()
    {
        if (EquipmentManager.instance.currentEquipment[5] != null)
        {
            if ((int)EquipmentManager.instance.currentEquipment[5].equipSlot == 7)
            {
                EquipmentManager.instance.Unequip(5);
            }
        }

        DataController.SaveValue("SkillPoints", DataController.GetValue<int>("SkillPoints") + DataController.GetValue<int>("SpentPoints"));

        DataController.SaveValue("AllPts", 0);

        DataController.SaveValue("SpentPoints", 0);

        foreach (string StatsName in ServerNames)
        {
            DataController.SaveValue(StatsName + "Mine", 0);
            DataController.SaveValue(StatsName + "Incr", 0);
        }
        WasChanged = true;
    }


    Mechanics.ConvertFromStats Conv = new Mechanics.ConvertFromStats();

    Mechanics Mechs = new Mechanics();


    void Update()
    {
        if (WasChanged == true)
        {
            LoadPlayerBase();
            //Debug.Log("Strength and Endurance: " + player.strength + "    " + player.endurance);
            count = 0;
            foreach (Image img in BarsList)
            {
                CurStat = DataController.GetValue<int>(ServerNames[count] + "Mine");
                CurIncrStat = DataController.GetValue<int>(ServerNames[count] + "Incr");
                title = img.GetComponentInChildren<Text>();
                title.text = LocalisationSystem.GetLocalisedValue(ServerNames[count].Substring(5).ToLower()) + ": " + (CurStat + CurIncrStat);

                Price = (CurStat + CurIncrStat) / 8 + 1;
                if (Price < 6)
                {
                    img.GetComponentsInChildren<Text>()[3].text = (Price).ToString();
                }
                else
                {
                    img.GetComponentsInChildren<Text>()[3].text = "";
                }


                img.fillAmount = ((40f - CurStat - CurIncrStat) / 40f);
                SpecialsList = img.GetComponentsInChildren<Image>();
                for (int i = 1; i <= 5; i++)
                {
                    if (i * 8 <= (CurStat + CurIncrStat))
                    {
                        SpecialsList[i].color = Color.yellow;
                    }
                    else
                    {
                        SpecialsList[i].color = Color.black;
                    }
                }
                count += 1;
            }
            foreach (Text text in Texts)
            {
                if (text.name.StartsWith("Modifier"))
                {
                    if (text.name == "ModifierCritical") 
                    {
                        text.text = (int)(DataController.GetValue<int>("Total" + text.name + "ChanceMine") + player.critModif * 100) + " %";
                    }
                    else if (text.name == "ModifierBash")
                    {
                        text.text = (int)(DataController.GetValue<int>("Total" + text.name + "ChanceMine") + player.bashModif * 100) + " %";
                    }
                    else if (text.name == "ModifierStun")
                    {
                        text.text = (int)(DataController.GetValue<int>("Total" + text.name + "ChanceMine") + player.stunModif * 100) + " %";
                    }
                    else if (text.name == "ModifierMiss")
                    {
                        text.text = (int)(DataController.GetValue<int>("Total" + text.name + "ChanceMine") + player.missModif * 100) + " %";
                    }            
                }
                if (text.name.StartsWith("Stats"))
                {
                    text.text = (DataController.GetValue<int>(text.name + "Mine") + DataController.GetValue<int>(text.name + "Incr")).ToString();
                }
                if (text.name == "AllSkillPoints")
                {
                    text.text = (DataController.GetValue<int>("SkillPoints") - DataController.GetValue<int>("AllPts")).ToString();
                }
                if (text.name.Contains("MagDamage"))
                {
                    if (DataController.GetValue<int>("StatsSleepMine") + DataController.GetValue<int>("StatsSleepIncr") >= 16)
                    {
                        if (DataController.GetValue<string>("EquippedMagic") != "")
                        {
                            text.text = "magic: " + DataController.GetValue<string>("EquippedMagicMine") + "\n";
                            if (DataController.GetValue<string>("EquippedMagicMine") != "Curse")
                            {
                                text.text += LocalisationSystem.GetLocalisedValue("Damage") + " : " + Conv.MagicDmgFunc(player) + " + " + (int)(Conv.MagicDmgFunc(player) / Helper.debuffDmgMultCurse) + " x" +
                                    Conv.MagicDurationFunc(player) + "\n";
                            }
                            else
                            {
                                text.text += LocalisationSystem.GetLocalisedValue("Damage") + " : " + Conv.MagicDmgFunc(player) + " + " + (int)(Conv.MagicDmgFunc(player) / Helper.debuffDmgMultFire) + " x" +
                                    Conv.MagicDurationFunc(player) + "\n";
                            }

                            text.text += LocalisationSystem.GetLocalisedValue("magic_debuff_chance") + " : " + (int)(Conv.MagicDebuffChance(player)) + " %" + "\n";  // magic_debuff_chance
                            text.text += LocalisationSystem.GetLocalisedValue("magic_debuff_duration") + " : " + Conv.MagicDurationFunc(player) + " Turns"; //magic_debuff_duration
                        }
                        else
                        {
                            text.text = "";
                        }

                    }
                    else
                    {
                        text.text = LocalisationSystem.GetLocalisedValue("magic_dmg") + "  : " + 0; // magic_dmg
                    }
                }
                if (text.name.StartsWith("Cnt"))
                {
                    if (text.name.ToCharArray()[3].ToString() + text.name.ToCharArray()[4].ToString() == "Ba")
                    {
                        text.text = ((int)(Conv.DmgFunc(player) * (1 + player.damageModif))).ToString();
                    }
                    if (text.name.ToCharArray()[3].ToString() + text.name.ToCharArray()[4].ToString() == "Bl")
                    {
                        text.text = ((int)((Conv.BlockFunc(player) + player.blockModif) * 100)).ToString() + " %";
                    }
                    if (text.name.ToCharArray()[3].ToString() + text.name.ToCharArray()[4].ToString() == "Dm")
                    {
                        text.text = ((int)((Conv.DmgRedFunc(player) + player.armorModif) * 100)).ToString() + " %";
                    }
                    if (text.name.ToCharArray()[3].ToString() + text.name.ToCharArray()[4].ToString() == "Tl")
                    {
                        text.text = player.maxHealth.ToString();
                    }
                    if (text.name.ToCharArray()[3].ToString() + text.name.ToCharArray()[4].ToString() == "Tt")
                    {
                        text.text = player.maxStamina.ToString();
                    }
                    if (text.name.ToCharArray()[3].ToString() + text.name.ToCharArray()[4].ToString() == "Mo")
                    {
                        text.text = (Conv.MoveFunc(player)).ToString();
                    }
                    if (text.name.ToCharArray()[3].ToString() + text.name.ToCharArray()[4].ToString() == "St")
                    {
                        text.text = (Conv.StaminaRestoreFunc(player)).ToString();
                    }
                    if (text.name.ToCharArray()[3].ToString() + text.name.ToCharArray()[4].ToString() == "HP")
                    {
                        text.text = (Conv.HealthRestoreFunc(player)).ToString();
                    }
                }
            }

            WasChanged = false;
        }
    }


    GameProcess player;

    string MineOrOther;

    public void LoadPlayerBase()
    {
        if (player == null) 
        {
            player = new GameProcess();
        }

        player.magicEquipped = DataController.GetValue<string>("EquippedMagicMine");
        player.Lvl = DataController.GetValue<int>("Exp") / 100;

        MineOrOther = "Mine";

        player.attack = DataController.GetValue<int>("StatsAttack" + MineOrOther) + DataController.GetValue<int>("StatsAttackIncr");
        player.agility = DataController.GetValue<int>("StatsAgility" + MineOrOther) + DataController.GetValue<int>("StatsAgilityIncr");
        player.power = DataController.GetValue<int>("StatsPower" + MineOrOther) + DataController.GetValue<int>("StatsPowerIncr");
        player.strength = DataController.GetValue<int>("StatsStrength" + MineOrOther) + DataController.GetValue<int>("StatsStrengthIncr");
        player.endurance = DataController.GetValue<int>("StatsEndurance" + MineOrOther) + DataController.GetValue<int>("StatsEnduranceIncr");
        player.speed = DataController.GetValue<int>("StatsSpeed" + MineOrOther) + DataController.GetValue<int>("StatsSpeedIncr");
        player.sleep = DataController.GetValue<int>("StatsSleep" + MineOrOther) + DataController.GetValue<int>("StatsSleepIncr");
        player.regen = DataController.GetValue<int>("StatsRegen" + MineOrOther) + DataController.GetValue<int>("StatsRegenIncr");

        // info about modifs in Equipment class

        player.armorModif = (float)Math.Round((double)((DataController.GetValue<int>("TotalModifierArmor" + MineOrOther)) / 100.0f), 2);
        player.damageModif = (float)Math.Round((double)((DataController.GetValue<int>("TotalModifierDamage" + MineOrOther)) / 100.0f), 2);
        player.missModif = (float)Math.Round((double)((DataController.GetValue<int>("TotalModifierMissChance" + MineOrOther)) / 100.0f), 2);
        player.critModif = (float)Math.Round((double)((DataController.GetValue<int>("TotalModifierCriticalChance" + MineOrOther)) / 100.0f), 2);
        player.bashModif = (float)Math.Round((double)((DataController.GetValue<int>("TotalModifierBashChance" + MineOrOther)) / 100.0f), 2); ;
        player.stunModif = (float)Math.Round((double)((DataController.GetValue<int>("TotalModifierStunChance" + MineOrOther)) / 100.0f), 2);
        player.blockModif = (float)Math.Round((double)((DataController.GetValue<int>("TotalModifierBlockChance" + MineOrOther)) / 100.0f), 2);
        player.magicModif = (float)Math.Round((double)((DataController.GetValue<int>("TotalModifierMagic" + MineOrOther)) / 100.0f), 2);

        //Debug.Log(DataController.GetValue<int>("TotalModifierBlockChance" + MineOrOther) + "  data controller value from clothes, and player modif: " + player.blockModif + ", and player Magic modif to check if its working properly: " + player.magicModif);
        
        LoadPlayerSecondary(player);
    }


    Mechanics.MechsHelper Helper = new Mechanics.MechsHelper();

    Mechanics.Specials specials = new Mechanics.Specials();


    public void LoadPlayerSecondary(GameProcess player)
    {
        player.baseDamage = Helper.BaseDamage;    /////
        player.heavyMult = Helper.heavyMult;
        player.mediumMult = Helper.mediumMult;
        player.lightMult = Helper.lightMult;

        player.lightChance = Conv.BlockFunc(player) * Helper.lightChanceMult;
        player.mediumChance = Conv.BlockFunc(player) * Helper.mediumChanceMult;
        player.heavyChance = Conv.BlockFunc(player) * Helper.heavyChanceMult;

        player.ifLowHpDmg = 1.0f;
        player.blockMult = Helper.blockMult;
        player.critMult = Helper.critMult;

        player.staminaPerMove = Helper.staminaPerMove;
        player.staminaHeavyMult = Helper.staminaHeavyMult;
        player.staminaMediumMult = Helper.staminaMediumMult;
        player.staminaLightMult = Helper.staminaLightMult;
        player.staminaMagic = Helper.staminaMagic;
        player.blockStam = Helper.blockStam;

        player.stamina = Conv.TtlStamina(player);
        player.maxStamina = Conv.TtlStamina(player);

        player.baseStamForSleep = Helper.baseStamForSleep;
        player.baseStamForSwap = Helper.baseStamForSwap;
        player.heavyDist = Helper.heavyDist;

        player.moveMult = Helper.moveMult;

        player.health = Conv.TtlHealth(player);
        player.maxHealth = Conv.TtlHealth(player);
        player.regBonus = Helper.regBonus;
        player.baseHealthForSleep = Helper.baseHealthForSleep;
        player.baseHealthForSwap = Helper.baseHealthForSwap; /////

        player.curseDebuff = 0.0f;
        player.fireDebuff = 1.0f;
        player.magicDebuffMult = Helper.magicDebuffMult;

        //Debug.Log("Health and Stamina: " + Conv.TtlHealth(player) + "   " + Conv.TtlStamina(player) + " - Before specials");
        specials.applySpecials(player, null);
        //Debug.Log("Health and Stamina: " + player.maxHealth + "   " + player.maxStamina + " - After specials");
    }


}
