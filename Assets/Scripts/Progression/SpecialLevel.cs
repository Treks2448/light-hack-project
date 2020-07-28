using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Progression
{
    // Performs special level actions for the level that it is attached to
    [RequireComponent(typeof(AudioSource))]
    public class SpecialLevel : MonoBehaviour
    {
        // Variables visible in editor
        [SerializeField] private string[] messageTexts;
        [SerializeField] private int[] durationsOfMessages;
        [SerializeField] private AudioClip[] narratorLines;

        // Private variables
        private Canvas specialLevelUI;
        private Text specialLevelText;
        private Level parentLevel;
        private Message[] messages = null;
        private bool runActions;
        private bool skip = false;

        // Unity functions
        void Start()
        {
            // Set references
            specialLevelUI = GameObject.FindGameObjectWithTag("Special Level UI").GetComponent<Canvas>();
            specialLevelText = GameObject.FindGameObjectWithTag("Special Level UI").GetComponentInChildren<Text>();
            parentLevel = GetComponentInParent<Level>();

            // Set initial values
            specialLevelUI.enabled = false;
            runActions = false;

            if (messageTexts.Length > 0 && durationsOfMessages.Length > 0 && messageTexts.Length <= durationsOfMessages.Length)
            {
                messages = new Message[messageTexts.Length];
                for (int i = 0; i < messageTexts.Length; i++)
                {
                    messages[i] = new Message(messageTexts[i], durationsOfMessages[i]);
                }
            }
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.Q)) skip = true;
        }

        // Public functions
        public void StartSpecialAction()
        {
            runActions = true;

            // Show messages specific to level
            if (messages != null) { StartCoroutine(DisplayMessages(messages, 0, true)); }
            

            // Do other level specific actions
            string levelName = parentLevel.name;
            switch (levelName)
            {
                case "Basics":
                    StartCoroutine(HideDetectorForSeconds(129.3f));
                    break;
                case "Reflection":
                    StartCoroutine(HideDetectorForSeconds(49f));
                    break;
                case "Refraction":
                    StartCoroutine(HideDetectorForSeconds(56f));
                    break;
                case "Rotations":
                    StartCoroutine(HideDetectorForSeconds(61f));
                    break;
                default:
                    break;

            }
        }

        public void StopSpecialLevelActions()
        {
            GetComponent<AudioSource>().Stop();
            runActions = false;
            specialLevelUI.enabled = false;
            StopAllCoroutines();
        }

        // Private functions
        private IEnumerator DisplayMessage(string message, float time, bool disableCanvas)
        {
            if (runActions)
            {
                specialLevelUI.enabled = true;
                specialLevelText.text = message;
                yield return new WaitForSeconds(time);
                if (disableCanvas) { specialLevelUI.enabled = false; }
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
        }

        /*
        private void DisplayMessages(Message[] messages, int i, bool DisableCanvas)
        {
            Debug.Assert(messages != null && i < messages.Length);
            if (narratorLines.Length > 0) return;
            if (!runActions) return;
            

            ReadMessage(i);

            specialLevelUI.enabled = true;
            specialLevelText.enabled = true;
            specialLevelText.text = messages[i].text;

            if (startMsg + 1 < messages.Length)
            {
                // continue reading messages
            }
        }
        */
        private IEnumerator DisplayMessages(Message[] messages, int startMsg, bool disableCanvas)
        {
            Debug.Assert(messages != null && startMsg < messages.Length);
            
            // Check to see if the dialogue should continue being output and read
            if (runActions)
            {
                if (narratorLines.Length > 0) ReadMessage(startMsg);
                
                specialLevelUI.enabled = true;
                specialLevelText.enabled = true;
                specialLevelText.text = messages[startMsg].text;

                yield return new WaitForSeconds(messages[startMsg].time);
                if (startMsg + 1 < messages.Length)
                {
                    StartCoroutine(DisplayMessages(messages, startMsg + 1, disableCanvas));
                }
                else if (disableCanvas) { specialLevelUI.enabled = false; }
            }
            else
            {
                specialLevelUI.enabled = false;
                yield return new WaitForEndOfFrame();
            }
        }

        private void ReadMessage(int msgIndex)
        {
            GetComponent<AudioSource>().PlayOneShot(narratorLines[msgIndex]);
        }

        private IEnumerator HideDetectorForSeconds(float seconds)
        {
            transform.Find("Detector").gameObject.SetActive(false);
            yield return new WaitForSeconds(seconds);
            transform.Find("Detector").gameObject.SetActive(true);
        }

        private class Message
        {
            public Message(string _text, float _time)
            {
                text = _text;
                time = _time;
            }
            public string text;
            public float time;

        };
    }
}


