using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Menu_Controller : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void loadMenu()
    {
        Application.LoadLevel("UI-Marc");
    }

    public void loadInstruction()
    {
        Application.LoadLevel("UI-Instruction-Marc");
    }

    public void loadCredits()
    {
        Application.LoadLevel("UI-Credits-Marc");
    }

    public void exitApplication()
    {
        Application.Quit();
    }
}
