using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparksTutorials;

public class Mechanics
{
    #region Singleton
    public static Mechanics instance;

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


    ConvertFromStats Convert = new ConvertFromStats();

    MechsHelper Helper = new MechsHelper();


    /// <summary>
    /// Contains in-game values which shouldnt be changed by stats, but may be changed with specials
    /// </summary>
    public class MechsHelper
    {
        public float debuffDmgMultFire = 5.0f;

        public float debuffDmgMultCurse = 4.0f;

        public float lightChanceMult = 0.5f;
        public float mediumChanceMult = 1.0f;
        public float heavyChanceMult = 2.0f;

        public int BaseDamage = 20;

        public float heavyMult = 1.5f;
        public float mediumMult = 1.3f;
        public float lightMult = 1.0f;


        public float blockMult = 0.1f;

        public float critMult = 2.0f;

        public int staminaPerMove = 20;
        public float staminaHeavyMult = 2.0f;
        public float staminaMediumMult = 1.5f;
        public float staminaLightMult = 1.0f;

        public float moveMult = 1.0f;

        public float staminaMagic = 0.25f;
        public float magicDebuffMult = 0.0f;

        public int blockStam = 5;

        public int baseStamForSleep = 40;
        public int baseStamForSwap = 8;

        public float heavyDist = 5.0f;

        public float ifLowHpDmg = 1.0f;

        public int baseHealthForSleep = 3;
        public int baseHealthForSwap = 1;

        public float regBonus = 0.0f;
    }


    /// <summary>
    /// Converts player Stats to actual in-game numbers
    /// </summary>
    public class ConvertFromStats
    {
        public int MagicFireDebuff(GameProcess player)
        {
            return (int)((player.sleep) / 2.0f * (1 + player.magicModif));
        }

        public int MagicCurseDebuff(GameProcess player)
        {
            return (int)((player.sleep) / 2.0f * (1 + player.magicModif));
        }

        public int MagicDmgFunc(GameProcess player)
        {
            return (int)((player.sleep) / 2.0f * (1 + player.magicModif));
        }

        public int MagicDurationFunc(GameProcess player)
        {
            Debug.Log((int)((player.sleep + 11) / 10 * (1 + player.magicModif)) + "  " + player.magicModif);
            return (int)((player.sleep) / 10 * (1 + player.magicModif));
        }

        public int MagicDebuffChance(GameProcess player)
        {
            return (int)((player.sleep) / 2.0f * (1 + player.magicDebuffMult) * (1 + player.magicModif)) * 100;
        }

        public int DmgFunc(GameProcess player)
        {
            return (int)((player.baseDamage + player.attack / 2.0f) * (1 + player.damageModif));
        }

        public float BlockFunc(GameProcess player)
        {
            return (float)Math.Round( 0.1f + 0.3f * player.agility / 40.0f, 2);
        }

        public float BlockResistFunc(GameProcess player)
        {
            return (float)Math.Round(0.2f * player.regen / 40.0f, 2);
        }

        public float DmgRedFunc(GameProcess player)
        {
            return (float)Math.Round((0.3f * player.power / 40.0f * (1 + player.armorModif)), 3);
        }

        public int StaminaRestoreFunc(GameProcess player)
        {
            int temp;

            int tempText;

            //Debug.Log("Curse debuff, so health decreased: " + player.curseDebuff + "000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");

            temp = (int)(player.sleep * 1.5f) + player.baseStamForSleep;

            if (player.animation.Animator != null)
            {
                if (player.maxStamina != player.stamina)
                {
                    tempText = player.maxStamina - player.stamina;

                    if (player.stamina + temp <= player.maxStamina)
                    {
                        tempText = temp;
                    }

                    player.animation.instantiateText("", player.playerObj.transform.position, "+ " + tempText.ToString(), player.playerObj.transform.position, player.iAmLeft, false, false, false, false, true);
                }
            }
            else
            {
                //Debug.Log("NO ANIMATOR");
            }

            return temp;
        }

