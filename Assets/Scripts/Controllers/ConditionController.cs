using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConditionController : MonoBehaviour {
    public Slider stimuliLifetimeSlider;
    public Slider timeBetweenStimuliSlider;
    public Slider traceConditionSlider;
    public Slider responseTimeSlider;
    public Slider gridSizeSlider;

    public TextMeshProUGUI stimuliLifetimeValueText;
    public TextMeshProUGUI timeBetweenStimuliValueText;
    public TextMeshProUGUI traceConditionValueText;
    public TextMeshProUGUI responseTimeValueText;
    public TextMeshProUGUI gridSizeValueText;


    public float stimuliLifetime = 15;
    public float timeBetweenStimuli = 0;
    public float traceCondition = 2;
    public float responseTime = 5;
    public float gridSize = 3;

    GridCreator gridCreator;

    void Awake() {
        gridCreator = FindObjectOfType<GridCreator>();
    }

    void Start() {
        stimuliLifetimeSlider.onValueChanged.AddListener(delegate {StimuliLifetimeSliderChange();});
        timeBetweenStimuliSlider.onValueChanged.AddListener(delegate {TimeBetweenStimuliSliderChange();});
        traceConditionSlider.onValueChanged.AddListener(delegate {TraceConditionSliderChange();});
        responseTimeSlider.onValueChanged.AddListener(delegate {ResponseTimeSliderChange();});
        gridSizeSlider.onValueChanged.AddListener(delegate {GridSizeSliderChange();});
    }

    void StimuliLifetimeSliderChange() {
		stimuliLifetime = stimuliLifetimeSlider.value*50;
        stimuliLifetimeValueText.text = stimuliLifetime.ToString();
	}

    void TimeBetweenStimuliSliderChange() {
		timeBetweenStimuli = timeBetweenStimuliSlider.value*50;
        timeBetweenStimuliValueText.text = timeBetweenStimuli.ToString();
	}

    void TraceConditionSliderChange() {
		traceCondition = traceConditionSlider.value*1000;
        traceConditionValueText.text = traceCondition.ToString();
	}

    void ResponseTimeSliderChange() {
		responseTime = responseTimeSlider.value*500;
        responseTimeValueText.text = responseTime.ToString();
	}

    void GridSizeSliderChange() {
		gridSize = gridSizeSlider.value;
        gridSizeValueText.text = gridSize.ToString();
        gridCreator.CreateGrid((int)gridSize);
	}
}
