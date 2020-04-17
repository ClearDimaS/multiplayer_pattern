
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class MultiPlayerCamera : MonoBehaviour
{
    public GameObject ScaledWithCameraFront;
    public GameObject ScaledWithCameraBack;

    public List<Transform> targets;
    public Vector3 offset;
    public float smoothTime = .5f;

    public float minZoom = 80f;
    public float maxZoom = 10f;
    public float zoomLimiter = 50f;

    private Vector3 velocity;
    private Camera cam;
    private GameObject temp1;
    private bool flag1;

    int multFotBtnsSymmetry;
    float newZoomTemp;
    //Vector3 TempForSymmetryRight = new Vector3();
    //Vector3 TempForSymmetryLeft = new Vector3();

    private void Start()
    {
        if (PhotonNetwork.isMasterClient)
        {
            multFotBtnsSymmetry = 1;
        }
        else 
        {
            multFotBtnsSymmetry = -1;
        }

        foreach (GameObject btn in GameObject.FindGameObjectsWithTag("Buttons")) 
        {
            if (btn.name == "ButtonRight")
            {
                if (PhotonNetwork.isNonMasterClientInRoom) 
                {
                    btn.transform.localScale = new Vector3(multFotBtnsSymmetry * -1, 1, 0);
                    btn.GetComponentInChildren<Text>().transform.localScale = new Vector3(multFotBtnsSymmetry * 1, 1, 0);
                }
            }
            else
            if (btn.name == "ButtonLeft")
            {
                if (PhotonNetwork.isNonMasterClientInRoom)
                {
                    btn.transform.localScale = new Vector3(multFotBtnsSymmetry * 1, 1, 0);
                    btn.GetComponentInChildren<Text>().transform.localScale = new Vector3(multFotBtnsSymmetry * -1, 1, 0);
                }
            }
            else 
            {
                btn.transform.localScale = new Vector3(multFotBtnsSymmetry * 1, 1, 0);
            }
        }

        minZoom = 85f;
        flag1 = true;
        cam = GetComponent<Camera>();
        temp1 = (GameObject.FindGameObjectsWithTag("Player")[0]);
    }
    void FixedUpdate()
    {
        if (flag1 == true)
        {
            if ((GameObject.FindGameObjectsWithTag("Player").Length > 1))
            {
                flag1 = false;
                targets[0] = temp1.transform;
                targets[1] = GameObject.FindGameObjectsWithTag("Player")[1].transform;
            }
        }
        if (targets.Count == 0)
            return;

        if (targets[1] != null) 
        {

            Move();
            Zoom();
        }

    }

    void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter) ;
        newZoomTemp = 0.9f * newZoom / 2;
        ScaledWithCameraFront.transform.localScale = new Vector3(newZoomTemp, newZoomTemp, 1);
        ScaledWithCameraBack.transform.localScale = new Vector3(newZoomTemp, newZoomTemp, 1);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime * 3);
    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }


    float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.size.x;
    }
    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }
        
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.center;
    }
}
