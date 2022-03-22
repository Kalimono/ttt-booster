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
    GridController gridController;

    void Awake() {
        soundFxController = FindObjectOfType<SoundFxController>();
        conditionController = FindObjectOfType<ConditionController>();
        timerController = FindObjectOfType<TimerController>();
        squareController = FindObjectOfType<SquareController>();
        gridController = FindObjectOfType<GridController>();
    }

    IEnumerator DelayNextTimerStart(float delayTime) {
        yield return new WaitForSeconds(delayTime);
        
        timerController.StartNextTimer();
    }

    public int GetNAdditionalRainbowStimuli(float currenTrialTimeOut, float stimuliLifetime) {
        // float additionalRainbowStimuli = currenTrialTimeOut/stimuliLifetime;
        // if(additionalRainbowStimuli < 3) additionalRainbowStimuli = 3;
        // Debug.Log("additional from function: " + additionalRainbowStimuli.ToString());
        float additionalRainbowStimuli = 3;
        return (int)additionalRainbowStimuli;
    }

    IEnumerator RunStimuli() {
        runningStims = true;
        // int traceStimStartIndex = (squareController.currentStimuliCells.Count-1)*2;
        // Debug.Log("currentRainbowCells " + squareController.currentRainbowCells.Count.ToString());
        // Debug.Log("currentStimuliCells " + squareController.currentStimuliCells.Count.ToString());
        int totalCellsCount = squareController.currentStimuliCells.Count + squareController.currentRainbowCells.Count;
        
        int currentStimuliIndex = 0;
        int currentRainbowStimuliIndex = 0;
        float stimuliLifetime = conditionController.stimuliLifetime/1000;
        // int nAdditionalRainbowStims = GetNAdditionalRainbowStimuli(squareController.currenTrialTimeOut, conditionController.stimuliLifetime);
        // Debug.Log(nAdditionalRainbowStims);
        // totalCellsCount += nAdditionalRainbowStims;
        // Debug.Log(totalCellsCount);
        List<Color> colorSequence = StimuliSequencer.GetRainbowColorSequence((int)conditionController.nRainbowStim);
        List<Color> colorSequenceAdditional = StimuliSequencer.GetRainbowColorSequence(squareController.nAdditionalRainbowstimuli);
        List<Color> combinedColorList = AddLists(colorSequence, colorSequenceAdditional);
        // List<Color> cCombinedColorList = StimuliSequencer.GetRainbowColorSequence(squareController.currentAdditionalRainbowCells.Count+squareController.currentRainbowCells.Count);
        // int colorSequenceAdditionalIndex = 0;
        int colorSequenceIndex = 0;
        
        // Debug.Log("totalCellsCount " + totalCellsCount.ToString());
        // Debug.Log("colorSequence " + colorSequence.Count.ToString());
        // Debug.Log("colorSequenceAdditional " + colorSequenceAdditional.Count.ToString());
        // Debug.Log("combinedColorList " + combinedColorList.Count.ToString());
        // Debug.Log("cCombinedColorList " + cCombinedColorList.Count.ToString());
        // Debug.Log("totalCellsCount " + totalCellsCount.ToString());
        for (int i = 0; i < totalCellsCount; i++) {
            // Debug.Log(i);
            if(i%2==0 && currentStimuliIndex < squareController.currentStimuliCells.Count) {
                squareController.currentStimuliCells[currentStimuliIndex].HighlightMeWhite(stimuliLifetime);
                currentStimuliIndex++;
                // traceStimStartIndex++;
            } else {
                // if(currentRainbowStimuliIndex < squareController.currentRainbowCells.Count) {
                    if(combinedColorList[colorSequenceIndex] == Color.blue) gridController.timedBlue.TimedBlueCell();
                    squareController.currentRainbowCells[currentRainbowStimuliIndex].HighlightMeColor(stimuliLifetime, combinedColorList[colorSequenceIndex]);
                    colorSequenceIndex++;
                    currentRainbowStimuliIndex++;
                    
                    // currentRainbowStimuliIndex++;
                // } else {
                    // squareController.currentStimuliCells[currentStimuliIndex].HighlightMeWhite(stimuliLifetime);
                    // currentStimuliIndex++;
                }
                // traceStimStartIndex++;
                
            // }
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

  public List<Color> AddLists(List<Color> list1, List<Color> list2) {
      List<Color> combinedList = new List<Color>();
      foreach(Color color in list1) {
          combinedList.Add(color);
      }
      foreach(Color color in list2) {
          combinedList.Add(color);
      }
      return combinedList;
  }
} //pre
    
    

