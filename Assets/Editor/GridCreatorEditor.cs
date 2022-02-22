using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridCreator))]
public class GridCreatorEdtior : Editor {
  public override void OnInspectorGUI() {
    GridCreator gridCreator = (GridCreator)target;

    if (DrawDefaultInspector()) {
      if (gridCreator.autoUpdate) {
        gridCreator.InitializeGrid();
      }
    }

    if (GUILayout.Button("Create grid")) {
      gridCreator.InitializeGrid();
    }
  }
}
