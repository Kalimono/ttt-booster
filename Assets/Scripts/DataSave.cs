using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataSave : MonoBehaviour {
    public ConditionController conditionController;
    public SquareController squareController;
    public int whiteCorrect;
    public List<float> rTimeBlue;
    public int nResponses;
    public float isi;

    void Awake() {
        conditionController = FindObjectOfType<ConditionController>();
        squareController = FindObjectOfType<SquareController>();
    }

    public void WriteString() {
        string path = "/test.txt";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(GetRoundDataString());
        writer.Close();
    }

    public string GetRoundDataString() {
        string dataString = whiteCorrect.ToString() + "," + conditionController.nResponses.ToString() + "," + squareController.currenTrialTimeOut.ToString();
        foreach (float rTime in rTimeBlue) {
        dataString += ",";
        dataString += rTime.ToString();
        }
        return dataString;
    }

}
