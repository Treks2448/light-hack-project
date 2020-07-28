using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Progression
{
    public class FinalLevel : MonoBehaviour
    {
        [SerializeField] float endGameScreenDuration = 5f;

        GameObject endGameCanvas;
        private bool endGameScreenFinished = false;

        void Start()
        {
            endGameCanvas = GameObject.FindGameObjectWithTag("Game End Canvas");
            endGameCanvas.SetActive(false);
        }
        public void StartGameFinishedSeuquence()
        {
            endGameCanvas.SetActive(true);
            StartCoroutine(ShowEndGameScreen());
        }

        private IEnumerator ShowEndGameScreen()
        {
            yield return new WaitForSeconds(endGameScreenDuration);
            endGameScreenFinished = true;
            StartCoroutine(ResetEndGame());
        }
        private IEnumerator ResetEndGame()
        {
            yield return new WaitForSeconds(0.0005f);
            endGameScreenFinished = false;
            endGameCanvas.SetActive(false);
        }

        public bool EndGameScreenFinished() { return endGameScreenFinished; }
    }
}
