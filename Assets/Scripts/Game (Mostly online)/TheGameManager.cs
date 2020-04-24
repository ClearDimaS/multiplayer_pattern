using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSparksTutorials;
using System;

public class TheGameManager : MonoBehaviour
{
    #region Singleton
    public static TheGameManager instance;

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

    public GameProcess Me;

    public GameProcess Enemy;

    double StartTime;

    public void SetMe(GameProcess Me)
    {
        this.Me = Me;
    }


    public void SetEnemy(GameProcess Enemy)
    {
        this.Enemy = Enemy;
    }


    Mechanics Mechs = new Mechanics();
    Mechanics.ConvertFromStats Conv = new Mechanics.ConvertFromStats();
    PlayerInterface interFace = new PlayerInterface();


    // Start is called before the first frame update
    void Start()
    {
        ready = false;
        synced = false;
        syncedCopy = false;

        DataController.SaveValue("syncedOther", -1);
        DataController.SaveValue("syncedMine", -1);
    }


    public bool synced;


    public void SetSynced(bool synced)
    {
        this.synced = synced;
    }


    public bool syncedCopy;


    public void SetSyncedCopy(bool syncedCopy)
    {
        this.syncedCopy = syncedCopy;
    }

    bool ready;

    public float tempHealthMe;

    public float tempHealthEnemy;

    // Update is called once per frame
    void Update()
    {
        if (!ready)
        {
            if (synced && syncedCopy)
            {
                SceneToLoad = 5;

                TheGameManager.instance.LoadPlayerSecondary(Me, Enemy);
                TheGameManager.instance.LoadPlayerSecondary(Enemy, Me);

                interFace.SetBars(Me, Enemy);

                interFace.Refresh(Me.health, Me.maxHealth, Me.stamina, Me.maxStamina, Enemy.health, Enemy.maxHealth, Enemy.stamina, Enemy.maxStamina);
                interFace.UpdImagesAndTexts(Me.playerInfoPanel, Enemy.playerInfoPanel, Me, Enemy, Me.playerLoadingScreen);

                Enemy.buttons.BtnLeave.SetActive(false);
                Enemy.playerInfoPanel.SetActive(false);
                Enemy.buttons.BtnsPlace.SetActive(false);


                Me.buttons.BtnLeave.SetActive(false);
                Me.playerInfoPanel.SetActive(false);


                Interaction.instance.SetPlayer1(Me);
                Interaction.instance.SetPlayer2(Enemy);

                UpdButtonsTexts();

                # region LoadSpritesForDebuffs

                Image TempImage;

                TempImage = Me.playerDebuffs.GetComponentInChildren<Image>();

                TempImage.sprite = Resources.Load<Sprite>("ButtonMagic" + Enemy.magicEquipped);

                TempImage = Enemy.playerDebuffs.GetComponentInChildren<Image>();

                TempImage.sprite = Resources.Load<Sprite>("ButtonMagic" + Me.magicEquipped);

                #endregion

                if (Enemy.sleep >= 16)
                {
                    Me.buttons.BtnMagic.SetActive(true);
                    Me.buttons.BtnMagic.GetComponentInChildren<Text>().text = DataController.GetValue<string>("EquippedMagicMine");
                }
                else
                {
                    Me.buttons.BtnMagic.SetActive(false);
                }

                StartTime = PhotonNetwork.time;

                UpdPlayerState(Me, Enemy);
                UpdPlayerState(Enemy, Me);

                leftBorderPos = GameObject.FindGameObjectWithTag("LeftBoundary").transform.position.x;

                rightBorderPos = GameObject.FindGameObjectWithTag("RightBoundary").transform.position.x;

                #region TimerSetAndModelsRotation
                if (PhotonNetwork.isMasterClient)
                {
                    GameObject.FindGameObjectsWithTag("HaveToBeRotated")[1].transform.localScale += new Vector3(-2 * GameObject.FindGameObjectsWithTag("HaveToBeRotated")[1].transform.localScale.x, 0, 0);

                    Me.myTurn = true;
                    Me.turnTimer = TimeForTurn;

                    Enemy.myTurn = false;
                    Enemy.turnTimer = 0.0f;
                }
                else
                {
                    GameObject.FindGameObjectsWithTag("HaveToBeRotated")[0].transform.localScale += new Vector3(-2 * GameObject.FindGameObjectsWithTag("HaveToBeRotated")[0].transform.localScale.x, 0, 0);

                    Enemy.myTurn = true;
                    Enemy.turnTimer = TimeForTurn;

                    Me.myTurn = false;
                    Me.turnTimer = 0.0f;
                    Me.buttons.BtnsPlace.SetActive(false);
                }
                #endregion

                Enemy.playerInterface.SetActive(false);
                Me.playerLoadingScreen.SetActive(false);

                tempHealthEnemy = (float)Enemy.health;

                tempHealthMe = (float)Me.health;

                Debug.Log("Loaded HERE: " + tempHealthMe + "   " + tempHealthEnemy);

                playerMeTimeBar.fillAmount = 0;

                playerEnemyTimeBar.fillAmount = 0;

                ready = true;
            }
        }
        else 
        {
            if (MoveMent())
            {
                TimerText("");
            }
            else
            {
                Timer(Me, Enemy);
            }

            // Animating HealthBars

            if (Me.health < (int)tempHealthMe)
            {
                tempHealthMe = interFace.RefreshAnimate(true, tempHealthMe, Me.maxHealth);
            }
            else if (Me.health > tempHealthMe)
            {
                tempHealthMe = 1 + (int)interFace.RefreshAnimate(true, Me.health, Me.maxHealth);
            }
            if (Enemy.health < (int)tempHealthEnemy)
            {
                tempHealthEnemy = interFace.RefreshAnimate(false, tempHealthEnemy, Enemy.maxHealth);
            }
            else if (Enemy.health > tempHealthEnemy)
            {
                tempHealthEnemy = 1 + (int)interFace.RefreshAnimate(false, Enemy.health, Enemy.maxHealth);
            }
        }
    }


