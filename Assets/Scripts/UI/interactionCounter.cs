using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mechanics;
using UnityEngine.UI;

namespace UI 
{
    // This class handles displaying of remaining laser beam interactions to the screen
    public class interactionCounter : MonoBehaviour
    {
        void Update()
        {
            Text interactionText = GetComponent<Text>();
            BeamInteraction beamInteraction = GameObject.FindGameObjectWithTag("Beam").GetComponent<BeamInteraction>();
            float interactionsLeft = beamInteraction.GetRemainingInteractions();
            if (interactionsLeft < 0 ) { interactionText.text = "Interactions: " + 0; }
            else { interactionText.text = "Interactions: " + interactionsLeft.ToString(); }
        }
    }
}

