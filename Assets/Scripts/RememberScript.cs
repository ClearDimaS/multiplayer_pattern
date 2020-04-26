using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSparksTutorials;

public class RememberScript : MonoBehaviour
{
    public GameObject Toggle;
    // Start is called before the first frame update
    void Start()
    {
        if (DataController.GetValue<int>("RememberMe") == 1) 
        {
            Toggle.GetComponent<Toggle>().isOn = true;
        }
    }

    // Update is called once per frame
    public void Change()
    {
        if (Toggle.GetComponent<Toggle>().isOn)
        {
            DataController.SaveValue("RememberMe", 1);
        }
        else
        {
            DataController.SaveValue("RememberMe", 0);
        }
    }
}
