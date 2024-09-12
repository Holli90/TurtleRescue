using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class fogController : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float desity = 0.04f;

    void Start()
    {
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(0.442f,0.7109f,0.7264f,1);
        RenderSettings.fogDensity = desity;
    }

}
