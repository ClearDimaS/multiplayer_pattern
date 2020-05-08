using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningScript : MonoBehaviour
{
    #region Singleton
    public static WarningScript instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion
    public GameObject warning;

}
