using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class DataSave : MonoBehaviour {
    public ConditionController conditionController;
    public SquareController squareController;
    public GameController gameController;
    public LevelController levelController;
    public DotController dotController;
    public SceneController sceneController;

    string path;
    string fileName;

    public int whiteCorrect;
    public List<float> rTimeBlue;
    public int nResponses;
    public float isi;
    public float reactionTime;
    public int blueFailOut;
    public int blueFailMiss;

    void Awake() {
        conditionController = FindObjectOfType<ConditionController>();
        squareController = FindObjectOfType<SquareController>();
        gameController = FindObjectOfType<GameController>();
        levelController = FindObjectOfType<LevelController>();
        dotController = FindObjectOfType<DotController>();
        sceneController = FindObjectOfType<SceneController>();
        Initialize();
    }

    public void WriteString(string dataString) {
        string filePath = path + "/" + fileName + ".csv";

        StreamWriter writer = new StreamWriter(filePath, true);
        writer.WriteLine(dataString);
        writer.Close();
    }

    public string GetRoundDataString() {
        string dataString = gameController.totalTurn.ToString() + ";" +
            levelController.levels[sceneController.GetMemory()].name.ToString() + ";" +
            whiteCorrect.ToString() + ";" + 
            (conditionController.nResponses+1).ToString() + ";" + 
            isi.ToString()  + ";" + 
            FloatToCommaString(reactionTime)  + ";" + 
            FloatToCommaString(blueFailOut)  + ";" +
            FloatToCommaString(blueFailMiss)  + ";" + 
            ((dotController.toggleDot == true) ? 1 : 0).ToString();

        foreach (float rTime in rTimeBlue) {
            dataString += ";";
            // dataString += rTime.ToString();
            dataString += FloatToCommaString(rTime);
        } 

        for (int i = 0; i < 4-rTimeBlue.Count; i++) {
            dataString += ";";
            dataString += "0";
        }

        rTimeBlue.Clear();
        return dataString;
    }

        // FloatToCommaString(reactionTime)  + ";" + 
        // FloatToCommaString(blueFailOut)  + ";" +
        // FloatToCommaString(blueFailMiss)  + ";" + 

        // FloatToCommaString(rTime);

    public void WriteRoundDataString() {
        WriteString(GetRoundDataString());
    }

    void Initialize() {
        fileName = GetTimeNowString();
        path = "tictactoc++_Data/Data/" + fileName;
        
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        WriteString("Turn;level;correctResponse;nResponses;ISI;reactionTime;blueFailOut;blueFailMiss;dot;blueResponse1;blueResponse2;blueResponse3;blueResponse4");
    }

    string GetTimeNowString() {
        string nowString = System.DateTime.Now.Month.ToString()
                     + "." + System.DateTime.Now.Day.ToString()
                     + "_" + System.DateTime.Now.Hour.ToString()
                     + "." + System.DateTime.Now.Minute.ToString()
                     + "."  + System.DateTime.Now.Second.ToString();
        return nowString;
    }

    string FloatToCommaString(float floatNum) {
        string floatString = floatNum.ToString();
        string commaFloatString = "";

        for (int i = 0; i < floatString.Length; i++) {
            if(i == 1) {
                commaFloatString += ",";
            } else {
                commaFloatString += floatString[i];
            }
        }

        return commaFloatString;
    }

}