    void CheckForEnemyLeave() 
    {
        if (Enemy.playerObj != null)
        {
        }
        else
        {
            if (!GameOver)
            {
                WinGame();
            }
        }
    }


    bool invokeOnlyOnce;


    bool MoveMent() 
    {
        if (GameOver)
        {
            return true;
        }
        else 
        {
            CheckForEnemyLeave();
            if (Enemy.moveFarer || Enemy.moveCloser)
            {
                MovePlayers(Enemy, Me);
                return true;
            }
            else
            if (Me.moveFarer || Me.moveCloser)
            {
                MovePlayers(Me, Enemy);
                return true;
            }else
            if (Me.Animating || Enemy.Animating) 
            {
                return true;
            }
            return false;
        }      
    }


    void animStop()
    {
        Me.Animating = false;

        Enemy.Animating = false;

        invokeOnlyOnce = false;

        Me.animation.Animator.Play("PlayerAnimation", 0);

        Me.moveCloser = false;

        Me.moveFarer = false;

        Me.speedToMove = 0;

        if (Enemy.playerObj) 
        {
            Enemy.animation.Animator.Play("PlayerAnimation", 0);
        }

        Enemy.moveCloser = false;

        Enemy.moveFarer = false;

        Enemy.speedToMove = 0;
    }


    // are reinitialized every iteration
    float currentPosition;
    float playerDist;

    // have to be initialized at the beginning of the game
    float leftBorderPos;
    float rightBorderPos;

    void MovePlayers(GameProcess player1, GameProcess player2) 
    {
        currentPosition = player1.playerObj.transform.position.x;

        playerDist = Mathf.Abs(player1.playerObj.transform.position.x - player2.playerObj.transform.position.x);

        if (player1.moveCloser)
        {
            if (Mathf.Abs(currentPosition - player1.startPos) < player1.distToMove && playerDist > player1.heavyDist - 0.1f)
            {
                player1.playerObj.transform.position += new Vector3(player1.speedToMove, 0, 0);
            }
        }
        else
        {
            if (Mathf.Abs(currentPosition - player1.startPos) < player1.distToMove && Mathf.Abs(leftBorderPos - currentPosition) > 0.5f && Mathf.Abs(rightBorderPos - currentPosition) > 0.5f)
            {
                player1.playerObj.transform.position -= new Vector3(player1.speedToMove, 0, 0);
            }
        }
    }


