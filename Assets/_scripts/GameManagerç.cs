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
            string text = ("00" + Mathf.RoundToInt(timeLeft % 60));
            GetComponent<Text>().text = Mathf.Floor(timeLeft / 60) + ":" + text.Substring(text.Length-2);
        }
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(3);
        int[] scores = new int[4];
        GameObject p1 = GameObject.Find("Anim_Bot_01");
        GameObject p2 = GameObject.Find("Anim_Bot_02");
        GameObject p3 = GameObject.Find("Anim_Bot_03");
        GameObject p4 = GameObject.Find("Anim_Bot_04");

        if (p1)
            scores[0] = p1.GetComponent<GravityBody>().deaths;
        if (p2)
            scores[1] = p2.GetComponent<GravityBody>().deaths;
        if (p3)
            scores[2] = p3.GetComponent<GravityBody>().deaths;
        if (p4)
            scores[3] = p4.GetComponent<GravityBody>().deaths;

        Array.Sort(scores);
        if(scores[0] == scores[1])
        {
            SceneManager.LoadScene("Draw");
        }
        else
        {
            if (scores[0] == p4.GetComponent<GravityBody>().deaths)
                SceneManager.LoadScene("WinnerP4");
            else if (scores[0] == p3.GetComponent<GravityBody>().deaths)
                SceneManager.LoadScene("WinnerP3");
            else if (scores[0] == p2.GetComponent<GravityBody>().deaths)
                SceneManager.LoadScene("WinnerP2");
            else
                SceneManager.LoadScene("WinnerP1");
        }
    }
}