        public int HealthRestoreFunc(GameProcess player)
        {
            int temp;

            int tempText;

            Debug.Log(player.regBonus +  "    "  +  player.curseDebuff + "sssssssssssssssssssssssssssssssssss");
            temp = (int)(((player.regen * 0.3f) + player.baseHealthForSleep) * (1 + player.regBonus - 2 * player.curseDebuff));

            if (player.animation.Animator != null)
            {
                if (player.maxHealth != player.health)
                {
                    tempText = player.maxHealth - player.health;

                    if (player.health + temp <= player.maxHealth)
                    {
                        tempText = temp;
                    }

                    player.animation.instantiateText("", player.playerObj.transform.position, "+ " + tempText.ToString(), player.playerObj.transform.position, player.iAmLeft, false, true, false, false, false);
                }
            }

            return temp;
        }

        public int StaminaRestoreSwap(GameProcess player)
        {
            int temp;

            int tempText;

            //Debug.Log("Curse debuff, so health decreased: " + player.curseDebuff + "000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");

            temp = (int)(player.sleep * 0.1f) + player.baseStamForSwap;

            if (player.animation.Animator != null)
            {
                if (player.maxStamina != player.stamina)
                {
                    tempText = player.maxStamina - player.stamina;

                    if (player.stamina + temp <= player.maxStamina)
                    {
                        tempText = temp;
                    }

                    player.animation.instantiateText("", player.playerObj.transform.position, "+ " + tempText.ToString(), player.playerObj.transform.position, player.iAmLeft, false, false, false, false, true);
                }
            }
            else
            {
                Debug.Log("NO ANIMATOR");
            }

            return temp;
        }

        public int HealthRestoreSwap(GameProcess player)
        {
            int temp;

            int tempText;

            //Debug.Log("Curse debuff, so health decreased: " + player.curseDebuff + "000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");

            temp = (int)(((player.regen * 0.1f) + player.baseHealthForSwap) * (1 + player.regBonus - player.curseDebuff));

            if (player.animation.Animator != null)
            {
                if (player.maxHealth != player.health)
                {
                    tempText = player.maxHealth - player.health;

                    if (player.health + temp <= player.maxHealth)
                    {
                        tempText = temp;
                    }

                    player.animation.instantiateText("", player.playerObj.transform.position, "+ " + tempText.ToString(), player.playerObj.transform.position, player.iAmLeft, false, true, false, false, false);
                }
            }
            else 
            {
                Debug.Log("NO ANIMATOR");
            }

            return temp;
        }

        public enum MoveType
        {
            Simple,
            LightAttack,
            MediumAttack
        }

        public float UnifyMove(MoveType type, GameProcess player)
        {
            switch (type)
            {
                case MoveType.Simple:
                    return (MoveFunc(player));
                case MoveType.LightAttack:
                    return LightDistFunc(player);
                case MoveType.MediumAttack:
                    return MediumDistFunc(player);
                default:
                    return 0;
            }
        }

        public float MoveFunc(GameProcess player)
        {
            return (float)Math.Round((4f + (player.speed / 3.63f)) * player.moveMult, 1);
        }

        public float LightDistFunc(GameProcess player)
        {
            return (float)Math.Round((6f + (player.speed / 4.44f)) * player.moveMult, 1);
        }

        public float MediumDistFunc(GameProcess player)
        {
            return (float)Math.Round((6f + (player.speed / 10)) * player.moveMult, 1);
        }

        public int TtlHealth(GameProcess player)
        {
            return (int)(100 + (player.strength) * (4.0f));
        }

        public int TtlStamina(GameProcess player)
        {
            return (int)(150 + (player.endurance) * (3.0f));
        }
    }


    RandomHelper RandomCount = new RandomHelper();


    public class RandomHelper
    {
        ConvertFromStats Conv = new ConvertFromStats();

