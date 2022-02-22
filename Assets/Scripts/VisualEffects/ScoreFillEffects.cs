using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreFillEffects : MonoBehaviour {
  Material fillMaterial;
  public Image fill;

  public Color gold = new Color(255, 194, 0, 0);
  public Color diamond = new Color(100, 137, 191, 0);
  public Color red = new Color(210, 0, 0, 0);
  public Color green = new Color(0, 210, 0, 0);
  Color startColor = new Color(42/255, 62/255, 164/255, 0);

  Color endColor;

  // float startTime;
  float elapsedTime;

  bool pulseOn = false;

  void Start() {
    fillMaterial = fill.material; 
  }

  public void Pulse(Color pulseColor, float time) { 
    startColor = fillMaterial.color;
    fillMaterial.EnableKeyword("_EMISSION");
    endColor = pulseColor;
    elapsedTime = 0;
    pulseOn = true;
    StartCoroutine(PulseForSeconds(pulseColor, time));
  }

  IEnumerator PulseForSeconds(Color pulseColor, float time) {
    // startTime = Time.time;
    while(pulseOn) {
      Color lerpColor = Color.Lerp(endColor, startColor, Mathf.PingPong(Time.time, .5f));
      fillMaterial.SetColor("_EmissionColor", (lerpColor / 50));
      elapsedTime += Time.unscaledDeltaTime;
      yield return null;
    }
    StopAndReset();
  }

  public void Stop() {
    pulseOn = false;
  }
  
  void StopAndReset() {
    fillMaterial.SetColor("_EmissionColor", startColor);
    fillMaterial.DisableKeyword("_EMISSION");
  }
}

