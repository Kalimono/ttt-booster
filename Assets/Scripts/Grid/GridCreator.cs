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
    transform.position = Vector3.zero;
    ClearChildren();

    Cell[,] grid = new Cell[gridSize, gridSize];
    for (int x = 0; x < gridSize; x++) {
      for (int y = 0; y < gridSize; y++) {
        Cell cell = CreateCell(x, y);
        grid[x, y] = cell;
      }
    }
    transform.position = new Vector3(-3.33f, -5.6f, 1);
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
}
