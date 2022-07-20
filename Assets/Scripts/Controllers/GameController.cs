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
  public SquareController squareController;
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

  public int nTrialsCond;
  public int nConds;


  void Awake() {
    cursorController = FindObjectOfType<CursorController>();
    timer = FindObjectOfType<TimerController>();
    timer.onTimerFinished += OnTimerFinished;
    nConds = levelController.levels.Length;
    if(sceneController.GetMemory() > 0) levelController.LoadNextLevel();
  }

  void Start() {
    uiController.startButton.SetActive(true);
    playerX.nRoundsWon = 0;
    playerO.nRoundsWon = 0;
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
    uiController.ResetScoreBarMarkers();
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
    }
  }

  public GameValue GetPlayerSide() {
    return activePlayer.value;
  }


  void PreStartTurn() {
    if(conditionFinished) {
      sceneController.LoadSurveyScene();
      conditionFinished = false;
      
      return;
    } else {
      gridController.ToggleFadeAllCells(false);
      gridController.SetCellValueVisibiltyToggle(false);
      roundActive = true;
      gridController.lastCellInteractedWith = null;
      squareController.PrepareStimuliPhase();
      squareController.FlashMiddleCell(.5f);
      cursorController.CenterAndLockCursor();
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

  public void EndTurn(){
    dataSave.reactionTime = Time.time-reactionTime;
    reactionTime = 0f;
    gridController.SetCellValueVisibiltyToggle(false);
    timer.TimerBarDisplay(false);
    timer.AbortTimer();
    gridController.SetBoardInteractable(false);
    soundFxController.StopClock();

    dataSave.WriteRoundDataString();
    turnNum++;
    totalTurn++;
    uiController.UpdateTotalTurn(totalTurn);

    if(turnNum == nTrialsCond) conditionFinished = true;
    timer.StartNextTimer();
  }

  void GameOver(Player winningPlayer) {
    gridController.SetBoardInteractable(false);
    gridController.FadeCellsExceptWinning(); 
    roundActive = false;

    gridController.ToggleFadeAllCells(true);
    timer.TimerBarDisplay(false);
    squareController.ToggleOptions(false);

    squareController.Reset();
    uiController.gameOverPanel.SetActive(true);
    soundFxController.PlayGameOverSound(playerX);
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
} 
