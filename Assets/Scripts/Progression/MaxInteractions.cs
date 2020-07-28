using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Progression
{
    // Holds and returns the max number of interactions for the room
    public class MaxInteractions : MonoBehaviour
    {
        [SerializeField] private int maxInteractions = 1;
        public int GetMaxInteractions(){ return maxInteractions; }
    }
}

