using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Flock : MonoBehaviour {

    float speed;
    bool turning = false;
    bool run = false;
    bool trigger = false;
    GameObject obstacle;
    [SerializeField] GameObject manager;
    FlockManager flockManager;
    bool anim = false;
    void Start() {

        string name = manager.name;
        GameObject flockObj = GameObject.Find(name);
        flockManager = flockObj.GetComponent< FlockManager >();
        speed = Random.Range(flockManager.minSpeed, flockManager.maxSpeed);
    }


    void Update() {

        if (trigger)
        {
            
            Vector3 offset = new Vector3(5f, 5f, 5f);
            Vector3 direction = transform.position - obstacle.transform.position ;
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                flockManager.rotationSpeed * Time.deltaTime);
            if (run == true)
            {
                run = false;
                StartCoroutine(endTrigger(Random.Range(2f, 5f)));
            }
        }
        else
        {
            
            Bounds b = new Bounds(flockManager.transform.position, flockManager.swimLimits *2f);

            if (!b.Contains(transform.position))
            {

                turning = true;
            }
            else
            {

                turning = false;
            }

            if (turning)
            {

                Vector3 direction = flockManager.transform.position - transform.position;
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(direction),
                    flockManager.rotationSpeed * Time.deltaTime);
            }
            else
            {


                if (Random.Range(0, 100) < 10)
                {
                    speed = Random.Range(flockManager.minSpeed, flockManager.maxSpeed);
                }


                if (Random.Range(0, 100) < 10)
                {
                    ApplyRules();
                }
            }
        }
        this.transform.Translate(0.0f, 0.0f, speed * Time.deltaTime );
    }

    void LateUpdate()
    {
        if (!anim)
        {
            if (Random.Range(0, 1000) == 0)
            {
                anim = true;
                GetComponent<Animator>().enabled = true;
            }
        }
    }
    private void ApplyRules() {

        GameObject[] gos;
        gos = flockManager.allFish;

        if (gos.Length > 1)
        {
            Vector3 vCentre = Vector3.zero;
            Vector3 vAvoid = Vector3.zero;

            float gSpeed = 0.01f;
            float mDistance;
            int groupSize = 0;

            foreach (GameObject go in gos)
            {

                if (go != this.gameObject)
                {

                    mDistance = Vector3.Distance(go.transform.position, this.transform.position);
                    if (mDistance <= flockManager.neighbourDistance)
                    {

                        vCentre += go.transform.position;
                        groupSize++;

                        if (mDistance < 1.0f)
                        {

                            vAvoid = vAvoid + (this.transform.position - go.transform.position);
                        }

                        Flock anotherFlock = go.GetComponent<Flock>();
                        gSpeed = gSpeed + anotherFlock.speed;
                    }
                }
            }

            if (groupSize > 0)
            {

                vCentre = vCentre / groupSize + (flockManager.goalPos - this.transform.position);
                speed = gSpeed / groupSize;

                if (speed > flockManager.maxSpeed)
                {

                    speed = flockManager.maxSpeed;
                }

                Vector3 direction = (vCentre + vAvoid) - transform.position;
                if (direction != Vector3.zero)
                {

                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        Quaternion.LookRotation(direction),
                        flockManager.rotationSpeed * Time.deltaTime);
                }
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (trigger == false)
        { 
            if ((other.gameObject.tag == "Obstacle") || (other.gameObject.tag == "Player"))
            {
                obstacle = other.gameObject;
                trigger = true;
                run = true;
            }
        }
    }

    IEnumerator endTrigger(float sec)
    {
        yield return new WaitForSeconds(sec);
        trigger = false;
    }


}