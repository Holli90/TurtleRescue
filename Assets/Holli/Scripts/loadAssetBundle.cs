using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.HID;
using Microsoft.SqlServer.Server;
using System.Linq;



public class loadAssetBundle : MonoBehaviour
{
    AssetBundle bundle;
    UnityWebRequest www1;
    CachedAssetBundle cachedAssetBundle;
    int loaded = 0;
    [SerializeField] string buildingUrl;
   
/*    Vector3 buildingPos = new Vector3(11.46f, -6.8f, -158.17f);
    Vector3 buildingRot = new Vector3(0, 0, 0);
    Vector3 buildingScl = new Vector3(6.1088f, 6.1088f, 6.1088f);*/
    Vector3 buildingPos = new Vector3(0, 0, 0);
    Vector3 buildingRot = new Vector3(0, 0, 0);
    Vector3 buildingScl = new Vector3(1f, 1f, 1f);
    bool loading = false;
    public static string sceneName;

    void Start()
    {
        Caching.ClearAllCachedVersions("props");
        //StartCoroutine(GetAssets(www1, buildingPos, buildingRot, buildingScl, 1));
        //StartCoroutine(GetAssets(www2,sofa1Pos, sofa1Rot, sofa1Scl, 0));
    }
    void Awake()
    {
        //4178954553
        
        if (buildingUrl != null)
        {
            cachedAssetBundle.name = "props";
            www1 = UnityWebRequestAssetBundle.GetAssetBundle(buildingUrl, cachedAssetBundle, 0);
        }
        //www2 = UnityWebRequestAssetBundle.GetAssetBundle(assetsUrl, cachedAssetBundle1, 0);
    }

    private void FixedUpdate()
    {
        if ((buildingUrl != null) && (UIaction.sceneLoaded) && (!loading))
        {
            loading = true;


                List<Hash128> listing = new List<Hash128>();
                Caching.GetCachedVersions("props", listing);
                if (listing.Count > 0)
                {
                    www1 = UnityWebRequestAssetBundle.GetAssetBundle(buildingUrl, cachedAssetBundle, 0);
                }

                StartCoroutine(GetBackground(www1, buildingPos, buildingRot, buildingScl));

        }
    }
        private void LateUpdate()
    {
        //LoadGltfBinaryFromMemory();
        if (loaded == 1)
        {
            //UIaction.hideLoading();
            loaded += 1;
            activeFPScam.activeCam(true);
            skySetting.setSky();
            FlockManager.sceneName = sceneName;
            FlockManager.loading = false;
            fadeIn.fadeIN();
        }

    }

    bool checkMesh(Transform child)
    {
        if (child.GetComponent<MeshRenderer>() != null)
        {
            return true;
        } else
        {
            return false;
        }
    }

    List <Transform> findChilds(Transform parent)
    {
        int childCount = parent.childCount;
        List<Transform> childs = new List<Transform>();
        if (childCount > 0)
        {
            
            for (int i = 0; i <= childCount - 1; i++)
            {
                Transform child = parent.GetChild(i);
                childs.Add(child);
            }
        }

        if (childs.Count > 0) 
        {
            return childs;
        } else
        {
            return null;
        }
    }

