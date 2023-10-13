using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cylinders : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (this.name == "WinCylinder") gameManager.WinGame();
            if (this.name == "DeathSphere") FirstPersonController_Sam.SetDecay();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (this.name == "DeathSphere") FirstPersonController_Sam.RemoveDecay();
        }
    }
}
