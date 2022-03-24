using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConditionController : MonoBehaviour {
    public LevelSettings levelSettings;
    public LevelController levelController;
    public DotController dotController;

    public Slider stimuliLifetimeSlider;
    public Slider timeBetweenStimuliSlider;
    public Slider traceConditionSlider;
    public Slider responseTimeSlider;
    public Slider gridSizeSlider;
    public Slider nRainbowStimSlider;
    public Slider nResponsesSlider;
    public Slider nStimuliSlider;
    public Slider dotSlider;
    public Slider levelSlider;

    public TextMeshProUGUI stimuliLifetimeValueText;
    public TextMeshProUGUI timeBetweenStimuliValueText;
    public TextMeshProUGUI traceConditionValueText;
    public TextMeshProUGUI responseTimeValueText;
    public TextMeshProUGUI gridSizeValueText;
    public TextMeshProUGUI nRainbowStimTimeValueText;
    public TextMeshProUGUI nResponsesTimeValueText;
    public TextMeshProUGUI nStimuliTimeValueText;
    public TextMeshProUGUI dotValueText;
    public TextMeshProUGUI levelValueText;


    public float stimuliLifetime = 15;
    public float timeBetweenStimuli = 0;
    public float traceCondition = 2;
    public float responseTime = 5;
    public float gridSize = 1;
    public float nRainbowStim = 4;
    public float nResponses = 0;
    public float nStimuli = 4;
    public float dot = 1;
    public float level = 1;

    GridCreator gridCreator;
    // GameController gameController;

    void Awake() {
        gridCreator = FindObjectOfType<GridCreator>();
        levelSettings = FindObjectOfType<LevelSettings>();
        levelController = FindObjectOfType<LevelController>();
        dotController = FindObjectOfType<DotController>();
        // gameController = FindObjectOfType<GameController>();
        // Debug.Log(nStimuli);
    }

    public void LoadLevelSettings() {
        nResponses = levelSettings.responseOptions;
        nStimuli = levelSettings.stimuliLength;
    }

    public void ExportParameters() {
        Debug.Log(stimuliLifetime);
        Debug.Log(timeBetweenStimuli);
        Debug.Log(traceCondition);
        Debug.Log(responseTime);
        Debug.Log(gridSize);
        Debug.Log(nRainbowStim);
        Debug.Log(nResponses);
        Debug.Log(nStimuli);
    }

    void Start() {
        stimuliLifetimeSlider.onValueChanged.AddListener(delegate {StimuliLifetimeSliderChange();});
        timeBetweenStimuliSlider.onValueChanged.AddListener(delegate {TimeBetweenStimuliSliderChange();});
        traceConditionSlider.onValueChanged.AddListener(delegate {TraceConditionSliderChange();});
        responseTimeSlider.onValueChanged.AddListener(delegate {ResponseTimeSliderChange();});
        gridSizeSlider.onValueChanged.AddListener(delegate {GridSizeSliderChange();});
        nRainbowStimSlider.onValueChanged.AddListener(delegate {nRainbowStimSliderChange();});
        nResponsesSlider.onValueChanged.AddListener(delegate {nResponsesSliderChange();});
        nStimuliSlider.onValueChanged.AddListener(delegate {nStimuliSliderChange();});
        dotSlider.onValueChanged.AddListener(delegate {dotSliderChange();});
        levelSlider.onValueChanged.AddListener(delegate {levelSliderChange();});
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

    void nRainbowStimSliderChange() {
		nRainbowStim = nRainbowStimSlider.value;
        nRainbowStimTimeValueText.text = nRainbowStim.ToString();
	}

    void nResponsesSliderChange() {
		nResponses = nResponsesSlider.value;
        nResponsesTimeValueText.text = nResponses.ToString();
	}

    void nStimuliSliderChange() {
		nStimuli = nStimuliSlider.value;
        nStimuliTimeValueText.text = nStimuli.ToString();
	}

    void dotSliderChange() {
		dot = dotSlider.value;
        if(dot == 1f) {
            dotValueText.text = "ON";
            dotController.toggleDot = true;
        } else {
            dotValueText.text = "OFF";
            dotController.toggleDot = false;
        }
        
	}

    public void levelSliderChange() {
		level = levelSlider.value;
        levelController.SwitchLevel((int)level);
        levelValueText.text = level.ToString();
	}
}