    /// <summary>
    /// Load Stats and modifs for further calculating all characteristics which depend on both player's stats and modifs
    /// </summary>
    /// <param name="player"> A player class keeping player info </param>
    /// <param name="meOrEnemy"> True if a user, False if enemy's copy </param>
    public void LoadPlayerBase(GameProcess player, bool meOrEnemy) 
    {
        string MineOrOther; // for using DataController which keeps info about two players in terms ***Mine, or ***Other

        if (meOrEnemy)
        {
            player.MineOrOther = "Mine";
            MineOrOther = "Mine";
            player.myTurn = true;
            player.turnTimer = TimeForTurn;
        }
        else
        {
            player.MineOrOther = "Other";
            MineOrOther = "Other";
            player.myTurn = false;
            player.turnTimer = 0;
        }

        player.WinAnimNumber = DataController.GetValue<int>("WinAnimNumber" + MineOrOther);

        Debug.Log(DataController.GetValue<string>("EquippedMagicMine") + "    " + DataController.GetValue<string>("EquippedMagicOther"));

        if (DataController.GetValue<string>("EquippedMagic" + MineOrOther) != "") 
        {
            player.magicEquipped = DataController.GetValue<string>("EquippedMagic" + MineOrOther);
        }


        player.Lvl = DataController.GetValue<int>("Exp") / 100;

        player.attack = DataController.GetValue<int>("StatsAttack" + MineOrOther);
        player.agility = DataController.GetValue<int>("StatsAgility" + MineOrOther);
        player.power = DataController.GetValue<int>("StatsPower" + MineOrOther);
        player.strength = DataController.GetValue<int>("StatsStrength" + MineOrOther);
        player.endurance = DataController.GetValue<int>("StatsEndurance" + MineOrOther);
        player.speed = DataController.GetValue<int>("StatsSpeed" + MineOrOther);
        player.sleep = DataController.GetValue<int>("StatsSleep" + MineOrOther);
        player.regen = DataController.GetValue<int>("StatsRegen" + MineOrOther);

        // info about modifs in Equipment class
        player.armorModif = (float)Math.Round((double)(DataController.GetValue<int>("TotalModifierArmor" + MineOrOther) / 100.0f), 3);
        player.damageModif = (float)Math.Round((double)(DataController.GetValue<int>("TotalModifierDamage" + MineOrOther) / 100.0f), 3);
        player.missModif = (float)Math.Round((double)(DataController.GetValue<int>("TotalModifierMissChance" + MineOrOther) / 100.0f), 3);
        player.critModif = (float)Math.Round((double)(DataController.GetValue<int>("TotalModifierCriticalChance" + MineOrOther) / 100.0f), 3);
        player.bashModif = (float)Math.Round((double)(DataController.GetValue<int>("TotalModifierBashChance" + MineOrOther) / 100), 3) ;
        player.stunModif = (float)Math.Round((double)(DataController.GetValue<int>("TotalModifierStunChance" + MineOrOther) / 100.0f), 3);
        player.blockModif = (float)Math.Round((double)(DataController.GetValue<int>("TotalModifierBlockChance" + MineOrOther) / 100.0f), 3);
        player.magicModif = (float)Math.Round((double)(DataController.GetValue<int>("TotalModifierMagic" + MineOrOther) / 100.0f), 3);

        if (player.iAmLeft)
        {
            Debug.Log("Creating new System.Random with seed: " + DataController.GetValue<int>("blockvar1Left") + ". IsLeft: " + player.iAmLeft);

            player.rand1 = new System.Random(DataController.GetValue<int>("blockvar1Left"));
            player.rand2 = new System.Random(DataController.GetValue<int>("blockvar2Left"));
            player.rand3 = new System.Random(DataController.GetValue<int>("blockvar3Left"));
            player.rand4 = new System.Random(DataController.GetValue<int>("blockvar4Left"));
            player.rand5 = new System.Random(DataController.GetValue<int>("blockvar5Left"));
            player.rand6 = new System.Random(DataController.GetValue<int>("blockvar6Left"));
        }
        else 
        {
            Debug.Log("Creating new System.Random with seed: " + DataController.GetValue<int>("blockvar1Right") + ". IsLeft: " + player.iAmLeft);

            player.rand1 = new System.Random(DataController.GetValue<int>("blockvar1Right"));
            player.rand2 = new System.Random(DataController.GetValue<int>("blockvar2Right"));
            player.rand3 = new System.Random(DataController.GetValue<int>("blockvar3Right"));
            player.rand4 = new System.Random(DataController.GetValue<int>("blockvar4Right"));
            player.rand5 = new System.Random(DataController.GetValue<int>("blockvar5Right"));
            player.rand6 = new System.Random(DataController.GetValue<int>("blockvar6Right"));
        }

    }


