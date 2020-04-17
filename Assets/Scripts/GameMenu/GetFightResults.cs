
using System.Collections.Generic;
using System.Linq;
using DataController = GameSparksTutorials.DataController;
using GameObject = UnityEngine.GameObject;
using MonoBehaviour = UnityEngine.MonoBehaviour;
using Text = UnityEngine.UI.Text;

public class GetFightResults : MonoBehaviour
{
    private List<Text> TxtOutPut;
    public GameObject loadingscreen;

    void Start()
    {
        Invoke("Changes", 1);
    }
    private void Changes()
    {
        TxtOutPut = GetComponents<Text>().ToList();
        foreach (Text text in TxtOutPut)
        {
            if (text.name == "ExpGained")
            {
                text.text = "Expirience: +" + DataController.GetValue<int>("expAdded");
            }

            if (text.name == "BreadGained")
            {
                text.text = "Bread: +" + DataController.GetValue<int>("breadAdded");
                if (loadingscreen != null)
                {
                    loadingscreen.SetActive(false);
                }
            }

            string SymbolPlusOrMinus;

            if (DataController.GetValue<int>("RatingChange") > 0)
            {
                SymbolPlusOrMinus = "+";
            }
            else 
            {
                SymbolPlusOrMinus = "-";
            }

            if (text.name == "RatingChange")
            {
                text.text = "Raiting: " + SymbolPlusOrMinus + " " + DataController.GetValue<int>("RatingChange");
            }
        }
    }
}
