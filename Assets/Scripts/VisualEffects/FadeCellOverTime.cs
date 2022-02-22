using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCellOverTime : MonoBehaviour {
  public Cell thisCell;
  MeshRenderer rend;
  Color darkness = new Color(0.027f, 0.055f, 0.082f, 0.000f);
  Color lightness = new Color(0.106f, 0.251f, 0.357f, 0.000f);
  
  WaitForSecondsRealtime wait = new WaitForSecondsRealtime(.02f); //.01f

  void Awake() {
    rend = thisCell.shapeRenderer;
  }
  
  public void FadeCellDark() {
    StartCoroutine(FadeMe(thisCell, darkness));
  }

  public void FadeCellLight() {
    if(rend.material.HasProperty("ColorInactive")) StartCoroutine(FadeMe(thisCell, lightness));
  }

  IEnumerator FadeMe(Cell cell, Color targetColor) {
    Color cellValueColor = rend.material.GetColor("ColorInactive");
    Color lerpedColor;

    for (float t = 1; t > 0; t -= .1f) { //.035f
      lerpedColor = Color.Lerp(targetColor, cellValueColor, t);
      rend.material.SetColor("ColorInactive", lerpedColor);
      yield return wait;
    }
  }
}
