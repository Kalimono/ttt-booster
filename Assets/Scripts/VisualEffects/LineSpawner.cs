using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LineSpawner : MonoBehaviour {
  public GameObject line;
  GameObject[] lines;

  WaitForSeconds wait = new WaitForSeconds(.005f);

  public void DrawLineBetweenCells(List<List<Cell>> cellLists) {
    lines = new GameObject[cellLists.Count];
    int counter = 0;
    foreach (List<Cell> cellList in cellLists) {
      Quaternion rotation = GetRotation(cellList);

      GameObject newLine = Instantiate(line, cellList[0].transform.position - new Vector3(0f, 0f, -1f), rotation);
      lines[counter] = newLine;
      counter++;

      float targetXScale = Mathf.Sqrt(Vector3.Distance(cellList[cellList.Count - 1].transform.position, cellList[0].transform.position)) / 2;//cellList.Count/2;

      Vector3 startVectorPosition = cellList[0].transform.position;
      Vector3 halfWayVector = GetTargetVectorPosition(cellList);

      StartCoroutine(Draw(newLine, startVectorPosition, halfWayVector, targetXScale));
    }
  }

  IEnumerator Draw(GameObject line, Vector3 startVectorPosition, Vector3 halfWayVector, float targetXScale) {
    Vector3 startScale = line.transform.localScale;
    Vector3 currentPosition = startVectorPosition;
    // currentPosition.z = 1; //so that WebGL behaves
    float currentXScale = 0f;

    for (float t = 0f; t < 1; t += .01f) {
      currentPosition = new Vector3(
          Mathf.Lerp(currentPosition.x, halfWayVector.x, t),
          Mathf.Lerp(currentPosition.y, halfWayVector.y, t),
          1f); //Mathf.Lerp(currentPosition.z, halfWayVector.z, t)

      currentXScale = Mathf.Lerp(currentXScale, targetXScale, t);

      line.transform.position = currentPosition;
      line.transform.localScale = new Vector3(currentXScale, startScale.y, startScale.z);

      yield return wait;
    }
  }

  Vector3 GetTargetVectorPosition(List<Cell> list) {
    return Vector3.Lerp(list[0].transform.position, list[list.Count - 1].transform.position, .5f);
  }

  Quaternion GetRotation(List<Cell> list) {
    Vector3 targetDir = (list[list.Count - 1].transform.position - list[0].transform.position).normalized;
    float angle = Vector3.Angle(targetDir, list[list.Count - 1].transform.right);
    Vector3 rotationVector = new Vector3(0, 0, -angle);
    Quaternion rotation = Quaternion.Euler(rotationVector);
    return rotation;
  }

  public void DestroyLines() {
    if (lines == null) return;
    foreach (GameObject newline in lines) {
      Destroy(newline);
    }
  }
}
