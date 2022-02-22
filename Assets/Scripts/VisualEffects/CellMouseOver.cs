using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellMouseOver : MonoBehaviour {
    GameController gameController;

    public Cell parent;

    public GameObject hoverObject;
    public SpriteRenderer spriteRend;

    public Sprite cross;
    public Sprite nought;

    void Awake() {
        spriteRend = hoverObject.GetComponent<SpriteRenderer>();
        gameController = FindObjectOfType<GameController>();
    }

    void OnMouseOver() {
        if(parent.interactable) spriteRend.sprite = (gameController.activePlayer == gameController.playerX) ? cross : nought;
    }

    void OnMouseExit() {
        spriteRend.sprite = null;
    }
}
