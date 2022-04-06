using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThresholdMarker : MonoBehaviour {
  public PulseScoreMarker pulseScoreMarker;
  GameController gameController;
  UIController uiController;
  AttackOpponentController attackOpponentController;

  public ScoreBar scoreBarX;
  public ScoreBar scoreBarO;

  public ThresholdMarker thresholdMarker;

  public Material thresholdMarkerOn;
  public Material thresholdMarkerOff;

  public AudioSource thresholdMarkerAudioSource;

  public AudioClip thresholdSound;

  float pulseTime = 2f;

  void Awake() {
    pulseScoreMarker = GetComponent<PulseScoreMarker>();
    gameController = FindObjectOfType<GameController>();
    uiController = FindObjectOfType<UIController>();
    attackOpponentController = FindObjectOfType<AttackOpponentController>();
  }

  public void ThresholdPassed() {
    // thresholdMarkerAudioSource.PlayOneShot(thresholdSound);
    // pulseScoreMarker.StartPulse(pulseTime);

    // if (!gameController.strategicElements) {
    //   if (gameController.winningPlayer == gameController.playerNull) AttackOpponentScoreBar();
    // } else {
    //   attackOpponentController.StartReductionEvent();
    // }
  }

  public void AttackOpponentScoreBar() {
    ScoreBar opponentScoreBar = GetOpponentScoreBar();
    opponentScoreBar.ReduceScoreBar(pulseTime);
    ReduceOpponentScoreBy(1);
  }

  public ScoreBar GetOpponentScoreBar() {
    Player activePlayer = gameController.activePlayer;
    ScoreBar opponentScoreBar = (activePlayer == gameController.playerX) ? uiController.scoreBarO : uiController.scoreBarX;
    return opponentScoreBar;
  }

  public void ReduceOpponentScoreBy(int amount) {
    Player opponent = (gameController.activePlayer == gameController.playerX) ? gameController.playerO : gameController.playerX;
    opponent.score -= amount;
  }
}
