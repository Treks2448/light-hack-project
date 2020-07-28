using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Progression;
using UnityEngine.UI;

public class DimContinueButton : MonoBehaviour
{
    private LevelManager levelManager;

    void Start() { levelManager = GameObject.FindGameObjectWithTag("Levels").GetComponent<LevelManager>(); }
    private void Update()
    {
        if (levelManager.SaveFileExists())
        {
            GetComponent<Button>().interactable = true;
        }
        else
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