    Mechanics.MechsHelper Helper = new Mechanics.MechsHelper();

    Mechanics.Specials specials = new Mechanics.Specials();

    /// <summary>
    /// Loads player characteristics, should be implemented after syncronization
    /// </summary>
    /// <param name="player"> A player class keeping player info </param>
    /// <param name="player2"> A player class keeping enemy's copy info </param>
    public void LoadPlayerSecondary(GameProcess player, GameProcess player2)
    {
        player.baseDamage = Helper.BaseDamage;    /////
        player.heavyMult = Helper.heavyMult;
        player.mediumMult = Helper.mediumMult;
        player.lightMult = Helper.lightMult;

        player.lightChance = 1 - Mechs.FinalBlockChance(player2, player, Helper.lightChanceMult);
        player.mediumChance = 1 - Mechs.FinalBlockChance(player2, player, Helper.mediumChanceMult);
        player.heavyChance = 1 - Mechs.FinalBlockChance(player2, player, Helper.heavyChanceMult);

        player.ifLowHpDmg = Helper.ifLowHpDmg;
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
        player.distToMove = 0;
        player.speedToMove = 0;
        player.startPos = 0;
        player.moveFarer = false;
        player.moveCloser = false;

        player.moveMult = Helper.moveMult;

        player.health = Conv.TtlHealth(player);
        player.maxHealth = Conv.TtlHealth(player);
        player.regBonus = Helper.regBonus;
        player.baseHealthForSleep = Helper.baseHealthForSleep;
        player.baseHealthForSwap = Helper.baseHealthForSwap; /////

        player.bashed = false;
        player.stunned = false;
        player.magicDebuffMult = Helper.magicDebuffMult;
        player.onFire = 0;
        player.cursed = 0;
        player.curseDebuff = 0.0f;
        player.fireDebuff = 0.0f;

        specials.applySpecials(player, player2);
    }


    PlayerInterface.Points Points = new PlayerInterface.Points();


    int HealthLoss = 0;

    public void UpdPlayerState(GameProcess player1, GameProcess player2)
    {
        CheckGameOver();

        if (!GameOver) 
        {
            Debug.Log(player1.lightChance + "LightChance" + "\n" + player1.lightChance + "mediumChance" + "\n" + player1.lightChance + "heavyChance" + "\n");
            player1.lightChance = 1 - Mechs.FinalBlockChance(player2, player1, Helper.lightChanceMult);
            player1.mediumChance = 1 - Mechs.FinalBlockChance(player2, player1, Helper.mediumChanceMult);
            player1.heavyChance = 1 - Mechs.FinalBlockChance(player2, player1, Helper.heavyChanceMult);

            if (player1.cursed == 0 && player1.onFire == 0)
            {
                UpdButtonsTexts();
            }

            if (player1.cursed > 0)
            {
                player1.animation.setAnimatorBoolTrue("Magic" + player2.magicEquipped);

                player1.health = Points.Minus(player1.health, Mechs.FinalMagicDamage(player2, player1, true));

                player1.playerDebuffs.SetActive(true);
                player1.cursed -= 1;
                player1.playerDebuffs.GetComponentInChildren<Text>().text = player1.cursed.ToString();

                Invoke("UpdButtonsTexts", 1.5f);
            }
            else if (player1.onFire > 0)
            {
                player1.animation.setAnimatorBoolTrue("Magic" + player2.magicEquipped);

                player1.health = Points.Minus(player1.health, Mechs.FinalMagicDamage(player2, player1, true));

                player1.playerDebuffs.SetActive(true);
                player1.onFire -= 1;
                player1.playerDebuffs.GetComponentInChildren<Text>().text = player1.onFire.ToString();

                Invoke("UpdButtonsTexts", 1.5f);
            }

            if (player1.cursed == 0 && player1.onFire == 0)
            {
                player1.curseDebuff = 0.0f;
                player1.fireDebuff = 0.0f;
                player1.playerDebuffs.SetActive(false);
            }

            if (PhotonNetwork.time - StartTime > 110) 
            {
                TimeLeft.color = Color.clear;
                TimeLeft1.color = Color.clear;
                playerTimeBarText1.color = Color.red;
                player1.animation.instantiateText("", new Vector3(0, 0, 0), "Instant Death! \n" + HealthLoss, player1.playerObj.transform.position, player1.iAmLeft, true, false, false, true, false);
                player1.health -= HealthLoss;
                HealthLoss++;
            }

            CheckGameOver();
        }

        if (Me.myTurn)
        {
            interFace.Refresh(player1.health, player1.maxHealth, player1.stamina, player1.maxStamina, player2.health, player2.maxHealth, player2.stamina, player2.maxStamina);
        }
        else
        {
            interFace.Refresh(player2.health, player2.maxHealth, player2.stamina, player2.maxStamina, player1.health, player1.maxHealth, player1.stamina, player1.maxStamina);
        }
    }


