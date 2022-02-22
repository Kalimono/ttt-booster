using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SquareController : MonoBehaviour {
  public Image[] squarePositions;
  public GameObject squares;
  public GridController gridController;
  public SoundFxController soundFxController;
  // public StimuliSequencer stimuliSequencer;
  public GameController gameController;
  public GameLogic gameLogic;
  public UIController uiController;
  public StimuliRunner stimuliRunner;

  public List<Cell> currentPosition;
  public Image currentSquare;

  public int nStimulis;
  public int distractorPosition;
  public HashSet<Cell> currentCells = new HashSet<Cell>();
  public Cell distractorCell;
  public Cell correctCell;
  Cell cellToSkip;

  public int stimuliIndex;
  public int squarePositionIndex;
  public int distractorIndex;

  List<List<Cell>> positions = new List<List<Cell>>{
    new List<Cell>(),
    new List<Cell>(),
    new List<Cell>(),
    new List<Cell>()};

  public void PrepareStimuliPhase() {
    ClearPreviousRound();
    stimuliIndex = StimuliSequencer.GetStimuliIndex();

    HashSet<int> positionsTested = new HashSet<int> {};
    int currentSquarePositionIndex = 0;
    if (gameController.turnNum <= 9 && StimuliSequencer.squarePositionSequence.Count > 0) {
      while (positionsTested.Count < 4) {
        squarePositionIndex = StimuliSequencer.squarePositionSequence[currentSquarePositionIndex];

        SetSquareArea(squarePositionIndex);
        correctCell = currentPosition[stimuliIndex];
        cellToSkip = GetCellToSkip(currentPosition);

        if (!gameLogic.TestForWin(correctCell, gridController.grid)) break;
        
        positionsTested.Add(StimuliSequencer.squarePositionSequence[currentSquarePositionIndex]);
        currentSquarePositionIndex++;
      }
    } else {
      SetSquareArea(Random.Range(0, 4));
      cellToSkip = GetCellToSkip(currentPosition);
      correctCell = currentPosition[StimuliSequencer.GetRandomCorrectPosition()];
    }
    if (StimuliSequencer.squarePositionSequence.Count != 0) StimuliSequencer.squarePositionSequence.RemoveAt(currentSquarePositionIndex);

    FadeCellsOutsideSquare(currentPosition);
    currentCells.Add(correctCell);
    
    SetDistractor(currentPosition);
    
    AddNonTargetCellsToCurrentCells(GetNonTargetCells(currentPosition));
  }

  Cell GetCellToSkip(List<Cell> currentPosition) {
    cellToSkip = currentPosition[(currentPosition.Count - 1) / 2];
    return cellToSkip;
  }

  void ClearPreviousRound() {
    currentCells.Clear();
    correctCell = null;
    distractorCell = null;
    cellToSkip = null;
  }

  List<Cell> GetNonTargetCells(List<Cell> position) {
    List<Cell> nonTargetCells = new List<Cell>();
    while (nonTargetCells.Count < 3) {
      int randint = Random.Range(0, position.Count);
      Cell cell = position[randint];
      if (cell != cellToSkip && cell != distractorCell && cell != correctCell && !nonTargetCells.Contains(cell)) nonTargetCells.Add(cell);
    }
    return nonTargetCells;
  }

  void AddNonTargetCellsToCurrentCells(List<Cell> nonTargetCells) {
    foreach(Cell cell in nonTargetCells) {
      currentCells.Add(cell);
    }
  }

  void SetDistractor(List<Cell> position) {
    distractorCell = null;
    distractorPosition = StimuliSequencer.GetTargetNum();

    while (distractorCell == null) {
      if (distractorPosition == 1) {
        distractorIndex = PickRandomCorner();
      } else {
        distractorIndex = PickRandomNonCorner();
      }
      if (position[distractorIndex] != correctCell) {
        distractorCell = position[distractorIndex];
      } 
    }
  }

  public void CutGridIntoAreas(Cell[,] grid) {
    int side = (grid.Length / 4) - 1;
    ClearPositionLists();

    foreach (Cell cell in grid) {
      if (cell.position[0] < side && cell.position[1] < side) positions[0].Add(cell);
      if (cell.position[0] > side - side && cell.position[1] < side) positions[1].Add(cell);
      if (cell.position[0] < side && cell.position[1] > side - side) positions[2].Add(cell);
      if (cell.position[0] > side - side && cell.position[1] > side - side) positions[3].Add(cell);
    }
  }

  public void PresentStimuli() {
    // StartCoroutine(RunStims(currentCells));
    stimuliRunner.RunStimuli(currentCells);
  }

  IEnumerator RunStims(HashSet<Cell> cells) {
    List<Cell> cellList = RandomizeListOrder(cells.ToList());

    for (int i = 0; i < cells.Count; i++) {
      cellList[i].HighlightMe(.75f);
      cellList[i].soundFxController.outcomeAudioSource.PlayOneShot(soundFxController.stimuliSound);
      yield return new WaitForSecondsRealtime(1f);
    }
  }

  public void ShowCurrentSquare() {
    currentSquare.enabled = true;
    currentSquare.GetComponent<FocusSquareEnlarger>().Enlarge();
  }

  void SetSquareArea(int area) {
    switch (area) {
      case 0:
        currentPosition = positions[0];
        currentSquare = squarePositions[0];
        break;
      case 1:
        currentPosition = positions[1];
        currentSquare = squarePositions[1];
        break;
      case 2:
        currentPosition = positions[2];
        currentSquare = squarePositions[2];
        break;
      case 3:
        currentPosition = positions[3];
        currentSquare = squarePositions[3];
        break;
    }
  }

  public void HideSquares() {
    foreach (Image square in squarePositions) {
      currentSquare.GetComponent<FocusSquareEnlarger>().FadeOut();
      // square.enabled = false;
    }
  }

  public void ClearPositionLists() {
    foreach (List<Cell> position in positions) {
      position.Clear();
    }
  }

  public int PickRandomCorner() {
    List<int> corners = new List<int> { 0, 2, 6, 8 };
    int index = corners[Random.Range(0, corners.Count)];

    return index;
  }

  public int PickRandomNonCorner() {
    List<int> corners = new List<int> { 1, 3, 5, 7 };
    int index = corners[Random.Range(0, corners.Count)];

    return index;
  }

  public List<Cell> RandomizeListOrder(List<Cell> list) {
    System.Random rng = new System.Random();
    var randomizedList = list.OrderBy(a => rng.Next()).ToList();

    return randomizedList;
  }

  public void ToggleOptions(bool toggle) {
    // Debug.Log(distractorIndex);
    distractorCell.SetInteractive(toggle);
    correctCell.SetInteractive(toggle);
  }

  public void ShowAIoptions() {
    distractorCell.shapeRenderer.material = distractorCell.pink;
    correctCell.shapeRenderer.material = correctCell.pink;
  }

  public void HideAIOptions() {
    if (distractorCell != null && correctCell != null) {
      distractorCell.shapeRenderer.material = distractorCell.gridCell;
      correctCell.shapeRenderer.material = correctCell.gridCell;
    }
  }

  public void FadeCellsOutsideSquare(List<Cell> positionList) {
    foreach (Cell cell in gridController.grid) {
      if (!positionList.Contains(cell)) {
        cell.Fade(true);
      }
    }
  }
}

