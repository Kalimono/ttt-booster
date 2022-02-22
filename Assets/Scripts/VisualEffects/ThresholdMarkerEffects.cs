using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThresholdMarkerEffects : MonoBehaviour {
  GameController gameController;
  TimerController timerController;
  GridController gridController;

  public Slider playerScoreSlider;

  public ThresholdMarker firstThresholdMarker;
  public ThresholdMarker secondThresholdMarker;

  public ThresholdMarker activeThresholdMarker;

  public ScoreBar parentScoreBar;
  // public ReduceOpponentController reduceOpponentController;

  public bool firstThresholdPassed = false;
  public bool secondThresholdPassed = false;

  void Awake() {
    gameController = FindObjectOfType<GameController>();
    timerController = FindObjectOfType<TimerController>();
    gridController = FindObjectOfType<GridController>();
    parentScoreBar = GetComponentInParent<ScoreBar>();
    // reduceOpponentController = GetComponent<ReduceOpponentController>(); 
  }

  void Update() {
    if (!firstThresholdPassed && playerScoreSlider.value > 5f) {
      firstThresholdMarker.ThresholdPassed();
      firstThresholdPassed = true;
      activeThresholdMarker = firstThresholdMarker;
      parentScoreBar.firstThresholdPassed = true;
    }

    if (!secondThresholdPassed && playerScoreSlider.value > 10f) {
      secondThresholdMarker.ThresholdPassed();
      secondThresholdPassed = true;
      activeThresholdMarker = secondThresholdMarker;
      parentScoreBar.secondThresholdPassed = true;
    }
  }

  public void ResetThresholds() {
    firstThresholdPassed = false;
    secondThresholdPassed = false;
    activeThresholdMarker = null;
    parentScoreBar.firstThresholdPassed = false;
    parentScoreBar.secondThresholdPassed = false;
  }
}
