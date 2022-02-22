using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusSquareEnlarger : MonoBehaviour {
  private bool changeSizeActive = false;
  private bool fadeOut = false;

  private Vector3 startScale;
  private Vector3 currentScale;
  private Vector3 targetScale;

  private float duration;
  private float time;
  private float timeProgress;

  private CanvasRenderer imageRenderer;
  private float startAlpha;
  private float currentAlpha;
  private float targetAlpha;

  private float currentAlphaFadeOut;

  private int frameCounter;

  void Awake() {
    imageRenderer = GetComponent<CanvasRenderer>();
  }

  public void Update() {
    if (changeSizeActive) {
      time += Time.deltaTime;
      timeProgress = time / duration;
    
      // if(frameCounter % 2 == 0) {
        currentScale = new Vector3(
            Mathf.Lerp(startScale.x, targetScale.x, timeProgress),
            Mathf.Lerp(startScale.y, targetScale.y, timeProgress),
            Mathf.Lerp(startScale.z, targetScale.z, timeProgress));

        transform.localScale = currentScale;
        currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha, timeProgress / 2);
        imageRenderer.SetAlpha(currentAlpha);
        // }
      if (currentScale.Equals(targetScale)) changeSizeActive = false;
      frameCounter++;
    }

    if (fadeOut) {
      time += Time.deltaTime;
      timeProgress = time / duration;
      // if(frameCounter % 2 == 0) {
        currentAlphaFadeOut = Mathf.Lerp(startAlpha, targetAlpha, timeProgress / 2);
        imageRenderer.SetAlpha(currentAlphaFadeOut);
      // }
      
      if (currentAlphaFadeOut.Equals(0f)) fadeOut = false;
      frameCounter++;
    }
  }

  public void Enlarge() {
    time = 0;
    duration = 0.2f;
    transform.localScale = new Vector3(0f, 0f, 0f);
    startScale = transform.localScale;
    currentScale = startScale;
    changeSizeActive = true;
    targetScale = new Vector3(1f, 1f, 1f);
    targetAlpha = 1f;
    currentAlpha = 0;
    imageRenderer.SetAlpha(0f);
  }

  public void FadeOut() {
    time = 0;
    duration = .3f;
    fadeOut = true;
    targetAlpha = 0f;
    imageRenderer.SetAlpha(1f);
    startAlpha = imageRenderer.GetAlpha();
    currentAlphaFadeOut = startAlpha;
  }
}

