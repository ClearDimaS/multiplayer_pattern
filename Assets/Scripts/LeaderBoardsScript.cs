using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSparksTutorials;

public class LeaderBoardsScript : MonoBehaviour
{
    public static List<long?> Ranks;

    public static List<string> Names;

    public static List<long?> Ratings;


    public GameObject ParentRanks;

    public GameObject ParentNames;

    public GameObject ParentRatings;

    private void Start()
    {
        DispTexts();
    }
    public void DispTexts() 
    {
        Text[] RanksTexts = ParentRanks.GetComponentsInChildren<Text>();

        Text[] NamesTexts = ParentNames.GetComponentsInChildren<Text>();

        Text[] RatingsTexts = ParentRatings.GetComponentsInChildren<Text>();

        int i = 0;

        bool alreadyChanged = false;

        int addIfChangedPlace = 0;

        bool passOnce = false;

        bool flag = false;

        //Debug.Log(DataController.GetValue<int>("Rating"));

        //Debug.Log(DataController.GetValue<string>("displayName"));

        if (Ranks != null) 
        {
            foreach (Text txt in RanksTexts)
            {
                if (i + addIfChangedPlace < Ranks.Count)
                {
                    if (DataController.GetValue<int>("Rating") > Ratings[i] && !alreadyChanged && Ranks[i] != 1 && !flag)
                    {
                        RanksTexts[i].text = Ranks[i].ToString();

                        NamesTexts[i].text = DataController.GetValue<string>("displayName");

                        RatingsTexts[i].text = DataController.GetValue<int>("Rating").ToString();

                        addIfChangedPlace = 1;

                        alreadyChanged = true;
                    }
                    else if (RanksTexts != null)
                    {
                        if (Names[i] == DataController.GetValue<string>("displayName")) 
                        {
                            flag = true;
                        }
                        if (passOnce)
                        {
                            RanksTexts[i].text = (Ranks[i + addIfChangedPlace]).ToString();

                            NamesTexts[i].text = (Names[i + addIfChangedPlace]).ToString();

                            RatingsTexts[i].text = (Ratings[i + addIfChangedPlace]).ToString();
                        }
                        else
                        {
                            RanksTexts[i].text = (Ranks[i] - addIfChangedPlace).ToString();

                            NamesTexts[i].text = (Names[i]).ToString();

                            RatingsTexts[i].text = (Ratings[i]).ToString();
                        }
                    }

                    if (Names[i] == DataController.GetValue<string>("displayName"))
                    {
                        if (DataController.GetValue<int>("Rating") == 0)
                        {
                            DataController.SaveValue("Rating", (int)Ratings[i]);
                        }
                        else
                        {
                            Ratings[i] = DataController.GetValue<int>("Rating");
                        }

                        if (alreadyChanged)
                        {
                            passOnce = true;
                        }
                    }
                }
                else
                {
                    if (RanksTexts != null)
                    {
                        RanksTexts[i].text = "";

                        NamesTexts[i].text = "";

                        RatingsTexts[i].text = "";
                    }
                }

                i++;

            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
