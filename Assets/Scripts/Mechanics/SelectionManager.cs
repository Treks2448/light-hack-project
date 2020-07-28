using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mechanics
{
    // Class that manages the highlighting and selecting of objects in the game
    public class SelectionManager : MonoBehaviour
    {
        // Private variables
        private GameObject highlighted;
        private GameObject selected;

        // Unity functions
        private void Update()
        {
            if (selected == null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    // Unhighlight the currently highlighted game object if it's not being looked at anymore
                    if (highlighted != null && hit.transform.gameObject != highlighted)
                    {
                        highlighted.GetComponent<Selectable>().Unhighlight();
                        highlighted = null;
                    }

                    // Check whether the game object is selectable
                    if (hit.transform.gameObject.GetComponent<Selectable>() != null)
                    {
                        // Select the object if the player has clicked the button
                        if (Input.GetButtonDown("Fire2"))
                        {
                            selected = hit.transform.gameObject;
                            selected.GetComponent<Selectable>().Select();
                        }
                        // Otherwise highlight the object
                        else
                        {
                            highlighted = hit.transform.gameObject;
                            hit.transform.gameObject.GetComponent<Selectable>().Highlight();
                        }
                    }
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire2"))
                {
                    selected.GetComponent<Selectable>().Deselect();
                    selected = null;
                }
            }
        }
    }
}
