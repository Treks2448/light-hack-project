using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mechanics
{
    // Class to store obstacle properies
    public class ObstacleProperties : MonoBehaviour
    {
        // Varaibles visible in editor
        [SerializeField] private float refractiveIndex;
        [SerializeField] private float reflectivity;

        // Public functions
        public float GetRefractiveIndex() { return refractiveIndex; }
        public float GetReflectivity() { return reflectivity; }
    }
}

