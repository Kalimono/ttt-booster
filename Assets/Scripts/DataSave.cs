using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataSave : MonoBehaviour {
    public ConditionController conditionController;
    public SquareController squareController;
    public GameController gameController;
    public LevelController levelController;

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
        Initialize();
    }

    public void WriteString(string dataString) {
        string filePath = path + "/" + fileName + ".txt";

        StreamWriter writer = new StreamWriter(filePath, true);
        writer.WriteLine(dataString);
        writer.Close();
    }

    public string GetRoundDataString() {
        string dataString = gameController.totalTurn.ToString() + "," +
            levelController.lastLevelIndex.ToString() + "," +
            whiteCorrect.ToString() + "," + 
            (conditionController.nResponses+1).ToString() + "," + 
            isi.ToString()  + "," +
            reactionTime.ToString()  + "," +
            blueFailOut.ToString()  + "," +
            blueFailMiss.ToString();

        foreach (float rTime in rTimeBlue) {
            dataString += ",";
            dataString += rTime.ToString();
        }

        rTimeBlue.Clear();
        return dataString;
    }

    public void WriteRoundDataString() {
        WriteString(GetRoundDataString());
    }

    void Initialize() {
        fileName = GetTimeNowString();
        path = "tictactoc++_Data/Data/" + fileName;
        
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        WriteString("Turn, level, correctResponse, nResponses, ISI, reactionTime, blueFailOut, blueFailMiss, blueResponse1, blueResponse2");
    }

    string GetTimeNowString() {
        string nowString = System.DateTime.Now.Month.ToString()
                     + "." + System.DateTime.Now.Day.ToString()
                     + "_" + System.DateTime.Now.Hour.ToString()
                     + "." + System.DateTime.Now.Minute.ToString()
                     + "."  + System.DateTime.Now.Second.ToString();
        return nowString;
    }

}
