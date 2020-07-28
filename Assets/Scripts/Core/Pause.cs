using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    /* This class holds the paused state of the game. It freezes the physics when paused.
     * Other classes can use it to know whether the game has been paused.
     * Its pause/resume functions can be called to pause/resume the game.
    */
    public class Pause : MonoBehaviour
    {
        // Private variables
        private bool isPaused;

        // Unity Functions
        void Start()
        {

            // set variables
            isPaused = false;
        }
        
        void Update()
        {
            if (Input.GetButtonDown("Cancel")) 
            {
                if (isPaused) { ResumeGame(); }
                else { PauseGame(); } 
            }
        }

        // Public functions
        public bool IsPaused()
        {
            return isPaused;
        }

        // Private functions
        public void PauseGame()
        {
            isPaused = true;
            Time.timeScale = 0.00001f;
        }

        public void ResumeGame()
        {
            isPaused = false;
            Time.timeScale = 1f;
        }
    }
}

