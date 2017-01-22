using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerç : MonoBehaviour {

    public float timeLeft = 180.0f;
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        GetComponent<Text>().text = Mathf.Floor(timeLeft / 60)+":" +Mathf.RoundToInt(timeLeft%60);
        if (timeLeft < 0)
        {
// StartCoroutine(GameOver());
        }
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(3);
        int[] scores = new int[4];
        scores[0] = GameObject.Find("Anim_Bot_01").GetComponent<GravityBody>().deaths;
        scores[1] = GameObject.Find("Anim_Bot_02").GetComponent<GravityBody>().deaths;
        scores[2] = GameObject.Find("Anim_Bot_03").GetComponent<GravityBody>().deaths;
        scores[3] = GameObject.Find("Anim_Bot_04").GetComponent<GravityBody>().deaths;
// Array.Sort(scores);
        if(scores[3] == scores[2])
        {
            Application.LoadLevel("Draw");
        }
        else
        {
            if(scores[3] == GameObject.Find("Anim_Bot_04").GetComponent<GravityBody>().deaths)
                Application.LoadLevel("WinnerP4");
            else if (scores[3] == GameObject.Find("Anim_Bot_03").GetComponent<GravityBody>().deaths)
                Application.LoadLevel("WinnerP3");
            else if (scores[3] == GameObject.Find("Anim_Bot_02").GetComponent<GravityBody>().deaths)
                Application.LoadLevel("WinnerP2");
            else
                Application.LoadLevel("WinnerP1");
        }
    }
}
