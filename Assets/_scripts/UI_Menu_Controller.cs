using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Menu_Controller : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void loadGame()
    {
        SceneManager.LoadScene("Dim's little big planets");
    }

    public void loadGame2Player()
    {
        SceneManager.LoadScene("Dim's little big planets2Player");
    }

    public void loadMenu()
    {
        SceneManager.LoadScene("UI-Marc");
    }

    public void loadInstruction()
    {
        SceneManager.LoadScene("UI-Instruction-Marc");
    }

    public void loadCredits()
    {
        SceneManager.LoadScene("UI-Credits-Marc");
    }

    public void exitApplication()
    {
        Application.Quit();
    }
}
