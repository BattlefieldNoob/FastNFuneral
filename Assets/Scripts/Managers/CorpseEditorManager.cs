using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class CorpseEditorManager : Singleton<CorpseEditorManager>
{
    [SerializeField] private Linkable bust;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            var corpsTarget = GameManager.Instance.TargetCorpse; 
            Debug.Log(CorpsPartsArray());
            Debug.Log($"CorpsMatch {CorpsMatch(corpsTarget)},LibsMatchCounter {LibsMatchCounter(corpsTarget)}");
            // Start0: _Afro Hair0: _Beard0: _Conehead0: _Crab Claw0: _Pigeon Head[]
            Debug.Log(MatchCorpString());
        }
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
        // Start0: _Afro Hair0: _Beard0: _Conehead0: _Crab Claw0: _Pigeon Head[]
        return matchString;
    }
    
    public bool CorpsMatch(string corpsTargetString)
    {
        return corpsTargetString.Equals(MatchCorpString());
    }
    
    public float LibsMatchCounter(string corpsTargetString)
    {
        int counter = 0;
        var parts = CorpsPartsArray();
        foreach (var part in parts)
        {
            if (corpsTargetString.Contains(part)) counter++;
        }

        return counter / parts.Length;
    }
    
    public string[] CorpsPartsArray()
    {
        return MatchCorpString().Split();
    }

}
