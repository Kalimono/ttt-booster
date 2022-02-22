using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PulseScoreMarker : MonoBehaviour {
  public GameObject marker;
  public Image markerImage;

  public Material thresholdMarkerOn;
  public Material thresholdMarkerOff;
  public Material thresholdMarkerPassed;

  Color gold = new Color(255, 194, 0, 0);
  Color diamond = new Color(100, 137, 191, 0);

  Color lerpColor;

  public bool pulse;
  float startTime;
  float duration;


  public void StartPulse(float pulseTime) {
    pulse = true;
    markerImage.material = thresholdMarkerOn;
    startTime = Time.time;
    duration = pulseTime+1f;
  }

  void Update() {
    if (pulse) {
      lerpColor = Color.Lerp(diamond, gold, Mathf.PingPong(Time.time, .75f));
      markerImage.material.SetColor("_EmissionColor", lerpColor / 25);
      // if (!pulsating) Stop();
    }
  }

  public void Stop() {
    marker.GetComponent<Image>().material = thresholdMarkerPassed;
    pulse = false;
  }
}
