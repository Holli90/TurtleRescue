using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skySetting : MonoBehaviour
{
    [SerializeField] Material mat;
    static Material Mat;
    // Start is called before the first frame update
    void Start()
    {
        Mat = mat;
    }
    public static void setSky()
    {
        RenderSettings.skybox = Mat;
    }

}
