using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour {
  public bool autoUpdate;
  [Range(1, 5)]
  public int gridSize;
  public float cellSize = 1;

  [Range(0, 10)]
  public int padding;
  public GameObject cellPrefab;

  public SquareController squareController;
  public DotController dotController;
  public GameController gameController;
  public GridController gridController;

  Vector3 currentGridPosition;
  float currentCellSize;

  void ClearChildren() {
    var tempArray = new GameObject[this.transform.childCount];
    for (int i = 0; i < tempArray.Length; i++) {
      tempArray[i] = this.transform.GetChild(i).gameObject;
    }
    
    foreach (var child in tempArray) {
      DestroyImmediate(child);
    }
  }

  public Cell[,] InitializeGrid() {
    transform.position = GetGridPosition(gridSize);//Vector3.zero;
    cellSize = GetCellSize(gridSize);
    ClearChildren();

    Cell[,] grid = new Cell[gridSize, gridSize];
    for (int x = 0; x < gridSize; x++) {
      for (int y = 0; y < gridSize; y++) {
        Cell cell = CreateCell(x, y);
        grid[x, y] = cell;
      }
    }
    return grid;
  }

  Cell CreateCell(int x, int y) {
    GameObject cellObject = Instantiate(cellPrefab);
    cellObject.name = "x: " + x + ", y: " + y;
    cellObject.transform.parent = transform;
    cellObject.transform.localPosition = new Vector3(CellOffset(x), CellOffset(gridSize) - CellOffset(y));
    cellObject.transform.localScale = new Vector3(cellSize, cellSize, 1);
    Cell cell = cellObject.GetComponent<Cell>();
    cell.SetPositionInGrid(new Vector2Int(x, y));
    return cell;
  }

  float CellOffset(int i) { //cellSize == 2.05, padding == 2
    return (i * cellSize) + ((padding / 10f) * i);
  }

  Vector3 GetGridPosition(int gridSize) {
    Vector3 position;
    switch (gridSize) {
      case 4:
        position = new Vector3(-94, -152, 188);
        break;
      case 5:
        position = new Vector3(-105, -152, 188);
        break;
      default: //3
        position = new Vector3(-77, -152, 188);
        break;
    }
    return position;
  }

  float GetCellSize(int gridSize) {
    float cellSize;
    switch (gridSize) {
      case 4:
        cellSize = 2.05f;
        break;
      case 5:
        cellSize = 1.75f;
        break;
      default: //3
        cellSize = 2.7f;
        break;
    }
    return cellSize;
  }

  public void CreateGrid(int size) {
    gridSize = size;
    gridController.grid = InitializeGrid();
    squareController.Initialize();
  }
}
