using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
    
    public Scene scene1;

    public void ChangeScene() {
		SceneManager.LoadScene(1);
	}

    public void SwitchToSurveyScene() {
        ChangeScene();
    }
}
