using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparksTutorials;
using UnityEngine.UI;

public class SyncPlayers : Photon.PunBehaviour, IPunObservable
{
    #region Singleton
    public static SyncPlayers instance;
    bool IAmACopy;
    public void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Sync players Script found!");
            IAmACopy = true;
            return;
        }
        instance = this;
    }

    #endregion
    bool started;

    void Start() 
    {
        DataController.GetValue<string>("EquippedMagicMine");

        DataController.SaveValue("blockvar1Left", -1);
        DataController.SaveValue("blockvar2Left", -1);
        DataController.SaveValue("blockvar3Left", -1);
        DataController.SaveValue("blockvar4Left", -1);
        DataController.SaveValue("blockvar5Left", -1);
        DataController.SaveValue("blockvar6Left", -1);

        DataController.SaveValue("blockvar1Right", -1);
        DataController.SaveValue("blockvar2Right", -1);
        DataController.SaveValue("blockvar3Right", -1);
        DataController.SaveValue("blockvar4Right", -1);
        DataController.SaveValue("blockvar5Right", -1);
        DataController.SaveValue("blockvar6Right", -1);
    }
    // Start is called before the first frame update
    void StartSyncing()
    {
        if (!started) 
        {
            Debug.Log("Reseting synced data...");
            ResetSyncedDataAndUpdMine();
            CheckedForNotFairStats = false;
            allRecievedTemp = 0;
            started = true;
        }
    }


    bool syncedMine;
    bool syncedOther;
    bool CheckedForNotFairStats;

    GameProcess player = new GameProcess();

    public Image timerBarLeft;

    public Image timerBarRight;

    public Text TimeLeft;

    public Text TimeLeft1;

    public Text timerText;

    public Text timerText1;

    public List<GameObject> GameObjectsForInstantiation = new List<GameObject>();

    // Update is called once per frame

    GameObject LoadingScreen;

    Text[] LoadingScreenTexts;

    void Update()
    {
        if (!syncedOther)
        {
            if (DataController.GetValue<int>("syncedOther") > 0)
            {
                syncedOther = true;
            }

            if (DataController.GetValue<int>("syncedMine") > 0)
            {
                syncedMine = true;
            }
        }
        if (!CheckedForNotFairStats)
        {
            if (!started)
            {
                StartSyncing();
            }
            CheckIfSynced();

            if (syncedMine && syncedOther)
            {
                if (PhotonManager.TwoPlayersJoined)
                {
                    Debug.Log("Loading recieved players data...");

                    if (IAmACopy)
                    {
                        player.magicEquipped = "Curse";

                        player.buttons.BtnLeave = GameObject.FindGameObjectsWithTag("BtnLeave")[1];

                        player.buttons.BtnsPlace = GameObject.FindGameObjectsWithTag("BtnsPlace")[1];

                        player.playerInterface = GameObject.FindGameObjectsWithTag("Interface")[1];

                        player.playerInfoPanel = GameObject.FindGameObjectsWithTag("PlayerInfo")[1];

                        player.playerLoadingScreen = GameObject.FindGameObjectsWithTag("LoadingScreen")[1];

                        player.playerObj = GameObject.FindGameObjectsWithTag("Player")[1];

                        if (PhotonNetwork.isMasterClient)
                        {
                            player.iAmLeft = false;
                        }
                        else
                        {
                            player.iAmLeft = true;
                        }

                        player.animation.SetAnimator(player.playerObj.GetComponentInChildren<Animator>());

                        foreach (GameObject obj in GameObjectsForInstantiation)
                        {
                            player.animation.dict.Add(obj.name, obj);
                        }

                        TheGameManager.instance.LoadPlayerBase(player, false);

                        TheGameManager.instance.SetEnemy(player);

                        TheGameManager.instance.SetSyncedCopy(true);

                    }
                    else
                    {
                        player.magicEquipped = "Curse";

                        player.buttons.BtnLeave = GameObject.FindGameObjectsWithTag("BtnLeave")[0];

                        player.buttons.BtnsPlace = GameObject.FindGameObjectsWithTag("BtnsPlace")[0];

                        player.playerInterface = GameObject.FindGameObjectsWithTag("Interface")[0];

                        player.playerInfoPanel = GameObject.FindGameObjectsWithTag("PlayerInfo")[0];

                        player.playerLoadingScreen = GameObject.FindGameObjectsWithTag("LoadingScreen")[0];

                        player.playerObj = GameObject.FindGameObjectsWithTag("Player")[0];

                        if (PhotonNetwork.isMasterClient)
                        {
                            player.iAmLeft = true;
                        }
                        else
                        {
                            player.iAmLeft = false;
                        }

                        player.animation.SetAnimator(player.playerObj.GetComponentInChildren<Animator>());

                        foreach (GameObject obj in GameObjectsForInstantiation)
                        {
                            player.animation.dict.Add(obj.name, obj);
                        }

                        TheGameManager.instance.LoadPlayerBase(player, true);

                        TheGameManager.instance.SetMe(player);

                        TheGameManager.instance.SetSynced(true);

                        SetBtnObjects();

                        SetTimerObjects();
                    }
                    CheckForLegalStats();
                    LoadSprites();
                }
            }
            else
            {
                if (LoadingScreen == null)
                {
                    LoadingScreen = GameObject.FindGameObjectWithTag("LoadingScreen");
                }
                else
                {
                    if (LoadingScreenTexts == null)
                    {
                        LoadingScreenTexts = LoadingScreen.GetComponentsInChildren<Text>();
                    }
                    else
                    {
                        LoadingScreenTexts[0].text = "Searching for players.... \n \n" + "Currently playing: " + PhotonNetwork.countOfPlayersInRooms.ToString();
                        LoadingScreenTexts[1].text = "Searching for players.... \n \n" + "Currently playing: " + PhotonNetwork.countOfPlayersInRooms.ToString();
                    }
                }
            }
        }
    }

    void SetTimerObjects() 
    {
        TheGameManager.instance.playerTimeBarText = timerText;

        TheGameManager.instance.playerTimeBarText1 = timerText1;


        TheGameManager.instance.TimeLeft = TimeLeft;

        TheGameManager.instance.TimeLeft1 = TimeLeft1;

        if (PhotonNetwork.isMasterClient)
        {
            TheGameManager.instance.playerMeTimeBar = timerBarLeft;

            TheGameManager.instance.playerEnemyTimeBar = timerBarRight;
        }
        else
        {
            TheGameManager.instance.playerMeTimeBar = timerBarRight;

            TheGameManager.instance.playerEnemyTimeBar = timerBarLeft;
        }
    }

    void SetBtnObjects() 
    {
        int counter = 0; // Used for using only 1 copy of the buttons (because my button objects fo first on every client)
        foreach (GameObject gameObj in GameObject.FindGameObjectsWithTag("Buttons"))
        {
            if (counter < 7)
            {
                if (gameObj.name == "ButtonLightAttack")
                {
                    player.buttons.BtnLight = gameObj;
                    counter++;
                }
                else if (gameObj.name == "ButtonMediumAttack")
                {
                    player.buttons.BtnMedium = gameObj;
                    counter++;
                }
                else if (gameObj.name == "ButtonHeavyAttack")
                {
                    player.buttons.BtnHeavy = gameObj;
                    counter++;
                }
                else if (gameObj.name == "ButtonLeft")
                {
                    player.buttons.BtnMoveLeft = gameObj;
                    counter++;
                }
                else if (gameObj.name == "ButtonRight")
                {
                    player.buttons.BtnMoveRight = gameObj;
                    counter++;
                }
                else if (gameObj.name == "ButtonSleep")
                {
                    player.buttons.BtnSleep = gameObj;
                    counter++;
                }
                else if (gameObj.name == "ButtonMagicAttack")
                {
                    player.buttons.BtnMagic = gameObj;
                    counter++;
                }
            }
            else 
            {
                return;
            }         
        }
    }

    List<string> ForEquipmentImgsLoad = new List<string> { "Head", "Chest", "Arms", "Legs", "LeftHand", "RightHand", "Feet" };
    List<string> LocalNamesMine = new List<string> { "AttackMine", "AgilityMine", "PowerMine", "StrengthMine", "EnduranceMine", "SpeedMine", "SleepMine", "RegenMine" };
    List<string> LocalNamesOther = new List<string> { "AttackOther", "AgilityOther", "PowerOther", "StrengthOther", "EnduranceOther", "SpeedOther", "SleepOther", "RegenOther" };
    List<string> ModifiersList = Equipment.EquipmentModifiers;

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (PhotonManager.TwoPlayersJoined)
        {
            if (stream.isWriting)
            {
                if (syncedMine && syncedOther)
                {
                }
                else
                {
                    Debug.Log("NotSynced so im sending shit \n" + allRecievedTemp + " - temp about how many packs recieved \n" +
                        DataController.GetValue<int>("LvlOther") + " - Lvl of an oponent \n" + syncedMine + " " + DataController.GetValue<int>("syncedMine") 
                        + " " + syncedOther + " " + DataController.GetValue<int>("syncedOther"));

                    stream.SendNext(DataController.GetValue<int>("Rating"));

                    stream.SendNext((int)DataController.GetValue<int>("WinAnimNumberMine")); 

                    if (PhotonNetwork.isMasterClient)
                    {
                        stream.SendNext((int)DataController.GetValue<int>("blockvar1Left"));
                        stream.SendNext((int)DataController.GetValue<int>("blockvar2Left"));
                        stream.SendNext((int)DataController.GetValue<int>("blockvar3Left"));
                        stream.SendNext((int)DataController.GetValue<int>("blockvar4Left"));
                        stream.SendNext((int)DataController.GetValue<int>("blockvar5Left"));
                        stream.SendNext((int)DataController.GetValue<int>("blockvar6Left"));

                        stream.SendNext((int)DataController.GetValue<int>("blockvar1Right"));
                        stream.SendNext((int)DataController.GetValue<int>("blockvar2Right"));
                        stream.SendNext((int)DataController.GetValue<int>("blockvar3Right"));
                        stream.SendNext((int)DataController.GetValue<int>("blockvar4Right"));
                        stream.SendNext((int)DataController.GetValue<int>("blockvar5Right"));
                        stream.SendNext((int)DataController.GetValue<int>("blockvar6Right"));
                    }

                    stream.SendNext(DataController.GetValue<string>("EquippedMagicMine"));
                    stream.SendNext(DataController.GetValue<int>("HairStyle"));
                    stream.SendNext(DataController.GetValue<int>("Beard"));
                    stream.SendNext(DataController.GetValue<int>("BodyColor"));
                    stream.SendNext(DataController.GetValue<int>("HairColor"));

                    stream.SendNext((int)DataController.GetValue<int>("syncedMine"));
                    stream.SendNext(DataController.GetValue<string>("displayName"));
                    stream.SendNext(DataController.GetValue<int>("LvlMine"));

                    foreach (string EqSlot in ForEquipmentImgsLoad)
                    {
                        stream.SendNext(DataController.GetValue<string>("Equipped" + EqSlot + "Mine"));
                    }

                    foreach (string LocalName in LocalNamesMine)
                    {
                        stream.SendNext(DataController.GetValue<int>("Stats" + LocalName));
                    }
                    foreach (string modifier in ModifiersList)
                    {
                        stream.SendNext(DataController.GetValue<int>("Total" + modifier + "Mine"));
                    }
                }
            }
            else
            if (stream.isReading)
            {
                if (syncedMine && syncedOther)
                {
                }
                else
                {
                    DataController.SaveValue("RatingOther", (int)stream.ReceiveNext());

                    DataController.SaveValue("WinAnimNumberOther", (int)stream.ReceiveNext());

                    if (PhotonNetwork.isNonMasterClientInRoom)
                    {
                        DataController.SaveValue("blockvar1Left", (int)stream.ReceiveNext());
                        DataController.SaveValue("blockvar2Left", (int)stream.ReceiveNext());
                        DataController.SaveValue("blockvar3Left", (int)stream.ReceiveNext());
                        DataController.SaveValue("blockvar4Left", (int)stream.ReceiveNext());
                        DataController.SaveValue("blockvar5Left", (int)stream.ReceiveNext());
                        DataController.SaveValue("blockvar6Left", (int)stream.ReceiveNext());

                        DataController.SaveValue("blockvar1Right", (int)stream.ReceiveNext());
                        DataController.SaveValue("blockvar2Right", (int)stream.ReceiveNext());
                        DataController.SaveValue("blockvar3Right", (int)stream.ReceiveNext());
                        DataController.SaveValue("blockvar4Right", (int)stream.ReceiveNext());
                        DataController.SaveValue("blockvar5Right", (int)stream.ReceiveNext());
                        DataController.SaveValue("blockvar6Right", (int)stream.ReceiveNext());
                    }
                    DataController.SaveValue("EquippedMagicOther", (string)stream.ReceiveNext());
                    DataController.SaveValue("HairStyle" + "Other", (int)stream.ReceiveNext());
                    DataController.SaveValue("Beard" + "Other", (int)stream.ReceiveNext());
                    DataController.SaveValue("BodyColor" + "Other", (int)stream.ReceiveNext());
                    DataController.SaveValue("HairColor" + "Other", (int)stream.ReceiveNext());

                    DataController.SaveValue("syncedOther", (int)stream.ReceiveNext());
                    DataController.SaveValue("enemyName", (string)stream.ReceiveNext());
                    DataController.SaveValue("LvlOther", (int)stream.ReceiveNext());

                    foreach (string EqSlot in ForEquipmentImgsLoad)
                    {
                        DataController.SaveValue("Equipped" + EqSlot + "Other", (string)stream.ReceiveNext());
                    }

                    foreach (string LocalName in LocalNamesOther)
                    {
                        DataController.SaveValue("Stats" + LocalName, (int)stream.ReceiveNext());
                    }

                    foreach (string modifier in ModifiersList)
                    {
                        DataController.SaveValue("Total" + modifier + "Other", (int)stream.ReceiveNext());
                    }
                }
            }
        }
    }

    void ResetSyncedDataAndUpdMine()
    {
        CheckedForNotFairStats = false;
        DataController.DeleteValue("enemyName");
        DataController.SaveValue("LvlMine", DataController.GetValue<int>("Exp") / 100);
        DataController.SaveValue("LvlOther", -1);
        foreach (string LocalName in LocalNamesOther)
        {
            DataController.SaveValue("Stats" + LocalName, -1);
        }
        DataController.SaveValue("syncedOther", -1);
        DataController.SaveValue("syncedMine", -1);
        if (PhotonNetwork.isMasterClient)
        {
            DataController.SaveValue("blockvar1Left", (int)(PhotonNetwork.time + 0) % 15);
            DataController.SaveValue("blockvar2Left", (int)(PhotonNetwork.time + 1) % 15);
            DataController.SaveValue("blockvar3Left", (int)(PhotonNetwork.time + 2) % 15);
            DataController.SaveValue("blockvar4Left", (int)(PhotonNetwork.time + 3) % 15);
            DataController.SaveValue("blockvar5Left", (int)(PhotonNetwork.time + 4) % 15);
            DataController.SaveValue("blockvar6Left", (int)(PhotonNetwork.time + 5) % 15);

            DataController.SaveValue("blockvar1Right", (int)(PhotonNetwork.time + 6) % 15);
            DataController.SaveValue("blockvar2Right", (int)(PhotonNetwork.time + 7) % 15);
            DataController.SaveValue("blockvar3Right", (int)(PhotonNetwork.time + 8) % 15);
            DataController.SaveValue("blockvar4Right", (int)(PhotonNetwork.time + 9) % 15);
            DataController.SaveValue("blockvar5Right", (int)(PhotonNetwork.time + 10) % 15);
            DataController.SaveValue("blockvar6Right", (int)(PhotonNetwork.time + 11) % 15);
        }
    }

    int allRecievedTemp;
    void CheckIfSynced()
    {
        if ((allRecievedTemp < (LocalNamesOther.Count + ModifiersList.Count)))
        {
            allRecievedTemp = 0;
            if (DataController.GetValue<int>("LvlOther") >= 0)
            {
                allRecievedTemp += 1;
            }
            foreach (string LocNamesOth in LocalNamesOther)
            {
                if (DataController.GetValue<int>(LocNamesOth) >= 0)
                {
                    allRecievedTemp += 1;
                }
            }
            foreach (string modifier in ModifiersList)
            {
                if (DataController.GetValue<int>("Total" + modifier + "Other") >= 0)
                {
                    allRecievedTemp += 1;
                }
            }
        }
        else
        {
            DataController.SaveValue("syncedMine", 22);
        }
    }

    void CheckForLegalStats()
    {
        foreach (string LocalName in LocalNamesOther)
        {
            if (DataController.GetValue<int>(LocalName) > 40)
            {
                Debug.Log(LocalName + DataController.GetValue<int>(LocalName));
                OtherPlayerCheating();
            }
        }
        foreach (string modifier in ModifiersList)
        {
            Debug.Log(DataController.GetValue<int>("Total" + modifier + "Other") + modifier);
        }

        if (DataController.GetValue<int>("Total" + "ModifierArmor" + "Other") > 100)
        {
            OtherPlayerCheating();
        }

        if (DataController.GetValue<int>("Total" + "ModifierDamage" + "Other") > 100)
        {
            OtherPlayerCheating();
        }

        if (DataController.GetValue<int>("Total" + "ModifierMissChance" + "Other") > 100)
        {
            OtherPlayerCheating();
        }

        if (DataController.GetValue<int>("Total" + "ModifierCriticalChance" + "Other") > 100)
        {
            OtherPlayerCheating();
        }

        if (DataController.GetValue<int>("Total" + "ModifierBashChance" + "Other") > 100)
        {
            OtherPlayerCheating();
        }

        if (DataController.GetValue<int>("Total" + "ModifierStunChance" + "Other") > 100)
        {
            OtherPlayerCheating();
        }

        if (DataController.GetValue<int>("Total" + "ModifierBlockChance" + "Other") > 100)
        {
            OtherPlayerCheating();
        }
        CheckedForNotFairStats = true;
    }

    void OtherPlayerCheating()
    {
        WinAndLoseHandler.WinOnline();
        PhotonManager.GameOver = true;
    }

    void LoadSprites() 
    {
        if (PhotonNetwork.isMasterClient)
        {
            GameObject LeftHead = GameObject.FindGameObjectsWithTag("Head")[0];
            GameObject LeftHair = GameObject.FindGameObjectsWithTag("HairStyle")[0];
            GameObject LeftBeard = GameObject.FindGameObjectsWithTag("Beard")[0];
            GameObject LeftEyes = GameObject.FindGameObjectsWithTag("Eyes")[0];
            GameObject LeftBody = GameObject.FindGameObjectsWithTag("Body")[0];
            GameObject LeftWaist = GameObject.FindGameObjectsWithTag("Waist")[0];

            GameObject LeftLeftShoulder = GameObject.FindGameObjectsWithTag("LeftShoulder")[0];
            GameObject LeftLeftElbow = GameObject.FindGameObjectsWithTag("LeftElbow")[0];
            GameObject LeftLeftThigh = GameObject.FindGameObjectsWithTag("LeftThigh")[0];
            GameObject LeftLeftShin = GameObject.FindGameObjectsWithTag("LeftShin")[0];
            GameObject LeftLeftFoot = GameObject.FindGameObjectsWithTag("Feet")[0];

            GameObject LeftRightShoulder = GameObject.FindGameObjectsWithTag("RightShoulder")[0];
            GameObject LeftRightElbow = GameObject.FindGameObjectsWithTag("RightElbow")[0];
            GameObject LeftRightThigh = GameObject.FindGameObjectsWithTag("RightThigh")[0];
            GameObject LeftRightShin = GameObject.FindGameObjectsWithTag("RightShin")[0];
            GameObject LeftRightFoot = GameObject.FindGameObjectsWithTag("Feet")[1];

            GameObject LeftHelmet = GameObject.FindGameObjectsWithTag("Helmet")[0];
            GameObject LeftBreastPlate = GameObject.FindGameObjectsWithTag("BreastPlate")[0];
            GameObject LeftSleevesLeftShoulder = GameObject.FindGameObjectsWithTag("SleevesLeftShoulder")[0];
            GameObject LeftSleevesRightShoulder = GameObject.FindGameObjectsWithTag("SleevesRightShoulder")[0];
            GameObject LeftSleevesLeftElbow = GameObject.FindGameObjectsWithTag("SleevesLeftElbow")[0];
            GameObject LeftSleevesRightElbow = GameObject.FindGameObjectsWithTag("SleevesRightElbow")[0];
            GameObject LeftLeftHand = GameObject.FindGameObjectsWithTag("LeftHand")[0];
            GameObject LeftRightHand = GameObject.FindGameObjectsWithTag("RightHand")[0];
            GameObject LeftPantsWaist = GameObject.FindGameObjectsWithTag("PantsWaist")[0];
            GameObject LeftPantsLeftThigh = GameObject.FindGameObjectsWithTag("PantsLeftThigh")[0];
            GameObject LeftPantsRightThigh = GameObject.FindGameObjectsWithTag("PantsRightThigh")[0];
            GameObject LeftPantsLeftShin = GameObject.FindGameObjectsWithTag("PantsLeftShin")[0];
            GameObject LeftPantsRightShin = GameObject.FindGameObjectsWithTag("PantsRightShin")[0];
            GameObject LeftLeftBoot = GameObject.FindGameObjectsWithTag("LeftBoot")[0];
            GameObject LeftRightBoot = GameObject.FindGameObjectsWithTag("RightBoot")[0];


            GameObject RightHead = GameObject.FindGameObjectsWithTag("Head")[1];
            GameObject RightHair = GameObject.FindGameObjectsWithTag("HairStyle")[1];
            GameObject RightBeard = GameObject.FindGameObjectsWithTag("Beard")[1];
            GameObject RightEyes = GameObject.FindGameObjectsWithTag("Eyes")[1];
            GameObject RightBody = GameObject.FindGameObjectsWithTag("Body")[1];
            GameObject RightWaist = GameObject.FindGameObjectsWithTag("Waist")[1];

            GameObject RightLeftShoulder = GameObject.FindGameObjectsWithTag("LeftShoulder")[1];
            GameObject RightLeftElbow = GameObject.FindGameObjectsWithTag("LeftElbow")[1];
            GameObject RightLeftThigh = GameObject.FindGameObjectsWithTag("LeftThigh")[1];
            GameObject RightLeftShin = GameObject.FindGameObjectsWithTag("LeftShin")[1];
            GameObject RightLeftFoot = GameObject.FindGameObjectsWithTag("Feet")[2];

            GameObject RightRightShoulder = GameObject.FindGameObjectsWithTag("RightShoulder")[1];
            GameObject RightRightElbow = GameObject.FindGameObjectsWithTag("RightElbow")[1];
            GameObject RightRightThigh = GameObject.FindGameObjectsWithTag("RightThigh")[1];
            GameObject RightRightShin = GameObject.FindGameObjectsWithTag("RightShin")[1];
            GameObject RightRightFoot = GameObject.FindGameObjectsWithTag("Feet")[3];

            GameObject RightHelmet = GameObject.FindGameObjectsWithTag("Helmet")[1];
            GameObject RightBreastPlate = GameObject.FindGameObjectsWithTag("BreastPlate")[1];
            GameObject RightSleevesLeftShoulder = GameObject.FindGameObjectsWithTag("SleevesLeftShoulder")[1];
            GameObject RightSleevesRightShoulder = GameObject.FindGameObjectsWithTag("SleevesRightShoulder")[1];
            GameObject RightSleevesLeftElbow = GameObject.FindGameObjectsWithTag("SleevesLeftElbow")[1];
            GameObject RightSleevesRightElbow = GameObject.FindGameObjectsWithTag("SleevesRightElbow")[1];
            GameObject RightLeftHand = GameObject.FindGameObjectsWithTag("LeftHand")[1];
            GameObject RightRightHand = GameObject.FindGameObjectsWithTag("RightHand")[1];
            GameObject RightPantsWaist = GameObject.FindGameObjectsWithTag("PantsWaist")[1];
            GameObject RightPantsLeftThigh = GameObject.FindGameObjectsWithTag("PantsLeftThigh")[1];
            GameObject RightPantsRightThigh = GameObject.FindGameObjectsWithTag("PantsRightThigh")[1];
            GameObject RightPantsLeftShin = GameObject.FindGameObjectsWithTag("PantsLeftShin")[1];
            GameObject RightPantsRightShin = GameObject.FindGameObjectsWithTag("PantsRightShin")[1];
            GameObject RightLeftBoot = GameObject.FindGameObjectsWithTag("LeftBoot")[1];
            GameObject RightRightBoot = GameObject.FindGameObjectsWithTag("RightBoot")[1];

            if (DataController.GetValue<string>("Equipped" + "Head" + "Mine") != "")
            {
                LeftHelmet.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Head" + "Mine"));
            }
            else
            {
                LeftHelmet.GetComponent<SpriteRenderer>().color = Color.clear;
            }
            if (DataController.GetValue<string>("Equipped" + "Chest" + "Mine") != "")
            {
                LeftBreastPlate.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Chest" + "Mine"));
            }
            else
            {
                LeftBreastPlate.GetComponent<SpriteRenderer>().color = Color.clear;
            }
            if (DataController.GetValue<string>("Equipped" + "Arms" + "Mine") != "")
            {
                LeftSleevesLeftShoulder.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Arms" + "Mine") + "ShoulderLeft");
                LeftSleevesRightShoulder.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Arms" + "Mine") + "ShoulderRight");
                LeftSleevesLeftElbow.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Arms" + "Mine") + "ElbowLeft");
                LeftSleevesRightElbow.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Arms" + "Mine") + "ElbowRight");
            }
            else
            {
                LeftSleevesLeftShoulder.GetComponent<SpriteRenderer>().color = Color.clear;
                LeftSleevesRightShoulder.GetComponent<SpriteRenderer>().color = Color.clear;
                LeftSleevesLeftElbow.GetComponent<SpriteRenderer>().color = Color.clear;
                LeftSleevesRightElbow.GetComponent<SpriteRenderer>().color = Color.clear;
            }
            if (DataController.GetValue<string>("Equipped" + "LeftHand" + "Mine") != "")
            {
                LeftLeftHand.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "LeftHand" + "Mine"));
            }
            else
            {
                LeftLeftHand.GetComponent<SpriteRenderer>().color = Color.clear;
            }
            if (DataController.GetValue<string>("Equipped" + "RightHand" + "Mine") != "")
            {
                if (DataController.GetValue<string>("Equipped" + "RightHand" + "Mine").Contains("Daggers"))
                {
                    LeftLeftHand.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "RightHand" + "Mine") + "Left");

                    LeftRightHand.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "RightHand" + "Mine") + "Right");

                    LeftRightHand.GetComponent<SpriteRenderer>().color = Color.white;

                    LeftLeftHand.GetComponent<SpriteRenderer>().color = Color.white;
                }
                else 
                {
                    LeftRightHand.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "RightHand" + "Mine"));
                }
            }
            else
            {
                LeftRightHand.GetComponent<SpriteRenderer>().color = Color.clear;
            }
            if (DataController.GetValue<string>("Equipped" + "Legs" + "Mine") != "")
            {
                LeftPantsWaist.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Legs" + "Mine") + "Waist");
                LeftPantsLeftThigh.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Legs" + "Mine") + "Thigh");
                LeftPantsRightThigh.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Legs" + "Mine") + "Thigh");
                LeftPantsLeftShin.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Legs" + "Mine") + "Shin");
                LeftPantsRightShin.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Legs" + "Mine") + "Shin");
            }
            else
            {
                LeftPantsWaist.GetComponent<SpriteRenderer>().color = Color.clear;
                LeftPantsLeftThigh.GetComponent<SpriteRenderer>().color = Color.clear;
                LeftPantsRightThigh.GetComponent<SpriteRenderer>().color = Color.clear;
                LeftPantsLeftShin.GetComponent<SpriteRenderer>().color = Color.clear;
                LeftPantsRightShin.GetComponent<SpriteRenderer>().color = Color.clear;
            }
            if (DataController.GetValue<string>("Equipped" + "Feet" + "Mine") != "")
            {
                LeftLeftBoot.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Feet" + "Mine").TrimEnd('s'));
                LeftRightBoot.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Feet" + "Mine").TrimEnd('s'));
            }
            else
            {
                LeftLeftBoot.GetComponent<SpriteRenderer>().color = Color.clear;
                LeftRightBoot.GetComponent<SpriteRenderer>().color = Color.clear;
            }



            if (DataController.GetValue<string>("Equipped" + "Head" + "Other") != "")
            {
                RightHelmet.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Head" + "Other"));
            }
            else
            {
                RightHelmet.GetComponent<SpriteRenderer>().color = Color.clear;
            }
            if (DataController.GetValue<string>("Equipped" + "Chest" + "Other") != "")
            {
                RightBreastPlate.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Chest" + "Other"));
            }
            else
            {
                RightBreastPlate.GetComponent<SpriteRenderer>().color = Color.clear;
            }
            if (DataController.GetValue<string>("Equipped" + "Arms" + "Other") != "")
            {
                RightSleevesLeftShoulder.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Arms" + "Other") + "ShoulderLeft");
                RightSleevesRightShoulder.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Arms" + "Other") + "ShoulderRight");
                RightSleevesLeftElbow.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Arms" + "Other") + "ElbowLeft");
                RightSleevesRightElbow.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Arms" + "Other") + "ElbowRight");
            }
            else
            {
                RightSleevesLeftShoulder.GetComponent<SpriteRenderer>().color = Color.clear;
                RightSleevesRightShoulder.GetComponent<SpriteRenderer>().color = Color.clear;
                RightSleevesLeftElbow.GetComponent<SpriteRenderer>().color = Color.clear;
                RightSleevesRightElbow.GetComponent<SpriteRenderer>().color = Color.clear;
            }
            if (DataController.GetValue<string>("Equipped" + "LeftHand" + "Other") != "")
            {
                RightLeftHand.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "LeftHand" + "Other"));
            }
            else
            {
                RightLeftHand.GetComponent<SpriteRenderer>().color = Color.clear;
            }
            if (DataController.GetValue<string>("Equipped" + "RightHand" + "Other") != "")
            {
                if (DataController.GetValue<string>("Equipped" + "RightHand" + "Other").Contains("Daggers"))
                {
                    RightLeftHand.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "RightHand" + "Other") + "Left");

                    RightRightHand.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "RightHand" + "Other") + "Right");

                    RightRightHand.GetComponent<SpriteRenderer>().color = Color.white;

                    RightLeftHand.GetComponent<SpriteRenderer>().color = Color.white;
                }
                else
                {
                    RightRightHand.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "RightHand" + "Other"));
                }
            }
            else
            {
                RightRightHand.GetComponent<SpriteRenderer>().color = Color.clear;
            }
            if (DataController.GetValue<string>("Equipped" + "Legs" + "Other") != "")
            {
                RightPantsWaist.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Legs" + "Other") + "Waist");
                RightPantsLeftThigh.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Legs" + "Other") + "Thigh");
                RightPantsRightThigh.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Legs" + "Other") + "Thigh");
                RightPantsLeftShin.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Legs" + "Other") + "Shin");
                RightPantsRightShin.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Legs" + "Other") + "Shin");
            }
            else
            {
                RightPantsWaist.GetComponent<SpriteRenderer>().color = Color.clear;
                RightPantsLeftThigh.GetComponent<SpriteRenderer>().color = Color.clear;
                RightPantsRightThigh.GetComponent<SpriteRenderer>().color = Color.clear;
                RightPantsLeftShin.GetComponent<SpriteRenderer>().color = Color.clear;
                RightPantsRightShin.GetComponent<SpriteRenderer>().color = Color.clear;
            }
            if (DataController.GetValue<string>("Equipped" + "Feet" + "Other") != "")
            {
                RightLeftBoot.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Feet" + "Other").TrimEnd('s'));
                RightRightBoot.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Feet" + "Other").TrimEnd('s'));
            }
            else
            {
                RightLeftBoot.GetComponent<SpriteRenderer>().color = Color.clear;
                RightRightBoot.GetComponent<SpriteRenderer>().color = Color.clear;
            }

            LeftBeard.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Beards" + BeardsScript.BeardsList[DataController.GetValue<int>("Beard")]);
            LeftBeard.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("HairColor"));
            RightBeard.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Beards" + BeardsScript.BeardsList[DataController.GetValue<int>("Beard" + "Other")]);
            RightBeard.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("HairColor" + "Other"));

            LeftHead.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor"));
            LeftBody.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor"));
            LeftWaist.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor"));
            LeftLeftShoulder.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor"));
            LeftRightShoulder.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor"));
            LeftLeftElbow.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor"));
            LeftRightElbow.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor"));
            LeftLeftThigh.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor"));
            LeftRightThigh.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor"));
            LeftLeftShin.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor"));
            LeftRightShin.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor"));
            LeftLeftFoot.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor"));
            LeftRightFoot.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor"));


            LeftHair.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("HairStyles" + HairStylesScript.HairList[DataController.GetValue<int>("HairStyle")]);
            LeftHair.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("HairColor"));

            RightHair.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("HairStyles" + HairStylesScript.HairList[DataController.GetValue<int>("HairStyle" + "Other")]);
            RightHair.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("HairColor" + "Other"));

            RightHead.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor" + "Other"));
            RightBody.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor" + "Other"));
            RightWaist.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor" + "Other"));
            RightLeftShoulder.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor" + "Other"));
            RightRightShoulder.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor" + "Other"));
            RightLeftElbow.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor" + "Other"));
            RightRightElbow.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor" + "Other"));
            RightLeftThigh.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor" + "Other"));
            RightRightThigh.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor" + "Other"));
            RightLeftShin.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor" + "Other"));
            RightRightShin.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor" + "Other"));
            RightLeftFoot.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor" + "Other"));
            RightRightFoot.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor" + "Other"));

            #region LoadPlayerInfoBtnSprites

            GameObject LeftInfo = GameObject.FindGameObjectsWithTag("InfoBtn")[0];

            SpriteRenderer[] SpriteList = LeftInfo.GetComponentsInChildren<SpriteRenderer>();

            SpriteList[0].sprite = LeftHead.GetComponent<SpriteRenderer>().sprite;

            SpriteList[0].color = LeftHead.GetComponent<SpriteRenderer>().color;

            SpriteList[2].sprite = LeftHair.GetComponent<SpriteRenderer>().sprite;

            SpriteList[2].color = LeftHair.GetComponent<SpriteRenderer>().color;

            SpriteList[3].sprite = LeftBeard.GetComponent<SpriteRenderer>().sprite;

            SpriteList[3].color = LeftBeard.GetComponent<SpriteRenderer>().color;

            GameObject RightInfo = GameObject.FindGameObjectsWithTag("InfoBtn")[1];

            SpriteList = RightInfo.GetComponentsInChildren<SpriteRenderer>();

            SpriteList[0].sprite = RightHead.GetComponent<SpriteRenderer>().sprite;

            SpriteList[0].color = RightHead.GetComponent<SpriteRenderer>().color;

            SpriteList[2].sprite = RightHair.GetComponent<SpriteRenderer>().sprite;

            SpriteList[2].color = RightHair.GetComponent<SpriteRenderer>().color;

            SpriteList[3].sprite = RightBeard.GetComponent<SpriteRenderer>().sprite;

            SpriteList[3].color = RightBeard.GetComponent<SpriteRenderer>().color;

            #endregion
        }
        else if (PhotonNetwork.isNonMasterClientInRoom)
        {
            GameObject LeftHead = GameObject.FindGameObjectsWithTag("Head")[1];
            GameObject LeftHair = GameObject.FindGameObjectsWithTag("HairStyle")[1];
            GameObject LeftBeard = GameObject.FindGameObjectsWithTag("Beard")[1];
            GameObject LeftEyes = GameObject.FindGameObjectsWithTag("Eyes")[1];
            GameObject LeftBody = GameObject.FindGameObjectsWithTag("Body")[1];
            GameObject LeftWaist = GameObject.FindGameObjectsWithTag("Waist")[1];

            GameObject LeftLeftShoulder = GameObject.FindGameObjectsWithTag("LeftShoulder")[1];
            GameObject LeftLeftElbow = GameObject.FindGameObjectsWithTag("LeftElbow")[1];
            GameObject LeftLeftThigh = GameObject.FindGameObjectsWithTag("LeftThigh")[1];
            GameObject LeftLeftShin = GameObject.FindGameObjectsWithTag("LeftShin")[1];
            GameObject LeftLeftFoot = GameObject.FindGameObjectsWithTag("Feet")[2];

            GameObject LeftRightShoulder = GameObject.FindGameObjectsWithTag("RightShoulder")[1];
            GameObject LeftRightElbow = GameObject.FindGameObjectsWithTag("RightElbow")[1];
            GameObject LeftRightThigh = GameObject.FindGameObjectsWithTag("RightThigh")[1];
            GameObject LeftRightShin = GameObject.FindGameObjectsWithTag("RightShin")[1];
            GameObject LeftRightFoot = GameObject.FindGameObjectsWithTag("Feet")[3];

            GameObject LeftHelmet = GameObject.FindGameObjectsWithTag("Helmet")[1];
            GameObject LeftBreastPlate = GameObject.FindGameObjectsWithTag("BreastPlate")[1];
            GameObject LeftSleevesLeftShoulder = GameObject.FindGameObjectsWithTag("SleevesLeftShoulder")[1];
            GameObject LeftSleevesRightShoulder = GameObject.FindGameObjectsWithTag("SleevesRightShoulder")[1];
            GameObject LeftSleevesLeftElbow = GameObject.FindGameObjectsWithTag("SleevesLeftElbow")[1];
            GameObject LeftSleevesRightElbow = GameObject.FindGameObjectsWithTag("SleevesRightElbow")[1];
            GameObject LeftLeftHand = GameObject.FindGameObjectsWithTag("LeftHand")[1];
            GameObject LeftRightHand = GameObject.FindGameObjectsWithTag("RightHand")[1];
            GameObject LeftPantsWaist = GameObject.FindGameObjectsWithTag("PantsWaist")[1];
            GameObject LeftPantsLeftThigh = GameObject.FindGameObjectsWithTag("PantsLeftThigh")[1];
            GameObject LeftPantsRightThigh = GameObject.FindGameObjectsWithTag("PantsRightThigh")[1];
            GameObject LeftPantsLeftShin = GameObject.FindGameObjectsWithTag("PantsLeftShin")[1];
            GameObject LeftPantsRightShin = GameObject.FindGameObjectsWithTag("PantsRightShin")[1];
            GameObject LeftLeftBoot = GameObject.FindGameObjectsWithTag("LeftBoot")[1];
            GameObject LeftRightBoot = GameObject.FindGameObjectsWithTag("RightBoot")[1];


            GameObject RightHead = GameObject.FindGameObjectsWithTag("Head")[0];
            GameObject RightHair = GameObject.FindGameObjectsWithTag("HairStyle")[0];
            GameObject RightBeard = GameObject.FindGameObjectsWithTag("Beard")[0];
            GameObject RightEyes = GameObject.FindGameObjectsWithTag("Eyes")[0];
            GameObject RightBody = GameObject.FindGameObjectsWithTag("Body")[0];
            GameObject RightWaist = GameObject.FindGameObjectsWithTag("Waist")[0];

            GameObject RightLeftShoulder = GameObject.FindGameObjectsWithTag("LeftShoulder")[0];
            GameObject RightLeftElbow = GameObject.FindGameObjectsWithTag("LeftElbow")[0];
            GameObject RightLeftThigh = GameObject.FindGameObjectsWithTag("LeftThigh")[0];
            GameObject RightLeftShin = GameObject.FindGameObjectsWithTag("LeftShin")[0];
            GameObject RightLeftFoot = GameObject.FindGameObjectsWithTag("Feet")[0];

            GameObject RightRightShoulder = GameObject.FindGameObjectsWithTag("RightShoulder")[0];
            GameObject RightRightElbow = GameObject.FindGameObjectsWithTag("RightElbow")[0];
            GameObject RightRightThigh = GameObject.FindGameObjectsWithTag("RightThigh")[0];
            GameObject RightRightShin = GameObject.FindGameObjectsWithTag("RightShin")[0];
            GameObject RightRightFoot = GameObject.FindGameObjectsWithTag("Feet")[1];

            GameObject RightHelmet = GameObject.FindGameObjectsWithTag("Helmet")[0];
            GameObject RightBreastPlate = GameObject.FindGameObjectsWithTag("BreastPlate")[0];
            GameObject RightSleevesLeftShoulder = GameObject.FindGameObjectsWithTag("SleevesLeftShoulder")[0];
            GameObject RightSleevesRightShoulder = GameObject.FindGameObjectsWithTag("SleevesRightShoulder")[0];
            GameObject RightSleevesLeftElbow = GameObject.FindGameObjectsWithTag("SleevesLeftElbow")[0];
            GameObject RightSleevesRightElbow = GameObject.FindGameObjectsWithTag("SleevesRightElbow")[0];
            GameObject RightLeftHand = GameObject.FindGameObjectsWithTag("LeftHand")[0];
            GameObject RightRightHand = GameObject.FindGameObjectsWithTag("RightHand")[0];
            GameObject RightPantsWaist = GameObject.FindGameObjectsWithTag("PantsWaist")[0];
            GameObject RightPantsLeftThigh = GameObject.FindGameObjectsWithTag("PantsLeftThigh")[0];
            GameObject RightPantsRightThigh = GameObject.FindGameObjectsWithTag("PantsRightThigh")[0];
            GameObject RightPantsLeftShin = GameObject.FindGameObjectsWithTag("PantsLeftShin")[0];
            GameObject RightPantsRightShin = GameObject.FindGameObjectsWithTag("PantsRightShin")[0];
            GameObject RightLeftBoot = GameObject.FindGameObjectsWithTag("LeftBoot")[0];
            GameObject RightRightBoot = GameObject.FindGameObjectsWithTag("RightBoot")[0];

            if (DataController.GetValue<string>("Equipped" + "Head" + "Other") != "")
            {
                LeftHelmet.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Head" + "Other"));
            }
            else
            {
                LeftHelmet.GetComponent<SpriteRenderer>().color = Color.clear;
            }
            if (DataController.GetValue<string>("Equipped" + "Chest" + "Other") != "")
            {
                LeftBreastPlate.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Chest" + "Other"));
            }
            else
            {
                LeftBreastPlate.GetComponent<SpriteRenderer>().color = Color.clear;
            }
            if (DataController.GetValue<string>("Equipped" + "Arms" + "Other") != "")
            {
                LeftSleevesLeftShoulder.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Arms" + "Other") + "ShoulderLeft");
                LeftSleevesRightShoulder.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Arms" + "Other") + "ShoulderRight");
                LeftSleevesLeftElbow.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Arms" + "Other") + "ElbowLeft");
                LeftSleevesRightElbow.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Arms" + "Other") + "ElbowRight");
            }
            else
            {
                LeftSleevesLeftShoulder.GetComponent<SpriteRenderer>().color = Color.clear;
                LeftSleevesRightShoulder.GetComponent<SpriteRenderer>().color = Color.clear;
                LeftSleevesLeftElbow.GetComponent<SpriteRenderer>().color = Color.clear;
                LeftSleevesRightElbow.GetComponent<SpriteRenderer>().color = Color.clear;
            }
            if (DataController.GetValue<string>("Equipped" + "LeftHand" + "Other") != "")
            {
                LeftLeftHand.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "LeftHand" + "Other"));
            }
            else
            {
                LeftLeftHand.GetComponent<SpriteRenderer>().color = Color.clear;
            }
            if (DataController.GetValue<string>("Equipped" + "RightHand" + "Other") != "")
            {
                if (DataController.GetValue<string>("Equipped" + "RightHand" + "Other").Contains("Daggers"))
                {
                    LeftLeftHand.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "RightHand" + "Other") + "Left");

                    LeftRightHand.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "RightHand" + "Other") + "Right");

                    LeftRightHand.GetComponent<SpriteRenderer>().color = Color.white;

                    LeftLeftHand.GetComponent<SpriteRenderer>().color = Color.white;
                }
                else
                {
                    LeftRightHand.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "RightHand" + "Other"));
                }
            }
            else
            {
                LeftRightHand.GetComponent<SpriteRenderer>().color = Color.clear;
            }
            if (DataController.GetValue<string>("Equipped" + "Legs" + "Other") != "")
            {
                LeftPantsWaist.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Legs" + "Other") + "Waist");
                LeftPantsLeftThigh.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Legs" + "Other") + "Thigh");
                LeftPantsRightThigh.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Legs" + "Other") + "Thigh");
                LeftPantsLeftShin.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Legs" + "Other") + "Shin");
                LeftPantsRightShin.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Legs" + "Other") + "Shin");
            }
            else
            {
                LeftPantsWaist.GetComponent<SpriteRenderer>().color = Color.clear;
                LeftPantsLeftThigh.GetComponent<SpriteRenderer>().color = Color.clear;
                LeftPantsRightThigh.GetComponent<SpriteRenderer>().color = Color.clear;
                LeftPantsLeftShin.GetComponent<SpriteRenderer>().color = Color.clear;
                LeftPantsRightShin.GetComponent<SpriteRenderer>().color = Color.clear;
            }
            if (DataController.GetValue<string>("Equipped" + "Feet" + "Other") != "")
            {
                LeftLeftBoot.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Feet" + "Other").TrimEnd('s'));
                LeftRightBoot.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Feet" + "Other").TrimEnd('s'));
            }
            else
            {
                LeftLeftBoot.GetComponent<SpriteRenderer>().color = Color.clear;
                LeftRightBoot.GetComponent<SpriteRenderer>().color = Color.clear;
            }



            if (DataController.GetValue<string>("Equipped" + "Head" + "Mine") != "")
            {
                RightHelmet.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Head" + "Mine"));
            }
            else
            {
                RightHelmet.GetComponent<SpriteRenderer>().color = Color.clear;
            }
            if (DataController.GetValue<string>("Equipped" + "Chest" + "Mine") != "")
            {
                RightBreastPlate.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Chest" + "Mine"));
            }
            else
            {
                RightBreastPlate.GetComponent<SpriteRenderer>().color = Color.clear;
            }
            if (DataController.GetValue<string>("Equipped" + "Arms" + "Mine") != "")
            {
                RightSleevesLeftShoulder.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Arms" + "Mine") + "ShoulderLeft");
                RightSleevesRightShoulder.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Arms" + "Mine") + "ShoulderRight");
                RightSleevesLeftElbow.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Arms" + "Mine") + "ElbowLeft");
                RightSleevesRightElbow.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Arms" + "Mine") + "ElbowRight");
            }
            else
            {
                RightSleevesLeftShoulder.GetComponent<SpriteRenderer>().color = Color.clear;
                RightSleevesRightShoulder.GetComponent<SpriteRenderer>().color = Color.clear;
                RightSleevesLeftElbow.GetComponent<SpriteRenderer>().color = Color.clear;
                RightSleevesRightElbow.GetComponent<SpriteRenderer>().color = Color.clear;
            }
            if (DataController.GetValue<string>("Equipped" + "LeftHand" + "Mine") != "")
            {
                RightLeftHand.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "LeftHand" + "Mine"));
            }
            else
            {
                RightLeftHand.GetComponent<SpriteRenderer>().color = Color.clear;
            }
            if (DataController.GetValue<string>("Equipped" + "RightHand" + "Mine") != "")
            {
                if (DataController.GetValue<string>("Equipped" + "RightHand" + "Mine").Contains("Daggers"))
                {
                    RightLeftHand.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "RightHand" + "Mine") + "Left");

                    RightRightHand.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "RightHand" + "Mine") + "Right");

                    RightRightHand.GetComponent<SpriteRenderer>().color = Color.white;

                    RightLeftHand.GetComponent<SpriteRenderer>().color = Color.white;
                }
                else
                {
                    RightRightHand.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "RightHand" + "Mine"));
                }
            }
            else
            {
                RightRightHand.GetComponent<SpriteRenderer>().color = Color.clear;
            }
            if (DataController.GetValue<string>("Equipped" + "Legs" + "Mine") != "")
            {
                RightPantsWaist.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Legs" + "Mine") + "Waist");
                RightPantsLeftThigh.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Legs" + "Mine") + "Thigh");
                RightPantsRightThigh.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Legs" + "Mine") + "Thigh");
                RightPantsLeftShin.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Legs" + "Mine") + "Shin");
                RightPantsRightShin.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Legs" + "Mine") + "Shin");
            }
            else
            {
                RightPantsWaist.GetComponent<SpriteRenderer>().color = Color.clear;
                RightPantsLeftThigh.GetComponent<SpriteRenderer>().color = Color.clear;
                RightPantsRightThigh.GetComponent<SpriteRenderer>().color = Color.clear;
                RightPantsLeftShin.GetComponent<SpriteRenderer>().color = Color.clear;
                RightPantsRightShin.GetComponent<SpriteRenderer>().color = Color.clear;
            }
            if (DataController.GetValue<string>("Equipped" + "Feet" + "Mine") != "")
            {
                RightLeftBoot.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Feet" + "Mine").TrimEnd('s'));
                RightRightBoot.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "Feet" + "Mine").TrimEnd('s'));
            }
            else
            {
                RightLeftBoot.GetComponent<SpriteRenderer>().color = Color.clear;
                RightRightBoot.GetComponent<SpriteRenderer>().color = Color.clear;
            }

            LeftBeard.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Beards" + BeardsScript.BeardsList[DataController.GetValue<int>("Beard" + "Other")]);
            LeftBeard.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("HairColor" + "Other"));
            RightBeard.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Beards" + BeardsScript.BeardsList[DataController.GetValue<int>("Beard")]);
            RightBeard.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("HairColor"));

            LeftHead.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor" + "Other"));
            LeftBody.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor" + "Other"));
            LeftWaist.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor" + "Other"));
            LeftLeftShoulder.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor" + "Other"));
            LeftRightShoulder.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor" + "Other"));
            LeftLeftElbow.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor" + "Other"));
            LeftRightElbow.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor" + "Other"));
            LeftLeftThigh.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor" + "Other"));
            LeftRightThigh.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor" + "Other"));
            LeftLeftShin.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor" + "Other"));
            LeftRightShin.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor" + "Other"));
            LeftLeftFoot.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor" + "Other"));
            LeftRightFoot.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor" + "Other"));

            LeftHair.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("HairStyles" + HairStylesScript.HairList[DataController.GetValue<int>("HairStyle" + "Other")]);
            LeftHair.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("HairColor" + "Other"));
            RightHair.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("HairStyles" + HairStylesScript.HairList[DataController.GetValue<int>("HairStyle")]);
            RightHair.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("HairColor"));

            RightHead.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor"));
            RightBody.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor"));
            RightWaist.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor"));
            RightLeftShoulder.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor"));
            RightRightShoulder.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor"));
            RightLeftElbow.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor"));
            RightRightElbow.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor"));
            RightLeftThigh.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor"));
            RightRightThigh.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor"));
            RightLeftShin.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor"));
            RightRightShin.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor"));
            RightLeftFoot.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor"));
            RightRightFoot.GetComponent<SpriteRenderer>().color = ColorsScript.GetColorForScript(DataController.GetValue<int>("BodyColor"));

            #region LoadPlayerInfoBtnSprites

            GameObject LeftInfo = GameObject.FindGameObjectsWithTag("InfoBtn")[0];

            SpriteRenderer[] SpriteList = LeftInfo.GetComponentsInChildren<SpriteRenderer>();

            SpriteList[0].sprite = LeftHead.GetComponent<SpriteRenderer>().sprite;

            SpriteList[0].color = LeftHead.GetComponent<SpriteRenderer>().color;

            SpriteList[2].sprite = LeftHair.GetComponent<SpriteRenderer>().sprite;

            SpriteList[2].color = LeftHair.GetComponent<SpriteRenderer>().color;

            SpriteList[3].sprite = LeftBeard.GetComponent<SpriteRenderer>().sprite;

            SpriteList[3].color = LeftBeard.GetComponent<SpriteRenderer>().color;

            GameObject RightInfo = GameObject.FindGameObjectsWithTag("InfoBtn")[1];

            SpriteList = RightInfo.GetComponentsInChildren<SpriteRenderer>();

            SpriteList[0].sprite = RightHead.GetComponent<SpriteRenderer>().sprite;

            SpriteList[0].color = RightHead.GetComponent<SpriteRenderer>().color;

            SpriteList[2].sprite = RightHair.GetComponent<SpriteRenderer>().sprite;

            SpriteList[2].color = RightHair.GetComponent<SpriteRenderer>().color;

            SpriteList[3].sprite = RightBeard.GetComponent<SpriteRenderer>().sprite;

            SpriteList[3].color = RightBeard.GetComponent<SpriteRenderer>().color;

            #endregion
        }
    }
}
