using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fadeIn : MonoBehaviour
{
    [SerializeField] GameObject panel;
    static bool start = false;
    bool end = false;
    float speed = 1;

    void LateUpdate()
    {
        if ((start)&&(!end))
        {
            Image img = panel.GetComponentInChildren<Image>();
            Color col = img.color;
            if (col.a > 0)
            {
                col.a -= (0.001f*speed);
                speed += .1f;
                panel.GetComponentInChildren<Image>().color = col;
            } else
            {
                end = true;
            }
        }
    }

    public static void fadeIN()
    {
        start =true;
    }

}
