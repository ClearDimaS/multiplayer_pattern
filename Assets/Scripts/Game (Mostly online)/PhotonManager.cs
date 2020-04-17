
using UnityEngine;
using GameSparksTutorials;


public class PhotonManager : Photon.MonoBehaviour
{
    [SerializeField] public GameObject player;
    [SerializeField] private GameObject lobbyCamera;

    private GameObject leftspawn;
    private GameObject rightspawn;

    public static bool TwoPlayersJoined;
    public static bool GameOver;

    void Start()
    {
        GameOver = false;
        TwoPlayersJoined = false;
        PhotonNetwork.ConnectUsingSettings("1.0");
        lobbyCamera.SetActive(false);
    }

    void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
        if (PhotonNetwork.room == null) 
        {
            PhotonNetwork.JoinOrCreateRoom("Room" + PhotonNetwork.time, new RoomOptions() { MaxPlayers = 2 }, TypedLobby.Default) ;
        }
    }

    void OnJoinedRoom()
    {
        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.room.EmptyRoomTtl = 0;

            leftspawn = GameObject.FindGameObjectWithTag("LeftSpawn");
            PhotonNetwork.Instantiate("Player", new Vector3(leftspawn.transform.position.x, leftspawn.transform.position.y, leftspawn.transform.position.z), Quaternion.identity, 0);
        }
        else
        {
            PhotonNetwork.room.removedFromList = false;

            rightspawn = GameObject.FindGameObjectWithTag("RightSpawn");
            PhotonNetwork.Instantiate("Player", new Vector3(rightspawn.transform.position.x, rightspawn.transform.position.y, rightspawn.transform.position.z), Quaternion.identity, 0);
        }
        PhotonNetwork.room.removedFromList = true;
    }
    
    private void LateUpdate()
    {
        if (!TwoPlayersJoined)
        {
            if (GameObject.FindGameObjectsWithTag("Player").Length == 2) 
            {
                Debug.Log("Two players found...");
                PhotonNetwork.room.IsOpen = false;
                PhotonNetwork.room.IsVisible = false;

                TwoPlayersJoined = true;
            }
        }
    }
}