        double RandVar1;
        double RandVar2;
        double RandVar3;
        double RandVar4;
        double RandVar5;
        double RandVar6;

        float multToReturn;
        public float SpAttacks(GameProcess player1, GameProcess player2, float blockChance)
        {
            RandVar1 = player1.rand1.NextDouble();
            RandVar2 = player1.rand2.NextDouble();
            RandVar3 = player1.rand3.NextDouble();
            RandVar4 = player1.rand4.NextDouble();
            RandVar5 = player1.rand5.NextDouble();

            Debug.Log(RandVar1 + "  " + RandVar2 + "  " + RandVar3 + "  " + RandVar4 + "  " + RandVar5 + "   " + player1.iAmLeft);

            string boolName = "";

            string dispText = "";

            Debug.Log("BlockChance: " + blockChance + "\n" +
                "DodgeChance: " + player2.missModif + "\n" +
                "CritChance: " + player2.critModif + "\n" +
                "BashChance: " + player2.bashModif + "\n" +
                "StunChance: " + player2.stunModif + "\n" );

            multToReturn = 1;
            if (blockChance > RandVar1)
            {
                dispText += "Blocked ";
                Debug.Log(dispText);
                multToReturn *= player2.blockMult;
                boolName = "PlayerBlock";
            }
            else if (player2.missModif > RandVar2)
            {
                dispText += "Miss ";
                Debug.Log(dispText);
                multToReturn = 0;
                boolName = "PlayerGetMiss";
            }
            else
            {
                if (player1.critModif > RandVar3)
                {
                    dispText += "Crit ";
                    Debug.Log(dispText);
                    multToReturn *= player1.critMult;
                    boolName = "PlayerGetCrit";
                }
                if (player1.bashModif > RandVar4)
                {
                    dispText += "Bashed ";
                    Debug.Log(dispText);
                    player2.bashed = true;
                    boolName = "PlayerGetBash";
                }
                if (player1.stunModif > RandVar5)
                {
                    dispText += "Stunned";
                    Debug.Log(dispText);
                    player2.stunned = true;
                    boolName = "PlayerGetStun";
                }
            }

            if (multToReturn == 1)
            {
                Debug.Log(dispText);
                boolName = "PlayerGetDamage";
            }


            // add here debuff parents and how they should be chosen

            // player1.animation.instantiateText("", player2.playerObj.transform.position, boolName, player2.playerObj.transform.position, player2.iAmLeft, true);

            player2.animation.instantiateText("", new Vector3(0, 0, 0), dispText, player2.playerObj.transform.position, player2.iAmLeft, true, false, false, true, false);

            player2.animation.setAnimatorBoolTrue(boolName);

            player1.Animating = true;

            player2.Animating = true;

            return multToReturn;
        }


        public float MagicDebuff(GameProcess player1, GameProcess player2, Mechanics Mechs)
        {
            RandVar6 = player1.rand6.NextDouble();

            if ((Conv.MagicDebuffChance(player1) / 100.0f) > RandVar6)
            {
                if (player1.magicEquipped == "Fire")
                {
                    player2.onFire = Mechs.Convert.MagicDurationFunc(player1);
                    player2.fireDebuff = Mechs.Convert.MagicFireDebuff(player1) / 100.0f;
                }
                else if (player1.magicEquipped == "Curse")
                {
                    
                    player2.cursed = Mechs.Convert.MagicDurationFunc(player1);
                    Debug.Log("Sleep is:" + player1.sleep + " ; " + "Duration is: " + player2.cursed);
                    player2.curseDebuff = Mechs.Convert.MagicCurseDebuff(player1) / 100.0f;
                }
                else if (player1.magicEquipped == "Bolt")
                {
                    return 2;
                }
                else if (player1.magicEquipped == "Ice")
                {
                    player2.bashed = true;
                }
            }
            return 1;
        }
    }


