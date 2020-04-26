
using System.Collections.Generic;
using GameSparksTutorials ;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void SceneLoader(int SceneIndex) 
    {
        if (PhotonManager.TwoPlayersJoined && PhotonNetwork.inRoom)
        {
            Debug.Log("You exited an online game so u lose :(");
            WinAndLoseHandler.LoseOnline();
            SceneLoaderForScript(4);
            return;
        }
        else
        {
            Debug.Log("Loading scene number: " + SceneIndex);
            SceneLoaderForScript(SceneIndex);
            return;
        }
    }
    public static void SceneLoaderForScript(int SceneIndex1)
    {
        if (SingleGameManager.instance != null) 
        {
            SingleGameManager.instance.InGame = false;
        }


        PhotonManager.TwoPlayersJoined = false;
        if (PhotonNetwork.connected)
        {
            PhotonNetwork.Disconnect();
        }
        if (PhotonNetwork.inRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        /// ?????
        DataController.SaveValue("syncedOther", 0);
        DataController.SaveValue("syncedMine", 0);
        var Modifs = Equipment.EquipmentModifiers;
        foreach (string modifier in Modifs )
        {
            DataController.SaveValue("Total" + modifier + "Other", -1);
        }
        var LocalNamesList =  new List<string> { "attackOther", "agilityOther", "powerOther", "strengthOther", "enduranceOther", "speedOther", "sleepOther", "regenOther" };
        foreach (string LocalNamesOther in LocalNamesList)
        {
            DataController.SaveValue(LocalNamesOther, -1);
        }

        Debug.Log("Loading scene number: " + SceneIndex1);

        SceneManager.LoadScene(SceneIndex1);
    }
}
