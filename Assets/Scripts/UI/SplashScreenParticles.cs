using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreenParticles : MonoBehaviour
{
    
    void Start()
    {
        GetComponent<ParticleSystem>().Play();
    }

}
