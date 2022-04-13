using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class SurveyController : MonoBehaviour {
    public Slider pleasureSlider;
    public Slider arousalSlider;
    public Slider dominanceSlider;
    
    public float pleasureValue = 1;
    public float arousalValue = 1;
    public float dominanceValue = 1;

    public Image[] pleasureImages;
    public Image[] arousalImages;
    public Image[] dominanceImages;

    public Button finishedButton;

    public SceneController sceneController;

    string fileName;
    string path;

    void Awake() {
        sceneController = FindObjectOfType<SceneController>();
        // Debug.Log(sceneController);
        // finishedButton.onClick.RemoveAllListeners();
        finishedButton.onClick.AddListener(delegate {FinishedButtonClick();});
        Initialize();
        pleasureSlider.onValueChanged.AddListener(delegate {pleasureSliderChange();});
        arousalSlider.onValueChanged.AddListener(delegate {arousalSliderChange();});
        dominanceSlider.onValueChanged.AddListener(delegate {dominanceSliderChange();});
    }

    void FinishedButtonClick() {
        WriteString(pleasureValue.ToString() + ";" + arousalValue.ToString() + ";" + dominanceValue.ToString());
        sceneController.LoadGameScene();
        
    }

    void pleasureSliderChange() {
        pleasureValue = pleasureSlider.value;
        DisableImageArray(pleasureImages);
        pleasureImages[(int)pleasureValue-1].enabled = true;
    }

    void arousalSliderChange() {
        arousalValue = arousalSlider.value;
        DisableImageArray(arousalImages);
        arousalImages[(int)arousalValue-1].enabled = true;
    }

    void dominanceSliderChange() {
        dominanceValue = dominanceSlider.value;
        DisableImageArray(dominanceImages);
        dominanceImages[(int)dominanceValue-1].enabled = true;
    }

    void DisableImageArray(Image[] array) {
        foreach(Image im in array) {
            im.enabled = false;
        }
    }

    public void SwitchToGameScene() {
        WriteString(pleasureValue.ToString() + ";" + arousalValue.ToString() + ";" + dominanceValue.ToString());
		SceneManager.LoadScene(0);
    }

    public void WriteString(string dataString) {
        string filePath = path + "/" + fileName + ".csv";

        StreamWriter writer = new StreamWriter(filePath, true);
        writer.WriteLine(dataString);
        writer.Close();
    }

    void Initialize() {
        fileName = GetTimeNowString();
        path = "tictactoc++_Data/Data/" + fileName;
        
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        // WriteString("Turn, level, correctResponse, nResponses, ISI, reactionTime, blueResponse1, blueResponse2");
    }

    string GetTimeNowString() {
        string nowString = "Survey"
                     + "_" + System.DateTime.Now.Month.ToString()
                     + "." + System.DateTime.Now.Day.ToString()
                     + "_" + System.DateTime.Now.Hour.ToString()
                     + "." + System.DateTime.Now.Minute.ToString()
                     + "."  + System.DateTime.Now.Second.ToString();
        return nowString;
    }
}
