using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public enum GameValue {
  None,
  Cross,
  Nought
}

[System.Serializable]
public class Player {
  public GameValue value;
  public int score;
  public int nRoundsWon;
}

public class GameController : MonoBehaviour {
  public Player playerX;
  public Player playerO;
  public Player playerNull;

  public GridController gridController;

  public SoundFxController soundFxController;
  public UIController uiController;
  public AutoTurnEnderController autoTurnEnderController;
  public GameLogic gameLogic;
  public SquareController squareController;
  public LineSpawner lineSpawner;
  public DataPoster dataPoster;
  public DotController dotController;
  public SceneController sceneController;
  public ConditionController conditionController;
  public LevelController levelController;

  private TimerController timer;

  public Player activePlayer;
  public bool roundActive;

  public Player winningPlayer; 

  public int turnNum;
  public int currentRound;

  float reactionTimeStart;
  // public bool strategicElements = true;
  // public Image stratstatus;



  void Awake() {
    timer = FindObjectOfType<TimerController>();
    timer.onTimerFinished += OnTimerFinished;
    // autoTurnEnderController.Init(levelController.levels[0]);
    // dataPoster.InitializeGame(levelController.levels[0]);
    // Debug.Log("awake");
  }

  void Start() {
    uiController.startButton.SetActive(true);
    playerX.nRoundsWon = 0;
    playerO.nRoundsWon = 0;
    
    
    // dataPoster.SendHi();
    // sceneController.ChangeScene();
    // Application.targetFrameRate = 60;
    // Debug.Log(Application.targetFrameRate);
    // Debug.Log("start");
  }

  public void StartNewGame() {
    uiController.restartGameButton.SetActive(false);
    uiController.ResetRoundsWonMarkers();
    playerX.nRoundsWon = 0;
    playerO.nRoundsWon = 0;
    currentRound = 0;
    StartGame();
  }

  void ResetGameState() {
    // Debug.Log("reset");
    gridController.CreateGrid();
    gridController.SetCellValuesToNone();
    // squareController.CutGridIntoAreas(gridController.grid);  ###
    if(dotController.toggleDot) dotController.SetOutcomeAreas(gridController.grid);
    // dotController.AssignOutcomes();
    // squareController.Initialize();
    gridController.lastCellInteractedWith = null;
    StimuliSequencer.CreateSequences();
    uiController.gameOverPanel.SetActive(false);
    uiController.startButton.SetActive(false);
    uiController.restartButton.SetActive(false);
    activePlayer = playerX;
    uiController.ShowRoundsWonMarkers(false);
    uiController.ResetScoreBarMarkers();
    ResetPoints();
    // squareController.Initialize();
    turnNum = 1;
  }

  public void StartGame() {
    // Debug.Log("start game");
    winningPlayer = playerNull;
    ResetGameState();
    PreStartTurn();
  }

  void OnTimerFinished(GameEvent gameEvent) {
    if (!roundActive) return;

    switch (gameEvent) {
      case GameEvent.StartTurnDelay:
        StimuliPhase();
        break;
      case GameEvent.PresentStimuli:
        TraceCondition();
        break;
      case GameEvent.TraceCondition:
        ResponsePhase();
        break;
      case GameEvent.Response:
        EndTurn();//false); ###
        break;
      case GameEvent.EndTurnDelay:
        PreStartTurn();//false); ###
        break;
      // case GameEvent.EndTurnDelay:
      //   PresentBoardState();
      //   break;
      // case GameEvent.ShowGameState:
      //   PreStartTurn();
      //   break;
    }
  }

  // void OnTimerFinished(GameEvent gameEvent) { ###
  //   if (!roundActive) return;

  //   switch (gameEvent) {
  //     case GameEvent.PresentSquare:
  //       StartTurn();
  //       break;
  //     case GameEvent.PresentStimuli:
  //       HideStimuli();
  //       break;
  //     case GameEvent.TraceCondition:
  //       ActivatePlayerInteraction();
  //       break;
  //     case GameEvent.Turn:
  //       EndTurn();//false); ###
  //       break;
  //     case GameEvent.EndTurnDelay:
  //       PresentBoardState();
  //       break;
  //     case GameEvent.ShowGameState:
  //       PreStartTurn();
  //       break;
  //   }
  // }

  public GameValue GetPlayerSide() {
    return activePlayer.value;
  }

  void PreStartTurn() {
    // sceneController.ChangeScene();
    rTimeBlue.Clear();
    gridController.ToggleFadeAllCells(false);
    if(winningPlayer != playerNull) {
      GameOver(winningPlayer);
      return;
    }
    gridController.SetCellValueVisibiltyToggle(false);
    roundActive = true;
    gridController.lastCellInteractedWith = null;
    uiController.ShowTurnPanelActivePlayer(activePlayer);
    squareController.PrepareStimuliPhase();
    timer.StartNextTimer();
  }

  void StimuliPhase() {
    squareController.PresentStimuli();
    gridController.SetBoardInteractable(false);
    timer.TimerBarDisplay(false);
    gridController.SetCellValueVisibiltyToggle(false);
  }

