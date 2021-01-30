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
    [SerializeField]
    private List<LimbScriptableObject> targetLimbs;
    private int currentLimbIndex = 0;
    private int lieCounter = 0;
    private void Start()
    {
        ShuffleLimbs();
        CountdownManager.Instance.StartCountdown();
        EventManager.Instance.OnCountdownEnd.AddListener(remaining =>
        {
            // Do Final Cutscene
            Debug.Log("[GameManager] Restart Game");
        });
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
            limb.RandomDescription(lie),
            limb.RandomAdjective(lie)
        );
    }

    private void ShuffleLimbs()
    {
        currentLimbIndex = 0;
        targetLimbs = targetLimbs.OrderBy(a => new Guid()).ToList();
    }
}
