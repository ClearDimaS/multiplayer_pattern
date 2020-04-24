using UnityEngine;
using System.Collections.Generic;

// GameSparks
using GameSparks.Api.Requests;
using GameSparks.Core;

namespace GameSparksTutorials
{
    public enum ESignUpResponse 
    {
        EMAILISTAKEN, USERNAMEISTAKEN, SUCCESS, ERROR
    }

    public class GS_Authentication : GS_Base
    {
        private static List<string> ServerNamz = new List<string> { "StatsAttack", "StatsAgility", "StatsPower", "StatsStrength", "StatsEndurance", "StatsSpeed", "StatsSleep", "StatsRegen"};
        static List<string> EqModifiers = Equipment.EquipmentModifiers;
        private static List<string> EquipmentNames = ForEZEdit.EquipmentNames;

        static List<long> StatsList = new List<long>();
        static List<long> EquipmentList = new List<long>();
        static List<long> OtherStuffList = new List<long>();

        static List<long> PrimaryStuffList = new List<long>();
        public static bool IsUserLoggedIn { get; private set; }
        static int mod = 0;
        /// <summary>
        /// User Login
        /// </summary>>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="eventName"></param>
        public static void Login(string username, string password, string eventName) 
        {
            if (DataController.GetValue<string>("LastValidusername") != username) 
            {
                Login(DataController.GetValue<string>("LastValidusername"), DataController.GetValue<string>("LastValidPassword"), null);
            } 

            DataController.SaveValue("username", username);

            StatsList = new List<long>();

            EquipmentList = new List<long>();

            OtherStuffList = new List<long>();

            StatsList = new List<long>();

            EquipmentList = new List<long>();

            PrimaryStuffList = new List<long>();

            if (DataController.GetValue<int>("Rating") >= 0) 
            {
                var newRequest = new GameSparks.Api.Requests.LogEventRequest();// DataController.GetValue<int>("Rating"));

                newRequest.SetEventKey("RATING_UPDATE").SetEventAttribute("Rating", DataController.GetValue<int>("Rating")).Send(response =>
                {

                });
            }

            var newRequest1 = new GameSparks.Api.Requests.LeaderboardDataRequest();

            newRequest1.SetLeaderboardShortCode("LeaderboardRating").SetEntryCount(35).Send(response =>
            {
                Debug.Log(response.BaseData.JSON);

                LeaderBoardsScript.Ranks = new List<long?>();

                LeaderBoardsScript.Names = new List<string>();

                LeaderBoardsScript.Ratings = new List<long?>();

                foreach (var gd in response.BaseData.GetGSDataList("data")) 
                {
                    LeaderBoardsScript.Ranks.Add(gd.GetLong("rank"));

                    LeaderBoardsScript.Names.Add(gd.GetString("userName"));

                    LeaderBoardsScript.Ratings.Add(gd.GetLong("Rating"));

                    Debug.Log(gd.GetString("userName"));

                    Debug.Log(gd.GetLong("Rating"));
                }
            });

            Debug.Log("Authentication...");
            var loginRequest = new AuthenticationRequest();


            GameSparks.Core.GSRequestData data = new GameSparks.Core.GSRequestData();
            if (DataController.GetValue<int>("GSNotSynced" + username) > 0 && DataController.GetValue<string>("LastValidusername") == username)
            {
                foreach (string attr in ServerNamz)
                {
                    Debug.Log(DataController.GetValue<int>(attr + "Mine"));
                    StatsList.Add((long)DataController.GetValue<int>(attr + "Mine"));
                }
                data.AddNumberList("Stats", StatsList);

                foreach (string TypeItem in Equipment.ForInvLoad)
                {
                    foreach (string Name in EquipmentNames)
                    {
                        EquipmentList.Add((long)DataController.GetValue<int>(Name + TypeItem + "ammount"));
                    }
                }
                data.AddNumberList("Equipment", EquipmentList);

                int tempNum = 0;

                foreach (int num in CelebrationAnimation.Prices)
                {
                    Debug.Log(num);

                    if (DataController.GetValue<int>("WinAnimNumberMine" + tempNum + "ammount") > 0)
                    {
                        OtherStuffList.Add(1);
                    }
                    else
                    {
                        OtherStuffList.Add(0);
                    }
                    tempNum += 1;
                }

                PrimaryStuffList.Add(DataController.GetValue<int>("Exp"));

                PrimaryStuffList.Add(DataController.GetValue<int>("Bread"));

                PrimaryStuffList.Add(DataController.GetValue<int>("SkillPoints"));

                PrimaryStuffList.Add(DataController.GetValue<int>("Rating"));

                Debug.Log(DataController.GetValue<int>("SkillPoints"));

                data.AddNumberList("OtherStuffList", OtherStuffList);

                data.AddNumberList("PrimaryStuffList", PrimaryStuffList);
            }

            loginRequest.SetUserName(username);
            loginRequest.SetPassword(password);
            loginRequest.SetScriptData(data);


            loginRequest.Send(response =>
            {
                if (!response.HasErrors) 
                {
                    GameSparks.Core.GSData GSList = response.ScriptData;
                    foreach (string atributeName in ServerNamz)
                    {
                        DataController.SaveValue(atributeName + "Mine", (int)GSList.GetInt(atributeName));
                    }
                    foreach (string TypeItem in Equipment.ForInvLoad)
                    {
                        foreach (string Name in EquipmentNames)
                        {
                            if (GSList.GetGSData("BoughtOrNot").ContainsKey(Name + TypeItem))
                            {
                                if (GSList.GetGSData("BoughtOrNot").GetInt(Name + TypeItem) > 0)
                                {
                                    DataController.SaveValue(Name + TypeItem + "ammount", 1);
                                }
                                else 
                                {
                                    DataController.SaveValue(Name + TypeItem + "ammount", 0);
                                }
                            }
                        }
                    }
                    foreach (var vg in GSList.GetGSDataList("VirtualGoodsList"))
                    {
                        foreach (string modifier in EqModifiers)
                        {
                            if (vg.GetGSData("currencyCosts").GetInt(modifier) != null)
                            {
                                DataController.SaveValue(vg.GetString("name") + modifier, (int)vg.GetGSData("currencyCosts").GetInt(modifier));
                                //Debug.Log(vg.GetString("name"));
                            }
                        }
                        DataController.SaveValue(vg.GetString("name") + "Price", (int)vg.GetGSData("currencyCosts").GetInt("Bread"));
                        DataController.SaveValue(vg.GetString("name") + "SellPrice", (int)vg.GetGSData("currencyCosts").GetInt("BreadPrice"));
                    }

                    int tempNum = 0;
                    long? value = GSList.GetLong("Anim" + tempNum);
                    while (value != null)
                    {
                        DataController.SaveValue("WinAnimNumberMine" + tempNum + "ammount", (int)value);
                        tempNum += 1;
                        value = GSList.GetLong("Anim" + tempNum);
                    }

                    for (int i = tempNum; i < CelebrationAnimation.Prices.Count; i++) 
                    {
                        DataController.SaveValue("WinAnimNumberMine" + i + "ammount", 0);
                    }

                    long? Exp = GSList.GetLong("TotalExp");
                    DataController.SaveValue("Exp", (int)Exp);

                    long? Bread = GSList.GetLong("TotalBread");
                    DataController.SaveValue("Bread", (int)Bread);

                    long? SkillPoints = GSList.GetLong("TotalSkillPoints");
                    DataController.SaveValue("SkillPoints", (int)SkillPoints);

                    long? Rating = GSList.GetLong("Rating");
                    DataController.SaveValue("Rating", (int)Rating);

                    Debug.Log(DataController.GetValue<int>("SkillPoints"));


                    DataController.SaveValue("GSNotSynced" + username, 0);
                    Debug.Log("Player authenticated! \n Name:" + response.DisplayName + response.ScriptData.JSON);// + response.ScriptData);//.ScriptData.JSON.ToString());

                    EventManager.TriggerEvent(eventName, response.DisplayName);

                    IsUserLoggedIn = true;

                    DataController.SaveValue("LastValidPassword" + username, password);

                    DataController.SaveValue("LastValidusername", username);
                } else 
                {
                    Debug.Log("Error authenticating player.../n" + response.Errors.JSON.ToString());

                    EventManager.TriggerEvent(eventName, "");
                }
            });

        }
        /// <summary>
        /// Log the user in as a guest.
        /// </summary>
        /// <param name="eventName"></param> The event that will be called after the device authentication response
        public static void DeviceAuthentication(string eventName)
        {
            Debug.Log("Device authentication...");

            Login(DataController.GetValue<string>("username"), DataController.GetValue<string>("LastValidPassword" + DataController.GetValue<string>("username")), null);

            //DataController.GetValue<string>("LastValidPassword" + DataController.GetValue<string>("username"));

            //DataController.SaveValue("username", "");

            var deviceAuthenticationRequest = new DeviceAuthenticationRequest();

            deviceAuthenticationRequest.Send(response =>
            {
                if (!response.HasErrors)
                {
                    GameSparks.Core.GSData GSList = response.ScriptData;

                    foreach (var vg in GSList.GetGSDataList("VirtualGoodsList"))
                    {
                        foreach (string modifier in EqModifiers)
                        {
                            if (vg.GetGSData("currencyCosts").GetInt(modifier) != null)
                            {
                                DataController.SaveValue(vg.GetString("name") + modifier, (int)vg.GetGSData("currencyCosts").GetInt(modifier));
                            }
                        }
                        DataController.SaveValue(vg.GetString("name") + "Price", (int)vg.GetGSData("currencyCosts").GetInt("Bread"));
                        DataController.SaveValue(vg.GetString("name") + "SellPrice", (int)vg.GetGSData("currencyCosts").GetInt("BreadPrice"));
                    }
                    foreach (string atributeName in ServerNamz)
                    {
                        DataController.SaveValue(atributeName + "Mine", 0);
                    }
                    foreach (string TypeItem in Equipment.ForInvLoad)
                    {
                        foreach (string Name in EquipmentNames)
                        {
                            DataController.SaveValue(Name + TypeItem + "ammount", 0);
                        }
                    }
                    Debug.Log( "Player autheticated!\nName: " + response.DisplayName);

                    EventManager.TriggerEvent(eventName, response.DisplayName);

                    IsUserLoggedIn = true;

                    DataController.SaveValue("GSNotSynced" + DataController.GetValue<string>("username"), 0);

                    DataController.SaveValue("Exp", 0);

                    DataController.SaveValue("Bread", 0);

                    DataController.SaveValue("SkillPoints", 0);

                    DataController.SaveValue("Rating", 0);

                    for (int i = 0; i < CelebrationAnimation.Prices.Count; i++) 
                    {
                        DataController.SaveValue("WinAnimNumberMine" + i + "ammount", 0);
                    }
                }
                else
                {
                    Debug.Log("Error authenticating player... \n: " + response.Errors.JSON.ToString());

                    EventManager.TriggerEvent(eventName, "");
                }
            });
        }

