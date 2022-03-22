using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoTurnEnderController : MonoBehaviour {
  private GameController gameController;
  public SquareController squareController;
  public float successPercentage;
  private float timeToThink;

  public bool toggleAutoPlay = false;
  public Image autoPlayStatus;

  public void Init(LevelSettings levelSettings) {
    foreach (Timer timer in levelSettings.timers) {
      if (timer.gameEvent == GameEvent.Response) {
        timeToThink = timer.timeout * 0.2f;//Random.Range(0.5f, 0.8f);
      }
    }
    // successPercentage = levelSettings.aiCorrectResponseProbabilityPercent;
  }

  public bool CheckIfSuccessfulMove(float successPercentage) {
    float randint = Random.Range(1, 100);
    return randint <= successPercentage;
  }

  // public void AutoPlayTurn() {
  //   StartCoroutine(ThinkBeforeChoosing());
  // }

  // public void MakeMove() {
  //   Cell position;

  //   bool success = CheckIfSuccessfulMove(successPercentage);
  //   if (success) {
  //     position = squareController.correctCell;
  //   } else {
  //     position = squareController.distractorCell;
  //   }
  //   squareController.HideAIOptions();
  //   position.SetInteractive(true);
  //   position.OnClick();
  //   position.PresentHoverMarker(1);
  // }

  // IEnumerator ThinkBeforeChoosing() {
  //   squareController.ShowAIoptions();
  //   yield return new WaitForSeconds(timeToThink / 1000);
  //   MakeMove();
  // }

  public void ToggleAutoPlay() {
    if (toggleAutoPlay) {
      toggleAutoPlay = false;
      autoPlayStatus.color = Color.red;
    } else {
      toggleAutoPlay = true;
      autoPlayStatus.color = Color.green;
    }
  }
}
