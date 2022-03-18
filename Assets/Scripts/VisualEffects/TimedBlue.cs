using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedBlue : MonoBehaviour {

    public SoundFxController soundFxController;
    public GameController gameController;
    bool blueTime = false;

    float time = 0f;
    float maxTime = 1f;

    void Update() {
        if(blueTime) {
            time += Time.deltaTime;
            if (Input.GetKeyDown("space")) {
                // print("space key was pressed");
                soundFxController.PlayBlueTimeWin();
                gameController.rTimeBlue.Add(time);
                blueTime = false;
                // uIController.ToggleBlueText(false);
                } 
            if(time > maxTime) {
                soundFxController.PlayBlueTimeFail();
                gameController.rTimeBlue.Add(time+1f);
                blueTime = false;
            }
            
        }
    }

    public void TimedBlueCell() {
        // Debug.Log("bluetime");
        blueTime = true;
        time = 0f;
        // uIController.ToggleBlueText(true);
    }
}
