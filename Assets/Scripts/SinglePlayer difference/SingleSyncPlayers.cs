using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSparksTutorials;

public class SingleSyncPlayers : MonoBehaviour
{
    #region Singleton
    public static SingleSyncPlayers instance;
    bool IAmACopy = false;
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


    SingleEnemyHandler loadEnemy = new SingleEnemyHandler();


    void Start()
    {
        loadDone = false;

        DataUpdMine();
    }


    GameProcess player = new GameProcess();

    public Image timerBarLeft;

    public Image timerBarRight;

    public Text timerText;

    public Text timerText1;

    public List<GameObject> GameObjectsForInstantiation = new List<GameObject>();

    // Update is called once per frame

    bool loadDone;


    void Update()
    {

        if (!loadDone) 
        {
            Debug.Log("Loading recieved players data...");

            if (IAmACopy)
            {
                player.buttons.BtnLeave = GameObject.FindGameObjectsWithTag("BtnLeave")[1];

                player.buttons.BtnsPlace = GameObject.FindGameObjectsWithTag("BtnsPlace")[1];

                player.playerInterface = GameObject.FindGameObjectsWithTag("Interface")[1];

                player.playerInfoPanel = GameObject.FindGameObjectsWithTag("PlayerInfo")[1];

                player.playerLoadingScreen = GameObject.FindGameObjectsWithTag("LoadingScreen")[1];

                player.playerObj = GameObject.FindGameObjectsWithTag("Player")[1];

                player.iAmLeft = false;

                player.animation.SetAnimator(player.playerObj.GetComponentInChildren<Animator>());

                foreach (GameObject obj in GameObjectsForInstantiation)
                {
                    try
                    {
                        player.animation.dict.Add(obj.name, obj);
                    }
                    catch 
                    {
                    
                    }

                }

                SingleGameManager.instance.LoadPlayerBase(player, false);

                SingleGameManager.instance.SetEnemy(player);

                SingleGameManager.instance.SetSyncedCopy(true);
            }
            else
            {
                player.buttons.BtnLeave = GameObject.FindGameObjectsWithTag("BtnLeave")[0];

                player.buttons.BtnsPlace = GameObject.FindGameObjectsWithTag("BtnsPlace")[0];

                player.playerInterface = GameObject.FindGameObjectsWithTag("Interface")[0];

                player.playerInfoPanel = GameObject.FindGameObjectsWithTag("PlayerInfo")[0];

                player.playerLoadingScreen = GameObject.FindGameObjectsWithTag("LoadingScreen")[0];

                player.playerObj = GameObject.FindGameObjectsWithTag("Player")[0];

                player.iAmLeft = true;

                player.animation.SetAnimator(player.playerObj.GetComponentInChildren<Animator>());

                foreach (GameObject obj in GameObjectsForInstantiation)
                {
                    player.animation.dict.Add(obj.name, obj);
                }

                SingleGameManager.instance.LoadPlayerBase(player, true);

                SingleGameManager.instance.SetMe(player);

                SingleGameManager.instance.SetSynced(true);

                SetBtnObjects();

                SetTimerObjects();
            }

            LoadSprites();

            loadDone = true;
        }
    }

    void SetTimerObjects()
    {
        SingleGameManager.instance.playerTimeBarText = timerText;

        SingleGameManager.instance.playerTimeBarText1 = timerText1;

        SingleGameManager.instance.playerMeTimeBar = timerBarLeft;

        SingleGameManager.instance.playerEnemyTimeBar = timerBarRight;
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

    void DataUpdMine()
    {
        DataController.SaveValue("LvlMine", DataController.GetValue<int>("Exp") / 100);

        DataController.SaveValue("blockvar1Left", (int)(Time.time + 0) % 15);
        DataController.SaveValue("blockvar2Left", (int)(Time.time + 1) % 15);
        DataController.SaveValue("blockvar3Left", (int)(Time.time + 2) % 15);
        DataController.SaveValue("blockvar4Left", (int)(Time.time + 3) % 15);
        DataController.SaveValue("blockvar5Left", (int)(Time.time + 4) % 15);
        DataController.SaveValue("blockvar6Left", (int)(Time.time + 5) % 15);

        DataController.SaveValue("blockvar1Right", (int)(Time.time + 6) % 15);
        DataController.SaveValue("blockvar2Right", (int)(Time.time + 7) % 15);
        DataController.SaveValue("blockvar3Right", (int)(Time.time + 8) % 15);
        DataController.SaveValue("blockvar4Right", (int)(Time.time + 9) % 15);
        DataController.SaveValue("blockvar5Right", (int)(Time.time + 10) % 15);
        DataController.SaveValue("blockvar6Right", (int)(Time.time + 11) % 15);
    }

    void LoadSprites()
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
            LeftRightHand.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "RightHand" + "Mine"));
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
            RightRightHand.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataController.GetValue<string>("Equipped" + "RightHand" + "Other"));
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
}
