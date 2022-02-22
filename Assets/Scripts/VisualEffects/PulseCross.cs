using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PulseCross : MonoBehaviour {


  float z;

  float startPositionZ;
  float targetPositionZ;
  Vector3 currentPosition;
  Vector3 startPosition;

  public GameObject cross;

  bool pulsate = false;

  float speed = 1f;


  void Update() {
    if (pulsate) {
      z = Mathf.Lerp(startPositionZ, targetPositionZ, Mathf.PingPong(speed * Time.unscaledTime, .3f));
      cross.transform.localPosition = new Vector3(startPosition.x, startPosition.y, z);
    }
  }

  public void StartPulsating() {
    cross.SetActive(true);
    pulsate = true;
    startPosition = cross.transform.localPosition;
    startPositionZ = startPosition.z;
    targetPositionZ = startPositionZ - .3f;
    currentPosition = cross.transform.localPosition;
  }

  public void StopPulsating() {
    cross.SetActive(false);
    pulsate = false;
  }

  public void Toggle(bool toggle) {
    if (toggle) {
      StartPulsating();
    } else {
      StopPulsating();
    }
  }
}
