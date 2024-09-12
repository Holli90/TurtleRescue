using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class UImanager : MonoBehaviour
{
    public static UImanager UI;

    GameObject UIcanvas;
    public void hideUI()
    {
        UIcanvas = GameObject.Find("uiCanvas");
        UIcanvas.SetActive(false);
    }

}
