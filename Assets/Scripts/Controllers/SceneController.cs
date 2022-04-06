using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
    public int memory;
    // public int currentSceneIndex = 0;
    public static SceneController instance;

    void Awake() {
        // Debug.Log(Application.persistentDataPath);
       if (instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // public void ChangeScene() {
    //     int sceneToLoad;
        
    //     if(currentSceneIndex == 0) {
    //         memory++;
    //         // Debug.Log("mem: " + memory.ToString());
    //         sceneToLoad = 1;
    //         currentSceneIndex = 1;
    //     } else {
    //         sceneToLoad = 0;
    //         currentSceneIndex = 0;
    //     }
    //     // if(sceneToLoad == 1)
        
	// 	SceneManager.LoadScene(sceneToLoad);
	// }

    public void LoadGameScene() {
        SceneManager.LoadScene(0);
    }

    public void LoadSurveyScene() {
        memory++;
        Debug.Log("increment");
        Debug.Log(memory);
        SceneManager.LoadScene(1);
    }

    // public void SwitchToSurveyScene(float delay) {
    //     StartCoroutine(SwitchAfterDelay(delay));
    // }

    // IEnumerator SwitchAfterDelay(float delay) {
    //     yield return new WaitForSeconds(delay);
    //     ChangeScene();
    // }
}
