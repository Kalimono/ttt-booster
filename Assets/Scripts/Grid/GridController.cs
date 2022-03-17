using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;

public class GridController : MonoBehaviour {
  public GridCreator gridCreator;
  public Cell[,] grid;
  public GameLogic gameLogic;
  public Cell lastCellInteractedWith;
  public bool rotation = false;

  public SoundFxController soundFxController;
  public UIController uIController;
  public TimedBlue timedBlue;

  public void CreateGrid() {
    grid = gridCreator.InitializeGrid();
    foreach(Cell cell in grid) cell.shapeRenderer.material.SetInt("ShouldFlash", 0);
  }

  public void SetBoardInteractable(bool toggle) {
    foreach (Cell cell in grid) {
      cell.SetInteractive(toggle);
    }
  }

  public void ClearHoverMarkers() {
    foreach (Cell cell in grid) {
      cell.hoverValue.GetComponent<SpriteRenderer>().sprite = null;
    }
  }

  public void SetCellValueVisibiltyToggle(bool toggle) {
    foreach (Cell cell in grid) {
      SpriteRenderer currentCellSpriteRenderer = cell.GetComponentInChildren<SpriteRenderer>();
      currentCellSpriteRenderer.enabled = toggle;
    }
  }

  public void SetSingleCellValueVisibiltyToggle(Cell cell, bool toggle) {
    SpriteRenderer currentCellObject = cell.GetComponentInChildren<SpriteRenderer>();
    currentCellObject.enabled = toggle;
  }

  public void SetCellValuesToNone() {
    foreach (Cell cell in grid) {
      cell.value = GameValue.None;
    }
  }

  public void ToggleFadeAllCells(bool toggle) {
    foreach (Cell cell in grid) {
      cell.Fade(toggle);
    }
  }

  public void FadeCellsExceptLastCellInteractedWith() {
    foreach (Cell cell in grid) {
      if (cell != lastCellInteractedWith) {
        cell.shapeRenderer.material.SetInt("DarkCell", 1);
        cell.Fade(true);
      }
    }
  }

  public void FadeCellsExceptWinning() {
    foreach (Cell cell in grid) {
      cell.ToggleValueDisplayer(false);
    }

    foreach(List<Cell> cellList in gameLogic.winningListOfLists) {
        foreach(Cell cell in cellList) cell.ToggleValueDisplayer(true); 
      }
  }

  public void EnableOpponentCellsForClear() {
    List<Cell> opponentCells = GetOpponentCells();

    foreach(Cell cell in opponentCells) {
      cell.ReductionInteractionActive = true;
      SetSingleCellValueVisibiltyToggle(cell, true);
      cell.shapeRenderer.material = cell.highlightInteract;
      cell.ToggleCross(true);
    }
  }

  internal void ClickRandomOpponentCell() {
    List<Cell> opponentCells = GetOpponentCells();
    opponentCells[Random.Range(0, opponentCells.Count-1)].OnClick();
  }


  public void DisableOpponentCellsForClear() {
    foreach(Cell cell in grid) {
      cell.ReductionInteractionActive = false;
      SetSingleCellValueVisibiltyToggle(cell, false);
      cell.shapeRenderer.material = cell.gridCell;
      cell.ToggleCross(false);
    }
  }

      // cell.shapeRenderer.material.SetColor("ColorInactive", new Color(91/255, 107/255, 188/255)*2.2f);
      // cell.shapeRenderer.material.SetColor("ColorInactive", new Color(27/255, 64/255, 91/255));

  public List<Cell> GetOpponentCells() {
    // Debug.Log(lastCellInteractedWith);
    GameValue valueToLookFor = (lastCellInteractedWith.value == GameValue.Cross) ? GameValue.Nought : GameValue.Cross;
    List<Cell> cellList = new List<Cell>();

    foreach(Cell cell in grid) {
      if (cell.value == valueToLookFor) cellList.Add(cell); 
    }

    return cellList;
  }

  public void Populate() {
    for (int i = 0; i < 3; i++) {
        grid[i, i].value = GameValue.Cross;
        grid[i, i].valueDisplayer.sprite = grid[i, i].crossSprite;
    }
  }