    float TimeForTurn = 8.0f;

    public Image playerMeTimeBar;

    public Image playerEnemyTimeBar;

    public void Timer(GameProcess player1, GameProcess player2)
    {
        if (synced)
        {
            if (player1.myTurn)
            {
                player1.turnTimer -= Time.deltaTime;

                playerMeTimeBar.fillAmount = player1.turnTimer / TimeForTurn;
                TimerText(((int)player1.turnTimer).ToString());

                if(player1.turnTimer < - 0.5)
                {
                    TimerSwapAndText(player1, player2);
                }
            }
            else
            {
                player2.turnTimer -= Time.deltaTime;

                playerEnemyTimeBar.fillAmount = player2.turnTimer / TimeForTurn;
                TimerText(((int)player2.turnTimer).ToString());

                if (player2.turnTimer < - 0.5f)
                {
                    TimerSwapAndText(player1, player2);
                }
            }
        }
    }

    float TimeOutHost = 0;

    float TimeOut = 0;

    void TimerSwapAndText(GameProcess player1, GameProcess player2) 
    {
        if (PhotonNetwork.connected)
        {
            if (PhotonNetwork.isMasterClient)
            {
                if (player1.turnTimer < -0.3)
                {
                    Interaction.instance.Swap(true);
                }
                else
                {
                    Interaction.instance.Swap(false);
                }
            }
            else
            {
                TimeOutHost += Time.deltaTime;

                TimerText("Waiting for Host... " + (int)(10 - TimeOutHost));

                if ((int)(10 - TimeOutHost) < 0) 
                {
                    WinGame();
                }
            }
        }
        else
        {
            TimeOut += Time.deltaTime;

            TimerText("Check your connection! " + (int)(10 - TimeOut));

            if ((int)(8 - TimeOut) < 0)
            {
                LoseGame();
            }
        }
    }



    public Text playerTimeBarText;

    public Text playerTimeBarText1;

    public Text TimeLeft;

    public Text TimeLeft1;


    void TimerText(string text) 
    {
        playerTimeBarText.text = text;
        playerTimeBarText1.text = text;

        TimeLeft.text = ((int)(120 - (PhotonNetwork.time - StartTime))).ToString();
        TimeLeft1.text = ((int)(120 - (PhotonNetwork.time - StartTime))).ToString();
    }

    public void CheckGameOver() 
    {
        if (Me.health <= 0)
        {
            LoseGame();
        }
        else
        if (Enemy.health <= 0)
        {
            WinGame();
        }
    }

