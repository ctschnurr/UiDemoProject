using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cylinders : MonoBehaviour
{
    private static AudioSource sizzleSource;
    // Start is called before the first frame update
    void Awake()
    {
        sizzleSource = GameObject.Find("DeathSphere/sfx").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.paused)
        {
            sizzleSource.Pause();
        }
        else
        {
            sizzleSource.UnPause();

            float sfxSliderValue = menuManager.sfxVolumeSlider.value;
            sizzleSource.volume = sfxSliderValue;

        }
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
            if (this.name == "DeathSphere")
            {
                FirstPersonController_Sam.RemoveDecay();
                FirstPersonController_Sam.DecayTimer = FirstPersonController_Sam.DecayTimerReset;
            }
        }
    }
}