  public void Populate2() {
    grid[2, 0].value = GameValue.Cross;
    grid[2, 0].valueDisplayer.sprite = grid[2, 0].crossSprite;
    grid[1, 1].value = GameValue.Cross;
    grid[1, 1].valueDisplayer.sprite = grid[2, 0].crossSprite;
    grid[0, 2].value = GameValue.Cross;
    grid[0, 2].valueDisplayer.sprite = grid[2, 0].crossSprite;
    
  }

  public void Populate3() {
    grid[0, 0].value = GameValue.Cross;
    grid[0, 0].valueDisplayer.sprite = grid[2, 0].crossSprite;
    grid[0, 1].value = GameValue.Cross;
    grid[0, 1].valueDisplayer.sprite = grid[2, 0].crossSprite;
    grid[1, 1].value = GameValue.Cross;
    grid[1, 1].valueDisplayer.sprite = grid[2, 0].crossSprite;
    
  }

  public void Populate4() {
    grid[0, 0].value = GameValue.Cross;
    grid[0, 0].valueDisplayer.sprite = grid[2, 0].crossSprite;
    grid[0, 1].value = GameValue.Cross;
    grid[0, 1].valueDisplayer.sprite = grid[2, 0].crossSprite;
    grid[0, 2].value = GameValue.Cross;
    grid[0, 2].valueDisplayer.sprite = grid[2, 0].crossSprite; 
  }

  public void Populate5() {
    grid[0, 0].value = GameValue.Nought;
    grid[0, 0].valueDisplayer.sprite = grid[2, 0].noughtSprite;
    grid[1, 0].value = GameValue.Nought;
    grid[1, 0].valueDisplayer.sprite = grid[2, 0].noughtSprite;
    grid[2, 0].value = GameValue.Nought;
    grid[2, 0].valueDisplayer.sprite = grid[2, 0].noughtSprite;
  }

  public void Populate6() {
    grid[2, 0].value = GameValue.Cross;
    grid[2, 0].valueDisplayer.sprite = grid[2, 0].crossSprite;
    grid[2, 1].value = GameValue.Cross;
    grid[2, 1].valueDisplayer.sprite = grid[2, 0].crossSprite;
    grid[2, 2].value = GameValue.Cross;
    grid[2, 2].valueDisplayer.sprite = grid[2, 0].crossSprite;
    grid[2, 3].value = GameValue.Cross;
    grid[2, 3].valueDisplayer.sprite = grid[2, 0].crossSprite;
  }


  public void Populate7() {
    grid[2, 0].value = GameValue.Cross;
    grid[2, 0].valueDisplayer.sprite = grid[2, 0].crossSprite;
    grid[2, 1].value = GameValue.Cross;
    grid[2, 1].valueDisplayer.sprite = grid[2, 0].crossSprite;
    grid[2, 2].value = GameValue.Cross;
    grid[2, 2].valueDisplayer.sprite = grid[2, 0].crossSprite;
  }
  public void Populate8() {
    grid[2, 0].value = GameValue.Nought;
    grid[2, 0].valueDisplayer.sprite = grid[2, 0].noughtSprite;
    grid[3, 1].value = GameValue.Nought;
    grid[3, 1].valueDisplayer.sprite = grid[2, 0].noughtSprite;
    grid[2, 2].value = GameValue.Nought;
    grid[2, 2].valueDisplayer.sprite = grid[2, 0].noughtSprite;
  }

  public void Score() {
    UIController uiController = FindObjectOfType<UIController>();
    uiController.scoreBarX.playerScoreSlider.value = 4f;
    uiController.scoreBarO.playerScoreSlider.value = 4f;
    GameController gameController = FindObjectOfType<GameController>();
    gameController.playerX.score = 4;
    gameController.playerO.score = 4;
  }

  public void Score2() {
    UIController uiController = FindObjectOfType<UIController>();
    // uiController.scoreBarX.playerScoreSlider.value = 14f;
    GameController gameController = FindObjectOfType<GameController>();
    gameController.playerX.score = 14;
  }

  public void RoundsWon() {
    GameController gameController = FindObjectOfType<GameController>();
    gameController.playerX.nRoundsWon = 4;
    gameController.currentRound = 4;
  }

  public void Pause() {
    TimerController timerController = FindObjectOfType<TimerController>();
    if(Time.timeScale == 1) {
      timerController.pause = true;
      Time.timeScale = 0;
    } else {
      timerController.pause = false;
      Time.timeScale = 1;
    }
  }
}
