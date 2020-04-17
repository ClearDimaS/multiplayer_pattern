
using MonoBehaviour = UnityEngine.MonoBehaviour;
using GameObject = UnityEngine.GameObject;
using UIController = GameSparksTutorials.UIController;
using UI_Element = GameSparksTutorials.UI_Element;


public class btnBackScript : MonoBehaviour
{
    public GameObject Warning;
    public void GoBack()
    {
        UIController.SetActivePanel(UI_Element.Login);
        Warning.SetActive(false);
    }
}
