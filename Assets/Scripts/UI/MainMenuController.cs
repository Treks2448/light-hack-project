using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Progression;

namespace UI
{
    // Class that handles control of the main menu
    public class MainMenuController : MonoBehaviour
    {
        // Variables visible in editor
        [SerializeField] private AudioClip selectSound;
        [SerializeField] private AudioClip cancelSound;
        [SerializeField] private FinalLevel finalLevel;
        [SerializeField] private GameObject inGameUI;
        [SerializeField] private Canvas levelTransCanvas;
        [SerializeField] private Canvas specialLevelCanvas;

        // Private variables
        private GameObject gameManager;
        private Canvas menuCanvas;
        private GameObject optionsPanel;
        private GameObject newGamePanel;
        private GameObject creditsPanel;
        private GameObject controlsPanel;
        private bool isActive;
        private AudioSource menuAudioSource;

        // Unity functions
        void Start()
        {
            // Find references
            menuAudioSource = GetComponent<AudioSource>();
            menuCanvas = GetComponent<Canvas>();
            gameManager = GameObject.FindGameObjectWithTag("Game Manager");
            optionsPanel = gameObject.transform.Find("Tab Menu").Find("Options Panel").gameObject;
            creditsPanel = gameObject.transform.Find("Tab Menu").Find("Credits Panel").gameObject;
            controlsPanel = gameObject.transform.Find("Tab Menu").Find("Controls Panel").gameObject;
            newGamePanel = GameObject.FindGameObjectWithTag("New Game Panel");

            // Initialise parameters
            optionsPanel.SetActive(false);
            newGamePanel.SetActive(false);
            creditsPanel.SetActive(false);
            controlsPanel.SetActive(false);
            inGameUI.SetActive(false);
            Activate();
        }

        void Update()
        {
            if (finalLevel.EndGameScreenFinished()) { Activate(); }

            // Keep the game paused while the main menu is active
            if (isActive)
            {
                gameManager.GetComponent<Pause>().PauseGame();
                levelTransCanvas.enabled = false;
                specialLevelCanvas.enabled = false;
                inGameUI.SetActive(false);
            }
            else
            {
                levelTransCanvas.enabled = true;
                inGameUI.SetActive(true);
            }

        }

        // Public functions
        public bool IsActive() { return isActive; }

        public void EnterOptions()
        {
            if (optionsPanel.activeSelf)
            {
                creditsPanel.SetActive(false);
                optionsPanel.SetActive(false);
                controlsPanel.SetActive(false);
                menuAudioSource.PlayOneShot(cancelSound);
            }
            else
            {
                creditsPanel.SetActive(false);
                controlsPanel.SetActive(false);
                optionsPanel.SetActive(true);
                menuAudioSource.PlayOneShot(selectSound);
            }
        }

        public void NewGame()
        {
            menuAudioSource.PlayOneShot(selectSound);
            newGamePanel.SetActive(true);
        }

        public void HideNewGamePanel()
        {
            newGamePanel.SetActive(false);
            menuAudioSource.PlayOneShot(cancelSound);
        }

        public void Activate() 
        {
            isActive = true;
            menuCanvas.enabled = true;
            gameManager.GetComponent<Pause>().PauseGame();
        }

        public void Deactivate() 
        {
            menuAudioSource.PlayOneShot(selectSound);
            creditsPanel.SetActive(false);
            optionsPanel.SetActive(false);
            controlsPanel.SetActive(false);
            isActive = false;
            gameManager.GetComponent<Pause>().ResumeGame();
            menuCanvas.enabled = false;
            // TODO: if the door has been opened, load the next scene
        }

        public void EnterCredits()
        {
            if (creditsPanel.activeSelf)
            {
                creditsPanel.SetActive(false);
                optionsPanel.SetActive(false);
                controlsPanel.SetActive(false);
                menuAudioSource.PlayOneShot(cancelSound);
            }
            else
            {
                creditsPanel.SetActive(true);
                optionsPanel.SetActive(false);
                controlsPanel.SetActive(false);
                menuAudioSource.PlayOneShot(selectSound);
            }
            
        }

        public void EnterControls()
        {
            if (controlsPanel.activeSelf)
            {
                creditsPanel.SetActive(false);
                optionsPanel.SetActive(false);
                controlsPanel.SetActive(false);
                menuAudioSource.PlayOneShot(cancelSound);
            }
            else
            {
                creditsPanel.SetActive(false);
                optionsPanel.SetActive(false);
                controlsPanel.SetActive(true);
                menuAudioSource.PlayOneShot(selectSound);
            }

        }
    }
}


