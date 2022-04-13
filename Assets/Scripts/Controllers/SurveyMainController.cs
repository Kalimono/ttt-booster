using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class SurveyMainController : MonoBehaviour {
    public Slider mentallySlider;
    public Slider physicallySlider;
    public Slider temporalSlider;
    public Slider performanceSlider;
    public Slider effortSlider;
    public Slider frustrationSlider;
    
    public float mentallyValue = 11;
    public float physicallyValue = 11;
    public float temporalValue = 11;
    public float performanceValue = 11;
    public float effortValue = 11;
    public float frustrationValue = 11;

    public Button finishedButton;

    public SceneController sceneController;

    string fileName;
    string path;

    void Awake() {
        sceneController = FindObjectOfType<SceneController>();
        finishedButton.onClick.AddListener(delegate {FinishedButtonClick();});
        Initialize();
        mentallySlider.onValueChanged.AddListener(delegate {mentallySliderChange();});
        physicallySlider.onValueChanged.AddListener(delegate {physicallySliderChange();});
        temporalSlider.onValueChanged.AddListener(delegate {temporalSliderChange();});
        performanceSlider.onValueChanged.AddListener(delegate {performanceSliderChange();});
        effortSlider.onValueChanged.AddListener(delegate {effortSliderChange();});
        frustrationSlider.onValueChanged.AddListener(delegate {frustrationSliderChange();});
    }

    void FinishedButtonClick() {
        WriteString(mentallyValue.ToString() + ";" + 
                    physicallyValue.ToString() + ";" + 
                    temporalValue.ToString() + ";" + 
                    performanceValue.ToString() + ";" + 
                    effortValue.ToString() + ";" + 
                    frustrationValue.ToString());
        sceneController.LoadGameScene();
        
    }

    void mentallySliderChange() {
        mentallyValue = mentallySlider.value;
    }

    void physicallySliderChange() {
        physicallyValue = physicallySlider.value;
    }

    void temporalSliderChange() {
        temporalValue = temporalSlider.value;
    }

    void performanceSliderChange() {
        performanceValue = performanceSlider.value;
    }

    void effortSliderChange() {
        effortValue = effortSlider.value;
    }

    void frustrationSliderChange() {
        frustrationValue = frustrationSlider.value;
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
