using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearCellValue : MonoBehaviour
{   
    GridController gridController;
    AttackOpponentController attackOpponentController;

    private void Awake() {
        gridController = FindObjectOfType<GridController>();   
        attackOpponentController = FindObjectOfType<AttackOpponentController>();
    }

    public void DisappearCell(Cell cellToCLear) {
        StartCoroutine(Disappear(cellToCLear));
    }

    IEnumerator Disappear(Cell cell) {
        cell.particleEffect.Play();
        Color cellValueColor = cell.valueDisplayer.color;
        Color targetColor =  new Color(cellValueColor.r, cellValueColor.g, cellValueColor.b, 0f);
        Color lerpedColor;
        yield return new WaitForSecondsRealtime(1f);

        SpriteRenderer renderer = cell.GetComponentInChildren<SpriteRenderer>();
        
        for (float t = 1; t > 0; t-=.01f) {
            lerpedColor = Color.Lerp(targetColor, cellValueColor, t);
            
            renderer.color = lerpedColor;
            yield return new WaitForSecondsRealtime(.02f);
        }
        yield return new WaitForSecondsRealtime(1f);
        attackOpponentController.EndReductionEvent();
        renderer.color = cellValueColor;
        cell.particleEffect.Stop();
        cell.NullValue();
    }
}