    public class Specials
    {
        public List<string> AttackTexts = new List<string>();
        public List<string> AgilityTexts = new List<string>();
        public List<string> PowerTexts = new List<string>();
        public List<string> StrengthTexts = new List<string>();

        public List<string> EnduranceTexts = new List<string>();
        public List<string> SpeedTexts = new List<string>();
        public List<string> SleepTexts = new List<string>();
        public List<string> RegenTexts = new List<string>();

        #region NumbersForChanging

        #region Attack
        int Attack1 = 3;
        float Attack2 = 0.151f;
        float Attack3 = 0.151f;
        float Attack4 = 0.151f;
        float Attack5 = 0.301f;
        #endregion

        #region Agility
        float Agility1 = 0.011f;
        float Agility2 = 0.031f;
        float Agility3 = 0.051f;
        float Agility4 = 0.131f;
        float Agility5 = 0.0f;
        #endregion

        #region Power
        float Power1 = 0.011f;
        //Using double handed weapons
        float Power3 = 0.071f;
        float Power4 = 0.051f;
        float Power5 = 0.81f;
        #endregion

        #region Strength
        int Strength1 = 30;
        float Strength2 = 0.151f;
        float Strength3 = 0.051f;
        float Strength4 = 0.101f;
        float Strength5 = 0.101f;
        #endregion

        #region Endurance
        int Endurance1 = 40;
        float Endurance2 = 0.15f;
        float Endurance3 = 0.50f;
        float Endurance4 = 0.10f;
        float Endurance5 = 0.08f;
        #endregion

        #region Speed
        float Speed1 = 0.101f;
        float Speed2 = 0.501f;
        float Speed3 = 0.031f;
        float Speed4 = 0.091f;
        float Speed5 = 0.051f;
        #endregion

        #region Sleep
        int Sleep1 = 9;
        //
        int Sleep3 = 2;
        float Sleep4 = 0.121f;
        float Sleep5 = 0.101f;
        #endregion

        #region Regen
        int Regen1 = 6;
        float Regen2 = 0.101f;
        int Regen3 = 1;
        float Regen4 = 0.151f;
        float Regen5 = 0.251f;
        #endregion

        #endregion

        public void SetTexts() 
        {
            AttackTexts = new List<string> { " + " + Attack1 + " Damage", 
                " + " + (int)(Attack2 * 100) + " % To Damage to each Light attack ", 
                " + " + (int)(Attack3 * 100) + " % To Damage to each Medium attack", 
                " + " + (int)(Attack4 * 100) + " % To Damage to each Heavy attack ", 
                " Now if Your Current HP is below 20 % you deal " + (int)(Attack5 * 100) + " % more damage " };

            AgilityTexts = new List<string> { " + " + (int)(Agility1 * 100.0f) + " % To Crit chance",
                " + " + (int)(Agility2 * 100.0f) + " % To Block chance ",
                " + " + (int)(Agility3 * 100.0f) + " % To Dodge chance",
                " + " + (int)(Agility4 * 100.0f) + " % To all Melee Damage ",
                (int)(Agility5 * 100.0f) + " % less of Damage pass through block " };

            PowerTexts = new List<string> { " + " + (int)( Power1 * 100.0f) + " % To Damage reduction",
                " You can use Double Handed weapons ",
                " + " + (int)( Power3 * 100.0f) + " % To Damage reduction ",
                " - " + (int)( Power4 * 100.0f) + " % to enemy magic ",
                " + " + (int)( Power5 * 100.0f) + " % Damage through enemy block  " };

            StrengthTexts = new List<string> { " + " +  Strength1 + " Total HP ",
                " Health regeneration effects get " + (int)(Strength2 * 100.0f) + " % bonus",
                " Your Stun and Bash chances get + " + (int)(Strength3 * 100.0f) + " %",
                (int)(Strength4 * 100.0f) + " % Stamina use reduction to all Melee attacks ",
                " + " + (int)(Strength5 * 100.0f) + " % to Total HP" };

            EnduranceTexts = new List<string> { " + " + Endurance1  + " to Total Stamina",
                (int)(Endurance2 * 100.0f) + " % less Stamina for moving ",
                " Block needs " + (int)(Endurance3 * 100.0f) + " % less Stamina ",
                " + " + (int)(Endurance4 * 100.0f) + " % to Total Stamina",
                "" + (int)(Endurance5 * 100.0f) + " % Stamina use reduction to magic" };

            SpeedTexts = new List<string> { " + " + (int)(Speed1 * 100.0f) + " % to Total movement distance",
                " + " + (int)(Speed2 * 100.0f) + " % To Crit Damage",
                " Enemy Bash, Stun Chances get - " + (int)(Speed3 * 100.0f)  + " % ",
                " + " + (int)(Speed4 * 100.0f) + " % Crit Damage Chance",
                " Enemy Crit and Dodge Chances get - " + (int)(Speed5 * 100.0f)  + " % " };

            SleepTexts = new List<string> { " + " + Sleep1 + " To Stamina regeneration per sleep",
                " You can use Magic! ",
                " + " + Sleep3 + " Stamina each turn",
                " + " + (int)(Sleep4 * 100.0f) + " % to magic debuff chance",
                " + " + (int)(Sleep5 * 100.0f) + " % To magic" };

            RegenTexts = new List<string> { " + " + Regen1 + " To Health regeneration per sleep",
                " enemy block gets - " + (int)(Regen2 * 100.0f) + " % chance ",
                " + " + Regen3 + " HP each turn",
                " - " + (int)(Regen4 * 100.0f) + "  % to enemy magic damage",
                " + " + (int)(Regen5 * 100.0f) + " % To health regeneration effets" };
        }