    public void SwapTurn(GameProcess player1, GameProcess player2) 
    {
        TimeOut = 0;

        TimeOutHost = 0;

        player1.Animating = true;

        player2.Animating = true;

        CheckGameOver();

        if (!GameOver)
        {
            if (player1.myTurn)
            {
                player1.turnTimer = -0.2f;

                player2.turnTimer = TimeForTurn;

                player1.buttons.BtnsPlace.SetActive(false);

                player2.health = Points.Plus(player2.health, Conv.HealthRestoreSwap(player2), player2.maxHealth);
            }
            else
            if (player2.myTurn)
            {
                player2.turnTimer = -0.2f;

                player1.turnTimer = TimeForTurn;

                player1.health = Points.Plus(player1.health, Conv.HealthRestoreSwap(player1), player1.maxHealth);
            }

            player1.myTurn = !player1.myTurn;

            player2.myTurn = !player2.myTurn;

            playerMeTimeBar.fillAmount = player1.turnTimer / TimeForTurn;

            playerEnemyTimeBar.fillAmount = player2.turnTimer / TimeForTurn;
        }
        else 
        {
            playerMeTimeBar.fillAmount = 0;

            playerEnemyTimeBar.fillAmount = 0;
        }

        interFace.Refresh(player1.health, player1.maxHealth, player1.stamina, player1.maxStamina, player2.health, player2.maxHealth, player2.stamina, player2.maxStamina);

        if (player2.bashed || player1.bashed)
        {
            player2.bashed = false;

            player1.bashed = false;

            Invoke("UpdPlayerStateTemp", 1.0f);

            SwapTurnTemp();
        }
        else 
        {
            Invoke("UpdPlayerStateTemp", 1.0f);
        }

        if (player1.iAmLeft)
        {
            if (player1.myTurn)
            {
                playerTimeBarText.alignment = TextAnchor.UpperLeft;
                playerTimeBarText1.alignment = TextAnchor.UpperLeft;
            }
            else
            {
                playerTimeBarText.alignment = TextAnchor.UpperRight;
                playerTimeBarText1.alignment = TextAnchor.UpperRight;
            }
        }
        else
        {
            if (player2.myTurn)
            {
                playerTimeBarText.alignment = TextAnchor.UpperLeft;
                playerTimeBarText1.alignment = TextAnchor.UpperLeft;
            }
            else
            {
                playerTimeBarText.alignment = TextAnchor.UpperRight;
                playerTimeBarText1.alignment = TextAnchor.UpperRight;
            }
        }
    }


    void SwapTurnTemp() 
    {
        SwapTurn(Me, Enemy);
    }


    void UpdPlayerStateTemp()
    {
        if (Enemy.myTurn)
        {
            UpdPlayerState(Enemy, Me);
        }
        else if (Me.myTurn)
        {
            UpdPlayerState(Me, Enemy);
        }
    }

    void UpdButtonsTexts()
    {
        if (!GameOver) 
        {
            animStop();
        }

        if (Enemy.myTurn)
        {
        }
        else if (Me.myTurn) 
        {
            if (!GameOver) 
            {
                Me.buttons.BtnsPlace.SetActive(true);

                if (Enemy.playerObj != null)
                {
                    playerDist = Mathf.Abs(Me.playerObj.transform.position.x - Enemy.playerObj.transform.position.x);
                }

                if (playerDist <= Conv.LightDistFunc(Me))
                {
                    Me.buttons.BtnLight.SetActive(true);
                    Me.buttons.BtnLight.GetComponentsInChildren<Text>()[0].text = "LIGHT\n" + ((int)(Me.lightChance * 100)).ToString() + " %";
                    Me.buttons.BtnLight.GetComponentsInChildren<Text>()[1].text = ((int)(Me.staminaPerMove * Me.staminaLightMult)).ToString();
                }
                else
                {
                    Me.buttons.BtnLight.SetActive(false);
                }

                if (playerDist <= Conv.MediumDistFunc(Me))
                {
                    Me.buttons.BtnMedium.SetActive(true);
                    Me.buttons.BtnMedium.GetComponentsInChildren<Text>()[0].text = "MEDIUM\n" + ((int)(Me.mediumChance * 100)).ToString() + " %";
                    Me.buttons.BtnMedium.GetComponentsInChildren<Text>()[1].text = ((int)(Me.staminaPerMove * Me.staminaMediumMult)).ToString();
                }
                else
                {
                    Me.buttons.BtnMedium.SetActive(false);
                }

                if (playerDist <= Me.heavyDist)
                {
                    Me.buttons.BtnHeavy.SetActive(true);
                    Me.buttons.BtnHeavy.GetComponentsInChildren<Text>()[0].text = "HEAVY\n" + ((int)(Me.heavyChance * 100)).ToString() + " %";
                    Me.buttons.BtnHeavy.GetComponentsInChildren<Text>()[1].text = ((int)(Me.staminaPerMove * Me.staminaHeavyMult)).ToString();

                    Me.buttons.BtnMoveLeft.SetActive(true);
                    Me.buttons.BtnMoveLeft.GetComponentInChildren<Text>().text = Me.staminaPerMove.ToString();

                    Me.buttons.BtnMoveRight.SetActive(false);

                }
                else
                {
                    Me.buttons.BtnHeavy.SetActive(false);

                    Me.buttons.BtnMoveLeft.SetActive(true);
                    Me.buttons.BtnMoveLeft.GetComponentInChildren<Text>().text = Me.staminaPerMove.ToString();

                    Me.buttons.BtnMoveRight.SetActive(true);
                    Me.buttons.BtnMoveRight.GetComponentInChildren<Text>().text = Me.staminaPerMove.ToString();
                }

                if (Me.stunned)
                {
                    Me.stunned = false;
                }

                Me.buttons.BtnMagic.GetComponentsInChildren<Text>()[1].text = ((int)(Me.maxStamina * Me.staminaMagic)).ToString();
            }
        }
    }


