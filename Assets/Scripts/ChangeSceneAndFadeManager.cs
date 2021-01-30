using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneAndFadeManager : Singleton<ChangeSceneAndFadeManager>
{
    private bool inChangeScene;
    private CanvasGroup _canvasGroup;

    [SerializeField] private float duration=2;

    private void Start()
    {
        _canvasGroup = GetComponentInChildren<CanvasGroup>();

        SceneManager.activeSceneChanged += (arg0, scene) =>
        {
            if (!inChangeScene)
                return;

            _canvasGroup.DOFade(0, duration).OnComplete(() => inChangeScene = false);
        };
    }

    public void DoChangeScene(string to)
    {
        if(inChangeScene)
            return;
        
        Debug.Log($"[SceneAndFadeManager] Changing scene to {to}");
        inChangeScene = true;
        _canvasGroup.DOFade(1, duration).OnComplete(() => SceneManager.LoadScene(to));
    }

}
