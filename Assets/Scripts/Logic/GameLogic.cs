using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {
  public GridController gridController;
  public GameController gameController;
  public List<List<Cell>> winningListOfLists = new List<List<Cell>>();
  public List<List<Cell>> closeToWinningListOfLists = new List<List<Cell>>();
  public int rowLenToWin = 3;

  bool isWithinRange(int x, int y) {
    return x >= 0 && x < 3 && y >= 0 && y < 3;
  }

  public List<List<Cell>> GetPossibleRows(Cell[,] grid) {
    List<List<Cell>> rowList = new List<List<Cell>>();

    for (int x = -1; x < (grid.Length / 4) + 1; x++) {
      List<Cell> row = new List<Cell>();
      List<Cell> column = new List<Cell>();
      List<Cell> diagonalOne = new List<Cell>();
      List<Cell> diagonalTwo = new List<Cell>();
      for (int y = 0; y < (grid.Length / 4) + 1; y++) {
        if (isWithinRange(x, y)) row.Add(grid[x, y]);
        if (isWithinRange(y, x)) column.Add(grid[y, x]);
        if (isWithinRange(x + y, y)) diagonalOne.Add(grid[x + y, y]);
        if (isWithinRange(x - y, y)) diagonalTwo.Add(grid[x - y, y]);
      }
      rowList.Add(row);
      rowList.Add(column);
      rowList.Add(diagonalOne);
      rowList.Add(diagonalTwo);
    }
    return rowList;
  }

  public bool checkGridForWin(Cell[,] grid) {
    if (gameController.activePlayer.score > 14) return true;
    List<List<Cell>> rowList = GetPossibleRows(grid);

    foreach(List<Cell> cellList in rowList) {
      if (checkCells(cellList, rowLenToWin).Count == rowLenToWin) return true;
    }
    return false;
  }
  List<Cell> checkCells(List<Cell> row, int amount) {
    List<Cell> cells = new List<Cell>();

    foreach (Cell cell in row) {
      if (cell.value == gameController.activePlayer.value) { 
        cells.Add(cell);
        if (cells.Count == amount) break;
      } else {
        cells = new List<Cell>();
      }
    }
    return cells;
  }

  public void checkGridForCloseWin(Cell[,] grid) {
    closeToWinningListOfLists.Clear();

    List<List<Cell>> rowList = GetPossibleRows(grid);

    foreach(List<Cell> cellList in rowList) {
      if (checkCellsForCloseWin(cellList, rowLenToWin).Count > 1) closeToWinningListOfLists.Add(checkCellsForCloseWin(cellList, rowLenToWin));
    }
  }
  List<Cell> checkCellsForCloseWin(List<Cell> row, int amount) {
    List<Cell> cells = new List<Cell>();
    if(row.Count < 3) return cells;
    Player lastPlayer = (gameController.activePlayer == gameController.playerO) ? gameController.playerX : gameController.playerO;
    int spaceCounter = 0;
    foreach (Cell cell in row) {
      if (cell.value == lastPlayer.value) {
        cells.Add(cell);
        spaceCounter = 0;
      } else {
        if (spaceCounter == 0) {
          spaceCounter++;
        } else if (spaceCounter == 1 && cells.Count < 2){
          cells = new List<Cell>();
          spaceCounter = 0;
        }
      }
    }
    return cells;
  }

  public void checkGridForWinningCells(Cell[,] grid) {
    winningListOfLists.Clear();
    List<List<Cell>> rowList = GetPossibleRows(grid);

    foreach(List<Cell> cellList in rowList) {
      if (checkCellsForConnectedWinning(cellList, rowLenToWin).Count > 2) winningListOfLists.Add(checkCellsForConnectedWinning(cellList, rowLenToWin));
    }
  }
  List<Cell> checkCellsForConnectedWinning(List<Cell> row, int amount) {
    Player winningPlayer = gameController.winningPlayer;
    List<Cell> cells = new List<Cell>();
    foreach (Cell cell in row) {
      if (cell.value == winningPlayer.value) {
        cells.Add(cell);
      } else if (cells.Count < 3){
        cells = new List<Cell>();
      }
    }
    return cells;
  }

  public bool TestForWin(Cell cell, Cell[,] grid) {
    GameValue oldValue = cell.value;
    GameValue value = gameController.GetPlayerSide();
    grid[cell.position[0], cell.position[1]].value = value;
    if (checkGridForWin(grid)) {
      cell.value = oldValue;
      return true;
    } else {
      cell.value = oldValue;
      return false;
    }
  }
}
