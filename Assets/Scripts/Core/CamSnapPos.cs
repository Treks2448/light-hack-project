using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core  
{
    public class CamSnapPos : MonoBehaviour
    {
        // Variables visible in editor
        [SerializeField] private CamSnapPos nextPos;
        [SerializeField] private CamSnapPos previousPos;

        // public functions
        public CamSnapPos getNextPos() { return nextPos; }
        public CamSnapPos getPreviousPos() { return previousPos; }
    }
}
