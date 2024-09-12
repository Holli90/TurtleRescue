using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class activeFPScam : MonoBehaviour
{
    [SerializeField] GameObject cameraFPS;
    static GameObject cam;
    void Start()
    {
        cam = cameraFPS;
    }
    public static void activeCam(bool active)
    {
        print(active);
        cam.SetActive(active);
    }
}
