using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RotateMe : MonoBehaviour {
  GridController gridController;
  public bool rotationActive = false;

  private Vector3 startAngle;
  private Vector3 targetAngle;
  private Vector3 currentAngle;

  private Vector3 startPosition;
  private Vector3 targetPosition;
  private Vector3 currentPosition;

  private Vector3 defaultPosition;

  private float lerpTime;
  private float currentLerpTime;

  private int halfRot;

  private int frameCounter = 0;

  void Awake() {
    gridController = FindObjectOfType<GridController>();
  }

  void Start() {
    defaultPosition = transform.position;
  }

  void Update() {
    if (rotationActive) {
      currentLerpTime += Time.unscaledDeltaTime;
      // Debug.Log(targetPosition.x);
      // Debug.Log(targetPosition.y);
      // Debug.Log(targetPosition.z);
      // Debug.Log("yep");
      if(currentLerpTime/lerpTime < 1 && frameCounter % 2 == 0) {
        // currentAngle = Vector3.Lerp(startAngle, targetAngle, currentLerpTime/lerpTime);
        // currentAngle = new Vector3(
        //   Mathf.LerpAngle(startAngle.x, targetAngle.x, currentLerpTime / lerpTime),
        //   Mathf.LerpAngle(startAngle.y, targetAngle.y, currentLerpTime / lerpTime),
        //   Mathf.LerpAngle(startAngle.z, targetAngle.z, currentLerpTime / lerpTime));
        // currentPosition = Vector3.Lerp(startPosition, targetPosition, currentLerpTime/lerpTime);
        // currentPosition = new Vector3(
        //   Mathf.Lerp(startPosition.x, targetPosition.x, currentLerpTime / lerpTime),
        //   Mathf.Lerp(startPosition.y, targetPosition.y, currentLerpTime / lerpTime),
        //   Mathf.Lerp(startPosition.z, targetPosition.z, currentLerpTime / lerpTime));

        transform.eulerAngles = Vector3.Lerp(startAngle, targetAngle, currentLerpTime/lerpTime);//currentAngle;
        transform.position = Vector3.Lerp(startPosition, targetPosition, currentLerpTime/lerpTime);//currentPosition;
      }
      
      if (currentLerpTime / lerpTime >= 1) {
        targetAngle = new Vector3(0f, 0f, 0f);
        targetPosition = startPosition;
        startPosition = transform.position;
        startAngle = transform.eulerAngles;
        currentLerpTime = 0;
        halfRot++;

        if (halfRot == 1) {
          Cell thisCell = GetComponentInParent<Cell>();
          thisCell.PlayCorrectResponseSound(thisCell.outcomeArea);
          StartCoroutine(WaitAMomentHalfWay());
        }
      }

      if (halfRot == 2) StopAndReset();
      frameCounter++;
    }
  }

  public void Rotate() {
    gridController.rotation = true;
    lerpTime = .5f;
    halfRot = 0;
    startAngle = transform.eulerAngles;
    currentAngle = startAngle;
    targetAngle = new Vector3(0f, 180f, 0f);
    rotationActive = true;
    currentLerpTime = 0;
    targetPosition = new Vector3(0f, 0f, 0f);
    startPosition = transform.position;
    currentPosition = startPosition;
  }

  IEnumerator WaitAMomentHalfWay() {
    rotationActive = false;
    yield return new WaitForSecondsRealtime(1.6f);;
    rotationActive = true;
  }

  void StopAndReset() {
    gridController.rotation = false;
    targetAngle = new Vector3(0f, 180f, 0f);
    targetPosition = new Vector3(3.36f, 5.61f, -5.06f);
    transform.eulerAngles = new Vector3(0f, 0f, 0f);
    transform.position = defaultPosition;
    currentAngle = transform.eulerAngles;
    rotationActive = false;
    frameCounter = 0;
  }
}

