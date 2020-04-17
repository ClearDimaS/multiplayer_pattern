
using System.Collections.Generic;
using System.Linq;
using DataController = GameSparksTutorials.DataController;
using MonoBehaviour = UnityEngine.MonoBehaviour;
using Text = UnityEngine.UI.Text;

public class UpdateExp : MonoBehaviour
{
    public List<Text> StartMenuTexts;

    void Start()
    {
        Invoke("updateExp", 0.2f);
    }

    private void updateExp()
    {
        StartMenuTexts = GetComponents<Text>().ToList();
        foreach (Text text in StartMenuTexts)
        {
            if (text.name == "ExpBar")
            {
                text.text = "Exp: " + (DataController.GetValue<int>("currentExp") % 100).ToString() + " / 100";
            }
        }
    }
}

