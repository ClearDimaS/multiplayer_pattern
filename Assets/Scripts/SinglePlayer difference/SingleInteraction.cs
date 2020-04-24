using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSparksTutorials;

public class SingleInteraction : MonoBehaviour
{
    #region Singleton
    public static SingleInteraction instance;

    public void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of interaction interface found!");
            return;
        }
        instance = this;
    }

    #endregion

    public GameObject SuggestionParent;

    public List<Image> Bars;

    static GameProcess player1;  // User of the device
    static GameProcess player2;  // Copy of and opponent on user's device

    public void SetPlayer1(GameProcess Me)
    {
        player1 = Me;
    }

    public void SetPlayer2(GameProcess Enemy)
    {
        player2 = Enemy;
    }

    Mechanics Mechs = new Mechanics();

    Mechanics.ConvertFromStats Convert = new Mechanics.ConvertFromStats();

    Mechanics.ConvertFromStats.MoveType moveType;

    PlayerInterface.Points Points = new PlayerInterface.Points();

    SingleGameManager gameManager = SingleGameManager.instance;

    public List<Image> GetBars()
    {
        return Bars;
    }

    /// <summary>
    /// Performs actions from the first List if a 
    /// </summary>
    void RunFollowingMethods(GameProcess player, float stamMult, List<string> ifNoStamina, List<string> shouldRun)
    {
        if (player.stamina < (int)(player.staminaPerMove * stamMult))
        {
            if (player.iAmLeft)
            {
                SleepLeft();

                SwapTurnLeft();
            }
            else
            {
                SleepRight();

                SwapTurnRight();
            }
        }
        else
        {
            if (player.iAmLeft)
            {
                foreach (string methodName in shouldRun)
                {
                    if (methodName == "HealthLossRightHeavy")
                    {
                        HealthLossRightHeavy();
                    }
                    else if (methodName == "HealthLossRightMedium")
                    {
                        HealthLossRightMedium();
                    }
                    else if (methodName == "HealthLossRightLight")
                    {
                        HealthLossRightLight();
                    }
                    else if (methodName == "HealthLossRightMagic")
                    {
                        HealthLossRightMagic();
                    }
                    else if (methodName == "MoveCloserLeft")
                    {
                        MoveCloserLeft();
                    }
                    else if (methodName == "MoveFarerLeft")
                    {
                        MoveFarerLeft();
                    }
                    else if (methodName == "MoveCloserLeftLight")
                    {
                        MoveCloserLeftLight();
                    }
                    else if (methodName == "MoveCloserLeftMedium")
                    {
                        MoveCloserLeftMedium();
                    }
                    else if (methodName == "SleepLeft")
                    {
                        SleepLeft();
                    }
                    else if (methodName == "SwapTurnLeft")
                    {
                        SwapTurnLeft();
                    }
                }
            }
            else
            {
                foreach (string methodName in shouldRun)
                {
                    if (methodName == "HealthLossLeftHeavy")
                    {
                        HealthLossLeftHeavy();
                    }
                    else if (methodName == "HealthLossLeftMedium")
                    {
                        HealthLossLeftMedium();
                    }
                    else if (methodName == "HealthLossLeftLight")
                    {
                        HealthLossLeftLight();
                    }
                    else if (methodName == "HealthLossLeftMagic")
                    {
                        HealthLossLeftMagic();
                    }
                    else if (methodName == "MoveCloserRight")
                    {
                        MoveCloserRight();
                    }
                    else if (methodName == "MoveFarerRight")
                    {
                        MoveFarerRight();
                    }
                    else if (methodName == "MoveCloserRightLight")
                    {
                        MoveCloserRightLight();
                    }
                    else if (methodName == "MoveCloserRightMedium")
                    {
                        MoveCloserRightMedium();
                    }
                    else if (methodName == "SleepRight")
                    {
                        SleepRight();
                    }
                    else if (methodName == "SwapTurnRight")
                    {
                        SwapTurnRight();
                    }
                }
            }
        }
    }

    void isMasterCapable(GameProcess player, float stamMult, List<string> ifNoStamina, List<string> shouldRun)
    {
        if (player1.myTurn && player1.turnTimer > 0.8)
        {
            Debug.Log("acting");
            RunFollowingMethods(player, stamMult, ifNoStamina, shouldRun);
        }
        else
        {
            Debug.Log(player1.myTurn + "   " + player1.turnTimer + "   " + player.iAmLeft);
        }
    }

    void isNonMasterCapable(GameProcess player, float stamMult, List<string> ifNoStamina, List<string> shouldRun)
    {
        if (player2.myTurn && player2.turnTimer > 0.8)
        {
            RunFollowingMethods(player, stamMult, ifNoStamina, shouldRun);
        }
        else
        {
            Debug.Log(player1.myTurn + "   " + player1.turnTimer + "   " + player.iAmLeft);
        }
    }

    // Buttons and what RPC functions they perform 
    #region Buttons

    public void HeavyAttackLeftBtn()
    {
        isMasterCapable(player1, player1.staminaHeavyMult, new List<string> { "SleepLeft", "SwapTurnLeft" }, new List<string> { "HealthLossRightHeavy", "SwapTurnLeft" });
    }

    public void MediumAttackLeftBtn()
    {
        isMasterCapable(player1, player1.staminaMediumMult, new List<string> { "SleepLeft", "SwapTurnLeft" }, new List<string> { "MoveCloserLeftMedium", "HealthLossRightMedium", "SwapTurnLeft" });
    }

    public void LightAttackLeftBtn()
    {
        isMasterCapable(player1, player1.staminaLightMult, new List<string> { "SleepLeft", "SwapTurnLeft" }, new List<string> { "MoveCloserLeftLight", "HealthLossRightLight", "SwapTurnLeft" });
    }

    public void MagicAttackLeftBtn()
    {
        isMasterCapable(player1, player1.staminaMagic * player1.maxStamina * 1.0f / player1.staminaPerMove, new List<string> { "SleepLeft", "SwapTurnLeft" }, new List<string> { "HealthLossRightMagic", "SwapTurnLeft" });
    }

    public void SleepLeftBtn()
    {
        isMasterCapable(player1, 0, new List<string> { "SleepLeft", "SwapTurnLeft" }, new List<string> { "SleepLeft", "SwapTurnLeft" });
    }

    public void MoveCloserLeftBtn()
    {
        isMasterCapable(player1, 1, new List<string> { "SleepLeft", "SwapTurnLeft" }, new List<string> { "MoveCloserLeft", "SwapTurnLeft" });
    }

    public void MoveFarerLeftBtn()
    {
        isMasterCapable(player1, 1, new List<string> { "SleepLeft", "SwapTurnLeft" }, new List<string> { "MoveFarerLeft", "SwapTurnLeft" });
    }

    // RIGHT BUTTONS (Non master client)

    public void HeavyAttackRightBtn()
    {
        isNonMasterCapable(player2, player2.staminaHeavyMult, new List<string> { "SleepRight", "SwapTurnRight" }, new List<string> { "HealthLossLeftHeavy", "SwapTurnRight" });
    }

    public void MediumAttackRightBtn()
    {
        isNonMasterCapable(player2, player2.staminaMediumMult, new List<string> { "SleepRight", "SwapTurnRight" }, new List<string> { "MoveCloserRightMedium", "HealthLossLeftMedium", "SwapTurnRight" });
    }

    public void LightAttackRightBtn()
    {
        isNonMasterCapable(player2, player2.staminaLightMult, new List<string> { "SleepRight", "SwapTurnRight" }, new List<string> { "MoveCloserRightLight", "HealthLossLeftLight", "SwapTurnRight" });
    }
    public void MagicAttackRightBtn()
    {
        isNonMasterCapable(player2, player2.staminaMagic * player2.maxStamina * 1.0f / player2.staminaPerMove, new List<string> { "SleepRight", "SwapTurnRight" }, new List<string> { "HealthLossLeftMagic", "SwapTurnRight" });
    }

    public void SleepRightBtn()
    {
        isNonMasterCapable(player2, 1, new List<string> { "SleepRight", "SwapTurnRight" }, new List<string> { "SleepRight", "SwapTurnRight" });
    }

    public void MoveCloserRightBtn()
    {
        isNonMasterCapable(player2, 1, new List<string> { "SleepRight", "SwapTurnRight" }, new List<string> { "MoveCloserRight", "SwapTurnRight" });
    }

    public void MoveFarerRightBtn()
    {
        isNonMasterCapable(player2, 1, new List<string> { "SleepRight", "SwapTurnRight" }, new List<string> { "MoveFarerRight", "SwapTurnRight" });
    }

    public void Leave()
    {
        LoadScene.SceneLoaderForScript(SingleGameManager.instance.SceneToLoad);
    }

    #endregion


    /// Remote call procedure module
    #region RPC part

    bool isActionPossibleMaster(float distance)
    {
        if (Mathf.Abs(player1.playerObj.transform.position.x - player2.playerObj.transform.position.x) <= distance)
        {
            //Debug.Log("action Master Possible");
            return true;
        }
        //Debug.Log("action Master is not Possible");
        return false;
    }

    bool isActionPossibleNonMaster(float distance)
    {
        //if (PhotonNetwork.isNonMasterClientInRoom)
        //{
        //    if (Mathf.Abs(player1.playerObj.transform.position.x - player2.playerObj.transform.position.x) <= distance)
        //    {
        //        //Debug.Log("action NonMaster Possible");
        //        return true;
        //    }
        //}
        //Debug.Log("action NonMaster is not Possible");
        return false;
    }

    void HealthLossRightHeavy()
    {
        if (isActionPossibleMaster(player1.heavyDist))
        {
            player2.health = Points.Minus(player2.health, Mechs.FinalDamage(player1, player2, player1.heavyMult, player1.heavyChance));
            player1.stamina = Points.Minus(player1.stamina, (int)(player1.staminaPerMove * player1.staminaHeavyMult));
        }
        else if (isActionPossibleNonMaster(player2.heavyDist))
        {
            player1.health = Points.Minus(player1.health, Mechs.FinalDamage(player2, player1, player2.heavyMult, player2.heavyChance));
            player2.stamina = Points.Minus(player2.stamina, (int)(player2.staminaPerMove * player2.staminaHeavyMult));
        }
    }

    void HealthLossRightMedium()
    {
        if (isActionPossibleMaster(Convert.MediumDistFunc(player1)))
        {
            player2.health = Points.Minus(player2.health, Mechs.FinalDamage(player1, player2, player1.mediumMult, player1.mediumChance));
            player1.stamina = Points.Minus(player1.stamina, (int)(player1.staminaPerMove * player1.staminaMediumMult));
        }
        else if (isActionPossibleNonMaster(Convert.MediumDistFunc(player2)))
        {
            player1.health = Points.Minus(player1.health, Mechs.FinalDamage(player2, player1, player2.mediumMult, player2.mediumChance));
            player2.stamina = Points.Minus(player2.stamina, (int)(player2.staminaPerMove * player2.staminaMediumMult));
        }
    }

    void HealthLossRightLight()
    {
        if (isActionPossibleMaster(Convert.LightDistFunc(player1)))
        {
            player2.health = Points.Minus(player2.health, Mechs.FinalDamage(player1, player2, player1.lightMult, player1.lightChance));
            player1.stamina = Points.Minus(player1.stamina, (int)(player1.staminaPerMove * player1.staminaLightMult));
        }
        else if (isActionPossibleNonMaster(Convert.LightDistFunc(player2)))
        {
            player1.health = Points.Minus(player1.health, Mechs.FinalDamage(player2, player1, player2.lightMult, player2.lightChance));
            player2.stamina = Points.Minus(player2.stamina, (int)(player2.staminaPerMove * player2.staminaLightMult));
        }
    }

    void HealthLossRightMagic()
    {
        if (isActionPossibleMaster(100000.0f))
        {
            player2.health = Points.Minus(player2.health, Mechs.FinalMagicDamage(player1, player2, false)); //MechsRand.MagicDebuff(player1, player2, Mechs) * Convert.MagicDmgFunc(player1) / 100.0f)
            player1.stamina = Points.Minus(player1.stamina, (int)(player1.staminaMagic * player1.maxStamina));
        }
        else if (isActionPossibleNonMaster(100000.0f))
        {
            player1.health = Points.Minus(player1.health, Mechs.FinalMagicDamage(player2, player1, false)); // MechsRand.MagicDebuff(player2, player1, Mechs) * Convert.MagicDmgFunc(player2) / 100.0f));
            player2.stamina = Points.Minus(player2.stamina, (int)(player2.staminaMagic * player2.maxStamina));
        }
    }


    // Left Health


    void HealthLossLeftHeavy()
    {
        if (isActionPossibleMaster(player2.heavyDist))
        {
            player1.health = Points.Minus(player1.health, Mechs.FinalDamage(player2, player1, player2.heavyMult, player2.heavyChance));
            player2.stamina = Points.Minus(player2.stamina, (int)(player2.staminaPerMove * player2.staminaHeavyMult));
        }
        else if (isActionPossibleNonMaster(player1.heavyDist))
        {
            player2.health = Points.Minus(player2.health, Mechs.FinalDamage(player1, player2, player1.heavyMult, player1.heavyChance));
            player1.stamina = Points.Minus(player1.stamina, (int)(player1.staminaPerMove * player1.staminaHeavyMult));
        }
    }

    void HealthLossLeftMedium()
    {
        if (isActionPossibleMaster(Convert.MediumDistFunc(player2)))
        {
            player1.health = Points.Minus(player1.health, Mechs.FinalDamage(player2, player1, player2.mediumMult, player2.mediumChance));
            player2.stamina = Points.Minus(player2.stamina, (int)(player2.staminaPerMove * player2.staminaMediumMult));
        }
        else if (isActionPossibleNonMaster(Convert.MediumDistFunc(player1)))
        {
            player2.health = Points.Minus(player2.health, Mechs.FinalDamage(player1, player2, player1.mediumMult, player1.mediumChance));
            player1.stamina = Points.Minus(player1.stamina, (int)(player1.staminaPerMove * player1.staminaMediumMult));
        }
    }

    void HealthLossLeftLight()
    {
        if (isActionPossibleMaster(Convert.LightDistFunc(player2)))
        {
            player1.health = Points.Minus(player1.health, Mechs.FinalDamage(player2, player1, player2.lightMult, player2.lightChance));
            player2.stamina = Points.Minus(player2.stamina, (int)(player2.staminaPerMove * player2.staminaLightMult));
        }
        else if (isActionPossibleNonMaster(Convert.LightDistFunc(player1)))
        {
            player2.health = Points.Minus(player2.health, Mechs.FinalDamage(player1, player2, player1.lightMult, player1.lightChance));
            player1.stamina = Points.Minus(player1.stamina, (int)(player1.staminaPerMove * player1.staminaLightMult));
        }
    }


    void HealthLossLeftMagic()
    {
        if (isActionPossibleMaster(100000.0f))
        {
            player1.health = Points.Minus(player1.health, Mechs.FinalMagicDamage(player2, player1, false)); //MechsRand.MagicDebuff(player2, player1, Mechs) * Convert.MagicDmgFunc(player2) / 100.0f));
            player2.stamina = Points.Minus(player2.stamina, (int)(player2.staminaMagic * player2.maxStamina));
        }
        else if (isActionPossibleNonMaster(100000.0f))
        {
            player2.health = Points.Minus(player2.health, Mechs.FinalMagicDamage(player1, player2, false)); // MechsRand.MagicDebuff(player1, player2, Mechs) * Convert.MagicDmgFunc(player1) / 100.0f));
            player1.stamina = Points.Minus(player1.stamina, (int)(player1.staminaMagic * player1.maxStamina));
        }
    }


    void UnifyMoveFuncRight(bool ifCloser)
    {
        player1.startPos = player1.playerObj.transform.position.x;

        player2.startPos = player2.playerObj.transform.position.x;

        player2.stamina = Points.Minus(player2.stamina, (int)(player2.staminaPerMove * 1));

        player2.distToMove = Convert.UnifyMove(moveType, player2);

        player2.speedToMove = -player2.distToMove / 20.0f;

        if (ifCloser)
        {
            player2.moveCloser = true;
            if (moveType == Mechanics.ConvertFromStats.MoveType.Simple)
            {
                player2.animation.setAnimatorBoolTrue("PlayerMoveRight");
            }

        }
        else
        {
            player2.moveFarer = true;
            if (moveType == Mechanics.ConvertFromStats.MoveType.Simple)
            {
                player2.animation.setAnimatorBoolTrue("PlayerMoveLeft");
            }
        }
    }

    void UnifyMoveFuncLeft(bool ifCloser)
    {
        player1.startPos = player1.playerObj.transform.position.x;

        player2.startPos = player2.playerObj.transform.position.x;

        player1.stamina = Points.Minus(player1.stamina, (int)(player1.staminaPerMove * 1));

        player1.distToMove = Convert.UnifyMove(moveType, player1);

        player1.speedToMove = player1.distToMove / 20.0f;

        if (ifCloser)
        {
            if (moveType == Mechanics.ConvertFromStats.MoveType.Simple)
            {
                player1.animation.setAnimatorBoolTrue("PlayerMoveRight");
            }
            player1.moveCloser = true;
        }
        else
        {
            if (moveType == Mechanics.ConvertFromStats.MoveType.Simple)
            {
                player1.animation.setAnimatorBoolTrue("PlayerMoveLeft");
            }
            player1.moveFarer = true;
        }
    }

    void MoveCloserRight()
    {
        moveType = Mechanics.ConvertFromStats.MoveType.Simple;
        UnifyMoveFuncRight(true);
    }

    void MoveCloserLeft()
    {
        moveType = Mechanics.ConvertFromStats.MoveType.Simple;
        UnifyMoveFuncLeft(true);
    }

    void MoveCloserRightLight()
    {
        moveType = Mechanics.ConvertFromStats.MoveType.LightAttack;
        UnifyMoveFuncRight(true);
    }


    void MoveCloserLeftLight()
    {
        moveType = Mechanics.ConvertFromStats.MoveType.LightAttack;
        UnifyMoveFuncLeft(true);
    }


    void MoveCloserRightMedium()
    {
        moveType = Mechanics.ConvertFromStats.MoveType.MediumAttack;
        UnifyMoveFuncRight(true);
    }


    void MoveCloserLeftMedium()
    {
        moveType = Mechanics.ConvertFromStats.MoveType.MediumAttack;
        UnifyMoveFuncLeft(true);
    }


    void MoveFarerRight()
    {
        moveType = Mechanics.ConvertFromStats.MoveType.Simple;
        UnifyMoveFuncRight(false);
    }


    void MoveFarerLeft()
    {
        moveType = Mechanics.ConvertFromStats.MoveType.Simple;
        UnifyMoveFuncLeft(false);
    }


    //    // Sleep & swap


    void SleepRight()
    {
        player2.health = Points.Plus(player2.health, Convert.HealthRestoreFunc(player2), player2.maxHealth);
        player2.stamina = Points.Plus(player2.stamina, Convert.StaminaRestoreFunc(player2), player2.maxStamina);
        player2.animation.setAnimatorBoolTrue("PlayerSleep");
    }


    void SleepLeft()
    {
        player1.health = Points.Plus(player1.health, Convert.HealthRestoreFunc(player1), player1.maxHealth);
        player1.stamina = Points.Plus(player1.stamina, Convert.StaminaRestoreFunc(player1), player1.maxStamina);
        player1.animation.setAnimatorBoolTrue("PlayerSleep");
    }


    void SwapTurnLeft()
    {
        if (player1.turnTimer > 0) 
        {
            player1.turnTimer = -0.1f;

            SingleGameManager.instance.SwapTurn(player1, player2);
        }

        Invoke("BotMoveTemp", 3.0f + Time.time % 1.0f);
    }


    void SwapTurnRight()
    {
        if (player2.turnTimer > 0) 
        {
            player2.turnTimer = -0.1f;

            SingleGameManager.instance.SwapTurn(player1, player2);
        }
    }

    SingleEnemyHandler botHelper = new SingleEnemyHandler();

    void BotMoveTemp() 
    {
        if (DataController.GetValue<int>("LoadMode") == 2)
        {
            botHelper.MakeAMoveHard(player1, player2);
        }
        else
        if (player1.Lvl > 2)
        {
            botHelper.MakeAMoveMedium(player1, player2);
        }
        else 
        {
            botHelper.MakeAMoveEasy(player1, player2);
        }
    }

    #endregion

    public void TestButton() 
    {
        botHelper.RandomNumbersWithGivenSum(120, 8, 0, 40);
    }

    public void showDebuffLeft() 
    {
        if (player1.cursed > 0 || player1.onFire > 0)
        {
            player1.playerDebuffs.GetComponentsInChildren<Text>()[1].text = ReturnDebuffText(player1, player2);
        }
        else 
        {
        
        }
    }

    public void showDebuffRight()
    {
        if (player2.cursed > 0 || player2.onFire > 0)
        {
            player2.playerDebuffs.GetComponentsInChildren<Text>()[1].text = ReturnDebuffText(player2, player1);
        }
        else
        {

        }
    }

    string ReturnDebuffText(GameProcess player1, GameProcess player2)
    {
        float blockChancetemp = (player1.blockModif + Convert.BlockFunc(player1) - Convert.BlockResistFunc(player2));// Mechs.FinalBlockChance(player1, player2, 1.0f);

        if (blockChancetemp - player1.fireDebuff > 0.0f)
        {
            blockChancetemp = (int)((blockChancetemp + player1.fireDebuff) * 100.0f);
        }
        else
        {
            blockChancetemp = (int)(blockChancetemp * 100.0f);
        }

        Debug.Log(player1.curseDebuff + player1.regBonus);
        if (player2.magicEquipped == "Fire")
        {
            return "Block chance: " + (blockChancetemp - (int)(player1.fireDebuff * 100)) + " (-" + (int)(player1.fireDebuff * 100) + ") %\n" +
                "Damage rdct: " + (int)((Convert.DmgRedFunc(player1) - player1.fireDebuff) * 100) + " (-" + (int)(player1.fireDebuff * 100) + ") %";
        }
        else
        {
            return "Regeneration effects \nmultiplyer: " + (int)((player1.regBonus - player1.curseDebuff) * 100) + " (-" + (int)(player1.curseDebuff * 100) + ") %";
        }
    }


    public void LoseGameConverter() 
    {
        SingleGameManager.instance.Pause(false);

        SingleGameManager.instance.CheckGameOver();
    }


    bool addWatched;


    public void WatchAdd() 
    {
        // Add check if addvert watched

        addWatched = true;

        RestoreSomeHp();
    }


    bool ContinuePaid;


    public void PayPls()
    {
        if (DataController.GetValue<int>("Bread") >= 50) 
        {
            DataController.SaveValue("Bread", DataController.GetValue<int>("Bread") - 50);

            ContinuePaid = true;

            RestoreSomeHp();
        }
    }


    public void RestoreSomeHp()
    {
        if (ContinuePaid || addWatched) 
        {
            player1.health = player1.maxHealth / 2;

            SingleGameManager.instance.Pause(false);

            SingleGameManager.instance.UpdPlayerState(player1, player2);

            SuggestionParent.SetActive(false);
        }

    }
}
