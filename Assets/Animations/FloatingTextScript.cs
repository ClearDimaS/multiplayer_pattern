using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextScript : MonoBehaviour
{
    public int DontDestroy;
    void Start()
    {
        if (DontDestroy > -20) 
        {
            Destroy(gameObject, 1.5f);
        }
    }
}