        /// <summary>
        /// Sign up a new player.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="displayName"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="eventName"></param>
        public static void SignUp(string username, string displayName, string password, string email, string eventName) 
        {
            Debug.Log("Sign up...");

            if (!IsUserLoggedIn)
            {
                var signUpRequest = new RegistrationRequest();

                signUpRequest.SetUserName(username);
                signUpRequest.SetDisplayName(displayName);
                signUpRequest.SetPassword(password);
                signUpRequest.SetSegments(new GSRequestData().AddString("email", email));

                signUpRequest.Send(response =>
                {
                    if (!response.HasErrors)
                    {
                        Debug.Log("Player registration successful!\n Name: " + response.DisplayName);

                        EventManager.TriggerEvent(eventName, response.DisplayName);
                    }
                    else
                    {
                        Debug.Log("The username is already in use!! " + response.DisplayName);

                        EventManager.TriggerEvent(eventName, response.DisplayName);
                    }
                });
            }
            else 
            {
                UpgradeGuestUser(username, displayName, password, email, eventName);
            }
        }

        private static void UpgradeGuestUser(string username, string displayName, string password, string email, string eventName)
        {
            Debug.Log("Upgrade guest user...");

            var upgradeRequest = new LogEventRequest_upgradeGuestAccount();

            upgradeRequest.Set_username(username);
            upgradeRequest.Set_displayName(displayName);
            upgradeRequest.Set_password(password);
            upgradeRequest.Set_email(email);

            upgradeRequest.Send(response =>
            {
                if (!response.HasErrors)
                {
                    Debug.Log("Guest upgrade successful!");

                    EventManager.TriggerEvent(eventName, displayName);
                }
                else
                {
                    Debug.Log("Error upgrading player... \n: " + response.Errors.JSON.ToString());

                    EventManager.TriggerEvent(eventName, "");
                }
            });
        }
    }
}