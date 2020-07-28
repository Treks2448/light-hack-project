using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Progression
{
    // Class that handles events caused by the beam hitting the detector (that this is on)
    public class BeamDetector : MonoBehaviour
    {
        // Variables visible in editor
        [SerializeField] private float transitionFadeTime;
        [SerializeField] private AudioClip detectorHitSound;
        
        // Private variables
        private GameObject fadeCanvas;
        private Level nextLevel;
        private float fadeTime;
        private bool startFade;
        private bool startFadeOut;
        private float canvasAlpha;

        // Unity Functions
        private void Start()
        {
            // Set references
            fadeCanvas = GameObject.FindGameObjectWithTag("Level Transition Fade Canvas");
            fadeCanvas.GetComponentInChildren<Image>().color = new Color(0, 0, 0, 0);

            // Set intial values
            startFade = false;
            fadeTime = 0;
            canvasAlpha = 0;
        }
        private void Update()
        {
            FadeOnLevelTransition();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Beam"))
            {
                // Set the current level to complete
                Level parentLevel = GetComponentInParent<Level>();
                if (parentLevel == null) { Debug.LogError("Cannot find parent Level on " + gameObject); return; }
                parentLevel.Complete();

                // Save the players progress
                LevelManager levelManager = transform.parent.GetComponentInParent<LevelManager>();
                levelManager.Save();

                // Load the next level
                nextLevel = parentLevel.GetNextLevel();
                StartLevelTransition();

                // Play detector hit sound
                GetComponent<AudioSource>().PlayOneShot(detectorHitSound);
            }
        }

        // Private Functions
        private void StartLevelTransition()
        {
            startFade = true;
            fadeTime = 0;
            canvasAlpha = 0;
        }

        private void FadeOnLevelTransition()
        {
            if (startFade) // fade in the black screen
            {
                fadeCanvas.GetComponentInChildren<Image>().color = new Color(0, 0, 0, canvasAlpha);
                fadeTime += Time.deltaTime;
                canvasAlpha = fadeTime * 2 / transitionFadeTime;
                // load the level when screen is completely black
                if (fadeTime >= transitionFadeTime / 2)
                {
                    startFade = false;
                    startFadeOut = true;
                    fadeTime = 0;
                    
                    if (nextLevel != null) nextLevel.Load();

                    // If detector belongs to the final level
                    FinalLevel parentLevel = GetComponentInParent<FinalLevel>();
                    if (parentLevel != null)
                    {
                        GetComponentInParent<FinalLevel>().StartGameFinishedSeuquence();
                    }
                }
            }
            if (startFadeOut) // fade out the black screen
            {
                fadeCanvas.GetComponentInChildren<Image>().color = new Color(0, 0, 0, canvasAlpha);
                fadeTime += Time.deltaTime;
                canvasAlpha = 1 - fadeTime * 2 / transitionFadeTime;
                if (fadeTime >= transitionFadeTime / 2)
                {
                    startFadeOut = false;
                    fadeTime = 0;
                    canvasAlpha = 0;
                }
            }
        }
    }
}

