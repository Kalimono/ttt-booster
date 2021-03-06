using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {
  public LevelSettings[] levels;
  public TimerController timerController;
  public ConditionController conditionController;
  public UIController uIController;
  public SceneController sceneController;
  public int lastLevelIndex = 0;

  public static LevelController instance;

  void Awake() {
    FindReferences();
    
    lastLevelIndex = sceneController.GetMemory();
    uIController.UpdateCurrentLevelText(lastLevelIndex);

    timerController.levelSettings = levels[0];

    conditionController.levelSettings = levels[lastLevelIndex];
    conditionController.LoadLevelSettings();

    if (instance != null) {
        Destroy(gameObject);
        return;
    }

    instance = this;
    DontDestroyOnLoad(gameObject);
  }

  public void FindReferences() {
    timerController = FindObjectOfType<TimerController>();
    conditionController = FindObjectOfType<ConditionController>();
    uIController = FindObjectOfType<UIController>();
    sceneController = FindObjectOfType<SceneController>();
  }

  public void LoadNextLevel() {
    FindReferences();
    lastLevelIndex++;
    
    if (lastLevelIndex > levels.Length - 1) {
      lastLevelIndex = 0;
    }

    timerController.levelSettings = levels[lastLevelIndex];
    conditionController.levelSettings = levels[lastLevelIndex];
    conditionController.levelValueText.text = lastLevelIndex.ToString();
    conditionController.LoadLevelSettings();
  }

  public void SwitchLevel(int level) {
    timerController.levelSettings = levels[level-1];
    conditionController.levelSettings = levels[level-1];
    conditionController.LoadLevelSettings();
  }

}
