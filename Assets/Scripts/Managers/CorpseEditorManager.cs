using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;

public class CorpseEditorManager : Singleton<CorpseEditorManager>
{
    [SerializeField] private Linkable bust;

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.U))
        // {
        //     Debug.Log(MatchCorpString());
        // }
    }

    public Linkable GetBust()
    {
        return bust;
    }

    public string MatchCorpString()
    {

        var matchString = "";
        // bust.Linkables.Reverse();
        foreach (var limb in bust.Linkables)
        {
            matchString = matchString + limb.PrintMatchTree() + ',';
        }
        // 0: <_Afro Hair>,0: <_Pigeon Head>[],0: <_Beard>,
        return matchString;
    }
}
