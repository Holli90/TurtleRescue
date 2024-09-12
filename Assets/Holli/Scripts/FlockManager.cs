using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlockManager : MonoBehaviour
{

    public static FlockManager FM;
    [SerializeField] GameObject fishPrefab;
    [SerializeField] int numFish = 20;
    public GameObject[] allFish;
    public Vector3 swimLimits = new Vector3(5f, 5f, 5f);
    public Vector3 goalPos = Vector3.zero;
    
    [SerializeField] Transform limitBox;

    [Header("Fish Settings")]
    [Range(0.0f, 5.0f)] public float minSpeed;
    [Range(0.0f, 5.0f)] public float maxSpeed;
    [Range(1.0f, 100.0f)] public float neighbourDistance;
    [Range(0.1f, 5.0f)] public float rotationSpeed;

    private float[] boxSize = new float[6];
    private float[] genBox = new float[6];

    public static bool loading;
    public static string sceneName;
    bool loaded = false;

    private void Start()
    {
        loading = true;
    }

    void loadFish(string scene)
    {

        Vector3 boxPos = limitBox.position;
        Vector3 boxScl = limitBox.localScale;
        transform.position = boxPos;

        boxSize[0] = boxPos.x - (boxScl.x / 2.0f);
        boxSize[1] = boxPos.x + (boxScl.x / 2.0f);
        boxSize[2] = boxPos.y - (boxScl.y / 2.0f);
        boxSize[3] = boxPos.y + (boxScl.y / 2.0f);
        boxSize[4] = boxPos.z - (boxScl.z / 2.0f);
        boxSize[5] = boxPos.z + (boxScl.z / 2.0f);
        genBox[0] = boxPos.x - (boxScl.x / 40.0f);
        genBox[1] = boxPos.x + (boxScl.x / 40.0f);
        genBox[2] = boxPos.y - (boxScl.y / 40.0f);
        genBox[3] = boxPos.y + (boxScl.y / 40.0f);
        genBox[4] = boxPos.z - (boxScl.z / 40.0f);
        genBox[5] = boxPos.z + (boxScl.z / 40.0f);

        allFish = new GameObject[numFish];

        for (int i = 0; i < numFish; ++i)
        {

            Vector3 pos = new Vector3(
                Random.Range(genBox[0], genBox[1]),
                Random.Range(genBox[2], genBox[3]),
                Random.Range(genBox[4], genBox[5]));

            allFish[i] = Instantiate(fishPrefab, pos, Quaternion.identity);
            SceneManager.MoveGameObjectToScene(allFish[i], SceneManager.GetSceneByName(scene));
        }

        FM = this;
        goalPos = transform.position;
    }


    void FixedUpdate()
    {
        if (!loading)
        {
            if (!loaded)
            {
                loaded = true;
                loadFish(sceneName);
            }
            else
            {
                if (Random.Range(0, 100) < 10)
                {
                    goalPos = transform.position + new Vector3(
                        Random.Range(-swimLimits.x, swimLimits.x),
                        Random.Range(-swimLimits.y, swimLimits.y),
                        Random.Range(-swimLimits.z, swimLimits.z));
                }
            }
        }
    }
    void LateUpdate()
    {
        if (Random.Range(0, 10000) == 0)
        {
            float x = Random.Range(boxSize[0], boxSize[1]);
            float y = Random.Range(boxSize[2], boxSize[3]);
            float z = Random.Range(boxSize[4], boxSize[5]);
            transform.position = new Vector3(x, y, z);
        }
    }
}