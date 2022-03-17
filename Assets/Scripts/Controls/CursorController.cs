using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour {
  private CursorControls controls;
  private Camera mainCamera;

  void Awake() {
    controls = new CursorControls();
    mainCamera = Camera.main;
  }

  private void Start() {
    controls.Mouse.Click.performed += _ => EndedClick();
  }

  private void EndedClick() {
    Ray ray = mainCamera.ScreenPointToRay(controls.Mouse.Position.ReadValue<Vector2>());

    if (Physics.Raycast(ray, out RaycastHit hit)) {
      if (hit.collider != null) {
        // Debug.Log("hit");
        if(hit.collider.gameObject.CompareTag("Cell")){
          hit.collider.gameObject.GetComponentInParent<Cell>().OnClick();
        } 
        if(hit.collider.gameObject.CompareTag("Bar")){ 
          // Debug.Log("Bar");
          hit.collider.gameObject.GetComponentInParent<ScoreBar>().Clicked();
        }
      }
    }
  }

  private void OnEnable() {
    // Debug.Log("Enabled");
    controls.Enable();
  }

  private void OnDisable() {
    // Debug.Log("Disabled");
    controls.Disable();
  }
}
