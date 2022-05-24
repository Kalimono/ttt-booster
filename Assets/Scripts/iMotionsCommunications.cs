using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using System.Net;
// using System.Net.Sockets;
// using System.Text;
using UnityEngine.Networking;

public class iMotionsCommunications : MonoBehaviour {
    private string IP = "127.0.0.1";
    // private int PORT = 8089;

    public string startMessage = "/imotions/start";
    public string endMessage = "/imotions/stop";

    public static iMotionsCommunications instance;

    void Awake() {
       if (instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [System.Serializable]
    class ApiResponse {
        public string id;
    }

    // public void SendEvent() {
    //     StartCoroutine(PostEvent("http://localhost:4000/imotions/start"));
    // }

    public void SendStartMarker() {
        StartCoroutine(PostEvent("http://localhost:4000/imotions/start"));
    }

    public void SendStopMarker() {
        StartCoroutine(PostEvent("http://localhost:4000/imotions/stop"));
    }

    IEnumerator PostEvent(string url) {
        using (UnityWebRequest www = new UnityWebRequest(url, "GET")) {
        // byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        // www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        // www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();
        
        if (www.result == UnityWebRequest.Result.Success) {
            Debug.Log("Success");
            ApiResponse response = JsonUtility.FromJson<ApiResponse>(www.downloadHandler.text);
        } else {
            Debug.Log(www.error);
        }
        }
    }  
}
  

