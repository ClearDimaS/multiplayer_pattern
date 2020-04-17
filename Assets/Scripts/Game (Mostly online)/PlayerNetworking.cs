
using UnityEngine;

public class PlayerNetworking : Photon.MonoBehaviour
{
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private MonoBehaviour[] scriptsToIgnore;

    void Start()
    {
        PhotonView photonView = GetComponent<PhotonView>();
        Initialize();
    }

 
    void Initialize()
    {
        if (photonView.isMine)
        {
            playerCamera.SetActive(true);
        }
        else
        {
            playerCamera.SetActive(false);
            foreach (MonoBehaviour item in scriptsToIgnore)
            {
                item.enabled = false;
            }
        }
    }
}
