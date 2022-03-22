using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



public class FurHatCommunication : MonoBehaviour {


  public class Message {
    // public string message;
    public string message;

    public void CreateMessage(string anOutcome) {
      // message = aMessage;
      message = anOutcome;
    }

    public string SaveToString() {
        return JsonUtility.ToJson(this);
    }
  }
  
  static string API_URL = "https://183e-2001-6b0-2-2801-3d3c-b7b-b5b6-7c8b.ngrok.io";
  public string FURHAT_URL = "http://localhost:8888";

  static List<string> outcomeStringList = new List<string>{
    "dot_one",
    "dot_two",
    "dot_three",
    "dot_four"
  };

  static List<string> notcomeStringList = new List<string>{
    "not_one",
    "not_two",
    "not_three",
    "not_four"
  };

  static string incorrectResponse = "incorrect";
  static string timeOutString = "timeout";

  [System.Serializable]
  class ApiResponse {
    public string id;
  }

  public void DebugSend() {
    SendOutcome(1);
  }

  public void SendOutcome(int outcome) {
    Message m = new Message();
    m.CreateMessage(outcomeStringList[outcome]);
    String message = m.SaveToString();
    Debug.Log(message);
    StartCoroutine(PostEvent("/", message, FURHAT_URL));
  }

  public void SendNotcome(int outcome) {
    Message m = new Message();
    m.CreateMessage(notcomeStringList[outcome]);
    String message = m.SaveToString();
    Debug.Log(message);
    StartCoroutine(PostEvent("/", message, FURHAT_URL));
  }

    public void SendIncorrectResponse() {
    Message m = new Message();
    m.CreateMessage(incorrectResponse);
    String message = m.SaveToString();
    Debug.Log(message);
    StartCoroutine(PostEvent("/", message, FURHAT_URL));
  }

  public void SendTimeout() {
    Message m = new Message();
    m.CreateMessage(timeOutString);
    String message = m.SaveToString();
    Debug.Log(message);
    StartCoroutine(PostEvent("/", message, FURHAT_URL));
  }

  public void SendEvent() {
      StartCoroutine(PostEvent("/", "{ \"message\": \"dot_one\" }", FURHAT_URL));
  }

  IEnumerator PostEvent(string path, string json, string url) {
    using (UnityWebRequest www = new UnityWebRequest(url, "POST")) {
      byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
      www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
      www.downloadHandler = new DownloadHandlerBuffer();
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