        public void applySpecials(GameProcess Me, GameProcess Enemy) 
        {
            if (Me.attack >= 8) 
            {
                Me.baseDamage += Attack1;
                if (Me.attack >= 16) 
                {
                    Me.lightMult += Attack2;
                    if (Me.attack >= 24)
                    {
                        Me.mediumMult += Attack3;
                        if (Me.attack >= 32)
                        {
                            Me.heavyMult += Attack4;
                            if (Me.attack == 40)
                            {
                                Me.ifLowHpDmg += Attack5;
                            }
                        }
                    }
                }
            }

            if (Me.agility >= 8)
            {
                Me.critModif += Agility1;
                if (Me.agility >= 16)
                {
                    Me.blockModif += Agility2;
                    if (Me.agility >= 24)
                    {
                        Me.missModif += Agility3;
                        if (Me.agility >= 32)
                        {
                            Me.lightMult += Agility4;
                            Me.mediumMult += Agility4;
                            Me.heavyMult += Agility4;
                            if (Me.agility == 40)
                            {
                                Me.blockMult -= Agility5;
                            }
                        }
                    }
                }
            }

            if (Me.power >= 8)
            {
                Me.armorModif += Power1;
                if (Me.power >= 16)
                {

                    if (Me.power >= 24)
                    {
                        Me.armorModif += Power3;
                        if (Me.power >= 32)
                        {
                            if (Enemy != null)
                            {
                                Enemy.magicModif -= Power5;
                            }
                            if (Me.power == 40)
                            {
                                if (Enemy != null) 
                                {
                                    Enemy.blockModif -= Power5;
                                }
                            }
                        }
                    }
                }
            }

            if (Me.strength >= 8)
            {
                Me.health += Strength1;
                Me.maxHealth += Strength1;

                if (Me.strength >= 16)
                {
                    Me.regBonus += Strength2;
                    if (Me.strength >= 24)
                    {
                        Me.bashModif += Strength3;
                        Me.stunModif += Strength3;
                        if (Me.strength >= 32)
                        {
                            Me.staminaLightMult *= (1 - Power3);
                            Me.staminaMediumMult *= (1 - Power3);
                            Me.staminaHeavyMult *= (1 - Power3);
                            if (Me.strength == 40)
                            {
                                Me.health = (int)(Me.health * (1 + Strength5));
                                Me.maxHealth = (int)(Me.maxHealth * (1 + Strength5));
                            }
                        }
                    }
                }
            }


            if (Me.endurance >= 8)
            {
                Me.stamina += Endurance1;
                Me.maxStamina += Endurance1;
                if (Me.endurance >= 16)
                {
                    Me.staminaPerMove = (int)(Me.staminaPerMove * (1 - Endurance2));
                    if (Me.endurance >= 24)
                    {
                        Me.blockStam = (int)(Me.staminaPerMove * (1 - Endurance3));
                        if (Me.endurance >= 32)
                        {
                            Me.stamina = (int)(Me.stamina * (1 + Endurance4));
                            Me.maxStamina = (int)(Me.maxStamina * (1 + Endurance4));
                            if (Me.endurance == 40)
                            {
                                Me.staminaMagic -= Endurance5;
                            }
                        }
                    }
                }
            }

            if (Me.speed >= 8)
            {
                Me.moveMult += Speed1;
                if (Me.speed >= 16)
                {
                    Me.critMult += Speed2;
                    if (Me.speed >= 24)
                    {
                        if (Enemy != null) 
                        {
                            Enemy.bashModif -= Speed3;
                            Enemy.stunModif -= Speed3;
                        }
                        if (Me.speed >= 32)
                        {
                            Me.critModif += Speed4;
                            if (Me.speed == 40)
                            {
                                if (Enemy != null)
                                {
                                    Enemy.bashModif -= Speed5;
                                    Enemy.stunModif -= Speed5;
                                }
                            }
                        }
                    }
                }
            }

            if (Me.sleep >= 8)
            {
                Me.baseStamForSleep += Sleep1;
                if (Me.sleep >= 16)
                {
                    //
                    if (Me.sleep >= 24)
                    {
                        Me.baseStamForSwap += Sleep3;
                        if (Me.sleep >= 32)
                        {
                            Me.magicDebuffMult += Sleep4;
                            if (Me.sleep == 40)
                            {
                                Me.magicModif += Sleep5;
                            }
                        }
                    }
                }
            }

            if (Me.regen >= 8)
            {
                Me.baseHealthForSleep += Regen1;
                if (Me.regen >= 16)
                {
                    if (Enemy != null) 
                    {
                        Enemy.blockModif -= Regen2;
                    }
                    if (Me.regen >= 24)
                    {
                        Me.baseHealthForSwap += Regen3;
                        if (Me.regen >= 32)
                        {
                            if (Enemy != null) 
                            {
                                Enemy.magicModif -= Regen4;
                            }
                            if (Me.regen == 40)
                            {
                                Me.regBonus += Regen5;
                            }
                        }
                    }
                }
            }

        }
    }


