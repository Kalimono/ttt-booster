using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour {
  public bool interactable;
  public SpriteRenderer valueDisplayer;
  public Sprite crossSprite;
  public Sprite noughtSprite;
  public Sprite noneSprite;
  public MeshRenderer shapeRenderer;
  public ParticleSystem particleEffect;

  public GameObject hoverValue;

  public Vector2Int position;
  public GameValue value = GameValue.None;

  private GameController gameController;
  public UIController uiController;
  public SquareController squareController;
  public GridController gridController;
  public SoundFxController soundFxController;
  public DotController dotController;
  public FurHatCommunication furHatCommunication;
  public DataSave dataSave;

  public RotateMe rotateMe;
  public FadeCellOverTime fadeCellOverTime;

  public Material gridCell;
  public Material pink;
  public Material winningCellHighlight;
  public Material highlightInteract;

  public OutcomeColor outcomeColor;

  public AudioClip correctResponseSound;

  public Sprite[] outcomes;
  public SpriteRenderer outcome;

  public int outcomeValue;
  public int outcomeArea;

  public GameObject cross;

  private void Awake() {
    gameController = FindObjectOfType<GameController>();
    squareController = FindObjectOfType<SquareController>();
    rotateMe = FindObjectOfType<RotateMe>();
    gridController = FindObjectOfType<GridController>();
    soundFxController = FindObjectOfType<SoundFxController>();
    dotController = FindObjectOfType<DotController>();
    uiController = FindObjectOfType<UIController>();
    furHatCommunication = FindObjectOfType<FurHatCommunication>();
    dataSave = FindObjectOfType<DataSave>();
  }

  public void SetInteractive(bool value) {
    interactable = value;
    int toggle = value ? 0 : 1;
    shapeRenderer.material.SetInt("DarkCell", toggle);
  }

  public void OnClick() {
    gridController.lastCellInteractedWith = this;

    if (interactable) {
      
      interactable = false;
      bool isCorrectMove = CheckIfCorrectMove();
      valueDisplayer.enabled = false;

      dotController.SetOutcome(this);

      if (isCorrectMove) {
        if(dotController.toggleDot) {
          furHatCommunication.SendOutcome(this.outcomeArea);
        } else {
          furHatCommunication.SendNotcome(this.outcomeArea);
        }

        dataSave.whiteCorrect = 1;
        
        
        uiController.UpdateScoreBar();

        value = gameController.GetPlayerSide();
        gridController.FadeCellsExceptLastCellInteractedWith();
        valueDisplayer.sprite = value == GameValue.Cross ? crossSprite : noughtSprite;
        if(gameController.activePlayer == gameController.playerX) {
          rotateMe.Rotate();
        } else {
          GetComponent<FlashCell>().FlashGreen();
          soundFxController.PlayAICorrectSound();
        }
        
      } else {
        dataSave.whiteCorrect = 0;
        gridController.ToggleFadeAllCells(true);
        furHatCommunication.SendIncorrectResponse();
        soundFxController.PlayWrongResponseSound();
        GetComponent<FlashCell>().FlashRed();
      }
      gameController.EndTurn();//isCorrectMove); ##
    }
    gridCell.SetInt("DarkCell", 1);
  }

  bool CheckIfCorrectMove() {
    if (this == squareController.correctCell) return true;
    return false;
  }

  public void SetPositionInGrid(Vector2Int pos) {
    position = pos;
  }


  public void HighlightMeWhite(float seconds) {
    soundFxController.PlayStimuliSound();
    StartCoroutine(ChangeToWhiteAndBack(seconds));
  }

  IEnumerator ChangeToWhiteAndBack(float seconds) {
    Color defaultColor = shapeRenderer.material.GetColor("ColorInactive");
    shapeRenderer.material.SetColor("ColorInactive", new Color(2f, 2f, 2f, 1f));
    yield return new WaitForSeconds(seconds);
    shapeRenderer.material.SetColor("ColorInactive", defaultColor); //new Color(0.106f, 0.251f, 0.357f, 0.000f));
  }

  public void FlashMeGreenSeconds(float seconds) {
    StartCoroutine(FlashGreenSeconds(seconds));
  }

  IEnumerator FlashGreenSeconds(float seconds) {
    Color defaultColor = shapeRenderer.material.GetColor("ColorInactive");
    float timeIncrement = seconds/4;

    for (float i = 0; i < seconds; i+=timeIncrement) {
      shapeRenderer.material.SetColor("ColorInactive", Color.green*2);
      yield return new WaitForSeconds(timeIncrement/2);
      shapeRenderer.material.SetColor("ColorInactive", defaultColor);
      yield return new WaitForSeconds(timeIncrement/2);
    }

    this.Fade(false);
  }

  public void HighlightMeColor(float seconds, Color color) {
    soundFxController.PlayStimuliSound();
    StartCoroutine(ChangeToColorAndBack(seconds, color));
  }

  public IEnumerator ChangeToColorAndBack(float seconds, Color color) {
    Color defaultColor = shapeRenderer.material.GetColor("ColorInactive");
    // Color randomColor = GetRandomColor();
    shapeRenderer.material.SetColor("ColorInactive", color*4);
    
    yield return new WaitForSeconds(seconds);
    shapeRenderer.material.SetColor("ColorInactive", defaultColor); //new Color(0.106f, 0.251f, 0.357f, 0.000f));
  }



  public void Fade(bool toggle) {
    if (toggle) {
      fadeCellOverTime.FadeCellDark();
    } else {
      fadeCellOverTime.FadeCellLight();
    }
  }

  public void PlayCorrectResponseSound(int outcomeArea) {
    soundFxController.outcomeAudioSource.PlayOneShot(soundFxController.outcomeSounds[outcomeArea]);
  }

  public void ToggleValueDisplayer(bool toggle) {
    valueDisplayer.enabled = toggle;
  }

  public void NullValue() {
    this.value = GameValue.None;
    valueDisplayer.GetComponent<SpriteRenderer>().sprite = noneSprite;
  }

  public void PresentHoverMarker(int forSeconds) {
    StartCoroutine(ShowMarker(forSeconds));
  }

  IEnumerator ShowMarker(int forSeconds) {
    hoverValue.GetComponent<SpriteRenderer>().sprite = (gameController.activePlayer == gameController.playerX) ? crossSprite : noughtSprite;
    yield return new WaitForSeconds(forSeconds);
    hoverValue.GetComponent<SpriteRenderer>().sprite = null;
  }

  bool CheckIfThresholdPassed(int newScore) {
    ScoreBar currentScoreBar = (gameController.activePlayer == gameController.playerX) ? uiController.scoreBarX : uiController.scoreBarO;
    if(new List<int>{5, 6, 7}.Contains(newScore) && !currentScoreBar.firstThresholdPassed) {
      return true;
    } else if (new List<int>{10, 11, 12}.Contains(newScore) && !currentScoreBar.secondThresholdPassed) {
      return true;
    }
    return false;
  }

  void RemoveValueMarker() {
    NullValue();
  }
}
