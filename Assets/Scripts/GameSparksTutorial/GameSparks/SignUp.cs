
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
                ErrorOutPut("Invalid Email");
            }
            else
            {
                if (username.text.Length < 4 || !CreateRGX(username.text).IsMatch(username.text))
                {
                    ErrorOutPut("Username should contain more than 4 symbols: a-z or A-Z or numbers");
                }
                else 
                {
                    if (displayName.text.Length < 4 || !CreateRGX(displayName.text).IsMatch(displayName.text))
                    {
                        ErrorOutPut("Displayname should contain more than 4 symbols: a-z or A-Z or numbers");
                    }
                    else 
                    {
                        if (password.text.Length < 8 || !CreateRGX(password.text).IsMatch(password.text))
                        {
                            ErrorOutPut("Password should be at least 8 letters or numbers");
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
                ErrorOutPut("Username is busy or a Network error has occured");
            }
        }
    }
}

