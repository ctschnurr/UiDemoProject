using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder;

public class GuideManager : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject destination;
    GameObject dropPos;
    public GameObject drop;
    NavMeshAgent agent;
    float dropTimer = 0;
    float dropTimerReset = 1;

    static GuideManager instance;

    void Awake()
    {
        destination = GameObject.Find("WinCylinder");
        dropPos = GameObject.Find("DropPos");
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.SetDestination(destination.transform.position);
        transform.LookAt(destination.transform.position);

        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }



    // Update is called once per frame
    void Update()
    {
        dropTimer -= Time.deltaTime;
        if(dropTimer <= 0)
        {
            dropTimer = dropTimerReset;
            Instantiate(drop, dropPos.transform.position, dropPos.transform.rotation);
        }

        float distance = Vector3.Distance(transform.position, destination.transform.position);
        if(distance <= 0.5f)
        {
            Destroy(gameObject);
        }
    }
}
