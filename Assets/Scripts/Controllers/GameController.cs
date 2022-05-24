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
  public DataSave dataSave;
  public CursorController cursorController;
  public FurHatCommunication furHatCommunication;
  public iMotionsCommunications iMotionsCommunications;

  private TimerController timer;

  public Player activePlayer;
  public bool roundActive;

  public Player winningPlayer; 

  public int turnNum;
  public int currentRound;
  public int totalTurn;
  bool conditionFinished = false;

  float reactionTime;
  // public bool strategicElements = true;
  // public Image stratstatus;

  public int nTrialsCond;
  public int nConds;


  void Awake() {
    cursorController = FindObjectOfType<CursorController>();
    timer = FindObjectOfType<TimerController>();
    timer.onTimerFinished += OnTimerFinished;
    nConds = levelController.levels.Length;
    // Debug.Log(levelController.levels.Length);
    // autoTurnEnderController.Init(levelController.levels[0]);
    // dataPoster.InitializeGame(levelController.levels[0]);
    // Debug.Log("awake");
    if(sceneController.GetMemory() > 0) levelController.LoadNextLevel();
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
    gridController.CreateGrid();
    gridController.SetCellValuesToNone();
    gridController.lastCellInteractedWith = null;
    StimuliSequencer.CreateSequences();
    uiController.gameOverPanel.SetActive(false);
    uiController.startButton.SetActive(false);
    uiController.restartButton.SetActive(false);
    activePlayer = playerX;
    uiController.ShowRoundsWonMarkers(true);
    uiController.ResetScoreBarMarkers();
    uiController.ResetRoundsWonMarkers();
    ResetPoints();
    turnNum = 0;
  }

  public void StartGame() {
    // Debug.Log("start game");
    uiController.ResetScoreBarMarkers();
    winningPlayer = playerNull;
    ResetGameState();
    PreStartTurn();
    // squareController.Initialize();
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

  public GameValue GetPlayerSide() {
    return activePlayer.value;
  }


  void PreStartTurn() {
    // sceneController.LoadSurveyScene();
    // iMotionsCommunications.SendStartMarker();
    if(conditionFinished) {
      // GameObject buttonToactivate;

      // if(totalTurn >= nTrialsCond*nConds) {
        
      // // } else {
      //   // buttonToactivate = uiController.restartGameButton;
      //   uiController.SetGameOverText(playerX); 
      //   furHatCommunication.SendEnd();
      //   GameOver(playerX);
      //   return;
      // }

      
      sceneController.LoadSurveyScene();
      // buttonToactivate = uiController.restartButton;
      // buttonToactivate.SetActive(true);
      // uiController.SetGameOverText(playerX); 
      conditionFinished = false;
      // turnNum = 0;
      // GameOver(playerX);
      
      return;
    } else {
      gridController.ToggleFadeAllCells(false);
      gridController.SetCellValueVisibiltyToggle(false);
      roundActive = true;
      gridController.lastCellInteractedWith = null;
      squareController.PrepareStimuliPhase();
      squareController.FlashMiddleCell(.5f);
      cursorController.CenterAndLockCursor();
      // iMotionsCommunications.SendStartMarker();
      timer.StartNextTimer();
    }
  }

  void StimuliPhase() {
    squareController.PresentStimuli();
    gridController.SetBoardInteractable(false);
    timer.TimerBarDisplay(false);
    gridController.SetCellValueVisibiltyToggle(false);
  }

  void TraceCondition() { 
    // iMotionsCommunications.SendStopMarker();
    // iMotionsCommunications.SendEndMarker();
    gridController.SetCellValueVisibiltyToggle(false);
    timer.StartNextTimer();
  }

  void ResponsePhase() {
    cursorController.UnlockCursor();
    squareController.ToggleOptions(true);
    timer.TimerBarDisplay(true);
    soundFxController.PlayClock();
    gridController.ToggleFadeAllCells(true);
    squareController.correctCell.Fade(false);

    reactionTime = Time.time;
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
    dataSave.reactionTime = Time.time-reactionTime;
    reactionTime = 0f;
    // squareController.HideSquares(); ###
    // gridController.FadeCellsExceptLastCellInteractedWith(); ###
    gridController.SetCellValueVisibiltyToggle(false);
    // uiController.ToggleTurnPanels(false);
    // gridController.ToggleFadeAllCells(true);
    timer.TimerBarDisplay(false);
    timer.AbortTimer();
    gridController.SetBoardInteractable(false);
    soundFxController.StopClock();

    dataSave.WriteRoundDataString();
    // sceneController.ChangeScene();
    // if (wasCorrectMove) UpdateScore(activePlayer); ###

    // if(CheckIfPassedThreshold(activePlayer.score) && !gameLogic.checkGridForWin(gridController.grid)) timer.pausForThresholdEvent = true;
    // if (gameLogic.checkGridForWin(gridController.grid)) { ###
    //   activePlayer.nRoundsWon++;
    //   winningPlayer = activePlayer;
    // } else {
    //   gridController.SetBoardInteractable(false);
    // sceneController.LoadSurveyScene();
    turnNum++;
    totalTurn++;
    uiController.UpdateTotalTurn(totalTurn);

    // if(totalTurn == 96) GameOver(playerX);
    if(turnNum == nTrialsCond) conditionFinished = true;
    // if(turnNum > 1) sceneController.SwitchToSurveyScene();
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
    // currentRound++;
    gridController.SetBoardInteractable(false);
    
    gridController.FadeCellsExceptWinning(); 
    roundActive = false;

    gridController.ToggleFadeAllCells(true);
    timer.TimerBarDisplay(false);
    squareController.ToggleOptions(false);

    squareController.Reset();
    // uiController.ToggleTurnPanels(false); 
    uiController.gameOverPanel.SetActive(true);
    soundFxController.PlayGameOverSound(playerX);
    // gridController.ClearHoverMarkers();
    
    // Debug.Log(turnNum);
    // turnNum = 0;

    // if (playerX.nRoundsWon == 5 || playerO.nRoundsWon == 5) {
    //   uiController.restartButton.SetActive(false); 
    //   uiController.restartGameButton.SetActive(true);
    //   }
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

  // public int whiteCorrect;
  // public List<float> rTimeBlue;
  // public int nResponses;
  // public float isi;


  // public void WriteString() {
  //   string path = "Assets/test.txt";
 
  //   //Write some text to the test.txt file
  //   StreamWriter writer = new StreamWriter(path, true);
  //   writer.WriteLine(GetRoundDataString());
  //   writer.Close();
  // }

  // public string GetRoundDataString() {
  //   string dataString = whiteCorrect.ToString() + "," + conditionController.nResponses.ToString() + "," + squareController.currenTrialTimeOut.ToString();
  //   foreach(float rTime in rTimeBlue) {
  //     dataString += ",";
  //     dataString += rTime.ToString();
  //   }
  //   return dataString;
  // }
} 
