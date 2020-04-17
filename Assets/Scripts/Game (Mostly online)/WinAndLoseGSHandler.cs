
using MonoBehaviour = UnityEngine.MonoBehaviour;
using DataController = GameSparksTutorials.DataController;
using UnityEngine;

public class WinAndLoseHandler : MonoBehaviour
{

    public static int winExp = 29;

    public static int winBread = 13;

    public static int loseExp = 11;

    public static int loseBread = 5;


    public static int multDifference = 4;
    public static void WinOnline()
    {
        int RatingReward;

        RatingReward = (DataController.GetValue<int>("Rating") - DataController.GetValue<int>("RatingOther")) / 4;

        if (RatingReward >= 0)
        {
            RatingReward += 4;

            RatingReward = Mathf.Min(RatingReward, 25);
        }
        else 
        {
            RatingReward -= 4;

            RatingReward = Mathf.Min(Mathf.Abs(RatingReward), 10);
        }

        DataController.SaveValue("RatingChange", RatingReward);

        DataController.SaveValue("Rating", DataController.GetValue<int>("Rating") + RatingReward);

        DataController.SaveValue("Exp", DataController.GetValue<int>("Exp") + winExp);

        DataController.SaveValue("Bread", DataController.GetValue<int>("Bread") + winBread * multDifference);

        Debug.Log("You have just won Online and recieved some exp and bread ");

        DataController.SaveValue("GSNotSynced" + DataController.GetValue<string>("username"), 1) ;
    }


    static int RaitingSingleReward = 2;


    public static void WinSingle()
    {
        DataController.SaveValue("Rating", DataController.GetValue<int>("Rating") + RaitingSingleReward);

        if (DataController.GetValue<int>("LoadMode") == 2) 
        {
            DataController.SaveValue("CurrentBossNumber", 1 + DataController.GetValue<int>("CurrentBossNumber"));
        }

        DataController.SaveValue("RatingChange", RaitingSingleReward);

        DataController.SaveValue("Exp", DataController.GetValue<int>("Exp") + winExp * multDifference);

        DataController.SaveValue("Bread", DataController.GetValue<int>("Bread") + winBread);

        Debug.Log("You have just won Single and recieved some exp and bread ");

        DataController.SaveValue("GSNotSynced" + DataController.GetValue<string>("username"), 1);
    }


    public static void LoseOnline()
    {
        int RatingReward;

        RatingReward = (DataController.GetValue<int>("Rating") - DataController.GetValue<int>("RatingOther")) / 4;

        if (RatingReward >= 0)
        {
            RatingReward += 4;

            RatingReward = Mathf.Min(RatingReward, 25);
        }
        else
        {
            RatingReward -= 4;

            RatingReward = Mathf.Min(Mathf.Abs(RatingReward), 10);
        }

        DataController.SaveValue("RatingChange", -RatingReward);

        DataController.SaveValue("Rating", DataController.GetValue<int>("Rating") - RatingReward);

        DataController.SaveValue("Exp", DataController.GetValue<int>("Exp") + loseExp);

        DataController.SaveValue("Bread", DataController.GetValue<int>("Bread") + loseBread * multDifference);

        Debug.Log("You have just lost Online and recieved some exp and bread ");

        DataController.SaveValue("GSNotSynced" + DataController.GetValue<string>("username"), 1);
    }


    public static void LoseSingle()
    {
        DataController.SaveValue("RatingChange", -RaitingSingleReward);

        DataController.SaveValue("Rating", DataController.GetValue<int>("Rating") - RaitingSingleReward);

        DataController.SaveValue("Exp", DataController.GetValue<int>("Exp") + loseExp * multDifference);

        DataController.SaveValue("Bread", DataController.GetValue<int>("Bread") + loseBread);

        Debug.Log("You have just lost Single and recieved some exp and bread ");

        DataController.SaveValue("GSNotSynced" + DataController.GetValue<string>("username"), 1);
    }
}