using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundFxController : MonoBehaviour {
  public GameController gameController;

  public AudioClip negative;

  public AudioSource mainAudioSource;
  public AudioClip playerWin;
  public AudioClip aiWin;

  public AudioSource audioSourceClock;
  public AudioClip clock;

  public AudioSource outcomeAudioSource;

  public AudioClip stimuliSound;
  public AudioClip[] outcomeSounds;

  public AudioClip AICorrect;

  public AudioClip blueTimeWin;
  public AudioClip blueTimeFail;

  public void PlayClock() {
    audioSourceClock.PlayOneShot(clock);
  }

  public void StopClock() {
    audioSourceClock.Stop();
  }

  public void PlayWrongResponseSound() {
    outcomeAudioSource.PlayOneShot(negative);
  }

  public void PlayGameOverSound(Player player) {
    AudioClip gameOverSound = (player == gameController.playerX) ? playerWin : aiWin;
    mainAudioSource.PlayOneShot(gameOverSound);
  }

  public void PlayAICorrectSound() {
    outcomeAudioSource.PlayOneShot(AICorrect);
  }

  public void PlayStimuliSound() {
    mainAudioSource.PlayOneShot(stimuliSound);
  }

  public void PlayBlueTimeWin() {
    mainAudioSource.PlayOneShot(blueTimeWin);
  }

  public void PlayBlueTimeFail() {
    mainAudioSource.PlayOneShot(blueTimeFail);
  }
}
