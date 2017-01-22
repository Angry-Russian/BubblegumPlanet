using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerç : MonoBehaviour {

    public float timeLeft = 180.0f;
    [SerializeField]
    public GameObject timeout;
    // Use this for initialization
    void Start () {
        timeout.active = false;

    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            timeout.active = true;
            StartCoroutine(GameOver());
        }
        else
        {
            GetComponent<Text>().text = Mathf.Floor(timeLeft / 60) + ":" + Mathf.RoundToInt(timeLeft % 60);
        }
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(3);
        int[] scores = new int[4];
        scores[0] = GameObject.Find("Anim_Bot_01").GetComponent<GravityBody>().deaths;
        scores[1] = GameObject.Find("Anim_Bot_02").GetComponent<GravityBody>().deaths;
        //scores[2] = GameObject.Find("Anim_Bot_03").GetComponent<GravityBody>().deaths;
        // scores[3] = GameObject.Find("Anim_Bot_04").GetComponent<GravityBody>().deaths;
        Array.Sort(scores);
        if(scores[3] == scores[2])
        {
            SceneManager.LoadScene("Draw");
        }
        else
        {
            SceneManager.LoadScene("WinnerP4");
            if (scores[3] == GameObject.Find("Anim_Bot_04").GetComponent<GravityBody>().deaths)
                SceneManager.LoadScene("WinnerP4");
            else if (scores[3] == GameObject.Find("Anim_Bot_03").GetComponent<GravityBody>().deaths)
                SceneManager.LoadScene("WinnerP3");
            else if (scores[3] == GameObject.Find("Anim_Bot_02").GetComponent<GravityBody>().deaths)
                SceneManager.LoadScene("WinnerP2");
            else
                SceneManager.LoadScene("WinnerP1");
        }
    }
}
