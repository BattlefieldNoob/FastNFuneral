using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCanvasManager : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private string endingScene;
    
    private void Start()
    {
        EventManager.Instance.OnCountdownEnd.AddListener((x)=>SceneManager.LoadScene(endingScene));
    }

    private void Update()
    {
        timerText.text = GetTimerString(CountdownManager.Instance.GetRemainingSeconds);
    }
    
    private string GetTimerString(float time)
    {
        var flooredTime = Mathf.FloorToInt(time);
        var minutes = flooredTime / 60;
        var seconds = flooredTime - (minutes * 60);
        return $"{minutes:0}:{seconds:00}";
    }
}
