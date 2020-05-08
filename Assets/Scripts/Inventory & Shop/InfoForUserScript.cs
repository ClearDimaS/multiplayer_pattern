
using UnityEngine;
using UnityEngine.UI;

public class InfoForUserScript : MonoBehaviour
{
    public GameObject InfWindow;
    public Text InfoText;

    GameObject PanelsParent;
    public void ShowOrHideInfoWindow() 
    {
        PanelsParent = GameObject.FindGameObjectWithTag("StatsPanelParent");

        if (InfoText.name.Contains("specials"))
        {
            foreach (Image PanelImage in PanelsParent.GetComponentsInChildren<Image>())
            {
                if (PanelImage.gameObject.name.Contains("Panel"))
                {
                    PanelImage.gameObject.SetActive(false);
                }
            }

            InfWindow.SetActive(!InfWindow.activeSelf);

            InfoText.text = LocalisationSystem.GetLocalisedValue("armour") + " : " + LocalisationSystem.GetLocalisedValue("armor_info") + "\n" +
                LocalisationSystem.GetLocalisedValue("just_bash") + " : " + LocalisationSystem.GetLocalisedValue("bash_info") + "\n" +
                LocalisationSystem.GetLocalisedValue("just_miss") + " : " + LocalisationSystem.GetLocalisedValue("miss_info") + "\n" +
                LocalisationSystem.GetLocalisedValue("just_crit") + " : " + LocalisationSystem.GetLocalisedValue("crit_info") + "\n" +
                LocalisationSystem.GetLocalisedValue("Damage") + " : " + LocalisationSystem.GetLocalisedValue("damage_info") + "\n" +
                LocalisationSystem.GetLocalisedValue("just_stun") + " : " + LocalisationSystem.GetLocalisedValue("stun_info") + "\n";
        }
        else 
        {
            if (InfWindow.activeSelf)
            {
                InfWindow.SetActive(false);
            }
            else
            {
                foreach (Image PanelImage in PanelsParent.GetComponentsInChildren<Image>())
                {
                    if (PanelImage.gameObject.name.Contains("Panel"))
                    {
                        PanelImage.gameObject.SetActive(false);
                    }
                }
                InfWindow.SetActive(!InfWindow.activeSelf);
                InfoText.text = LocalisationSystem.GetLocalisedValue(InfoText.name) + "\n" + "\n" + LocalisationSystem.GetLocalisedValue(InfoText.name + "_info");
            }
        }        
    }
}
