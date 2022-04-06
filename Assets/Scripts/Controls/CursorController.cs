using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class CursorController : MonoBehaviour {
  private CursorControls controls;
  private Camera mainCamera;

      
  [DllImport("user32.dll")]
  static extern bool SetCursorPos(int X, int Y);

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

  public void CenterAndLockCursor() {
    int xPos = Screen.width/2; 
    int yPos = Screen.height/2;   
    SetCursorPos(xPos,yPos);
    Cursor.lockState = CursorLockMode.Locked;
  }

  public void UnlockCursor() {
    Cursor.lockState = CursorLockMode.None;
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
