using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debugger : MonoBehaviour
{   
    public Text currentTimer;
    private TimerController timer;

    void Start() {
        timer = FindObjectOfType<TimerController>();
        timer.onTimerFinished += OnTimerFinished;
    }

    void OnTimerFinished (GameEvent gameEvent) {
        currentTimer.text = gameEvent.ToString() + ", " + timer.lastTimerIndex;
    }
}
