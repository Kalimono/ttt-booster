using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StimuliRunner : MonoBehaviour {
    List<Cell> cellList;
    float startTime;
    int indexCounter;
    
    bool runningStims = false;

    SoundFxController soundFxController;
    ConditionController conditionController;
    TimerController timerController;

    void Awake() {
        soundFxController = FindObjectOfType<SoundFxController>();
        conditionController = FindObjectOfType<ConditionController>();
        timerController = FindObjectOfType<TimerController>();
    }

    void Update() {
        if(runningStims) {

            if(indexCounter == cellList.Count) {
                Debug.Log("done");
                runningStims = false;
                // timerController.StartNextTimer();
            }
            
            if (Time.time - startTime > conditionController.stimuliLifetime/1000+conditionController.timeBetweenStimuli/1000) {
                cellList[indexCounter].HighlightMe(conditionController.stimuliLifetime/1000);
                indexCounter++;
                startTime = Time.time;
                soundFxController.PlayStimuliSound();
            }
        } 
    }
    
    public void RunStimuli(HashSet<Cell> cells) {
        cellList = RandomizeListOrder(cells.ToList());
        indexCounter = 1;
        startTime = Time.time;
        runningStims = true;
        soundFxController.PlayStimuliSound();
        cellList[0].HighlightMe(conditionController.stimuliLifetime/1000);
    }

    public List<Cell> RandomizeListOrder(List<Cell> list) {
    System.Random rng = new System.Random();
    var randomizedList = list.OrderBy(a => rng.Next()).ToList();

    return randomizedList;
  }
}
    
    

