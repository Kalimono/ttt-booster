using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class StimuliSequencer {
  static public List<int> stimuliSequence = new List<int>();
  static public List<int> targetSequence = new List<int>();
  static public List<int> squarePositionSequence = new List<int>();
  static public List<int> nonDifferentialOutcomeSequence  = new List<int>();
  static public int sequenceLength = 48;

  public static void CreateSequences() {
    CreateStimuliSequence();
    CreateTargetSequence();
    CreateSquarePositionSequence();
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
