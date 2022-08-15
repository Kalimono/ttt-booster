using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedBlue : MonoBehaviour {
    public SoundFxController soundFxController;
    public GameController gameController;
    public DataSave dataSave;
    bool blueTime = false;

    float time = 0f;
    float maxTime = 1f;

    void Awake() {
        dataSave = FindObjectOfType<DataSave>();
    }

    void Update() {
        if(blueTime) {
            time += Time.deltaTime;
            if (Input.GetKeyDown("space")) {
                soundFxController.PlayBlueTimeWin();
                dataSave.rTimeBlue.Add(time);
                blueTime = false;
                } 
            if(time > maxTime) {
                soundFxController.PlayBlueTimeFail();
                dataSave.rTimeBlue.Add(time+1f);
                dataSave.blueFailMiss++;
                blueTime = false;
            }
            
        } else {
            if (Input.GetKeyDown("space")) {
                soundFxController.PlayBlueTimeFail();
                dataSave.blueFailOut++;
                } 
        }
    }

    public void TimedBlueCell() {
        blueTime = true;
        time = 0f;
    }
}
