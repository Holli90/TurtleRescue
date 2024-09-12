using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class changeShader : MonoBehaviour
{
    bool checkMesh(Transform child)
    {
        if (child.GetComponent<MeshRenderer>() != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    List<Transform> findChilds(Transform parent)
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
        }
        else
        {
            return null;
        }
    }

    public void showMats()
    {
        GameObject tree = GameObject.Find("Tree");
        bool status = true;

        List<Transform> allchild = new List<Transform>();
        List<Transform> newList = new List<Transform>();
        Transform parent = tree.transform;
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
                List<Material> orgMats = new List<Material>();
                orgMats.AddRange(matList);
                Material[] newMat = new Material[orgMats.Count];
                for (int i = 0; i <= orgMats.Count - 1; i++)
                {
                        Material orgMat = orgMats[i];
                    
                    Texture baseMap = orgMat.GetTexture("_MainTex");
                    print(baseMap);
                    /*string newName = ((orgMat.name).Replace(" (Instance)", "") + "_Toon");
                    if (!oldMatListName.Contains(newName))
                    {
                        oldMatListName.Add(newName);
                        *//*Material orgMat = child.GetComponentInChildren<MeshRenderer>().material;*//*
                        Texture baseMap = orgMat.GetTexture("_MainTex");
                        Texture metalMap = orgMat.GetTexture("_ExtraTex");
                        Texture normalMap = orgMat.GetTexture("_BumpMap");
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

                        newMat[i] = material;
                        oldMatList.Add(material);
                    }
                    else
                    {
                        newMat[i] = oldMatList[oldMatListName.IndexOf(newName)];
                    }*/
                }
                    //child.GetComponentInChildren<MeshRenderer>().materials = newMat;
            }
            
        }

     }
}
