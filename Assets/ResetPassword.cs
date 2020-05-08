using UnityEngine;
using UnityEngine.UI;

using GameSparks.Api.Requests;
using GameSparks.Core;

namespace GameSparksTutorials 
{
    public class ResetPassword : MonoBehaviour
    {
        public static ResetPassword instance;

        private void Awake()
        {
            if (instance == null) 
            {
                instance = this;
            }
        }

        public GameObject Warning;

        [SerializeField]
        private InputField email;

        [SerializeField]
        private InputField password;

        [SerializeField]
        private InputField username;

        [SerializeField]
        private InputField token;

        public void GetToken()
        {
            DataController.SaveValue("username", username.text);

            var loginRequest = new AuthenticationRequest();

            GameSparks.Core.GSRequestData data = new GameSparks.Core.GSRequestData();
            
            data.AddString("action", "passwordRecoveryRequest");

            data.AddString("email", email.text);

            loginRequest.SetScriptData(data);

            loginRequest.SetUserName(DataController.GetValue<string>("username"));

            loginRequest.SetPassword("");

            loginRequest.Send(response => {
                if (!response.HasErrors)
                {
                    Warning.SetActive(true);

                    Warning.GetComponentsInChildren<Button>()[1].gameObject.SetActive(false);

                    PopUpMessage.ActivatePopUp(delegate { UIController.SetActivePanel(UI_Element.ConfirmResetPassword); }, LocalisationSystem.GetLocalisedValue("resetpasswordwarning1"));
                }
                else
                {
                    Debug.Log("Error authenticating player.../n" + response.Errors.JSON.ToString());

                    Debug.Log(response.Errors.GetString("action"));

                    if (response.Errors.ContainsKey("action")) 
                    {
                        Warning.SetActive(true);

                        //Warning.GetComponentsInChildren<Button>()[1].gameObject.SetActive(false);

                        if (response.Errors.GetString("action").Contains("complete"))
                        {
                            Debug.Log("warn1");

                            UIController.SetActivePanel(UI_Element.ConfirmResetPassword);

                            PopUpMessage.ActivatePopUp(delegate { UIController.SetActivePanel(UI_Element.ConfirmResetPassword); }, LocalisationSystem.GetLocalisedValue("resetpasswordwarning1"));
                        }
                        else if (response.Errors.GetString("action").Contains("email") || response.Errors.GetString("action").Contains("invalid"))
                        {
                            Debug.Log("warn2");

                            PopUpMessage.ActivatePopUp(delegate { UIController.SetActivePanel(UI_Element.ResetPassword); }, LocalisationSystem.GetLocalisedValue("resetpasswordwarning2"));
                        }
                        else 
                        {
                            Debug.Log("exception");
                        }
                    }
                }
            });
        }

        public void NewPasswordLogin()
        {
            DataController.SaveValue("password", password.text);

            Debug.Log(password.text);

            var loginRequest = new AuthenticationRequest();

            GameSparks.Core.GSRequestData data = new GameSparks.Core.GSRequestData();

            data.AddString("password", DataController.GetValue<string>("password"));

            data.AddString("action", "resetPassword");

            data.AddString("token", token.text);

            loginRequest.SetScriptData(data);

            loginRequest.SetUserName(DataController.GetValue<string>("username"));

            loginRequest.SetPassword(password.text);

            loginRequest.Send(response =>
            {
                if (response.Errors.GetString("action").Contains("complete"))
                {
                    Warning.SetActive(true);

                    PopUpMessage.ActivatePopUp(delegate {  }, LocalisationSystem.GetLocalisedValue("resetpasswordwarning5"));
                    
                    UIController.SetActivePanel(UI_Element.Login);
                }

                if (response.Errors.GetString("action").Contains("invalid"))
                {
                    Warning.SetActive(true);

                    PopUpMessage.ActivatePopUp(delegate { }, LocalisationSystem.GetLocalisedValue("resetpasswordwarning4") + LocalisationSystem.GetLocalisedValue("password_error"));

                    UIController.SetActivePanel(UI_Element.ConfirmResetPassword);
                }


                Debug.Log("well done " + response.Errors.JSON.ToString());

                if (!response.HasErrors)
                {
                    Debug.Log("Error reseting password.../n" + response.Errors.JSON.ToString());
                }
            });
        }

        private void OnNewPassworLoginResponse(string displayName)
        {
            if (displayName.Length > 0)
            {
                DataController.SaveValue("displayName", displayName);

                UIController.SetActivePanel(UI_Element.MainMenu);
            }
            else
            {
                Debug.Log("Error OnNewPasswordLoginResponse");
            }

            EventManager.StopListening<string>("OnNewPasswordLoginResponse", OnNewPassworLoginResponse);
            //if (displayName.Length > 0)
            //{
            //    LoadScene.SceneLoaderForScript(1);
            //}
        }
    }
}


