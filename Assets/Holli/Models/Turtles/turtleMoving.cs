using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turtleMoving : MonoBehaviour
{
    Animator anim;
    Rigidbody rb;
    bool stay = true;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (spawnTurtle.finishLoad == true)
        {
            Debug.Log((rb.velocity.x ,rb.velocity.y)) ;

            /*        if (rb.velocity.x <= 0.01f)
                    {
                        stay = true;
                    }*/
            if (stay)
            {
                stay = false;
                float animVal = Random.Range(0, 2f);
                StartCoroutine(startAnim(animVal));
            }
        }
    }

    IEnumerator startAnim(float sec)
    {
        yield return new WaitForSeconds(sec);
        anim.SetBool("run", true);
        StartCoroutine(addForce(1.5f));
    }

        IEnumerator addForce(float sec)
    {
        yield return new WaitForSeconds(sec);
        rb.AddForce(transform.forward * 120f, ForceMode.Force);
        float endVal = Random.Range(1f, 3f);
        anim.SetBool("run", false);
        StartCoroutine(endForce(endVal));
    }
    IEnumerator endForce(float sec)
    {
        yield return new WaitForSeconds(sec);
        
        stay = true;
    }

}
