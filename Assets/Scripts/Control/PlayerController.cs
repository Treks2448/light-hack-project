using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Movement;
using Core;
using Mechanics;

namespace Control
{
    // This class processes user input into movement/actions of the player character
    public class PlayerController : MonoBehaviour
    {
        // Private variables
        private Mover mover;
        private Shooter shooter;
        private GameObject gameManager;
        private Camera cam;

        // Unity specific functions
        void Start()
        {
            // Build references
            cam = Camera.main;
            mover = GetComponent<Mover>();
            shooter = GetComponent<Shooter>();
            gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        }

        void Update()
        {
            // Ignore player input when paused
            if (gameManager.GetComponent<Pause>().IsPaused()) { return; }

            // Handle aiming and shooting
            ControlLooking();
            if (Input.GetButton("Fire1")) { shooter.Shoot(); }
        }

        void LateUpdate()
        {
            if (gameManager.GetComponent<Pause>().IsPaused()) { return; }
            mover.HandleMovement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }

        private void ControlLooking()
        {
            // Tell player character to look at mouse position in world
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 hitPoint = hit.point;
                mover.LookAt(hitPoint);
            }
        }
    }
}

