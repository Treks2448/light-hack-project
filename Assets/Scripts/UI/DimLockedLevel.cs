using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Progression;
using UnityEngine.UI;

namespace UI
{
    // Class that dims levels that are locked in the selection menu
    public class DimLockedLevel : MonoBehaviour
    {
        // Variables visible in editor
        [SerializeField] private Level level;

        // Unity functions
        private void Update()
        {
            if (level.GetCurrentState() == Level.State.locked)
            {
                GetComponent<Button>().interactable = false;
            }
            else
            {
                GetComponent<Button>().interactable = true;
            }
        }
    }
}

