using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashCell : MonoBehaviour {
  public Cell thisCell;
  MeshRenderer rend;

  WaitForSeconds wait = new WaitForSeconds(.85f);

  void Awake() {
    rend = thisCell.shapeRenderer;
  }

  public void FlashWhite() {
      rend.material.SetColor("FlashColor", new Color(1.2f, 1.6f, 1.2f, 1f));
      StartCoroutine(StartFlash());
  }

  public void FlashRed() {
      rend.material.SetColor("FlashColor", new Color(4f, 0f, 0f, 1f));
      StartCoroutine(StartFlash());
  }

  IEnumerator StartFlash() {
    rend.material.SetInt("ShouldFlash", 1);
    yield return wait;
    rend.material.SetInt("ShouldFlash", 0);
  }

  public void FlashGreen() {
    rend.material.SetColor("FlashColor", new Color(0f, 4f, 0f, 1f));
    StartCoroutine(StartFlash());
  }
}