    public int FinalDamage(GameProcess player1, GameProcess player2, float AttackTypeMult, float AttackTypeChance) 
    {

        int DamageTemp;
        float RandAttackMult;

        RandAttackMult = RandomCount.SpAttacks(player1, player2, 1 - AttackTypeChance);

        if ((player1.health / (float)player1.maxHealth) > 0.2f)
        {
            DamageTemp = (int)(Convert.DmgFunc(player1) * AttackTypeMult * (1 - Convert.DmgRedFunc(player2) + player2.fireDebuff) * (1 - player2.armorModif) * (1 + player2.damageModif) * RandAttackMult);

            //Debug.Log("Melee damage: " + Convert.DmgFunc(player1) + "  *  " + AttackTypeMult + "  *  " + (1 - Convert.DmgRedFunc(player2)) + "  *  " +
    //(1 - player2.armorModif) + "  *  " + (1 + player2.damageModif) + "  *  " + RandAttackMult);
        }
        else 
        {
            DamageTemp = (int)(Convert.DmgFunc(player1) * AttackTypeMult * (1 - Convert.DmgRedFunc(player2) + player2.fireDebuff) * (1 - player2.armorModif) * (1 + player2.damageModif) * RandAttackMult * player1.ifLowHpDmg);

            //Debug.Log("Melee damage: " + Convert.DmgFunc(player1) + "  *  " + AttackTypeMult + "  *  " + (1 - Convert.DmgRedFunc(player2)) + "  *  " +
    //(1 - player2.armorModif) + "  *  " + (1 + player2.damageModif) + "  *  " + RandAttackMult + "  *  " + player1.ifLowHpDmg);
        }



        #region AnimationPart

        if (AttackTypeMult == player1.lightMult)
        {
            player1.animation.setAnimatorBoolTrue("PlayerLightAttack");
        }
        else if (AttackTypeMult == player1.mediumMult)
        {
            player1.animation.setAnimatorBoolTrue("PlayerMediumAttack");
        }
        else if (AttackTypeMult == player1.heavyMult)
        {
            player1.animation.setAnimatorBoolTrue("PlayerHeavyAttack");
        }

        player1.animation.instantiateText("", new Vector3(0, 0, 0), "- " + DamageTemp.ToString(), player2.playerObj.transform.position, player2.iAmLeft, true, false, false, false, false);
        #endregion

        return DamageTemp;
    }


