using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CorpseEditorManager : MonoBehaviour
{
    [SerializeField] private Linkable bust;
    [SerializeField] private Transform endingTargetPosition;

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.U))
        // {
        //     Debug.Log(MatchCorpString());
        // }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneManagerOnsceneLoaded;
    }

    private void OnSceneManagerOnsceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainMenu":
                Destroy(gameObject);
                break;
            case "Ending":
            {
                var bodyTransform = bust.gameObject.transform;
                bodyTransform.position = endingTargetPosition.position;
                bodyTransform.rotation = endingTargetPosition.rotation;
                break;
            }
            default:
                return;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneManagerOnsceneLoaded;
    }

    public Linkable GetBust()
    {
        return bust;
    }

    public string MatchCorpString()
    {
        var matchString = "";
        foreach (var limb in bust.Linkables)
        {
            matchString = matchString + limb.PrintMatchTree() + ',';
        }

        // 0: <_Afro Hair>,0: <_Pigeon Head>[],0: <_Beard>,
        return matchString;
    }
}