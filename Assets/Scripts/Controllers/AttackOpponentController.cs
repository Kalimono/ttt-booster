using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackOpponentController : MonoBehaviour {
  GameController gameController;
  UIController uiController;
  GridController gridController;
  AutoTurnEnderController autoTurnEnderController;

  ScoreBar parentScoreBar;

  float starttime;
  float timeElapsed;
  float totalTime = 5f;

  public Slider timerBarSliderTactical;
  public GameObject timerBarTactical;

  public bool ReductionEventActive = false;

  void Awake() {
    gameController = FindObjectOfType<GameController>();
    uiController = FindObjectOfType<UIController>();
    gridController = FindObjectOfType<GridController>();
    autoTurnEnderController = FindObjectOfType<AutoTurnEnderController>();
    parentScoreBar = GetComponentInParent<ScoreBar>();
    timerBarSliderTactical = timerBarSliderTactical.GetComponent<Slider>();
  }

  public ScoreBar GetOpponentScoreBar() {
    Player activePlayer = gameController.activePlayer;
    ScoreBar opponentScoreBar = (activePlayer == gameController.playerX) ? uiController.scoreBarO : uiController.scoreBarX;
    return opponentScoreBar;
  }

  public void StartReductionEvent() {
    TacticalTimerBarToggle(true);
    // timerBarSlider.maxValue = totalTime;
    // timerBarSlider.value = totalTime;
    uiController.EnableChoiceText();
    Time.timeScale = 0;
    GetOpponentScoreBar().EnableInteraction();
    gridController.EnableOpponentCellsForClear();
    gridController.SetCellValueVisibiltyToggle(true);
    gridController.ToggleFadeAllCells(false);
    ReductionEventActive = true;
    starttime = Time.time;
    timeElapsed = 0;
    if(gameController.activePlayer == gameController.playerO || autoTurnEnderController.toggleAutoPlay) StartCoroutine(AIRandomChoice());
    // Debug.Log(starttime);
  }

  public void EndReductionEvent() {
    TacticalTimerBarToggle(false);
    uiController.DisableChoiceText();
    ReductionEventActive = false;
    Time.timeScale = 1;
    GetOpponentScoreBar().DisableInteraction();
    gridController.DisableOpponentCellsForClear();
    foreach(PulseScoreMarker pulseScoreMarker in FindObjectsOfType<PulseScoreMarker>()) {
      if(pulseScoreMarker.pulse) pulseScoreMarker.Stop();
    }
  }

  public void TacticalTimerBarToggle(bool toggle) {
      timerBarTactical.SetActive(toggle);
  }

  void Update() {
    if(ReductionEventActive) {
      Debug.Log(totalTime-timeElapsed);
      timerBarSliderTactical.value = totalTime-timeElapsed;
      if(timeElapsed > totalTime) EndReductionEvent();
      if(!gridController.rotation) timeElapsed+=Time.unscaledDeltaTime;
    }
  }

  IEnumerator AIRandomChoice() {
    yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(1f, 1.8f)); 
    int randint = UnityEngine.Random.Range(0, 2);
    if(randint == 0) {

      GetOpponentScoreBar().Clicked();
    } else {
      gridController.ClickRandomOpponentCell();
    }
    
  }
}
