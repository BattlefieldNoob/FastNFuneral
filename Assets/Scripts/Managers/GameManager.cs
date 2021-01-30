using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    [Range(0,10)]
    private int trueSentenceEvery = 4;
    [SerializeField]
    private SentenceListScriptableObject sentenceList;
    [SerializeField] private List<LimbScriptableObject> branchLimbs;
    [SerializeField] private List<LimbScriptableObject> leafLimbs;
    [SerializeField] private int maxDepth = 2;
    [SerializeField] private int maxLimbs = 10;
    private string targetCorpse;
    private List<LimbScriptableObject> targetLimbs;
    private int currentLimbIndex = 0;
    private int lieCounter = 0;

    public string TargetCorpse => targetCorpse;

    private void Start()
    {
        maxLimbs = Mathf.Clamp(maxLimbs, 1, branchLimbs.Count + leafLimbs.Count - 1);
        GenerateTargetCorpse();
        ShuffleLimbs();
        CountdownManager.Instance.StartCountdown();
        EventManager.Instance.OnCountdownEnd.AddListener(remaining =>
        {
            // Do Final Cutscene
            Debug.Log("[GameManager] Restart Game");
        });
    }

    public void GenerateTargetCorpse()
    {
        var branches = branchLimbs.OrderBy(a => new Guid()).ToList();
        var leaves = leafLimbs.OrderBy(a => new Guid()).ToList();
        foreach (var linkable in CorpseEditorManager.Instance.GetBust().Linkables)
        {
            //
        }
    }

    public string RandomSentence()
    {
        string randomSentence = sentenceList.ValidList()[Random.Range(0,sentenceList.ValidList().Length -1)];
        
        var limb = targetLimbs[currentLimbIndex];
        currentLimbIndex++;
        if (currentLimbIndex > targetLimbs.Count - 1)
        {
            ShuffleLimbs();
        }
        
        bool lie = lieCounter < trueSentenceEvery;
        lieCounter = lie ? lieCounter + 1 : 0;
        
        return string.Format(
            randomSentence,
            $"<color=\"red\"><b>{limb.RandomDescription(lie)}</b></color>",
            $"<color=\"yellow\"><b>{limb.RandomAdjective(lie)}</b></color>"
        );
    }

    private void ShuffleLimbs()
    {
        currentLimbIndex = 0;
        targetLimbs = targetLimbs.OrderBy(a => new Guid()).ToList();
    }
}
