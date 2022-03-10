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
  public GridCreator gridCreator;
  public ConditionController conditionController;

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

  // List<List<Cell>> positions = new List<List<Cell>>{
  //   new List<Cell>(),
  //   new List<Cell>(),
  //   new List<Cell>(),
  //   new List<Cell>()};

    //New

  List<Cell> targetCells = new List<Cell>();
  List<Cell> nontargetCells = new List<Cell>();

  void Awake() {
    gridCreator = FindObjectOfType<GridCreator>();
  }

  public void Initialize() {
    SetCellReinforcement(gridController.grid);
    currentPosition = GetCellList(gridController.grid);
  }

  void SetCellReinforcement(Cell[,] grid) {
    List<Cell> cellList = RandomizeListOrder(GetCellList(grid));  

    for (int i = 0; i < cellList.Count; i++) {
      if(i%2==0) {
        targetCells.Add(cellList[i]);
      } else {
        nontargetCells.Add(cellList[i]);
      }
    }
  }

  List<Cell> GetCellList(Cell[,] grid) {
    List<Cell> cellsToSkip = GetCellsToSkip(grid);
    List<Cell> cellList = new List<Cell>();
    foreach(Cell cell in grid) {
      if(!cellsToSkip.Contains(cell)) cellList.Add(cell);
    }
    return cellList;
  }

  List<Cell> GetCellsToSkip(Cell[,] grid) {
    List<Cell> cellsToSkip = new List<Cell>();
    foreach(Cell cell in grid) {
      if(!TestIfEdgeCell(cell)) cellsToSkip.Add(cell); 
    }
    return cellsToSkip;
  }

  bool TestIfEdgeCell(Cell cell) {
    if (cell.position.x == 0 || cell.position.x == gridCreator.gridSize-1) return true;
    if (cell.position.y == 0 || cell.position.y == gridCreator.gridSize-1) return true;
    return false;
  }

  public void PrepareStimuliPhase() {
    SetCurrentCells(gridController.grid);
    ClearPrevious();
    stimuliIndex = StimuliSequencer.GetStimuliIndex();
    correctCell = targetCells[stimuliIndex];
    currentCells.Add(correctCell);
    distractorCell = GetDistractor(currentPosition);
    // currentCells.Add(distractorCell);
    AddCellsToCurrentCells(GetNonTargetCells(currentPosition));
  }

  // public void PrepareRainbowPhase() {
  //   SetCurrentCells(gridController.grid);
  //   ClearPrevious();
  //   AddCellsToCurrentCells(GetRainbowCells(currentPosition, conditionController.nRainbowStim));
  // }

  Cell GetCellToSkip(List<Cell> currentPosition) {
    cellToSkip = currentPosition[(currentPosition.Count - 1) / 2];
    return cellToSkip;
  }

  void ClearPrevious() {
    currentCells.Clear();
    correctCell = null;
    distractorCell = null;
    cellToSkip = null;
  }

  List<Cell> GetNonTargetCells(List<Cell> cells) {
    List<Cell> nonTargetCells = new List<Cell>();
    while (nonTargetCells.Count < 3) {
      int randint = Random.Range(0, cells.Count-1);
      // Debug.Log(randint);
      Cell cell = cells[randint];
      if (cell != distractorCell && cell != correctCell && !nonTargetCells.Contains(cell)) nonTargetCells.Add(cell);
    }
    return nonTargetCells;
  }

  List<Cell> GetRainbowCells(List<Cell> position, float n) {
    Debug.Log(n);
    List<Cell> rainbowCells = new List<Cell>();
    while (rainbowCells.Count < n) {
      int randint = Random.Range(0, position.Count);
      Cell cell = position[randint];
      if (!rainbowCells.Contains(cell)) rainbowCells.Add(cell);
    }
    return rainbowCells;
  }

  void AddCellsToCurrentCells(List<Cell> nonTargetCells) {
    foreach(Cell cell in nonTargetCells) {
      currentCells.Add(cell);
    }
  }

  Cell GetDistractor(List<Cell> position) {
    Cell randomPick;
    Cell distractor = null;
    distractorPosition = StimuliSequencer.GetTargetNum();

    while (distractor == null) {
      if (distractorPosition == 1) {
        randomPick = PickRandomTargetCell();
      } else {
        randomPick = PickRandomNonTargetCell();
      }

      if(randomPick != correctCell) {
        distractor = randomPick;
      }
    }
    return distractor;
  }

  void SetCurrentCells(Cell[,] grid) {
    foreach(Cell cell in grid) currentCells.Add(cell);
  }

  // public void CutGridIntoAreas(Cell[,] grid) {
  //   // int side = (grid.Length / 4) - 1;
  //   // ClearPositionLists();

  //   // foreach (Cell cell in grid) {
  //   //   if (cell.position[0] < side && cell.position[1] < side) positions[0].Add(cell);
  //   //   if (cell.position[0] > side - side && cell.position[1] < side) positions[1].Add(cell);
  //   //   if (cell.position[0] < side && cell.position[1] > side - side) positions[2].Add(cell);
  //   //   if (cell.position[0] > side - side && cell.position[1] > side - side) positions[3].Add(cell);
  //   // }

  //     foreach(Cell cell in grid){
  //       positions[0].Add(cell);
  //       // if(TestIfEdgeCell(cell)) positions[0].Add(cell); 
  //     }  
  //   }

  public void PresentStimuli() {
    // StartCoroutine(RunStims(currentCells));
    stimuliRunner.RunStimuli(currentCells);
  }

  public void PresentRainbowDistractorStimuli() {
    List<Cell> randinbowCells = GetRainbowCells(currentPosition, conditionController.nRainbowStim);
    stimuliRunner.RunRainbowDistractorStimuli(randinbowCells);
  }

  public void ShowCurrentSquare() { 
    currentSquare.enabled = true;
    currentSquare.GetComponent<FocusSquareEnlarger>().Enlarge();
  }

  // void SetSquareArea(int area) {
  //   // switch (area) {
  //   //   case 0:
  //   //     currentPosition = positions[0];
  //   //     currentSquare = squarePositions[0];
  //   //     break;
  //   //   case 1:
  //   //     currentPosition = positions[1];
  //   //     currentSquare = squarePositions[1];
  //   //     break;
  //   //   case 2:
  //   //     currentPosition = positions[2];
  //   //     currentSquare = squarePositions[2];
  //   //     break;
  //   //   case 3:
  //   //     currentPosition = positions[3];
  //   //     currentSquare = squarePositions[3];
  //   //     break;
  //   // }
  //   currentPosition = positions[0];
  //   currentSquare = squarePositions[0];
  // }

  public void HideSquares() {
    foreach (Image square in squarePositions) {
      currentSquare.GetComponent<FocusSquareEnlarger>().FadeOut();
      // square.enabled = false;
    }
  }

  // public void ClearPositionLists() {
  //   foreach (List<Cell> position in positions) {
  //     position.Clear();
  //   }
  // }

  public Cell PickRandomTargetCell() {
    // List<int> corners = new List<int> { 0, 2, 6, 8 };
    Cell cell = targetCells[Random.Range(0, targetCells.Count)];

    return cell;
  }

  public Cell PickRandomNonTargetCell() {
    // List<int> corners = new List<int> { 1, 3, 5, 7 };
    Cell cell = nontargetCells[Random.Range(0, nontargetCells.Count)];

    return cell;
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

