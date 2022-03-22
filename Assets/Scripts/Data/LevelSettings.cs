using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LevelSettings : ScriptableObject {

    public Timer[] timers;
    // public float aiCorrectResponseProbabilityPercent;

    public int stimuliLength;
    public int responseOptions;

}
