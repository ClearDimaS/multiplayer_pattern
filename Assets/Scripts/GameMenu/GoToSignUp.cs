
using MonoBehaviour =  UnityEngine.MonoBehaviour;

namespace GameSparksTutorials
{
    public class GoToSignUp : MonoBehaviour
    {
        public void GoSignUp()
        {
            UIController.SetActivePanel(UI_Element.SignUp);
        }
    }
}
