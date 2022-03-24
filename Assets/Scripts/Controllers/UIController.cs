using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour {
  public GameController gameController;
  public GridController gridController;
  public GameLogic gameLogic;
  public FurHatCommunication furHatCommunication;
  public LineSpawner lineSpawner;

  public ScoreBar scoreBarX;
  public ScoreBar scoreBarO;

  public GameObject gameOverPanel;
  public TextMeshProUGUI gameOverText;

  public GameObject ChoiceText;

  public GameObject startButton;
  public GameObject restartButton;
  public GameObject restartGameButton;

  public GameObject blueTimeObject;

  public TextMeshProUGUI currentLevel;
  public TextMeshProUGUI currentTotalTurn;

  // public InputField urlInput;

  // void Start() {
  //   urlInput.onValueChanged.AddListener(delegate {UrlInputValueChangeCheck(); });
  // }

  // void UrlInputValueChangeCheck() {
  //   Debug.Log(urlInput.text.ToString());
  //   furHatCommunication.FURHAT_URL = urlInput.text.ToString();
  // }

  public void UpdateTotalTurn(int currentTurn) {
    currentTotalTurn.text = currentTurn.ToString();
  }

  public void UpdateScoreBar() {
    ScoreBar scoreBarToUpdate = (gameController.activePlayer == gameController.playerX) ? scoreBarX : scoreBarO;
    scoreBarToUpdate.UpdateScoreBar();
  }

  public void ToggleTurnPanels(bool toggle) {
    foreach (ScoreBar bar in FindObjectsOfType<ScoreBar>()) {
      bar.turnPanel.SetActive(toggle);
    }
  }

  public void ShowTurnPanelActivePlayer(Player activePlayer) {
    GameObject turnPanel = (activePlayer == gameController.playerX) ? scoreBarX.turnPanel : scoreBarO.turnPanel;
    turnPanel.SetActive(true);
  }

  public void SetGameOverText(Player player) {
    if(gameController.playerX.nRoundsWon == 5 || gameController.playerO.nRoundsWon == 5 ) {
      gameOverText.text = (gameController.playerX.nRoundsWon > gameController.playerO.nRoundsWon) ? "YOU WIN THE GAME" : "AI WIN THE GAME";
    } else {
      gameOverText.text = (player == gameController.playerX) ? "YOU WIN" : "AI WIN";
    }
  }

  public void UpdateRoundsWon() {
    foreach (ScoreBar bar in FindObjectsOfType<ScoreBar>()) {
      bar.roundsWon = (bar == scoreBarX) ? gameController.playerX.nRoundsWon : gameController.playerO.nRoundsWon;
    }
  }

  public void ShowRoundsWonMarkers(bool toggle) {
    UpdateRoundsWon();
    foreach (ScoreBar scorebar in FindObjectsOfType<ScoreBar>()) {
      scorebar.roundsWon = (scorebar == scoreBarX) ? gameController.playerX.nRoundsWon : gameController.playerO.nRoundsWon;
      scorebar.LightRoundsWon(toggle);
    }
  }

   public void ResetRoundsWonMarkers() {
    foreach (ScoreBar scorebar in FindObjectsOfType<ScoreBar>()) {
      scorebar.DarkenRoundsWon();
    }
  }

  public void ResetScoreBarMarkers() {
    foreach (ScoreBar scoreBar in FindObjectsOfType<ScoreBar>()) {
      scoreBar.ResetScoreBar();
    }
  }

  void ChangeWinningCellMaterial() { //cell
    gameLogic.checkGridForWinningCells(gridController.grid);
    gridController.SetBoardInteractable(false);

    foreach (List<Cell> cellList in gameLogic.winningListOfLists) {
      foreach (Cell cell in cellList) {
        cell.shapeRenderer.material = cell.winningCellHighlight;
      }
    }
  }

  public void HighlightWin(Player player) {
    ChangeWinningCellMaterial();
    lineSpawner.DrawLineBetweenCells(gameLogic.winningListOfLists);
    if (player.score > 14) ChangeWinningScoreBarToHighlight();
  }

  void ChangeWinningScoreBarToHighlight() {
    ScoreBar lastPlayerScoreBar = (gameController.activePlayer == gameController.playerX) ? scoreBarO : scoreBarX;
    lastPlayerScoreBar.fill.material = lastPlayerScoreBar.highlightMaterial;
  }

  // public void HideMarkersAndFadeNonWinningCells() {
  //   gridController.ToggleFadeAllCells(true);
  //   gridController.SetCellValueVisibiltyToggle(false);
  //   foreach (List<Cell> cellList in gameLogic.winningListOfLists) {
  //     foreach (Cell cell in cellList) {
  //       SpriteRenderer vDisp = cell.valueDisplayer.GetComponent<SpriteRenderer>();
  //       vDisp.enabled = true;
  //       vDisp.sprite = (gameController.winningPlayer == gameController.playerX) ? cell.crossSprite : cell.noughtSprite;
  //       cell.Fade(false);
  //     }
  //   }
  // }

  public void FlashCellsCloseWin() {
    gameLogic.checkGridForCloseWin(gridController.grid);
    // StartCoroutine(CloseToWinningWave(gameLogic.closeToWinningListOfLists));
    FlashCloseToWinning();
  }

  public void FlashCloseToWinning() {
    if (gameLogic.closeToWinningListOfLists == null) return;
    foreach (List<Cell> cellList in gameLogic.closeToWinningListOfLists) {
      foreach (Cell cell in cellList) {
        // Debug.Log(cell);
        cell.GetComponent<FlashCell>().FlashWhite();
      }
    }
  }

  public void UpdateCurrentLevelText(int newLevel) {
    currentLevel.text = newLevel.ToString();
  }

  public GameObject DebugButtons;
  bool debug = true;

  public GameObject debugSettings;

  void ToggleDebug() {
    if (debug) {
      debugSettings.SetActive(false);
      debug = false;
    } else {
      debugSettings.SetActive(true);
      debug = true;
    }
  }

  void Update() {
    if (Input.GetKeyDown(KeyCode.Tab)) {
      ToggleDebug();
    }
  }

  public void EnableChoiceText() {
    ChoiceText.SetActive(true);
  }

  public void DisableChoiceText() {
    ChoiceText.SetActive(false);
  }
}
