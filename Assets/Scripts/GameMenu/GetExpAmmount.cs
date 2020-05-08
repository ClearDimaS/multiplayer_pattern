
using UnityEngine;
using UnityEngine.UI;
using GameSparksTutorials;

public class GetExpAmmount : MonoBehaviour
{
    private Text Lvl;

    private void Start()
    {
        Lvl = GetComponent<Text>();
        Lvl.text = ((int)(DataController.GetValue<int>("Exp") / 100)).ToString();
        if ((DataController.GetValue<int>("Exp") / 100 > 60))
        {
            Lvl.text = "60";
            DataController.SaveValue("Exp", 6099);
        }else
        if ((DataController.GetValue<int>("Exp") / 100) > DataController.GetValue<int>("Lvl"))
        {
            DataController.SaveValue("SkillPoints", 4 * ((int)(DataController.GetValue<int>("Exp") / 100) - DataController.GetValue<int>("Lvl")) + DataController.GetValue<int>("SkillPoints"));
            DataController.SaveValue("Lvl", DataController.GetValue<int>("Exp") / 100);
        }
    }
}