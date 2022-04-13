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
  public DotController dotController;

  public List<Cell> currentPosition;
  public Image currentSquare;

  // public int nStimulis;
  public int distractorPosition;
  public List<Cell> currentStimuliCells = new List<Cell>();
  public List<Cell> currentRainbowCells = new List<Cell>();
  public List<Cell> currentAdditionalRainbowCells = new List<Cell>();
  public List<Cell> distractorCells;
  public Cell correctCell;
  Cell cellToSkip;

  public int stimuliIndex;
  public int squarePositionIndex;
  public int distractorIndex;

  List<Cell> incorrectResponses = new List<Cell>();
  public List<Cell> targetCells = new List<Cell>();
  List<Cell> nontargetCells = new List<Cell>();

  public float currenTrialTimeOut;
  public int nAdditionalRainbowstimuli;

  Cell lastCellAdded = null;

  void Start() {
    gridCreator = FindObjectOfType<GridCreator>();
    dotController = FindObjectOfType<DotController>();
    // Initialize();
  }

  public void Initialize() {
    ClearPreviousSetup();
    // ClearPreviousRound();
    SetCellReinforcement(gridController.grid);
    currentPosition = GetCellList(gridController.grid);
    // cellToSkip = GetCellFromGridToSkip(gridController.grid);
    // Debug.Log(GetCellFromGridToSkip(gridController.grid));
  }

  public void Reset() {
    ClearPreviousSetup();
    ClearPreviousRound();
  }

  void SetCellReinforcement(Cell[,] grid) {
    // ClearPrevious();
    List<Cell> cellList = RandomizeListOrder(GetCellList(grid));  

    for (int i = 0; i < cellList.Count; i++) {
      if(i%2==0 && targetCells.Count < 4) {
        targetCells.Add(cellList[i]);
      } else {
        nontargetCells.Add(cellList[i]);
      }
    }
    dotController.SetDotOutcomes(targetCells);
  }

  List<Cell> GetCellList(Cell[,] grid) {
    // List<Cell> cellToSkip = GetCellsToSkip(grid);
    cellToSkip = GetCellFromGridToSkip(gridController.grid);
    List<Cell> cellList = new List<Cell>();
    foreach(Cell cell in grid) {
      if(cell != cellToSkip) cellList.Add(cell);
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
    ClearPreviousRound();
    if(targetCells.Count < 4) Initialize();

    currenTrialTimeOut = StimuliSequencer.GetTraceLength();
    stimuliIndex = StimuliSequencer.GetStimuliIndex();
    correctCell = targetCells[stimuliIndex];
    currentStimuliCells.Add(correctCell);

    SetDistractorCells(currentPosition, conditionController.nResponses);
    AddCellsToCurrentStimuliCells(GetNonTargetStimuliCells(nontargetCells));

    SetRainbowincorrectResponsestimuli(conditionController.nRainbowStim);
    nAdditionalRainbowstimuli = stimuliRunner.GetNAdditionalRainbowStimuli(currenTrialTimeOut, conditionController.stimuliLifetime);
    currentAdditionalRainbowCells =  GetRainbowCells(currentPosition, stimuliRunner.GetNAdditionalRainbowStimuli(currenTrialTimeOut, conditionController.stimuliLifetime));
    AddCellsToCurrentRainbowStimuliCells(currentAdditionalRainbowCells);
  }

  Cell GetCellFromGridToSkip(Cell[,] grid) {
    cellToSkip = grid[(gridCreator.gridSize-1)/2, (gridCreator.gridSize-1)/2];
    return cellToSkip;
  }

  void SetDistractorCells(List<Cell> position, float n) {
    
    for (int i = 0; i < n; i++) {
      if(i == 0) {
        incorrectResponses.Add(GetDistractor(position));
      } else {
        incorrectResponses.Add(GetAdditionalDistractor(position));
      }
    }
  }

  void ClearPreviousRound() {
    currentStimuliCells.Clear();
    currentRainbowCells.Clear();
    correctCell = null;
    incorrectResponses.Clear();
    // cellToSkip = null;
  }

  void ClearPreviousSetup() {
    targetCells.Clear();
    nontargetCells.Clear();
  }

  List<Cell> GetNonTargetStimuliCells(List<Cell> nonTargetCells) {
    List<Cell> nonTargetStimuli = new List<Cell>();
    while (nonTargetStimuli.Count+1 < conditionController.nStimuli) {
      int randint = Random.Range(0, nonTargetCells.Count-1);
      Cell cell = nonTargetCells[randint];
      if (!incorrectResponses.Contains(cell) && !nonTargetStimuli.Contains(cell)) nonTargetStimuli.Add(cell);//
    }
    return nonTargetStimuli;
  }

  List<Cell> GetRainbowCells(List<Cell> position, float nRainbowStim) {
    List<Cell> rainbowCells = new List<Cell>();
    
    while (rainbowCells.Count < nRainbowStim) {
      // int randint = Random.Range(0, position.Count);
      // Cell candidateCell = position[randint];
      // Cell cell = null;
      
      // if(rainbowCells.Count != 0) {
      //   while(cell == null) {
      //     int randint = Random.Range(0, position.Count);
      //     Cell candidateCell = position[randint];
      //     if(candidateCell != rainbowCells[rainbowCells.Count-1]) cell = candidateCell;
      //   }
      // } else {
        int randint = Random.Range(0, position.Count);
        Cell cell = position[randint];
      // }

      if(!targetCells.Contains(cell) && !currentStimuliCells.Contains(cell) && cell != lastCellAdded) {
        rainbowCells.Add(cell);
        lastCellAdded = cell;
      }

    }
    return rainbowCells;
  }

  void AddCellsToCurrentStimuliCells(List<Cell> cells) {
    foreach(Cell cell in cells) {
      currentStimuliCells.Add(cell);
    }
  }

  void AddCellsToCurrentRainbowStimuliCells(List<Cell> cells) {
    foreach(Cell cell in cells) {
      currentRainbowCells.Add(cell);
    }
  }

  void SetRainbowincorrectResponsestimuli(float n) {
    currentRainbowCells = GetRainbowCells(currentPosition, n);
  }

  Cell GetDistractor(List<Cell> position) {
    Cell randomPick;
    Cell distractor = null;
    distractorPosition = StimuliSequencer.GetTargetNum();

    while (distractor == null) {
      // if (distractorPosition == 1) {
        randomPick = PickRandomTargetCell();
      // } else {
      //   randomPick = PickRandomNonTargetCell();
      // }

      if(randomPick != correctCell && !currentStimuliCells.Contains(randomPick)) {
        distractor = randomPick;
      }
    }
    return distractor;
  }

  Cell GetAdditionalDistractor(List<Cell> position) {
    Cell randomPick;
    Cell distractor = null;
    distractorPosition = Random.Range(0, position.Count);

    while (distractor == null) {
      // if (distractorPosition == 1) {
        randomPick = PickRandomTargetCell();
      // } else {
      //   randomPick = PickRandomNonTargetCell();
      // }

      if(randomPick != correctCell && !incorrectResponses.Contains(randomPick) && !currentStimuliCells.Contains(randomPick)) {
        distractor = randomPick;
      }
    }
    return distractor;
  }

  // List<Cell> GetAdditionalRainbowStimuli(int n) {
  //   // Debug.Log("getting n additional: " + n.ToString());
  //   List<Cell> additionalrainbowStimuli = new List<Cell>();
  //   while(additionalrainbowStimuli.Count<n){
  //     int randInt = Random.Range(0, nontargetCells.Count);
  //     Cell testCell = nontargetCells[randInt];
  //     if(!currentStimuliCells.Contains(testCell) && !targetCells.Contains(testCell)) additionalrainbowStimuli.Add(testCell);
  //   }
  //   // Debug.Log("returning n additional: " + additionalrainbowStimuli.Count.ToString());
  //   return additionalrainbowStimuli;
  //   // StimuliSequencer.GetRainbowColorSequence(20);
    
  // }

  // void SetCurrentStimuliCells(Cell[,] grid) {
  //   foreach(Cell cell in grid) currentStimuliCells.Add(cell);
  // }

  public void PresentStimuli() {
    currentStimuliCells = RandomizeListOrder(currentStimuliCells);
    // currentRainbowCells = RandomizeListOrder(currentRainbowCells);
    // Debug.Log(currentStimuliCells.Count);
    stimuliRunner.RunMixedStimuli();
  }

  public void FlashMiddleCell(float seconds) {
    cellToSkip.FlashMeGreenSeconds(seconds);
  }

  public void ShowCurrentSquare() { 
    currentSquare.enabled = true;
    currentSquare.GetComponent<FocusSquareEnlarger>().Enlarge();
  }

  public void HideSquares() {
    foreach (Image square in squarePositions) {
      currentSquare.GetComponent<FocusSquareEnlarger>().FadeOut();
    }
  }

  public Cell PickRandomTargetCell() {
    Cell cell = targetCells[Random.Range(0, targetCells.Count)];

    return cell;
  }

  public Cell PickRandomNonTargetCell() {
    Cell cell = nontargetCells[Random.Range(0, nontargetCells.Count)];

    return cell;
  }

  public List<Cell> RandomizeListOrder(List<Cell> list) {
    System.Random rng = new System.Random();
    var randomizedList = list.OrderBy(a => rng.Next()).ToList();

    return randomizedList;
  }

  public void ToggleOptions(bool toggle) {
    foreach(Cell distractorCell in incorrectResponses) {
      distractorCell.SetInteractive(toggle);
    }
    correctCell.SetInteractive(toggle);
  }

  public void FadeCellsOutsideSquare(List<Cell> positionList) {
    foreach (Cell cell in gridController.grid) {
      if (!positionList.Contains(cell)) {
        cell.Fade(true);
      }
    }
  }
}
