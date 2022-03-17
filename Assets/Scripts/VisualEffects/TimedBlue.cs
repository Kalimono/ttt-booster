using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedBlue : MonoBehaviour {

    public SoundFxController soundFxController;
    bool blueTime = false;

    float time = 0;
    float maxTime = 1f;

    void Update() {
        if(blueTime) {
            time += Time.deltaTime;
            if (Input.GetKeyDown("space")) {
                // print("space key was pressed");
                soundFxController.PlayBlueTimeWin();
                blueTime = false;
                // uIController.ToggleBlueText(false);
                } 
            if(time > maxTime) {
                soundFxController.PlayBlueTimeFail();
                blueTime = false;
            }
            
        }
    }

    public void TimedBlueCell() {
        // Debug.Log("bluetime");
        blueTime = true;
        // uIController.ToggleBlueText(true);
    }
}
