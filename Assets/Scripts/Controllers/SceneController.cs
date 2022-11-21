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
    public FurHatCommunication furHatCommunication;

    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            furHatCommunication = FindObjectOfType<FurHatCommunication>();
            return;
        }

        conds = gameController.nConds;

        instance = this;
        DontDestroyOnLoad(gameObject);

        furHatCommunication = FindObjectOfType<FurHatCommunication>();
    }

    public void LoadGameScene() {
        Debug.Log(memory);
        if(memory >= conds) {
            LoadEnd();
            return;
        }
        SceneManager.LoadScene(0);
    }

    public void LoadSurveyScene() {
        IncrementMemory();
        furHatCommunication.SendPerformance();
        SceneManager.LoadScene(1);
    }

    void IncrementMemory() {
        memory++;
    }

    public int GetMemory() {
        return memory;
    }

    public void LoadEnd() {
        SceneManager.LoadScene(2);
  }
}
