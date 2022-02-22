using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinningCellHighlightPulse : MonoBehaviour
{   
    public Material winningCellHighlight;

    Color gold = new Color(255, 194, 0, 0);
    Color diamond = new Color(100, 137, 191, 0);

    Color lerpColor;

    void Start() {
        winningCellHighlight.EnableKeyword("_EMISSION");
    }

    void Update() {
        lerpColor = Color.Lerp(diamond, gold, Mathf.PingPong(Time.time, .75f));
        winningCellHighlight.SetColor("_EmissionColor", lerpColor/80);
    }
}
