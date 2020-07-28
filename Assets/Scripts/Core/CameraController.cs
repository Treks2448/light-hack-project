using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    // This class handles camera actions such as snapping the camera to different locations in the room
    public class CameraController : MonoBehaviour
    {
        // Variables visible in editor
        [SerializeField] private float rotationSpeed = 0.1f;
        [SerializeField] private float translationSpeed = 0.1f;
        [SerializeField] private AudioClip snapSound;

        // Private variables
        private CamSnapPos currentPos;
        private Pause pause;
        private AudioSource audioSource;

        // Unity functions
        void Start()
        {
            // Set references
            pause = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<Pause>();
            audioSource = GetComponent<AudioSource>();
        }

        void Update()
        {
            // Don't do anything if game is paused
            if (pause.IsPaused()) { return; }

            // Set next camera position based on user input
            if (Input.GetKeyDown(KeyCode.E))
            {
                currentPos = currentPos.getNextPos();
                if (audioSource) audioSource.PlayOneShot(snapSound);
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                currentPos = currentPos.getPreviousPos();
                if (audioSource && snapSound) audioSource.PlayOneShot(snapSound);
            }
            
            // Move to next position
            if (currentPos != null) MoveToNextPos();
        }

        // Private functions
        private void MoveToNextPos()
        {
            // Handle rotation
            Vector3 forward = currentPos.transform.forward;
            Vector3 up = currentPos.transform.up;
            Quaternion targetRotation = Quaternion.LookRotation(forward, up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);

            // Handle translation
            Vector3 targetPosition = currentPos.transform.position;
            transform.position = Vector3.Slerp(transform.position, targetPosition, translationSpeed);

        }

        public void SetSnapPos(CamSnapPos levelDefaultSnapPos)
        {
            currentPos = levelDefaultSnapPos;
        }
    }
}


