
using UnityEngine;
using UnityEngine.UI;

namespace GameSparksTutorials
{
    public class Login : MonoBehaviour
    {
        public GameObject Warning;
        public int RememberMe ;
        [SerializeField]
        private InputField username;

        [SerializeField]
        private InputField password;

        private void Start()
        {
            RememberMe = DataController.GetValue<int>("RememberMe");
            if (RememberMe > 0)
            {
                username.text = DataController.GetValue<string>("username");
                password.text = DataController.GetValue<string>("password");
            }
            else 
            {
                DataController.DeleteValue("username");
                DataController.DeleteValue("password");
            }

        }

        public void UserLogin()
        {
            EventManager.StartListening<string>("OnLoginResponse", OnLoginResponse);
            DataController.SaveValue("username", username.text);
            DataController.SaveValue("password", password.text);
            GS_Authentication.Login(username.text, password.text, "OnLoginResponse");
        }

        public void GuestLogin()
        {
            Warning.SetActive(true);

            EventManager.StartListening<string>("OnGuestLoginResponse", OnGuestLoginResponse);

            GS_Authentication.DeviceAuthentication("OnGuestLoginResponse");
        }

        public void SignUp()
        {
            UIController.SetActivePanel(UI_Element.SignUp);
        }

        private void OnLoginResponse(string displayName)
        {
            if (displayName.Length > 0)
            {
                DataController.SaveValue("displayName", displayName);

                UIController.SetActivePanel(UI_Element.MainMenu);
            } else
            {
                Debug.Log("Error OnLoginResponse");
            }

            EventManager.StopListening<string>("OnLoginResponse", OnLoginResponse);
            //if (displayName.Length > 0)
            //{
            //    LoadScene.SceneLoaderForScript(1);
            //}
        }
        private void OnGuestLoginResponse(string displayName)
        {
            if (displayName.Length > 0)
            {
                OnLoginResponse(displayName);

                PopUpMessage.ActivatePopUp(delegate { UIController.SetActivePanel(UI_Element.SignUp); }, LocalisationSystem.GetLocalisedValue("guest_warning"));
            } else 
            {
                Debug.Log("Error OnGuestLoginResponse");
            }

            EventManager.StopListening<string>("OnGuestLoginResponse", OnGuestLoginResponse);
        }
    }
}
