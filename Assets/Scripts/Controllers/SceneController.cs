using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
    public static int memory;
    public int conds;
    public static SceneController instance;
    public GameController gameController;
    public LevelController levelController;

    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }

        conds = gameController.nConds;

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadGameScene() {
        
        if(memory >= conds) {
            LoadEnd();
            return;
        }
        SceneManager.LoadScene(0);
        
    }

    public void LoadSurveyScene() {
        IncrementMemory();
        SceneManager.LoadScene(1);
    }

    void IncrementMemory() {
        memory++;
        // if(memory > 2) memory = 0;
    }

    public int GetMemory() {
        return memory;
    }

    public void LoadEnd() {
        SceneManager.LoadScene(2);
  }

}
