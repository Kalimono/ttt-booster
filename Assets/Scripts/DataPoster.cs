using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataPoster : MonoBehaviour {
  static string API_URL = "https://183e-2001-6b0-2-2801-3d3c-b7b-b5b6-7c8b.ngrok.io";
  //static string API_URL_M = "https://5674-188-148-206.ngrok-io"
  static string API_LOCAL = "http://0.0.0.0:8080/";
  class Game {
    public string gameID;

    public Game(string id) {
      gameID = id;
    }
  }

  [System.Serializable]
  class ApiResponse {
    public string id;
  }

  class TurnData {
    public int turnNumber;
    public int roundNumber;
    public bool dot;
    public int differentialOutcome;
    public int squarePosition;
    public int stimuliPosition;
    public float reactionTime;
    public string player;
    public bool successfulMove;
    public int timeOut;
    public int cornerDistractor;
    public int distractorIndex;
    public string gameID;

    public TurnData(int turn, int round, bool dotToggle, int dOut, int sqPos, int stimPos, float rTime, string plr, bool successMove, int tOut, int cornerDist, int dInd, string gId) {
      turnNumber = turn;
      roundNumber = round;
      dot = dotToggle;
      differentialOutcome = dOut;
      squarePosition = sqPos;
      stimuliPosition = stimPos;
      reactionTime = rTime;
      player = plr;
      successfulMove = successMove;
      timeOut = tOut;
      cornerDistractor = cornerDist;
      distractorIndex = dInd;
      gameID = gId;
    }
  }

  Game currentGame;

  public void InitializeGame(LevelSettings levelSettings) {
    // StartGame(JsonUtility.ToJson(levelSettings));
  }

  public void SendTurn(int TurnNum, int round, bool dot, int differentialOutcome, int squarePosition, int stimuliPosition, float reactionTime, string player, bool successMove, int timeOut, int cornerDist, int distractorIndex) {
    // TurnData turn = new TurnData(TurnNum, round, dot, differentialOutcome, squarePosition, stimuliPosition, reactionTime, player, successMove, timeOut, cornerDist, distractorIndex, currentGame.gameID);
    // string jsonTurnData = JsonUtility.ToJson(turn);
    // SendTurn(jsonTurnData);
  }

  public void SendGameOver() {
    StartCoroutine(PostData("/game/end", "{}"));
  }

  void SendTurn(string json) {
    StartCoroutine(PostData("/turn", json));
  }

  void StartGame(string json) {
    StartCoroutine(PostData("/game", json));
  }

  public void SendHi() {
    StartCoroutine(PostData("/turn", "hi"));
  }

  IEnumerator PostData(string path, string json) {
    using (UnityWebRequest www = new UnityWebRequest(API_LOCAL, "POST")) {
      byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
      www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
      www.downloadHandler = new DownloadHandlerBuffer();
      www.SetRequestHeader("Content-Type", "application/json");
      yield return www.SendWebRequest();
      
      if (www.result == UnityWebRequest.Result.Success) {
        ApiResponse response = JsonUtility.FromJson<ApiResponse>(www.downloadHandler.text);
        if (path == "/game") currentGame = new Game(response.id);
        // Debug.Log(response.id);
      } else {
        Debug.Log(www.error);
      }
    }
  }
}