    public bool GameOver;

    public int SceneToLoad = 2;

    GameProcess LoserTemp;

    GameProcess WinnerTemp;
    public void LoseGame()
    {
        WinAndLoseHandler.LoseOnline();
        
        GameOver = true;
        SceneToLoad = 5;

        Me.buttons.BtnLeave.SetActive(true);
        Me.buttons.BtnsPlace.SetActive(false);

        LoserTemp = Me;
        WinnerTemp = Enemy;

        Invoke("WinAndLoseAnimation", 1.0f);
    }


    void WinGame()
    {
        WinAndLoseHandler.WinOnline();

        GameOver = true;
        SceneToLoad = 4;

        Me.buttons.BtnLeave.SetActive(true);
        Me.buttons.BtnsPlace.SetActive(false);

        LoserTemp = Enemy;
        WinnerTemp = Me;

        Invoke("WinAndLoseAnimation", 1.0f);
    }

    int LocalNumberForLoseAnimName;

    int LocalNumberForWinAnimName;

    void WinAndLoseAnimation() 
    {
        #region AnimationRotationForPropperDisplaying

        GameObject obj;

        if (LoserTemp.iAmLeft)
        {
            GameObject tempForAnim;

            if (PhotonNetwork.isMasterClient)
            {
                tempForAnim = GameObject.FindGameObjectsWithTag("HaveToBeRotated")[0];
            }
            else 
            {
                tempForAnim = GameObject.FindGameObjectsWithTag("HaveToBeRotated")[1];
            }

            tempForAnim.transform.localScale = new Vector3(-tempForAnim.transform.localScale.x, tempForAnim.transform.localScale.y, tempForAnim.transform.localScale.z);
        }
        else 
        {
            if (PhotonNetwork.isMasterClient)
            {
                if (Enemy.playerObj != null)
                {
                    obj = GameObject.FindGameObjectsWithTag("AnimationRotation")[1];
                }
                else 
                {
                    obj = new GameObject();
                }
            }
            else
            {
                obj = GameObject.FindGameObjectsWithTag("AnimationRotation")[0];
            }

            obj.transform.localScale = new Vector3(-obj.transform.localScale.x, obj.transform.localScale.y, obj.transform.localScale.z);

            obj = LoserTemp.playerObj;

            if (obj != null)
            {
                obj.transform.localScale = new Vector3(-obj.transform.localScale.x, obj.transform.localScale.y, obj.transform.localScale.z);
            }
        }

        #endregion

        LocalNumberForLoseAnimName = (int)PhotonNetwork.time % 3;

        LocalNumberForWinAnimName = WinnerTemp.WinAnimNumber;

        LoserTemp.animation.setAnimatorBoolTrue("PlayerDeath" + LocalNumberForLoseAnimName.ToString());

        WinnerTemp.animation.setAnimatorBoolTrue("PlayerVictory" + LocalNumberForWinAnimName.ToString());
    }
}
