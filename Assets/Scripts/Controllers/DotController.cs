using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum OutcomeColor {
  gold,
  diamond
}

public class DotController : MonoBehaviour {
  public SoundFxController soundFxController;

  public Image dotStatus;

  // public List<int> NonDotSequence;
  public bool toggleDot;

  public void SetOutcome(Cell cell) {
    if (!toggleDot) cell.outcomeArea = StimuliSequencer.GetNonDifferentialOutcome();

    if (cell.outcomeArea == 1 || cell.outcomeArea == 3) {
      cell.outcomeValue = 3;
    } else {
      cell.outcomeValue = 1;
    }

    if (cell.outcomeArea == 0 || cell.outcomeArea == 1) {
      cell.outcomeColor = OutcomeColor.gold;
    } else {
      cell.outcomeColor = OutcomeColor.diamond;
    }

    cell.outcome.sprite = cell.outcomes[cell.outcomeArea];
    cell.correctResponseSound = soundFxController.outcomeSounds[cell.outcomeArea];
  }

  public void SetDotOutcomes(List<Cell> targetCellList) {
    int outcomeindex = 0;
    for (int i = 0; i < targetCellList.Count; i++) {
      if(outcomeindex > 3) outcomeindex = 0;
      // targetCellList[i].outcomeNum = outcomeindex;
      targetCellList[i].outcomeArea = outcomeindex;
      outcomeindex++;
    }
  }

  // public void SetOutcomeAreas(Cell[,] grid) {
  //   int side = grid.Length / 4;

  //   foreach (Cell cell in grid) {
  //     if (cell.position[0] < side / 2 && cell.position[1] < side / 2) cell.outcomeArea = 0;
  //     if (cell.position[0] >= side / 2 && cell.position[1] < side / 2) cell.outcomeArea = 1;
  //     if (cell.position[0] < side / 2 && cell.position[1] >= side / 2) cell.outcomeArea = 2;
  //     if (cell.position[0] >= side / 2 && cell.position[1] >= side / 2) cell.outcomeArea = 3;
  //   }
  // }

  public void ToggleDotButton() {
    if (toggleDot) {
      toggleDot = false;
      dotStatus.color = Color.red;
    } else {
      toggleDot = true;
      dotStatus.color = Color.green;
    }
  }
}

