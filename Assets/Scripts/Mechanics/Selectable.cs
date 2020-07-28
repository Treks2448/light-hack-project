using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

namespace Mechanics
{
    // Class that handles properties of selectable objects.
    public class Selectable : MonoBehaviour
    {
        // Variables visible in editor
        [SerializeField] private Material regular;
        [SerializeField] private Material highlighted;
        [SerializeField] private Material selected;
        [SerializeField] private AudioClip rotationSound;

        // Private varaibles
        private AudioSource audioSource;
        private float angle = 45f;
        private float rotationSpeed = 90f;
        private GameObject gameManager;
        private bool isSelected;
        private bool rotateClockwise = false;

        // Unity functions
        void Start()
        {
            // Build references
            gameManager = GameObject.FindGameObjectWithTag("Game Manager");
            audioSource = GetComponent<AudioSource>();
        }

        void Update()
        {
            // If not paused, this is selected and isn't currently rotating
            // Determine which direction to rotate depending on user input
            if (!gameManager.GetComponent<Pause>().IsPaused() && isSelected && angle >= 45.0f)
            {
                if (Input.GetKeyDown(KeyCode.R)) 
                {
                    rotateClockwise = true;
                    angle = 0;
                    if (audioSource && rotationSound) audioSource.PlayOneShot(rotationSound);

                }
                else if (Input.GetKeyDown(KeyCode.F)) 
                {
                    rotateClockwise = false;
                    angle = 0;
                    if (audioSource && rotationSound) audioSource.PlayOneShot(rotationSound);
                }
            }

            // Rotate an angle of 45 degrees
            if (angle <= 45.0f)
            {
                if (rotateClockwise)
                {
                    if (angle + rotationSpeed * Time.deltaTime >= 45.0f)
                    {
                        float remainingRotation = 45.0f - angle;
                        transform.Rotate(new Vector3(0, remainingRotation, 0), Space.World);
                    }
                    else
                    {
                        transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0), Space.World);
                    }

                }
                else
                {
                    if (angle + rotationSpeed * Time.deltaTime >= 45.0f)
                    {
                        float remainingRotation = 45.0f - angle;
                        transform.Rotate(new Vector3(0, -remainingRotation, 0), Space.World);
                    }
                    else
                    {
                        transform.Rotate(new Vector3(0, -rotationSpeed * Time.deltaTime, 0), Space.World);
                    }
                }
                angle += rotationSpeed * Time.deltaTime;
            }

        }

        // Public fucntions
        public void Highlight()
        {
            if (GetComponent<Renderer>().material != selected)
            {
                GetComponent<Renderer>().material = highlighted;
            }
        }

        public void Unhighlight()
        {
            GetComponent<Renderer>().material = regular;
        }

        public void Deselect()
        {
            isSelected = false;
            GetComponent<Renderer>().material = regular;
        }

        public void Select()
        {
            isSelected = true;
            GetComponent<Renderer>().material = selected;
        }
    }
}

