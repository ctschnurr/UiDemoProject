using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideDropManager : MonoBehaviour
{
    // Start is called before the first frame update
    float dropTimer = 5;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dropTimer -= Time.deltaTime;
        if (dropTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
