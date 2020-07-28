using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mechanics
{
    // Class that handles shooting of the beam
    public class Shooter : MonoBehaviour
    {
        // Private variables
        private GameObject beam;
        private Transform emissionSource;

        // Unity functions
        void Start()
        {
            beam = GameObject.FindGameObjectWithTag("Beam");
            emissionSource = GameObject.FindGameObjectWithTag("Emitter Source").transform;
        }

        // Public functions
        public void Shoot()
        {
            // The shoot function simply teleports the beam to the tip of the gun (there is only one beam in the scene)
            BeamInteraction beamInteraction = beam.GetComponent<BeamInteraction>();
            if (beamInteraction.CanBeFired())
            {
                // Disable the trail of the beam when it is being moved to the gun to avoid awkward visual effect
                beam.transform.GetChild(0).gameObject.SetActive(false);
                beam.transform.SetPositionAndRotation(emissionSource.position, emissionSource.rotation);
                beam.transform.GetChild(0).gameObject.SetActive(true);
                beamInteraction.Fire();
            }
        }
    }
}

