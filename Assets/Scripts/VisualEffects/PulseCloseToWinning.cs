using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseCloseToWinning : MonoBehaviour {
  public Cell thisCell;
  MeshRenderer rend;

  Color whiteColor1 = new Color(2.8f, 2.8f, 2.8f, 1f);
  Color whiteColor2 = new Color(2f, 2f, 2f, 1f);

  Color lerpColor;
  Color startColor;

  void Awake() {
    thisCell = GetComponent<Cell>();
    
    rend = thisCell.shapeRenderer;
    // Debug.Log(rend.material.GetColor("ColorInactive"));
  }

  public void StartFlash() {
    startColor = rend.material.GetColor("ColorInactive");
    
    rend.material.SetColor("ColorInactive", whiteColor1);
    StartCoroutine(FlashMe(whiteColor1, startColor));

  }

  IEnumerator FlashMe(Color start, Color finish) {
    // for (float t = 0; t < 1; t += .01f) {
    //   lerpColor = Color.Lerp(start, finish, t);
    //   rend.material.SetColor("ColorInactive", lerpColor);
    //   yield return new WaitForSeconds(.01f);
    // }
    
    float elapsedTime = 0f;
    float totalTime = .75f;

    while(elapsedTime < totalTime) {
      lerpColor = start;
      lerpColor = Color.Lerp(lerpColor, finish, elapsedTime/totalTime);
      // Debug.Log(lerpColor);
      rend.material.SetColor("ColorInactive", lerpColor);
      elapsedTime += Time.deltaTime;
      yield return null;
    }
    rend.material.SetColor("ColorInactive", new Color(0.106f, 0.251f, 0.357f, 0.000f));
  }
}