    void convertToToonShader(GameObject obj)
    {
        bool status = true;

        List<Transform> allchild = new List<Transform>();
        List<Transform> newList = new List<Transform>();
        Transform parent = obj.transform;
        int counter = 0;
        while (status && (counter < 100))
        {

            if (allchild.Count > 0)
            {
                List<Transform> newestList = new List<Transform>();

                foreach (Transform mchild in newList)
                {
                    List<Transform> childs = findChilds(mchild);
                    if (childs != null)
                    {
                        foreach (Transform child in childs)
                        {
                            allchild.Add(child);
                            newestList.Add(child);
                        }

                    }
                }

                newList = newestList;
                if (newList.Count == 0)
                {
                    status = false;
                }
            }
            else
            {
                List<Transform> childs = findChilds(parent);
                if (childs != null)
                {

                    foreach (Transform child in childs)
                    {
                        allchild.Add(child);
                        newList.Add(child);
                    }

                }
                else
                {
                    status = false;
                }
            }
            counter++;
        }
        List<Material> oldMatList = new List<Material>();
        List<string> oldMatListName = new List<string>();
        foreach (Transform child in allchild)
        {
            bool checker = checkMesh(child);
            if (checker)
            {
                Material[] matList = child.GetComponentInChildren<MeshRenderer>().materials;
                List <Material> orgMats = new List<Material>();
                orgMats.AddRange(matList);
                Material[] newMat = new Material[orgMats.Count];
                for (int i = 0; i <= orgMats.Count - 1; i++)
                {
                    Material orgMat = orgMats[i];
                    string newName = ((orgMat.name).Replace(" (Instance)", "") + "_Toon"); 
                    if (!oldMatListName.Contains(newName))
                    {
                        oldMatListName.Add(newName);
                        /*Material orgMat = child.GetComponentInChildren<MeshRenderer>().material;*/
                        Texture baseMap = orgMat.GetTexture("_BaseMap");
                        Texture metalMap = orgMat.GetTexture("_MetallicGlossMap");
                        Texture normalMap = orgMat.GetTexture("_BumpMap");
                        Texture emissMap = orgMat.GetTexture("_EmissionMap");
                        Shader shader = Resources.Load<Shader>("Shaders/ToonShading");
                        Material material = new Material(shader);
                        material.name = newName;

                        if (baseMap != null)
                        {
                            material.SetTexture("_baseColor", baseMap);
                        }
                        if (metalMap != null)
                        {
                            material.SetTexture("_metallic", metalMap);
                        }
                        if (normalMap != null)
                        {
                            material.SetTexture("_normal", normalMap);
                        }
                        if (emissMap != null)
                        {
                            material.SetTexture("_emission", emissMap);
                        }
                        newMat[i] = material;
                        oldMatList.Add(material);
                    } else
                    {
                        newMat[i] = oldMatList[oldMatListName.IndexOf(newName)];
                    }
                }
                child.GetComponentInChildren<MeshRenderer>().materials = newMat;
                /*child.GetComponent<MeshRenderer>().material = material;*/

            }
        }
    }

    IEnumerator GetBackground(UnityWebRequest www, Vector3 pos, Vector3 rot, Vector3 scl)
    {
        if (bundleStorage.bundle == null)
        {
            www.SendWebRequest();
            while (!www.isDone)
            {
                UIaction.Slider.GetComponent<Slider>().value = 0.25f + ((www.downloadProgress) * 3f / 4f);
                yield return null;
            }
            if (www.isDone)
            {
                yield return new WaitForSeconds(1);

            }
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                bundle = DownloadHandlerAssetBundle.GetContent(www);
            }
            if (www.error == null)
            {
                Debug.Log("FINISH");
                bundleStorage.bundle = bundle;
                GameObject[] objs = bundle.LoadAllAssets<GameObject>();
                GameObject getObj = null;
                foreach (GameObject obj in objs)
                {

                    if ((sceneName == "AquariumScene") && (obj.name == "aquariumProps"))
                    {
                        
                        getObj = obj;
                    }
                    else if ((sceneName == "OasisScene") && (obj.name == "oasisProps"))
                    {
                        getObj = obj;
                    }
                }


                GameObject homeClone = Instantiate(getObj);
                convertToToonShader(homeClone);
                SceneManager.MoveGameObjectToScene(homeClone, SceneManager.GetSceneByName(sceneName));
                homeClone.transform.localPosition = pos;
                homeClone.transform.localScale = scl;
                Quaternion newEuler = new Quaternion();
                newEuler.eulerAngles = rot;
                homeClone.transform.localRotation = newEuler;

                loaded += 1;
            }
        } else
        {
            Debug.Log("FINISH");
            bundle = bundleStorage.bundle;
            GameObject[] objs = bundle.LoadAllAssets<GameObject>();
            GameObject getObj = null;
            print(sceneName);
            foreach (GameObject obj in objs)
            {
                print(obj.name);
                if ((sceneName == "AquariumScene") && (obj.name == "aquariumProps"))
                {
                    getObj = obj;
                }
                else if ((sceneName == "OasisScene") && (obj.name == "oasisProps"))
                {
                    getObj = obj;
                }
            }
            GameObject homeClone = Instantiate(getObj);
            convertToToonShader(homeClone);
            SceneManager.MoveGameObjectToScene(homeClone, SceneManager.GetSceneByName(sceneName));

            homeClone.transform.localPosition = pos;
            homeClone.transform.localScale = scl;
            Quaternion newEuler = new Quaternion();
            newEuler.eulerAngles = rot;
            homeClone.transform.localRotation = newEuler;
 
            loaded += 1;
        }
    }
}
