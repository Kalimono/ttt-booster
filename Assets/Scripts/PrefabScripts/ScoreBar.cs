using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ScoreBar : MonoBehaviour {
  public GameController gameController;
  public GridController gridController;
  public UIController uiController;
  public TimerController timerController;
  public AttackOpponentController attackOpponentController;

  public Image fill;
  public Material highlightMaterial;
  public Material defaultFillMaterialX;
  public Material defaultFillMaterialO;
  public Material highlightInteract;
  public Material defaultMaterialFill;

  public string myName;
  public Slider playerScoreSlider;
  public GameObject playerScoreBar;
  public ScoreFillEffects scoreFillEffects;
  public GameObject turnPanel;
  public TextMeshProUGUI turnPanelText;
  public GameObject roundMarker;
  public GameObject[] roundMarkers;
  // public GameObject crosses;

  public GameObject meshCollider;

  public AudioSource scoreLossAudioSource;
  public AudioClip scoreLoss;

  public int roundsWon;
  float pulseTime;

  public bool firstThresholdPassed = false;
  public bool secondThresholdPassed = false;

  public void EnableInteraction() {
    meshCollider.SetActive(true);
    fill.material = highlightInteract;
    GetComponentInChildren<PulseCross>().Toggle(true);
  }

  public void DisableInteraction() {
    meshCollider.SetActive(false);
    fill.material = defaultMaterialFill;
    GetComponentInChildren<PulseCross>().Toggle(false);
    attackOpponentController.TacticalTimerBarToggle(false);
    uiController.DisableChoiceText();
  }

  WaitForSecondsRealtime waitHalfSecond = new WaitForSecondsRealtime(.5f);
  WaitForSecondsRealtime waitForSecond = new WaitForSecondsRealtime(1f);

  void Awake() {
    gameController = FindObjectOfType<GameController>();
    gridController = FindObjectOfType<GridController>();
    uiController = FindObjectOfType<UIController>();
    timerController = FindObjectOfType<TimerController>();
    attackOpponentController = FindObjectOfType<AttackOpponentController>();
    SetTurnPanelText();
    defaultMaterialFill = fill.material;
  }

  public void Clicked() {
    attackOpponentController.EndReductionEvent();
    Debug.Log("Clicked " +  myName);
    // GetComponentInChildren<ThresholdMarker>().ReduceOpponentScoreBy(1);
    Player PlayerScore = (myName == "X") ? gameController.playerX : gameController.playerO;
    ScoreBar myScoreBar = (myName == "X") ? uiController.scoreBarX : uiController.scoreBarO;
    myScoreBar.ReduceScoreBar(2f);
    PlayerScore.score--;
    // ReduceScore(2f, 1f);
  }

  void SetTurnPanelText() {
    turnPanelText.text = (myName == "X") ? "Your\nturn" : "AI\nturn";
  }

  public void UpdateScoreBar() {
    StartCoroutine(FillScore(1f));
  }

  public void ReduceScoreBar(float duration) {
    Debug.Log("reduct");
    StartCoroutine(ReduceScore(duration, 1f));
  }

  IEnumerator FillScore(float pulseTime) {
    WaitForSeconds wait = new WaitForSeconds((pulseTime+.35f) / 100);
    
    yield return waitHalfSecond;
    float scoreIncome = (float)gridController.lastCellInteractedWith.outcomeValue;
    float targetScore = (playerScoreSlider.value + scoreIncome > 15) ? 15 : playerScoreSlider.value + scoreIncome;
    Color pulseColor = (gridController.lastCellInteractedWith.outcomeColor == OutcomeColor.gold) ? scoreFillEffects.gold : scoreFillEffects.diamond;
    if(gameController.activePlayer == gameController.playerO) pulseColor = scoreFillEffects.green;
    scoreFillEffects.Pulse(pulseColor, pulseTime);
    while (playerScoreSlider.value < targetScore) {
      playerScoreSlider.value += scoreIncome / 100;
      yield return wait;
    }
    playerScoreSlider.value = Mathf.Round(playerScoreSlider.value);
    scoreFillEffects.Stop();
  }
  

  IEnumerator ReduceScore(float pulseTime, float amount) {
    Debug.Log("reducing");
    WaitForSecondsRealtime wait = new WaitForSecondsRealtime(pulseTime*1.15f / 100);
    yield return waitForSecond;
    PlayScoreLossAudio();
    float targetScore = (playerScoreSlider.value - amount < 0) ? 0 : playerScoreSlider.value - amount;
    scoreFillEffects.Pulse(scoreFillEffects.red, pulseTime);
    // pulsatingReduction = true;
    while (playerScoreSlider.value > targetScore) {
      playerScoreSlider.value -= amount / 100;
      yield return wait;
    }
    playerScoreSlider.value = Mathf.Round(playerScoreSlider.value);
    scoreFillEffects.Stop();
    TurnOffOpponentPulsatingScoreMarkers();
  }

  void TurnOffOpponentPulsatingScoreMarkers() {
    ScoreBar opponentScoreBar = (myName == "X") ? uiController.scoreBarO : uiController.scoreBarX;
    foreach(PulseScoreMarker pulseScoremarker in opponentScoreBar.GetComponentsInChildren<PulseScoreMarker>()) {
      if(pulseScoremarker.pulse) pulseScoremarker.Stop();
    } 
  }

  public void LightRoundsWon(bool toggle) {
    for (int i = 0; i < roundsWon; i++) {
      SpriteRenderer renderer = roundMarkers[i].GetComponent<SpriteRenderer>();
      Color newColor = renderer.color;
      newColor.a = 1;
      renderer.color = newColor;
    }
    roundMarker.SetActive(toggle);
  }

  public void DarkenRoundsWon() {
    for (int i = 0; i < roundsWon; i++) {
      SpriteRenderer renderer = roundMarkers[i].GetComponent<SpriteRenderer>();
      Color newColor = renderer.color;
      Debug.Log(newColor.a);
      newColor.a = 0.235f;
      renderer.color = newColor;
    }
  }

  public void PlayScoreLossAudio() {
    scoreLossAudioSource.PlayOneShot(scoreLoss);
  }

  public void ResetScoreBar() {
    fill.material = (myName == "X") ? defaultFillMaterialX : defaultFillMaterialO;
    foreach(ThresholdMarker thresholdMarker in GetComponentsInChildren<ThresholdMarker>()) {
      thresholdMarker.GetComponent<Image>().material = thresholdMarker.thresholdMarkerOff;
      thresholdMarker.pulseScoreMarker.pulse = false;
    }

    foreach(ThresholdMarkerEffects markerEffects in GetComponentsInChildren<ThresholdMarkerEffects>()) {
      markerEffects.ResetThresholds();
    }
  }
}
