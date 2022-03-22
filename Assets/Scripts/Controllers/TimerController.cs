using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum GameEvent {
  StartTurnDelay,
  PresentStimuli,
  TraceCondition,
  Response,
  EndTurnDelay
}

// public enum GameEvent {
//   PresentSquare,
//   PresentStimuli,
//   TraceCondition,
//   Turn,
//   EndTurnDelay,
//   ShowGameState
// }

[System.Serializable]
public class Timer {
  public GameEvent gameEvent;
  public float timeout;

  public Timer(GameEvent gameEvent, float timeout) {
    this.gameEvent = gameEvent;
    this.timeout = timeout;
  }
}

public class TimerController : MonoBehaviour {
  public Timer[] timers;
  public event System.Action<GameEvent> onTimerStarted;
  public event System.Action<GameEvent> onTimerFinished;
  public LevelSettings levelSettings;
  public ConditionController conditionController;
  public StimuliRunner stimuliRunner;
  public SquareController squareController;
  public FurHatCommunication furHatCommunication;

  private double timeElapsed;
  // private float currentTimerLength;
  private Coroutine activeTimer;

  private System.DateTime timerStarted;
  public int lastTimerIndex = 0;

  public Slider timerBarSlider;
  public GameObject timerBar;

  public bool pausForThresholdEvent = false;
  public bool pause;

  public float currentTimeout;

  void Update() {
    if (activeTimer != null && !pause) {
      timeElapsed = System.DateTime.Now.Subtract(timerStarted).TotalMilliseconds;
      TimerBarUpdate();
    }
  }

  public void StartNextTimer() {
    // Debug.Log(levelSettings.timers[lastTimerIndex].gameEvent);
    StartTimer(levelSettings.timers[lastTimerIndex]);

    lastTimerIndex++;
    if (lastTimerIndex > levelSettings.timers.Length - 1) {
      lastTimerIndex = 0;
    }
  }

  public void Reset() {
    lastTimerIndex = 0;
    AbortTimer();
    TimerBarDisplay(false);
  }

  void StartTimer(Timer timer) {
    timerBarSlider.maxValue = timer.timeout/10;
    // Debug.Log(timer.gameEvent);
    activeTimer = StartCoroutine(WaitForTimeout(timer));
  }

  IEnumerator WaitForTimeout(Timer timer) {
    timerStarted = System.DateTime.Now;

    if (onTimerStarted != null) {
      onTimerStarted(timer.gameEvent);
    }
    float timeOut = timer.timeout;

    // if (timer.gameEvent == GameEvent.TraceCondition) {
    //   timeOut = squareController.currenTrialTimeOut;
    //   }
    if (timer.gameEvent == GameEvent.Response) timeOut = conditionController.responseTime; 

    yield return new WaitForSeconds(timeOut / 1000);

    if(timer.gameEvent == GameEvent.Response) furHatCommunication.SendTimeout();

    if (onTimerFinished != null && !stimuliRunner.runningStims) {
      onTimerFinished(timer.gameEvent);
    }
  }

  public void AbortTimer() {
    StopCoroutine(activeTimer);
  }

  public void TimerBarDisplay(bool display) {
    if (display) {
      
      timerBar.SetActive(true);
    } else {
      timerBar.SetActive(false);
    }
  }

  public void TimerBarUpdate() {
    // timerBarSlider.maxValue = 500;
    timerBarSlider.value = timerBarSlider.maxValue - (float)Math.Round(timeElapsed / 10);
  }
}
