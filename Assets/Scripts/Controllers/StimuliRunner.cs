using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StimuliRunner : MonoBehaviour {
    List<Cell> cellList;
    float timeInBetween = 1f;
    float startTime;
    int indexCounter;
    
    bool runningStims = false;

    SoundFxController soundFxController;

    void Awake() {
        soundFxController = FindObjectOfType<SoundFxController>();

    }

    void Update() {
        if(runningStims) {
            
            if (Time.time - startTime > timeInBetween) {
                cellList[indexCounter].HighlightMe(.75f);
                indexCounter++;
                startTime = Time.time;
                soundFxController.PlayStimuliSound();
            }
            if(indexCounter == cellList.Count) runningStims = false;
        } 
    }
    
    public void RunStimuli(HashSet<Cell> cells) {
        cellList = RandomizeListOrder(cells.ToList());
        indexCounter = 1;
        startTime = Time.time;
        runningStims = true;
        soundFxController.PlayStimuliSound();
        cellList[0].HighlightMe(.75f);

        

        // while(indexCounter<cellList.Count) {
        // float startTime = Time.time;
        // if (Time.time - startTime < timeInBetween) cellList[indexCounter].HighlightMe(.75f);
        // indexCounter++;
    }

    public List<Cell> RandomizeListOrder(List<Cell> list) {
    System.Random rng = new System.Random();
    var randomizedList = list.OrderBy(a => rng.Next()).ToList();

    return randomizedList;
  }

    // for (int i = 0; i < cells.Count; i++) {
    //   cellList[i].HighlightMe(.75f);
    //   cellList[i].soundFxController.outcomeAudioSource.PlayOneShot(soundFxController.stimuliSound);
    //   yield return new WaitForSecondsRealtime(1f);
    // }
}
    
    

