using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class spawnTurtle : MonoBehaviour
{
    [SerializeField] GameObject turtlePrefab;
    [SerializeField] int numTurtle = 20;
    [SerializeField] Transform limitBox;
    public GameObject[] allTurtle;
    private float[] boxSize = new float[6];
    bool loaded = false;
    public static bool finishLoad = false;
    void Start()
    {
        
    }
    void loadTurtle()
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

        allTurtle = new GameObject[numTurtle];

        for (int i = 0; i < numTurtle; ++i)
        {

            Vector3 pos = new Vector3(
                Random.Range(boxSize[0], boxSize[1]),
                Random.Range(boxSize[2], boxSize[3]),
                Random.Range(boxSize[4], boxSize[5]));
            
            Quaternion rot = Quaternion.Euler(0, Random.Range(0, 270f), 0);
            allTurtle[i] = Instantiate(turtlePrefab, pos, rot);
        }
    }

        // Update is called once per frame
        void FixedUpdate()
    {
        if (!loaded)
        {
            loaded = true;
            loadTurtle();
            StartCoroutine(fixRot(2));
        }
    }

    IEnumerator fixRot(float sec)
    {
        yield return new WaitForSeconds(sec);
        foreach (GameObject turtle in allTurtle)
        {
            Quaternion baseQuat = turtle.transform.rotation;
            Vector3 euler = baseQuat.eulerAngles;
            turtle.transform.rotation = Quaternion.Euler(euler.x, euler.y, 0);
        }
        finishLoad = true;
    }


}
