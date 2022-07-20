using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StimuliRunner : MonoBehaviour {
    public bool runningStims = false;

    ConditionController conditionController;
    TimerController timerController;
    SquareController squareController;
    GridController gridController;
    DataSave dataSave;

    void Awake() {
        conditionController = FindObjectOfType<ConditionController>();
        timerController = FindObjectOfType<TimerController>();
        squareController = FindObjectOfType<SquareController>();
        gridController = FindObjectOfType<GridController>();
        dataSave = FindObjectOfType<DataSave>();
    }

    IEnumerator DelayNextTimerStart(float delayTime) {
        yield return new WaitForSeconds(delayTime);
        
        timerController.StartNextTimer();
    }

    public int GetNAdditionalRainbowStimuli(float currenTrialTimeOut, float stimuliLifetime) {
        dataSave.isi = currenTrialTimeOut;
        float additionalRainbowStimuli = (currenTrialTimeOut == 15000f) ? 12f : 4f;
        return (int)additionalRainbowStimuli;
    }

    IEnumerator RunStimuli() {
        runningStims = true;

        int totalCellsCount = squareController.currentStimuliCells.Count + squareController.currentRainbowCells.Count;
        
        int currentStimuliIndex = 0;
        int currentRainbowStimuliIndex = 0;
        float stimuliLifetime = conditionController.stimuliLifetime/1000;

        List<Color> colorSequence = StimuliSequencer.GetRainbowColorSequence((int)conditionController.nRainbowStim);
        List<Color> colorSequenceAdditional = StimuliSequencer.GetRainbowColorSequence(squareController.nAdditionalRainbowstimuli);
        List<Color> combinedColorList = AddLists(colorSequence, colorSequenceAdditional);

        int colorSequenceIndex = 0;

        for (int i = 0; i < totalCellsCount; i++) {

            if(i%2==0 && currentStimuliIndex < squareController.currentStimuliCells.Count) {
                squareController.currentStimuliCells[currentStimuliIndex].HighlightMeWhite(stimuliLifetime);
                currentStimuliIndex++;
            } else {
                    if(combinedColorList[colorSequenceIndex] == Color.blue) gridController.timedBlue.TimedBlueCell();
                    squareController.currentRainbowCells[currentRainbowStimuliIndex].HighlightMeColor(stimuliLifetime, combinedColorList[colorSequenceIndex]);
                    colorSequenceIndex++;
                    currentRainbowStimuliIndex++;
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
}
    
    

