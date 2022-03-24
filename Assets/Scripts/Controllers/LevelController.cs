using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {
  public LevelSettings[] levels;
  TimerController timerController;
  ConditionController conditionController;
  AutoTurnEnderController autoTurnEnderController;
  UIController uIController;
  public int lastLevelIndex = 0;

  

  void Awake() {
    timerController = FindObjectOfType<TimerController>();
    conditionController = FindObjectOfType<ConditionController>();
    autoTurnEnderController = FindObjectOfType<AutoTurnEnderController>();
    uIController = FindObjectOfType<UIController>();

    autoTurnEnderController.Init(levels[0]);

    timerController.levelSettings = levels[0];
    conditionController.levelSettings = levels[lastLevelIndex];
    conditionController.LoadLevelSettings();
    // dataPoster.InitializeGame(levels[0]);
  }

  public void LoadNextLevel() {
    lastLevelIndex++;
    if (lastLevelIndex > levels.Length - 1) {
      lastLevelIndex = 0;
    }

    timerController.levelSettings = levels[lastLevelIndex];
    conditionController.levelSettings = levels[lastLevelIndex];

    uIController.UpdateCurrentLevelText(lastLevelIndex+1);
    conditionController.LoadLevelSettings();
  }

  public void SwitchLevel(int level) {
    timerController.levelSettings = levels[level-1];
    conditionController.levelSettings = levels[level-1];
    conditionController.LoadLevelSettings();
    // conditionController.levelSlider.value = level-1;
    // conditionController.levelSliderChange();
  }

}
