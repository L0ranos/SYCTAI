using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    public float missionTime;
    public float timeRemaining;
    public TMP_Text timerText;
    bool canBeTriggered = true;
    public static event Action<string> OntimerStateChanged;

    void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    void onDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(string state)
    {
        if (state == "briefing")
        {
            timeRemaining = missionTime;
            canBeTriggered = true;
        }
    }

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timerText.text = timeRemaining.ToString("00");
        }
        else if(canBeTriggered)
        {
            OntimerStateChanged?.Invoke("fail");
        }
    }
}