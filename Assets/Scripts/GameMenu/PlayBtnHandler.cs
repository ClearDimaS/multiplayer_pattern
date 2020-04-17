
using MonoBehaviour = UnityEngine.MonoBehaviour;
using GameObject = UnityEngine.GameObject;

public class PlayBtnHandler : MonoBehaviour
{
    public GameObject GameChoosePanel;
    public void HideOrShowPanel() 
    {
        GameChoosePanel.SetActive(!GameChoosePanel.activeSelf);
    }
}
