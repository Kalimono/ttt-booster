using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class SurveyController : MonoBehaviour {
    public Button finishedButton;

    public SceneController sceneController;

    string fileName;
    string path;

    void Awake() {
        sceneController = FindObjectOfType<SceneController>();
        finishedButton.onClick.AddListener(delegate {FinishedButtonClick();});
    }

    void FinishedButtonClick() {
        sceneController.LoadGameScene();
    }
}
