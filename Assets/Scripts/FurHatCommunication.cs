using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FurHatCommunication : MonoBehaviour {
  static string API_URL = "https://183e-2001-6b0-2-2801-3d3c-b7b-b5b6-7c8b.ngrok.io";
  static string FURHAT_URL = "";
  //static string API_URL_M = "https://5674-188-148-206.ngrok-io"
  // static string API_LOCAL = "http://0.0.0.0:8080/";
//   class Game {
//     public string gameID;

//     public Game(string id) {
//       gameID = id;
//     }
//   }

  [System.Serializable]
  class ApiResponse {
    public string id;
  }

//   Game currentGame;

  public void InitializeGame(LevelSettings levelSettings) {
    // StartGame(JsonUtility.ToJson(levelSettings));
  }

  public void SendTurn(int TurnNum, int round, bool dot, int differentialOutcome, int squarePosition, int stimuliPosition, float reactionTime, string player, bool successMove, int timeOut, int cornerDist, int distractorIndex) {
    // TurnData turn = new TurnData(TurnNum, round, dot, differentialOutcome, squarePosition, stimuliPosition, reactionTime, player, successMove, timeOut, cornerDist, distractorIndex, currentGame.gameID);
    // string jsonTurnData = JsonUtility.ToJson(turn);
    // SendTurn(jsonTurnData);
  }

//   public void SendGameOver() {
//     StartCoroutine(PostData("/game/end", "{}", API_URL));
//   }

//   void SendTurn(string json) {
//     StartCoroutine(PostData("/turn", json, API_URL));
//   }

//   void StartGame(string json) {
//     StartCoroutine(PostEvent(json, API_URL));
//   }

  // public void SendHi() {
  //   StartCoroutine(PostData("/turn", "hi"));
  // }

  public void SendEvent() {
      StartCoroutine(PostEvent("message", FURHAT_URL));
  }

  IEnumerator PostEvent(string eventMessage, string url) {
    using (UnityWebRequest www = new UnityWebRequest(url, "POST")) {
      byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(eventMessage);
      www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
      www.downloadHandler = new DownloadHandlerBuffer();
    //   www.SetRequestHeader("Content-Type", "application/json");
      yield return www.SendWebRequest();
      
      if (www.result == UnityWebRequest.Result.Success) {
        Debug.Log("Success");
        ApiResponse response = JsonUtility.FromJson<ApiResponse>(www.downloadHandler.text);
        // if (path == "/game") currentGame = new Game(response.id);
        // Debug.Log(response.id);
      } else {
        Debug.Log(www.error);
      }
    }
  }
}
