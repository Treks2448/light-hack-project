using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Mechanics;
using Core;

namespace Progression
{
    public class Level : MonoBehaviour
    {
        // Variables visible in editor
        [SerializeField] private Level nextLevel;
        [SerializeField] private float levelNameDisplayTime;

        // Private variables 
        private Vector3 playerStartPosition;
        private GameObject player;
        private Text levelNameText;
        private Transform defaultCamPosObj;
        private ParticleSystem detectorParticles;

        public enum State { locked, unlocked, active };
        private State currentState = State.locked;

        // Unity functions
        private void Start()
        {
            detectorParticles = transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
            levelNameText = GameObject.FindGameObjectWithTag("Level Name Display Text").GetComponent<Text>();
            player = GameObject.FindGameObjectWithTag("Player");
            playerStartPosition = transform.Find("Player Start Position").transform.position;
            defaultCamPosObj = transform.Find("Camera Positions").Find("1");

            levelNameText.enabled = false;
        }

        // Public functions
        public Level GetNextLevel() { return nextLevel; }
        public State GetCurrentState() { return currentState; }
        public void SetState(State state) { currentState = state; }

        public void Load()
        {
            // Stop any actions in the previous level
            Level currentLevel = transform.parent.GetComponent<LevelManager>().CurrentLevel();
            if (currentLevel && currentLevel.gameObject.GetComponent<SpecialLevel>())
            {
                currentLevel.gameObject.GetComponent<SpecialLevel>().StopSpecialLevelActions();
            }

            // Set up the level properties
            BeamInteraction beamInteraction = GameObject.FindGameObjectWithTag("Beam").GetComponent<BeamInteraction>();
            beamInteraction.SetMaxInteractions(GetComponent<MaxInteractions>().GetMaxInteractions());
            beamInteraction.SetCanBeFired(true);
            currentState = State.active;

            // Animate level transition
            StartCoroutine(DisplayLevelText());
            SpecialLevel specialLevel = GetComponent<SpecialLevel>();
            
            // Move the player and camera to the level
            player.transform.position = playerStartPosition;
            Camera.main.transform.position = defaultCamPosObj.transform.position;
            Camera.main.transform.rotation = defaultCamPosObj.transform.rotation;
            CamSnapPos defaultCamPos = defaultCamPosObj.GetComponent<CamSnapPos>();
            Camera.main.gameObject.GetComponent<CameraController>().SetSnapPos(defaultCamPos);

            // Perform any special level actions for this level
            if (specialLevel != null)
            {
                specialLevel.StartSpecialAction();
            }

            // Play particle system
            detectorParticles.Play();
        }

        public void Complete()
        {
            currentState = State.unlocked;
            // TODO: reset the transforms of each object in the level to default
            if (nextLevel == null) { return; }
            // Set next level to unlocked (as it may not be loaded/set active)
            nextLevel.SetState(State.unlocked); 
        }

        // Private functions
        private IEnumerator DisplayLevelText()
        {
            levelNameText.text = gameObject.name;
            levelNameText.enabled = true;
            yield return new WaitForSeconds(levelNameDisplayTime);
            levelNameText.enabled = false;
        }
    }
}