  void TraceCondition() { 
    gridController.SetCellValueVisibiltyToggle(false);
    timer.StartNextTimer();
  }

  void ResponsePhase() {
      squareController.ToggleOptions(true);

    timer.TimerBarDisplay(true);
    soundFxController.PlayClock();
    gridController.ToggleFadeAllCells(true);
    squareController.correctCell.Fade(false);


    reactionTimeStart = Time.time;
    timer.StartNextTimer();
  }

  public void EndTurn(){//bool wasCorrectMove) { ###
    // dataPoster.SendTurn(turnNum, 
    //   currentRound,
    //   dotController.toggleDot,
    //   squareController.correctCell.outcomeArea,
    //   squareController.squarePositionIndex,
    //   squareController.stimuliIndex,  
    //   Time.time-reactionTimeStart, 
    //   (activePlayer == playerX) ? "X" : "O", 
    //   wasCorrectMove, 
    //   ((Time.time-reactionTimeStart)*1000 > timer.levelSettings.timers[timer.lastTimerIndex-1].timeout) ? 1 : 0,
    //   squareController.distractorPosition,
    //   squareController.distractorIndex); 
    
    // reactionTimeStart = 0f;
    // squareController.HideSquares(); ###
    // gridController.FadeCellsExceptLastCellInteractedWith(); ###
    gridController.SetCellValueVisibiltyToggle(false);
    uiController.ToggleTurnPanels(false);
    timer.TimerBarDisplay(false);
    timer.AbortTimer();
    gridController.SetBoardInteractable(false);
    soundFxController.StopClock();
    // WriteString();
    // sceneController.SwitchToSurveyScene();
    // if (wasCorrectMove) UpdateScore(activePlayer); ###

    // if(CheckIfPassedThreshold(activePlayer.score) && !gameLogic.checkGridForWin(gridController.grid)) timer.pausForThresholdEvent = true;
    // if (gameLogic.checkGridForWin(gridController.grid)) { ###
    //   activePlayer.nRoundsWon++;
    //   winningPlayer = activePlayer;
    // } else {
    //   gridController.SetBoardInteractable(false);
    turnNum++;

    if(turnNum > 1) levelController.LoadNextLevel();
    // } ###
    timer.StartNextTimer();
  }

  // void PresentBoardState() { ###
  //   //activePlayer = (activePlayer == playerX) ? playerO : playerX; ###
  //   gridController.SetCellValueVisibiltyToggle(true);
  //   gridController.SetBoardInteractable(false);
    
  //   if(winningPlayer == playerNull) uiController.FlashCellsCloseWin();
  //   gridController.ToggleFadeAllCells(false);

  //   if(winningPlayer != playerNull) uiController.HighlightWin(winningPlayer);

  //   timer.StartNextTimer();
  // }

  void GameOver(Player winningPlayer) {
    currentRound++;
    gridController.SetBoardInteractable(false);
    gridController.FadeCellsExceptWinning(); 
    roundActive = false;
    if(currentRound < 9) uiController.restartButton.SetActive(true); 
    uiController.ShowRoundsWonMarkers(true); 
    uiController.SetGameOverText(winningPlayer); 
    uiController.ToggleTurnPanels(false); 
    uiController.gameOverPanel.SetActive(true);
    soundFxController.PlayGameOverSound(winningPlayer);
    gridController.ClearHoverMarkers();
    
    // Debug.Log(turnNum);

    if (playerX.nRoundsWon == 5 || playerO.nRoundsWon == 5) {
      uiController.restartButton.SetActive(false); 
      uiController.restartGameButton.SetActive(true);
      }
    timer.Reset();
  }

  void UpdateScore(Player player) { 
    int updateValue = gridController.lastCellInteractedWith.outcomeValue; 
    player.score += updateValue;
    uiController.UpdateScoreBar(); 
  }

  void ResetPoints() {
    foreach(ScoreBar bar in FindObjectsOfType<ScoreBar>()) bar.GetComponent<Slider>().value = 0f;
    playerX.score = 0;
    playerO.score = 0;
  }

  // bool CheckIfPassedThreshold(int newScore) {
  //   List<int> thresholdValues = new List<int>{5, 6, 7, 10, 11, 12};
  //   if(thresholdValues.Contains(newScore)) return true;
  //   return false;
  // }
  // public void ToggleStrats() {
  //   if (strategicElements) {
  //     strategicElements = false;
  //     stratstatus.color = Color.red;
  //   } else {
  //     strategicElements = true;
  //     stratstatus.color = Color.green;
  //   }
  // } pre

  public int whiteCorrect;
  public List<float> rTimeBlue;
  public int nResponses;
  public float isi;


  public void WriteString() {
    string path = "Assets/test.txt";
 
    //Write some text to the test.txt file
    StreamWriter writer = new StreamWriter(path, true);
    writer.WriteLine(GetRoundDataString());
    writer.Close();
  }

  public string GetRoundDataString() {
    string dataString = whiteCorrect.ToString() + "," + conditionController.nResponses.ToString() + "," + squareController.currenTrialTimeOut.ToString();
    foreach(float rTime in rTimeBlue) {
      dataString += ",";
      dataString += rTime.ToString();
    }
    return dataString;
  }
} 
