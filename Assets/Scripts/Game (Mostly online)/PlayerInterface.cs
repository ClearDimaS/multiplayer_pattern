using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSparksTutorials;
using System;

public class PlayerInterface
{
    #region Singleton
    public static PlayerInterface instance;

    public void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of TheGameManager found!");
            return;
        }
        instance = this;
    }
    #endregion

    public class Points
    {
        /// <summary>
        /// Subtracts ammount of points if result is > 0, otherwise turns result to zero
        /// </summary>
        /// <param name="Points"> Current ammount of points </param>
        /// <param name="NumberToSubtract"> Ammount to subtract </param>
        public int Minus(int Points, int NumberToSubtract)
        {
            if (Points > NumberToSubtract)
            {
                Points -= NumberToSubtract;
            }
            else
            {
                Points = 0;
            }
            return Points;

        }
        /// <summary>
        /// Adds ammount of points if result is not greater then a max value, otherwise turns result to a max value
        /// </summary>
        /// <param name="Points"> Current ammount of points </param>
        /// <param name="NumberToAdd"> Ammount to add </param>
        /// <param name="MaxPoints"> A maximum ammount of points </param>
        public int Plus(int Points, int NumberToAdd, int MaxPoints)
        {
            if (Points + NumberToAdd <= MaxPoints)
            {
                Points += NumberToAdd;
            }
            else
            {
                Points = MaxPoints;
            }
            return Points;
        }
    }

    public Image myHealthBar;
    public Image myStaminaBar;
    public Image enemyHealthBar;
    public Image enemyStaminaBar;

    public Text myHealthText;
    public Text myStaminaText;
    public Text enemyHealthText;
    public Text enemyStaminaText;

    public GameObject MyDebuff;
    public GameObject EnemyDebuff;

    public List<Image> Bars;

    public void SetBars(GameProcess Me, GameProcess Enemy)
    {
        if (PhotonNetwork.isMasterClient)
        {
            Bars = Interaction.instance.GetBars();
            myHealthBar = Bars[0];
            enemyHealthBar = Bars[1];
            myStaminaBar = Bars[2];
            enemyStaminaBar = Bars[3];
            myHealthBarAnim = Bars[4];
            enemyHealthBarAnim = Bars[5];

            Me.playerDebuffs = GameObject.FindGameObjectsWithTag("Debuff")[1];
            Enemy.playerDebuffs = GameObject.FindGameObjectsWithTag("Debuff")[0];
        }
        else if (PhotonNetwork.isNonMasterClientInRoom)
        {
            Bars = Interaction.instance.GetBars();
            myHealthBar = Bars[1];
            enemyHealthBar = Bars[0];
            myStaminaBar = Bars[3];
            enemyStaminaBar = Bars[2];
            myHealthBarAnim = Bars[5];
            enemyHealthBarAnim = Bars[4];

            Me.playerDebuffs = GameObject.FindGameObjectsWithTag("Debuff")[0];
            Enemy.playerDebuffs = GameObject.FindGameObjectsWithTag("Debuff")[1];
        }
        else 
        {
            Bars = SingleInteraction.instance.GetBars();
            myHealthBar = Bars[0];
            enemyHealthBar = Bars[1];
            myStaminaBar = Bars[2];
            enemyStaminaBar = Bars[3];
            myHealthBarAnim = Bars[4];
            enemyHealthBarAnim = Bars[5];

            Me.playerDebuffs = GameObject.FindGameObjectsWithTag("Debuff")[1];
            Enemy.playerDebuffs = GameObject.FindGameObjectsWithTag("Debuff")[0];
        }
        myHealthText = myHealthBar.GetComponentInChildren<Text>();
        enemyHealthText = enemyHealthBar.GetComponentInChildren<Text>();
        myStaminaText = myStaminaBar.GetComponentInChildren<Text>();
        enemyStaminaText = enemyStaminaBar.GetComponentInChildren<Text>();

        myHealthBarAnim.fillAmount = 1;
        enemyHealthBarAnim.fillAmount = 1;
    }


    public Image myHealthBarAnim;

    public Image enemyHealthBarAnim;


    public float RefreshAnimate(bool MineOrNot, float currentHealth, int maxHealth)
    {
        currentHealth -= 0.3f;

        if (MineOrNot)
        {
            myHealthBarAnim.fillAmount = currentHealth / maxHealth;
        }
        else
        {
            enemyHealthBarAnim.fillAmount = currentHealth / maxHealth;
        }

        return currentHealth;
    }
    /// <summary>
    /// Refreshes Image component's fillAmount and Text component's text associated with it
    /// </summary>
    public void Refresh(int myHealth, int myHealthMax, int myStamina, int myStaminaMax, int enemyHealth, int enemyHealthMax, int enemyStamina, int enemyStaminaMax)
    {
        myHealthBar.fillAmount = (float)myHealth / (float)myHealthMax;
        myHealthText.text = myHealth + " / " + myHealthMax;
        myStaminaBar.fillAmount = (float)myStamina / (float)myStaminaMax;
        myStaminaText.text = myStamina + " / " + myStaminaMax;

        enemyHealthBar.fillAmount = (float)enemyHealth / (float)enemyHealthMax;
        enemyHealthText.text = enemyHealth + " / " + enemyHealthMax;
        enemyStaminaBar.fillAmount = (float)enemyStamina / (float)enemyStaminaMax;
        enemyStaminaText.text = enemyStamina + " / " + enemyStaminaMax;
    }

    Mechanics.ConvertFromStats Convert = new Mechanics.ConvertFromStats();

    #region UpdImgsAndTexts_1stTime
    public void UpdImagesAndTexts(GameObject myPlayerInfo, GameObject enemyPlayerInfo, GameProcess me, GameProcess enemy, GameObject LoadingScreen)
    {
        me.buttons.BtnMagic.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("ButtonMagic" + DataController.GetValue<string>("EquippedMagicMine"));

        Text[] myInfo;
        Text[] enemyInfo;

        myInfo = myPlayerInfo.GetComponentsInChildren<Text>();
        enemyInfo = enemyPlayerInfo.GetComponentsInChildren<Text>();

        TextsLoadFunc(me, enemy, myInfo);
        TextsLoadFunc(enemy, me, enemyInfo);
    }
    #endregion


    List<string> btnsToUpdTexts = new List<string> { "ButtonLightAttack", "ButtonMediumAttack", "ButtonHeavyAttack" };
    /// <summary>
    /// Updates debuffs and buttons info - chances of attack 
    /// </summary>
    /// <param name="Buttons"> <para> A list containing player Buttons </para> 
    /// <para> Light </para> 
    /// <para> Medium </para> 
    /// <para> Heavy </para> 
    /// </param>
    /// <param name="debuffs"> Debuffs on a player </param>

    Mechanics Mechs = new Mechanics();

    #region LoadInfoScreen
    void TextsLoadFunc(GameProcess player, GameProcess player2, Text[] TextsArray) // 3
    {
        TextsArray[0].text = LocalisationSystem.GetLocalisedValue("base_damage") + ": " + (int)(Convert.DmgFunc(player)) + " ; " + LocalisationSystem.GetLocalisedValue("light") + ": " + Math.Round(player.lightMult, 2) + " ; " + LocalisationSystem.GetLocalisedValue("medium") + ": " + Math.Round(player.mediumMult, 2) + " ; " + LocalisationSystem.GetLocalisedValue("heavy") + ": " + Math.Round(player.heavyMult, 2) + " ; " + LocalisationSystem.GetLocalisedValue("berserk") + ": " + Math.Round(player.ifLowHpDmg, 2); // DamageLightMultMine, DamageMediumMultMine , DamageHeavyMultMine, IfLowHpDmgMine
        
        TextsArray[1].text = LocalisationSystem.GetLocalisedValue("block_chance") + ": " + (int)(Mechs.FinalBlockChance(player, player2, 1.0f) * 100) + " % ;" + LocalisationSystem.GetLocalisedValue("block_pierce") + " :" + (int)(player.blockMult * 100) + " %"; // BlockMultMine

        TextsArray[2].text = LocalisationSystem.GetLocalisedValue("damage_reduction") + ": " + (Math.Round(Convert.DmgRedFunc(player), 2)) * 100 + " %";

        TextsArray[3].text = LocalisationSystem.GetLocalisedValue("total_hp") + ": " + player.maxHealth + " ; " + LocalisationSystem.GetLocalisedValue("regen_effects") + " : +" + player.regBonus + " % "; // HealthBarScriptLeft.maxHealthLeft,   RegBonusMine
        if (player.strength >= 32)
        {
            TextsArray[3].text += "; " + LocalisationSystem.GetLocalisedValue("stamina_melee_reduction") + ": 15 %";
        }
        
        TextsArray[4].text = LocalisationSystem.GetLocalisedValue("total_stamina") + " : " + player.maxStamina + " ;" + LocalisationSystem.GetLocalisedValue("block_stam") + " : " + player.blockStam + " ; " + LocalisationSystem.GetLocalisedValue("magic_stam") + " : " + player.staminaMagic * 100 + " %"; // StaminaBarScriptLeft.maxStaminaLeft, BlockStamLeft, staminaLossMagicLeft
        if (player.endurance >= 16)
        {
            TextsArray[4].text += " ; " + LocalisationSystem.GetLocalisedValue("stamina_use_reduction") + " : 10 %";
        }

        TextsArray[5].text = LocalisationSystem.GetLocalisedValue("move_dist") + " : " + Convert.MoveFunc(player) + " ; " + LocalisationSystem.GetLocalisedValue("light_dist") + " : " + Convert.LightDistFunc(player) + " ; " + LocalisationSystem.GetLocalisedValue("medium_dist") + " : " + Convert.MediumDistFunc(player) + " ; " + LocalisationSystem.GetLocalisedValue("heavy_dist") + " : " + player.heavyDist * player.moveMult;
        
        TextsArray[6].text = LocalisationSystem.GetLocalisedValue("stam_sleep") + " : " + Convert.StaminaRestoreFunc(player) + " ; " + LocalisationSystem.GetLocalisedValue("stam_regen") + " : " + Convert.StaminaRestoreSwap(player); // StaminaPerSleepMine, StaminaRegenWhenSwapMine
        if (player.sleep >= 16)
        {
            TextsArray[6].text += " ; " + LocalisationSystem.GetLocalisedValue("magic_dmg") + " : " + Convert.MagicDmgFunc(player) + " ;" + LocalisationSystem.GetLocalisedValue("magic_debuff_chance") + " : " + Convert.MagicDebuffChance(player); //  IceMagicMultMine
            //Debug.Log(Convert.MagicDmgFunc(player) + "    " + player.magicModif);
        }

        TextsArray[7].text = LocalisationSystem.GetLocalisedValue("hp_sleep") + ": " + Convert.HealthRestoreFunc(player) + " ; " + LocalisationSystem.GetLocalisedValue("hp_regen") + " : " + Convert.HealthRestoreSwap(player); // HealthPerSleepMine, HealthRegenWhenSwapMine
       
        TextsArray[8].text = (player.Lvl).ToString();
        if (player.critModif > 0)
        {
            TextsArray[9].text = ((int)(player.critModif * 100)) + " %";
        }
        else
        {
            TextsArray[9].text = ": 0 %";
        }
        if (player.bashModif > 0)
        {
            TextsArray[10].text = ((int)(player.bashModif * 100)) + " %";
        }
        else
        {
            TextsArray[10].text = ": 0 %";
        }
        if (player.missModif > 0)
        {
            TextsArray[11].text = ((int)(player.missModif * 100)) + " %";
        }
        else
        {
            TextsArray[11].text = ": 0 %";
        }
        if (player.stunModif > 0)
        {
            TextsArray[12].text = ((int)(player.stunModif * 100))  + " %";
        }
        else
        {
            TextsArray[12].text = ": 0 %";
        }
        TextsArray[13].text = LocalisationSystem.GetLocalisedValue("attack") + "\n" + player.attack;
        TextsArray[14].text = LocalisationSystem.GetLocalisedValue("agility") + "\n" + player.agility;
        TextsArray[15].text = LocalisationSystem.GetLocalisedValue("power") + "\n" + player.power;
        TextsArray[16].text = LocalisationSystem.GetLocalisedValue("strength") + "\n" + player.strength;
        TextsArray[17].text = LocalisationSystem.GetLocalisedValue("endurance") + "\n" + player.endurance;
        TextsArray[18].text = LocalisationSystem.GetLocalisedValue("speed") + "\n" + player.speed;
        TextsArray[19].text = LocalisationSystem.GetLocalisedValue("sleep") + "\n" + player.sleep;
        TextsArray[20].text = LocalisationSystem.GetLocalisedValue("regen") + "\n" + player.regen;
    }
    #endregion
}
