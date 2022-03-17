using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StimuliRunner : MonoBehaviour {
    // List<Cell> cellList;
    // float startTime;
    // int indexCounter;
    
    public bool runningStims = false;
    // bool rainbow = false;

    SoundFxController soundFxController;
    ConditionController conditionController;
    TimerController timerController;
    SquareController squareController;

    void Awake() {
        soundFxController = FindObjectOfType<SoundFxController>();
        conditionController = FindObjectOfType<ConditionController>();
        timerController = FindObjectOfType<TimerController>();
        squareController = FindObjectOfType<SquareController>();
    }

    IEnumerator DelayNextTimerStart(float delayTime) {
        yield return new WaitForSeconds(delayTime);
        
        timerController.StartNextTimer();
    }

    IEnumerator RunStimuli() {
        runningStims = true;
        int totalCellsCount = squareController.currentStimuliCells.Count + squareController.currentRainbowCells.Count;
        int currentStimuliIndex = 0;
        int currentRainbowStimuliIndex = 0;
        float stimuliLifetime = conditionController.stimuliLifetime/1000;
        // Debug.Log(stimuliLifetime);
        for (int i = 0; i < totalCellsCount; i++) {
            if(i%2==0 && currentStimuliIndex < squareController.currentStimuliCells.Count) {
                squareController.currentStimuliCells[currentStimuliIndex].HighlightMeWhite(stimuliLifetime);
                currentStimuliIndex++;
            } else {
                if(currentRainbowStimuliIndex < squareController.currentRainbowCells.Count) {
                    squareController.currentRainbowCells[currentRainbowStimuliIndex].HighlightMeRainbow(stimuliLifetime);
                    currentRainbowStimuliIndex++;
                } else {
                    squareController.currentStimuliCells[currentStimuliIndex].HighlightMeWhite(stimuliLifetime);
                    currentStimuliIndex++;
                }
                
            }
            yield return new WaitForSeconds(stimuliLifetime);
        }
        timerController.StartNextTimer();
        runningStims = false;
    }

    public void RunMixedStimuli() {
        StartCoroutine(RunStimuli());
    }

    public List<Cell> RandomizeListOrder(List<Cell> list) {
    System.Random rng = new System.Random();
    var randomizedList = list.OrderBy(a => rng.Next()).ToList();

    return randomizedList;
  }
} //pre
    
    

