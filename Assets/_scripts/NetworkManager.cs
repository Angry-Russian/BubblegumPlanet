using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour {

    private string baseUrl = "https://safe-spire-50798.herokuapp.com/";

    public void onClickButtonConnection()
    {
        WWW result = GET(baseUrl);
    }


    public WWW GET(string url)
    {
        WWW www = new WWW(url);

        StartCoroutine(WaitForWWW(www));
        //do nothing untill json is loaded 
        while (!www.isDone) { }

        if (www.error == null || www.error == "")
        {
            print(www.text);
        }
        else
        {
            Debug.Log("WWW error: " + www.error);
        }

        return www;
    }
    IEnumerator WaitForWWW(WWW www)
    {
        yield return www;
    }
}
