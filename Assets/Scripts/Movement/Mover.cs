using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mechanics;

namespace Movement
{
    // Class that handles movement of character
    public class Mover : MonoBehaviour
    {
        // variables visible in editor
        [SerializeField] private float acceleration;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float sensitivity;
        [SerializeField] private float maxRotUp;
        [SerializeField] private float minRotUp;

        // private variables
        private Camera cam;
        private Rigidbody rb;
        private Animator animator;

        // Unity functions
        void Start()
        {
            cam = Camera.main;
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
        }

        // Public functions
        public void HandleMovement(float sidewaysThrow, float forwardThrow)
        {
            Vector3 direction = cam.transform.forward.normalized * forwardThrow + cam.transform.right.normalized * sidewaysThrow;
            direction.y = 0;

            rb.AddForce(acceleration * direction.normalized * Time.deltaTime, ForceMode.VelocityChange);
            
            // Clamp velocity to a magnitued of max speed
            if (rb.velocity.magnitude > maxSpeed) { rb.velocity = rb.velocity.normalized * maxSpeed; }
            
            // Update the animator
            if (animator != null) 
            { 
                animator.SetFloat("speed", (acceleration * direction.normalized * Time.deltaTime).magnitude);
            }
        }

        public void LookAt(Vector3 pos)
        {
            Vector3 lookAtPoint = new Vector3(pos.x, transform.position.y, pos.z);
            transform.LookAt(lookAtPoint);
        }

    }
}

