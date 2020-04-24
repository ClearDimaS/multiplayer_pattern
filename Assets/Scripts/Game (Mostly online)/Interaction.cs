using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : Photon.PunBehaviour
{
    #region Singleton
    public static Interaction instance;

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

    TheGameManager gameManager = TheGameManager.instance;

    public List<Image> GetBars()
    {
        return Bars;
    }


    public void Swap(bool LeftOrNot) 
    {
        if (LeftOrNot)
        {
            photonView.RPC("SwapTurnLeft", PhotonTargets.AllBuffered);
        }
        else 
        {
            photonView.RPC("SwapTurnRight", PhotonTargets.AllBuffered);
        }
    }
    /// <summary>
    /// Performs actions from the first List if a 
    /// </summary>
    void RPCRunFollowingMethods(GameProcess player, float stamMult, List<string> ifNoStamina, List<string> shouldRun)
    {

        if (player.stamina < (int)(player.staminaPerMove * stamMult))
        {
            foreach (string methodName in ifNoStamina)
            {
                photonView.RPC(methodName, PhotonTargets.AllBuffered);
            }
        }
        else
        {
            foreach (string methodName in shouldRun)
            {
                photonView.RPC(methodName, PhotonTargets.AllBuffered);
            }
        }
    }

    void isMasterCapable(GameProcess player, float stamMult, List<string> ifNoStamina, List<string> shouldRun)
    {
        if (PhotonNetwork.isMasterClient && PhotonNetwork.connected)
        {
            if (player1.myTurn && player1.turnTimer > 0.8)
            {
                Debug.Log("acting");
                RPCRunFollowingMethods(player, stamMult, ifNoStamina, shouldRun);
            }
            else 
            {
                Debug.Log(player1.myTurn + "   " + player1.turnTimer);
            }
        }
    }

    void isNonMasterCapable(GameProcess player, float stamMult, List<string> ifNoStamina, List<string> shouldRun)
    {
        if (PhotonNetwork.isNonMasterClientInRoom && PhotonNetwork.connected)
        {
            if (player1.myTurn && player1.turnTimer > 0.8)
            {
                RPCRunFollowingMethods(player, stamMult, ifNoStamina, shouldRun);
            }
            else
            {
            }
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
        isNonMasterCapable(player1, player1.staminaHeavyMult, new List<string> { "SleepRight", "SwapTurnRight" }, new List<string> { "HealthLossLeftHeavy", "SwapTurnRight" });
    }

    public void MediumAttackRightBtn()
    {
        isNonMasterCapable(player1, player1.staminaMediumMult, new List<string> { "SleepRight", "SwapTurnRight" }, new List<string> { "MoveCloserRightMedium", "HealthLossLeftMedium", "SwapTurnRight" });
    }

    public void LightAttackRightBtn()
    {
        isNonMasterCapable(player1, player1.staminaLightMult, new List<string> { "SleepRight", "SwapTurnRight" }, new List<string> { "MoveCloserRightLight", "HealthLossLeftLight", "SwapTurnRight" });
    }
    public void MagicAttackRightBtn()
    {
        isNonMasterCapable(player1, player1.staminaMagic * player1.maxStamina * 1.0f / player1.staminaPerMove, new List<string> { "SleepRight", "SwapTurnRight" }, new List<string> { "HealthLossLeftMagic", "SwapTurnRight" });
    }

    public void SleepRightBtn()
    {
        isNonMasterCapable(player1, 1, new List<string> { "SleepRight", "SwapTurnRight" }, new List<string> { "SleepRight", "SwapTurnRight" });
    }

    public void MoveCloserRightBtn()
    {
        isNonMasterCapable(player1, 1, new List<string> { "SleepRight", "SwapTurnRight" }, new List<string> { "MoveCloserRight", "SwapTurnRight" });
    }

    public void MoveFarerRightBtn()
    {
        isNonMasterCapable(player1, 1, new List<string> { "SleepRight", "SwapTurnRight" }, new List<string> { "MoveFarerRight", "SwapTurnRight" });
    }

    public void Leave()
    {
        if (TheGameManager.instance.GameOver)
        {
            LoadScene.SceneLoaderForScript(TheGameManager.instance.SceneToLoad);
        }
        else 
        {
            LoadScene.SceneLoaderForScript(TheGameManager.instance.SceneToLoad);
        }
    }

    #endregion


    /// Remote call procedure module
    #region RPC part

    bool isActionPossibleMaster(float distance)
    {
        if (PhotonNetwork.isMasterClient)
        {
            if (Mathf.Abs(player1.playerObj.transform.position.x - player2.playerObj.transform.position.x) <= distance)
            {
                //Debug.Log("action Master Possible");
                return true;
            }
        }
        //Debug.Log("action Master is not Possible");
        return false;
    }

    bool isActionPossibleNonMaster(float distance)
    {
        if (PhotonNetwork.isNonMasterClientInRoom)
        {
            if (Mathf.Abs(player1.playerObj.transform.position.x - player2.playerObj.transform.position.x) <= distance)
            {
                //Debug.Log("action NonMaster Possible");
                return true;
            }
        }
        //Debug.Log("action NonMaster is not Possible");
        return false;
    }

    [PunRPC]
    void HealthLossRightHeavy()
    {
        if (!TheGameManager.instance.GameOver)
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

    }

    [PunRPC]
    void HealthLossRightMedium()
    {
        if (!TheGameManager.instance.GameOver)
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

    }

    [PunRPC]
    void HealthLossRightLight()
    {
        if (!TheGameManager.instance.GameOver)
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

    }

    [PunRPC]
    void HealthLossRightMagic()
    {
        if (!TheGameManager.instance.GameOver)
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

    }


    // Left Health


    [PunRPC]
    void HealthLossLeftHeavy()
    {
        if (!TheGameManager.instance.GameOver)
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

    }

    [PunRPC]
    void HealthLossLeftMedium()
    {
        if (!TheGameManager.instance.GameOver)
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

    }

    [PunRPC]
    void HealthLossLeftLight()
    {
        if (!TheGameManager.instance.GameOver)
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

    }
    [PunRPC]
    void HealthLossLeftMagic()
    {
        if (!TheGameManager.instance.GameOver)
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

    }

    void UnifyMoveFuncRight(bool ifCloser) 
    {
        if (!TheGameManager.instance.GameOver)
        {
            player1.startPos = player1.playerObj.transform.position.x;
            player2.startPos = player2.playerObj.transform.position.x;
            if (PhotonNetwork.isMasterClient)
            {
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
            else
            {
                player1.stamina = Points.Minus(player1.stamina, (int)(player1.staminaPerMove * 1));
                player1.distToMove = Convert.UnifyMove(moveType, player1);
                player1.speedToMove = -player1.distToMove / 20.0f;
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
        }

    }

    void UnifyMoveFuncLeft(bool ifCloser)
    {
        if (!TheGameManager.instance.GameOver)
        {
            player1.startPos = player1.playerObj.transform.position.x;
            player2.startPos = player2.playerObj.transform.position.x;
            if (PhotonNetwork.isMasterClient)
            {
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
            else
            {
                player2.stamina = Points.Minus(player2.stamina, (int)(player2.staminaPerMove * 1));
                player2.distToMove = Convert.UnifyMove(moveType, player2);
                player2.speedToMove = player2.distToMove / 20.0f;
                if (ifCloser)
                {
                    if (moveType == Mechanics.ConvertFromStats.MoveType.Simple)
                    {
                        player2.animation.setAnimatorBoolTrue("PlayerMoveRight");
                    }
                    player2.moveCloser = true;
                }
                else
                {
                    if (moveType == Mechanics.ConvertFromStats.MoveType.Simple)
                    {
                        player2.animation.setAnimatorBoolTrue("PlayerMoveLeft");
                    }
                    player2.moveFarer = true;
                }
            }
        }
        
    }

    [PunRPC]
    void MoveCloserRight()
    {
        moveType = Mechanics.ConvertFromStats.MoveType.Simple;
        UnifyMoveFuncRight(true);
    }

    [PunRPC]
    void MoveCloserLeft()
    {
        moveType = Mechanics.ConvertFromStats.MoveType.Simple;
        UnifyMoveFuncLeft(true);
    }

    [PunRPC]
    void MoveCloserRightLight()
    {
        moveType = Mechanics.ConvertFromStats.MoveType.LightAttack;
        UnifyMoveFuncRight(true);
    }

    [PunRPC]
    void MoveCloserLeftLight()
    {
        moveType = Mechanics.ConvertFromStats.MoveType.LightAttack;
        UnifyMoveFuncLeft(true);
    }

    [PunRPC]
    void MoveCloserRightMedium()
    {
        moveType = Mechanics.ConvertFromStats.MoveType.MediumAttack;
        UnifyMoveFuncRight(true);
    }

    [PunRPC]
    void MoveCloserLeftMedium()
    {
        moveType = Mechanics.ConvertFromStats.MoveType.MediumAttack;
        UnifyMoveFuncLeft(true);
    }

    [PunRPC]
    void MoveFarerRight()
    {
        moveType = Mechanics.ConvertFromStats.MoveType.Simple;
        UnifyMoveFuncRight(false);
    }

    [PunRPC]
    void MoveFarerLeft()
    {
        moveType = Mechanics.ConvertFromStats.MoveType.Simple;
        UnifyMoveFuncLeft(false);
    }


    //    // Sleep & swap

    [PunRPC]
    void SleepRight()
    {
        if (!TheGameManager.instance.GameOver)
        {
            if (PhotonNetwork.isMasterClient)
            {
                player2.health = Points.Plus(player2.health, Convert.HealthRestoreFunc(player2), player2.maxHealth);
                player2.stamina = Points.Plus(player2.stamina, Convert.StaminaRestoreFunc(player2), player2.maxStamina);
                player2.animation.setAnimatorBoolTrue("PlayerSleep");
            }
            else
            {
                player1.health = Points.Plus(player1.health, Convert.HealthRestoreFunc(player1), player1.maxHealth);
                player1.stamina = Points.Plus(player1.stamina, Convert.StaminaRestoreFunc(player1), player1.maxStamina);
                player1.animation.setAnimatorBoolTrue("PlayerSleep");
            }
        }

    }

    [PunRPC]
    void SleepLeft()
    {
        if (!TheGameManager.instance.GameOver)
        {
            if (PhotonNetwork.isMasterClient)
            {
                player1.health = Points.Plus(player1.health, Convert.HealthRestoreFunc(player1), player1.maxHealth);
                player1.stamina = Points.Plus(player1.stamina, Convert.StaminaRestoreFunc(player1), player1.maxStamina);
                player1.animation.setAnimatorBoolTrue("PlayerSleep");
            }
            else
            {
                player2.health = Points.Plus(player2.health, Convert.HealthRestoreFunc(player2), player2.maxHealth);
                player2.stamina = Points.Plus(player2.stamina, Convert.StaminaRestoreFunc(player2), player2.maxStamina);
                player2.animation.setAnimatorBoolTrue("PlayerSleep");
            }
        }

    }

    [PunRPC]
    void SwapTurnLeft()
    {
        if (!TheGameManager.instance.GameOver)
        {
            if (PhotonNetwork.isMasterClient)
            {
                player1.turnTimer = -0.1f;
            }
            else
            {
                player2.turnTimer = -0.1f;
            }

            TheGameManager.instance.SwapTurn(player1, player2);
        }

    }

    [PunRPC]
    void SwapTurnRight()
    {
        if (!TheGameManager.instance.GameOver)
        {        //Debug.Log(player1.health + "    " + player2.health);
            if (PhotonNetwork.isMasterClient)
            {
                player2.turnTimer = -0.1f;
            }
            else
            {
                player1.turnTimer = -0.1f;
            }

            TheGameManager.instance.SwapTurn(player1, player2);
        }

    }


    #endregion

    public void showDebuffLeft()
    {
        if (PhotonNetwork.isMasterClient)
        {
            if (player1.cursed > 0 || player1.onFire > 0)
            {
                player1.playerDebuffs.GetComponentsInChildren<Text>()[1].text = ReturnDebuffText(player1, player2);
            }
        }
        else 
        {
            if (player2.cursed > 0 || player2.onFire > 0)
            {
                player2.playerDebuffs.GetComponentsInChildren<Text>()[1].text = ReturnDebuffText(player2, player1);
            }
        }
    }

    public void showDebuffRight()
    {
        if (PhotonNetwork.isMasterClient)
        {
            if (player2.cursed > 0 || player2.onFire > 0)
            {
                player2.playerDebuffs.GetComponentsInChildren<Text>()[1].text = ReturnDebuffText(player2, player1);
            }
        }
        else
        {
            if (player1.cursed > 0 || player1.onFire > 0)
            {
                player1.playerDebuffs.GetComponentsInChildren<Text>()[1].text = ReturnDebuffText(player1, player2);
            }
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


}
