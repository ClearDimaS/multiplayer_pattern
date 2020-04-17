
using UnityEngine;
using UnityEngine.UI;

public class InfoForUserScript : MonoBehaviour
{
    public GameObject InfWindow;
    public Text InfoText;

    public void ShowOrHideInfoWindow() 
    {
        InfWindow.SetActive(!InfWindow.activeSelf);
        InfoText.text  = "Hi :)";
    }
}
