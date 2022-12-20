using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public string gameState;
    public static event Action<string> OnGameStateChanged;
    public TMP_Text debriefingText;
    public Canvas briefingCanvas;
    public Canvas actionCanvas;
    public Canvas debriefingCanvas;
    public GameObject briefDecor;
    public GameObject timerText;
    private bool winState;

    // Start is called before the first frame update

    void Awake()
    {
        instance = this;
        ActionHandler.OnMissionEvaluationChanged += OnMissionEvaluationChanged;
        Timer.OntimerStateChanged += OnTimerCountdownOver;
    }

    void onDestroy()
    {
        ActionHandler.OnMissionEvaluationChanged -= OnMissionEvaluationChanged;
        Timer.OntimerStateChanged -= OnTimerCountdownOver;
    }

    private void OnTimerCountdownOver(string condition)
    {
        winState = false;
        UpdateGameState("debriefing");
    }

    private void OnMissionEvaluationChanged(string state)
    {
        if (state == "victory")
        {
            winState = true;
            UpdateGameState("debriefing");
        }
        else if (state == "failure")
        {
            winState = false;
            UpdateGameState("debriefing");
        }
    }

    public void UpdateGameState(string newState)
    {
        switch (newState)
        {
            case "briefing":
                HandleBriefing();
                break;
            case "action":
                HandleAction();
                break;
            case "debriefing":
                HandleDebriefing();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        OnGameStateChanged?.Invoke(newState);
    }

    void HandleBriefing()
    {
        briefingCanvas.gameObject.SetActive(true);
        briefDecor.SetActive(true);
        actionCanvas.gameObject.SetActive(false);
        debriefingCanvas.gameObject.SetActive(false);
        timerText.SetActive(true);
    }

    void HandleAction()
    {
        briefingCanvas.gameObject.SetActive(false);
        briefDecor.SetActive(false);
        actionCanvas.gameObject.SetActive(true);
        debriefingCanvas.gameObject.SetActive(false);
        timerText.SetActive(true);
    }

    void HandleDebriefing()
    {
        briefingCanvas.gameObject.SetActive(false);
        briefDecor.SetActive(false);
        actionCanvas.gameObject.SetActive(false);
        debriefingCanvas.gameObject.SetActive(true);
        timerText.SetActive(false);
        if (winState)
        {
            debriefingText.text = "Congratulations agent";
        }
        else
        {
            debriefingText.text = "You have failed";
        }
        

    }

    void Start()
    {
        UpdateGameState("briefing");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

