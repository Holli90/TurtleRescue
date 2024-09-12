using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIaction : MonoBehaviour
{
    public Button aquaBut,oasBut;
    [SerializeField] GameObject startupPanel,scenePanel;
    static GameObject ScenePanel;
    [SerializeField] GameObject cam;
    public static GameObject Cam;
    [SerializeField] GameObject slider;
    public static GameObject Slider;
    public static bool sceneLoaded = false;

    string port = "";
    // Start is called before the first frame update
    void Start()
    {
        ScenePanel = scenePanel;
        Slider = slider;
        Cam = cam;
        //Calls the TaskOnClick/TaskWithParameters/ButtonClicked method when you click the Button
        aquaBut.onClick.AddListener(aquaPort);
        oasBut.onClick.AddListener(oasPort);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            showMenuUI();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            hideMenuUI();
        }

    }
    void hideMenuUI()
    {
        ScenePanel.SetActive(false);
        activeFPScam.activeCam(true);
    }
    void showMenuUI()
    {
        startupPanel.SetActive(true);
        ScenePanel.SetActive(false);
        activeFPScam.activeCam(false);
    }

    void aquaPort()
    {
        Debug.Log("AQUARIUM PORT");
        //SceneManager.LoadScene("AquariumScene", LoadSceneMode.Additive);
        StartCoroutine(LoadScene("AquariumScene"));
        //Debug.Log("percent :" + asyncOperation.progress);

        
        //loading();
        //cam.SetActive(false);
    }
    void oasPort()
    {
        Debug.Log("OASIS PORT");
        StartCoroutine(LoadScene("OasisScene"));
        //startupPanel.SetActive(false);
        //StartCoroutine(loading());
        //cam.SetActive(false);
    }
    IEnumerator LoadScene(string scene)
    {
        bool changePort = false;
        if (port != "")
        {
            if (port != scene)
            {
                changePort = true;
                SceneManager.UnloadSceneAsync(port);
                port = scene;
            }
        }
        else
        {
            changePort = true;
            port = scene;
        }
        loadAssetBundle.sceneName = port;
        if (changePort)
        {
            startupPanel.SetActive(false);
            ScenePanel.SetActive(true);
            yield return null;

            //Begin to load the Scene you specify
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            //Don't let the Scene activate until you allow it to
            asyncOperation.allowSceneActivation = false;
            Slider.GetComponent<Slider>().value = (asyncOperation.progress + 0.1f) / 4f;
            //When the load is still in progress, output the Text and progress bar
            while (!asyncOperation.isDone)
            {
                //Output the current progress
                Slider.GetComponent<Slider>().value = (asyncOperation.progress + 0.1f) / 4f;

                // Check if the load has finished
                if (asyncOperation.progress >= 0.9f)
                {
                    asyncOperation.allowSceneActivation = true;
                    sceneLoaded = true;
                }

                yield return null;
            }
        } else
        {
            ScenePanel.SetActive(false);
            activeFPScam.activeCam(true);
            yield return null;
        }

    }

}