    public int FinalMagicDamage(GameProcess player1, GameProcess player2, bool isDebuff)
    {
        int Damage;
        float MultFromRand;

        player2.animation.setAnimatorBoolTrue("Magic" + player1.magicEquipped);

        if (!isDebuff)
        {
            player1.animation.setAnimatorBoolTrue("PlayerMagicAttack");
            MultFromRand = RandomCount.MagicDebuff(player1, player2, this);
            Damage = (int)(MultFromRand * Convert.MagicDmgFunc(player1));
            //Debug.Log("Magic damage: " + MultFromRand + "  *  " + Convert.MagicDmgFunc(player1));
        }
        else 
        {
            if (player1.magicEquipped == "Fire")
            {
                Damage = (int)(Convert.MagicDmgFunc(player1) / Helper.debuffDmgMultFire);
            }
            else 
            {
                Damage = (int)(Convert.MagicDmgFunc(player1) / Helper.debuffDmgMultCurse);
            }

            //Debug.Log("Magic damage: "  + Convert.MagicDmgFunc(player1) / Helper.debuffDmgMult);
        }

        player1.animation.instantiateText("", new Vector3(0, 0, 0), player1.magicEquipped, player2.playerObj.transform.position, player2.iAmLeft, false, false, true, true, false);

        player1.animation.instantiateText(player1.magicEquipped + "Parent", player2.playerObj.transform.position, "- " + Damage.ToString(), player2.playerObj.transform.position, player2.iAmLeft, false, false, true, false, false);

        return Damage;
    }


    /// <summary>
    /// Returns block chance 
    /// </summary>
    /// <param name="player1"></param>
    /// <param name="player2"></param>
    /// <returns></returns>
    public float FinalBlockChance(GameProcess player1, GameProcess player2, float AttackTypeMult) 
    {
        if (player2 == null)
        {
            return (float)Math.Round(DataController.GetValue<int>("TotalModifierBlockChanceMine") / 100.0f + Convert.BlockFunc(player1), 2);
        }
        else 
        {
            float blockChanceTemp;

            Debug.Log("Block chance was: " + player1.blockModif + "  +  " + Convert.BlockFunc(player1) + "  -   " + Convert.BlockResistFunc(player2) + "  -  " + player1.fireDebuff + "   *   " + AttackTypeMult);

            blockChanceTemp = (float)Math.Round((player1.blockModif + Convert.BlockFunc(player1) - Convert.BlockResistFunc(player2) - player1.fireDebuff) * AttackTypeMult, 2);

            if (blockChanceTemp > 0.0f)
            {
                return blockChanceTemp;
            }
            else 
            {
                return 0.0f;
            }
        }
    }



}
