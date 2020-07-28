using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Progression;

namespace UI
{
    // Class that handles control of the pause menu
    public class PauseMenuController : MonoBehaviour
    {
        // Variables visible in editor
        [SerializeField] private AudioClip selectSound;
        [SerializeField] private AudioClip cancelSound;
        [SerializeField] private FinalLevel finalLevel;

        // Private variables
        private GameObject gameManager;
        private Canvas pauseCanvas;
        private GameObject optionsPanel;
        private GameObject levelSelectPanel;
        private GameObject controlsPanel;
        private MainMenuController mainMenu;
        private AudioSource menuAudioSource;

        // Unity functions
        void Start()
        {
            // Find references
            pauseCanvas = GetComponent<Canvas>();
            gameManager = GameObject.FindGameObjectWithTag("Game Manager");
            optionsPanel = gameObject.transform.Find("Tab Menu").Find("Options Panel").gameObject;
            controlsPanel = gameObject.transform.Find("Tab Menu").Find("Controls Panel").gameObject;
            levelSelectPanel = gameObject.transform.Find("Tab Menu").Find("Level Select Panel").gameObject;
            mainMenu = GameObject.FindGameObjectWithTag("Main Menu").GetComponent<MainMenuController>();
            menuAudioSource = GetComponent<AudioSource>();

            // Set initial values
            pauseCanvas.enabled = false;
            optionsPanel.SetActive(false);
            levelSelectPanel.SetActive(false);
            controlsPanel.SetActive(false); ;

            Cursor.lockState = CursorLockMode.None;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        void Update()
        {
            if (gameManager.GetComponent<Pause>().IsPaused() && !mainMenu.IsActive())
            {
                if (!pauseCanvas.enabled) menuAudioSource.PlayOneShot(cancelSound);
                pauseCanvas.enabled = true;
            }
            else if (!mainMenu.IsActive())
            {
                if (pauseCanvas.enabled) menuAudioSource.PlayOneShot(cancelSound);
                pauseCanvas.enabled = false;
            }
        }

        // Public functions
        public void Resume()
        {
            controlsPanel.SetActive(false);
            levelSelectPanel.SetActive(false);
            optionsPanel.SetActive(true);
            gameManager.GetComponent<Pause>().ResumeGame();
        }

        public void EnterLevelSelect()
        {
            controlsPanel.SetActive(false);
            levelSelectPanel.SetActive(true);
            optionsPanel.SetActive(false);
            menuAudioSource.PlayOneShot(selectSound);
        }

        public void EnterOptions()
        {
            controlsPanel.SetActive(false);
            levelSelectPanel.SetActive(false);
            optionsPanel.SetActive(true);
            menuAudioSource.PlayOneShot(selectSound);
        }

        public void EnterControls()
        {
            controlsPanel.SetActive(true);
            levelSelectPanel.SetActive(false);
            optionsPanel.SetActive(false);
            menuAudioSource.PlayOneShot(selectSound);
        }

        public void Quit()
        {
            mainMenu.Activate();
            pauseCanvas.enabled = false;
            Camera.main.transform.position = GameObject.FindGameObjectWithTag("Camera Start Pos").transform.position;
            Camera.main.transform.rotation = GameObject.FindGameObjectWithTag("Camera Start Pos").transform.rotation;
            menuAudioSource.PlayOneShot(selectSound);
        }
    }
}

