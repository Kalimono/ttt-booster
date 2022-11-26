using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
    public int memory = 0;
    public int conds = 3;
    public static SceneController instance;
    public FurHatCommunication furHatCommunication;

    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            furHatCommunication = FindObjectOfType<FurHatCommunication>();
            return;
        }



        instance = this;
        DontDestroyOnLoad(gameObject);

        furHatCommunication = FindObjectOfType<FurHatCommunication>();
        Debug.Log("awake");
    }

    public void LoadGameScene() {
        IncrementMemory();

        if(memory == conds) {
            LoadSAMScene();
            return;
        }
        if(memory > conds) {
            LoadEndScene();
            return;
        }
        SceneManager.LoadScene(0);
    }

    public void LoadPauseScene() {
        
        furHatCommunication.SendPerformance();
        SceneManager.LoadScene(1);
    }

    private void IncrementMemory() {
        memory++;
    }

    public int GetMemory() {
        return memory;
    }

    private void LoadEndScene() {
        SceneManager.LoadScene(3);
    }

    private void LoadSAMScene() {
        SceneManager.LoadScene(2);
    }
}
