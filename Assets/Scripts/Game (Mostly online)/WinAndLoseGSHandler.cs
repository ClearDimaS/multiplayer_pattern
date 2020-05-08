
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
        int RatingReward = 0;

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

        DataController.SaveValue("Rating", DataController.GetValue<int>("Rating") + RatingReward / 2);

        int expChange = 0;

        expChange = winExp;

        DataController.SaveValue("expAdded", expChange);

        int breadChange = 0;

        breadChange = winBread * multDifference;

        DataController.SaveValue("breadAdded", breadChange / 2);

        DataController.SaveValue("Exp", DataController.GetValue<int>("Exp") + expChange / 2);

        DataController.SaveValue("Bread", DataController.GetValue<int>("Bread") + breadChange / 2);

        PostGameActions(true);
    }


    static int RaitingSingleReward = 2;


    public static void WinSingle()
    {
        DataController.SaveValue("Rating", DataController.GetValue<int>("Rating") + RaitingSingleReward);

        if (DataController.GetValue<int>("LoadMode") == 2) 
        {
            winExp = 200;

            winBread = 100;

            DataController.SaveValue("CurrentBossNumber", 1 + DataController.GetValue<int>("CurrentBossNumber"));
        }

        DataController.SaveValue("RatingChange", RaitingSingleReward);

        int expChange = 0;

        expChange = winExp * multDifference;

        DataController.SaveValue("expAdded", expChange);

        int breadChange = 0;

        breadChange = winBread;

        DataController.SaveValue("breadAdded", breadChange);

        DataController.SaveValue("Exp", DataController.GetValue<int>("Exp") + expChange);

        DataController.SaveValue("Bread", DataController.GetValue<int>("Bread") + breadChange);

        PostGameActions(true);
    }


    public static void LoseOnline()
    {
        int RatingReward = 0;

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

        DataController.SaveValue("RatingChange", -RatingReward / 2);

        DataController.SaveValue("Rating", DataController.GetValue<int>("Rating") - RatingReward / 2);

        int expChange = 0;

        expChange = loseExp;

        DataController.SaveValue("expAdded", expChange);

        int breadChange = 0;

        breadChange = loseBread * multDifference;

        DataController.SaveValue("breadAdded", breadChange);

        DataController.SaveValue("Exp", DataController.GetValue<int>("Exp") + expChange / 2);

        DataController.SaveValue("Bread", DataController.GetValue<int>("Bread") + breadChange / 2);

        PostGameActions(false);
    }


    public static void LoseSingle()
    {
        DataController.SaveValue("RatingChange", -RaitingSingleReward);

        DataController.SaveValue("Rating", DataController.GetValue<int>("Rating") - RaitingSingleReward);

        int expChange = 0 ;

        expChange = loseExp * multDifference;

        DataController.SaveValue("expAdded", expChange);

        int breadChange = 0;

        breadChange = loseBread;

        DataController.SaveValue("breadAdded", breadChange);

        DataController.SaveValue("Exp", DataController.GetValue<int>("Exp") + expChange);

        DataController.SaveValue("Bread", DataController.GetValue<int>("Bread") + breadChange);

        PostGameActions(false);
    }

    public static void PostGameActions(bool ifWin) 
    {
        DataController.SaveValue("GSNotSynced" + DataController.GetValue<string>("username"), 2);

        if (ifWin)
        {
            DataController.SaveValue("WinsForChest", DataController.GetValue<int>("WinsForChest") + 1);
        }
    }


}