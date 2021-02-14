using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FMODUnity;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class ChangeSceneAndFadeManager : Singleton<ChangeSceneAndFadeManager>
{
    private bool inChangeScene;
    private CanvasGroup _canvasGroup;

    [SerializeField] private float duration = 2;

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
        if (inChangeScene)
            return;

        Debug.Log($"[SceneAndFadeManager] Changing scene to {to}");
        inChangeScene = true;

        var emitters = FindObjectsOfType<StudioEventEmitter>();
        
        foreach (var emitter in emitters)
        {
            emitter.Stop();
        }

        var audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            audioManager._ambienceInstance.stop(STOP_MODE.ALLOWFADEOUT);
            audioManager._musicInstance.stop(STOP_MODE.ALLOWFADEOUT);
            audioManager._snapshotInstance.stop(STOP_MODE.ALLOWFADEOUT);
        }
        

        _canvasGroup.DOFade(1, duration).OnComplete(() => SceneManager.LoadScene(to));
    }
}