using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour {

    private string baseUrl = "https://safe-spire-50798.herokuapp.com/";

    private string player = "players";

    WWW postPlayer(string jsonData) {
        var postData = System.Text.Encoding.UTF8.GetBytes(jsonData);
        using (UnityWebRequest req = UnityWebRequest.Post(baseUrl+"/", postData)) {
            yield return www.Send();
            // ...
        }
    }

    WWW getPlayers() {
        WWW www = new WWW(baseUrl + "/players");

        StartCoroutine(WaitForWWW(www));

        if (www.error == null || www.error == "") {
            print(www.text);
        } else {
            Debug.Log("WWW error: " + www.error);
        }

        return www;
    }

    IEnumerator WaitForWWW(WWW www)
    {
        yield return www;
    }
}
