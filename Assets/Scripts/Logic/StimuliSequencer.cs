using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class StimuliSequencer {
  static public List<int> stimuliSequence = new List<int>();
  static public List<int> targetSequence = new List<int>();
  static public List<int> squarePositionSequence = new List<int>();
  static public List<int> nonDifferentialOutcomeSequence  = new List<int>();
  static public List<int> traceLengthSequence  = new List<int>();
  static public List<Color> rainbowSTimuliColorSequence = new List<Color>(); 

  static public int sequenceLength = 48;

  public static void CreateSequences() {
    CreateStimuliSequence();
    CreateTargetSequence();
    CreateSquarePositionSequence();
    CreateTraceLengthSequence();
  }

  static public void CreateStimuliSequence() {
    stimuliSequence = new List<int>();
    while (stimuliSequence.Count < sequenceLength) {
      List<int> cornerIndexes = new List<int> { 0, 0, 1, 1, 2, 2, 3, 3};
      List<int> randomizedStimuli = RandomizeList(cornerIndexes);
      AddListToList(stimuliSequence, randomizedStimuli);
    }
  }

  static public int GetRandomCorrectPosition() {
    List<int> cornerIndexes = new List<int> {0, 2, 6, 8};
    int corner = cornerIndexes[Random.Range(0, cornerIndexes.Count)];
    return corner;
  }

  static public void CreateSquarePositionSequence() {
    while (squarePositionSequence.Count < sequenceLength) {
      List<int> squarePositionIndices = new List<int> { 0, 0, 1, 1, 2, 2, 3, 3 };
      squarePositionIndices = RandomizeList(squarePositionIndices);
      AddListToList(squarePositionSequence, squarePositionIndices);
    }
  }

  static public void CreateNonDifferentialOutcomeSequence() {
    while (nonDifferentialOutcomeSequence.Count < sequenceLength) {
      List<int> nonDifferntialOutcomeBin = new List<int> { 0, 0, 1, 1, 2, 2, 3, 3 };
      nonDifferntialOutcomeBin = RandomizeList(nonDifferntialOutcomeBin);
      AddListToList(nonDifferentialOutcomeSequence, nonDifferntialOutcomeBin);
    }
  }

  static public int GetNonDifferentialOutcome() {
    if (nonDifferentialOutcomeSequence.Count < 1) CreateNonDifferentialOutcomeSequence();
    int nonDifferentialOutcome = nonDifferentialOutcomeSequence[0];
    nonDifferentialOutcomeSequence.RemoveAt(0);
    return nonDifferentialOutcome;
  }

  static public void CreateTargetSequence() {
    targetSequence = new List<int>();
    for (int i = 0; i < sequenceLength; i++) {
      if (i % 2 == 0) {
        targetSequence.Add(0);
      } else {
        targetSequence.Add(1);
      }
    }
    targetSequence = RandomizeList(targetSequence);
  }

  static public void CreateTraceLengthSequence() {
    traceLengthSequence = new List<int>();
    for (int i = 0; i < sequenceLength; i++) {
      if (i % 2 == 0) {
        traceLengthSequence.Add(0);
      } else {
        traceLengthSequence.Add(1);
      }
    }
    traceLengthSequence = RandomizeList(traceLengthSequence);
  }

  static public int GetStimuliIndex() {
    if (stimuliSequence.Count == 0) CreateStimuliSequence();

    int stimuliIndex = stimuliSequence[0];
    stimuliSequence.RemoveAt(0);
    return stimuliIndex;
  }

  static public int GetTargetNum() {
    if (targetSequence.Count == 0) {
      CreateTargetSequence();
    }

    int targetNumber = targetSequence[0];
    targetSequence.RemoveAt(0);
    return targetNumber;
  }

  static public float GetTraceLength() {
    float timeOut;
    if (traceLengthSequence.Count == 0) {
      CreateTraceLengthSequence();
    }

    int traceLengthNumber = traceLengthSequence[0];
    traceLengthSequence.RemoveAt(0);
    timeOut = (traceLengthNumber == 0) ? 2000f : 15000f;
    return timeOut;
  }

  static public List<Color> GetRainbowColorSequence(int sequenceLength) {
    // Debug.Log("sequencelength in: " + sequenceLength.ToString());
    List<Color> colorSequence = new List<Color>();
    List<Color> colors = new List<Color>{Color.red};

    for (int i = 0; i < sequenceLength; i++) {
      // Debug.Log(colors[Random.Range(0, colors.Count)]);
      colorSequence.Add(colors[Random.Range(0, colors.Count)]);
    }

    int maxIndex;
    
    if(sequenceLength < 1) {
      maxIndex = 0;
    } else {
      maxIndex = sequenceLength-1;
    }
    int index = Random.Range(0, maxIndex);
    // Debug.Log(index);
    // colorSequence[index] = Color.blue;
    // Debug.Log("returns list of length: " + colorSequence.Count.ToString());
    return colorSequence;
  }

  static public void AddListToList(List<int> sequence, List<int> list) {
    foreach (int element in list) {
      sequence.Add(element);
    }
  }

  static public List<int> RandomizeList(List<int> list) {
    System.Random rng = new System.Random();
    var randomizedList = list.OrderBy(a => rng.Next()).ToList();
    return randomizedList;
  }
}
