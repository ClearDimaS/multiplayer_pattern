
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

namespace GameSparksTutorials
{
    public class SignUp : MonoBehaviour
    {

        private Text[] AllTexts;

        [SerializeField]
        InputField username;

        [SerializeField]
        InputField displayName;

        [SerializeField]
        InputField password;

        [SerializeField]
        InputField email;

        private void ErrorOutPut(string TheOutPut) 
        {
            AllTexts = SignUp.FindObjectsOfType<Text>();
            foreach (Text text in AllTexts)
            {
                if (text.name == "ErrorTxt")
                {
                    text.text = TheOutPut;
                }
            }
        }

        private Regex CreateRGX(string TheText) 
        {
            return new Regex(@"^[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789]{" + TheText.Length.ToString() + "}$");
        }

        public void UserSignUp()
        {
            if (!IsValidEmail.IsValidmail(email.text))
            {
                ErrorOutPut(LocalisationSystem.GetLocalisedValue("email_error"));
            }
            else
            {
                if (username.text.Length < 4 || !CreateRGX(username.text).IsMatch(username.text))
                {
                    ErrorOutPut(LocalisationSystem.GetLocalisedValue("username_error"));
                }
                else 
                {
                    if (displayName.text.Length < 4 || !CreateRGX(displayName.text).IsMatch(displayName.text))
                    {
                        ErrorOutPut(LocalisationSystem.GetLocalisedValue("displayname_error"));
                    }
                    else 
                    {
                        if (password.text.Length < 8 || !CreateRGX(password.text).IsMatch(password.text))
                        {
                            ErrorOutPut(LocalisationSystem.GetLocalisedValue("password_error"));
                        }
                        else 
                        {
                            ErrorOutPut("");
                            EventManager.StartListening<string>("OnSignUpResponse", OnSignUpResponse);

                            GS_Authentication.SignUp(username.text, displayName.text, password.text, email.text, "OnSignUpResponse");
                        }
                    }
                }
            }
        }


        private void OnSignUpResponse(string displayName)
        {
            if (displayName != null)
            {
                DataController.SaveValue("displayName", displayName);

                UIController.SetActivePanel(UI_Element.MainMenu);

                EventManager.StopListening<string>("OnSignUpResponse", OnSignUpResponse);
            }
            else 
            {
                ErrorOutPut(LocalisationSystem.GetLocalisedValue("signup_error"));
            }
        }
    }
}

