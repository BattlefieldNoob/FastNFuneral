using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] [Range(0, 10)] private int trueSentenceEvery = 4;
    [SerializeField] private SentenceListScriptableObject sentenceList;
    [SerializeField] private List<LimbScriptableObject> branchLimbs;
    [SerializeField] private List<LimbScriptableObject> leafLimbs;
    [SerializeField] private int bustLinks = 5;
    [SerializeField] private int maxDepth = 2;
    [SerializeField] private int maxLimbs = 10;
    private readonly List<string> orderedPositioning = new List<string>{"on the head","on the right arm","on the left arm","on the left leg","on the right leg"};

    public List<string> OrderedPositioning => orderedPositioning;

    private string targetCorpse;
    private List<LimbScriptableObject> targetLimbs = new List<LimbScriptableObject>();
    private int currentLimbIndex = 0;
    private int lieCounter = 0;

    public string TargetCorpse => targetCorpse;

    private void Start()
    {
        maxLimbs = Mathf.Clamp(maxLimbs, 1, branchLimbs.Count + leafLimbs.Count - 1);
        GenerateTargetCorpse();
        Debug.Log("TARGET: "+targetCorpse);
        //ShuffleLimbs();
        CountdownManager.Instance.StartCountdown();
        EventManager.Instance.OnCountdownEnd.AddListener(remaining =>
        {
            // Do Final Cutscene
            Debug.Log("[GameManager] Restart Game");
        });
        
        branchLimbs = branchLimbs.OrderBy(a => new Guid()).ToList();
        leafLimbs = leafLimbs.OrderBy(a => new Guid()).ToList();
    }

    public void GenerateTargetCorpse()
    {
        var start = "";
        var currentLimbs = 0;
        for (var index = 0; index < bustLinks; index++)
        {
            var limbsGenerated = GenerateRecursive(currentLimbs, index).Item1;
            start += limbsGenerated + (limbsGenerated != "" ? "," : "");
        }

        targetCorpse = start;
        ShuffleLimbs();
    }

    private Tuple<string,int> GenerateRecursive(int currentLimbs, int limbIndex, int layer = 0)
    {
        if (layer>= maxDepth) return new Tuple<string, int>("",currentLimbs);
        currentLimbs++;
        if (Random.Range(0, 2) == 0)
        {
            if (branchLimbs.Count <= 0)
            {
                return new Tuple<string, int>("",currentLimbs);
            }
            var branch = branchLimbs.FirstOrDefault();
            branch.SetPositioning(limbIndex);
            targetLimbs.Add(branch);
            branchLimbs.Remove(branch);
            var matchTree = $"{layer}: <_{branch.GetName()}<";
            matchTree = matchTree + "[";
            for (var i = 0; i < branch.GetLinkNumber(); i++)
            {
                if(currentLimbs>=maxLimbs || Random.Range(0, 2) == 0) continue;
                var ret = GenerateRecursive(currentLimbs, layer+1);
                currentLimbs = ret.Item2;
                matchTree = matchTree + ret.Item1 + ",";
            }
            matchTree = matchTree + "]";
            return new Tuple<string, int>(matchTree,currentLimbs);
        }
        else
        {
            if (leafLimbs.Count <= 0)
            {
                return new Tuple<string, int>("",currentLimbs);
            }
            var leaf = leafLimbs.FirstOrDefault();
            leaf.SetPositioning(limbIndex);
            targetLimbs.Add(leaf);
            leafLimbs.Remove(leaf);
            var matchTree = $"{layer}: <_{leaf.GetName()}<";
            return new Tuple<string, int>(matchTree,currentLimbs);
        }
    }

    public string RandomSentence()
    {
        string randomSentence = sentenceList.ValidList()[Random.Range(0, sentenceList.ValidList().Length - 1)];

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
            $"<color=\"yellow\"><b>{limb.GetPositioning()}</b></color>"
            /*,
            $"<color=\"yellow\"><b>{limb.RandomAdjective(lie)}</b></color>"*/
        );
    }

    private void ShuffleLimbs()
    {
        currentLimbIndex = 0;
        targetLimbs = targetLimbs.OrderBy(a => new Guid()).ToList();
    }
    
    public bool CorpsMatch()
    {
        return targetCorpse.Equals(CorpseEditorManager.Instance.MatchCorpString());
    }
    
    public float LibsMatchCounter()
    {
        int counter = 0;
        var parts = CorpsPartsArray();
        foreach (var part in parts)
        {
            if (targetCorpse.Contains(part))
            {
                counter++;
                Debug.Log("match"+part);
            };
        }

        return counter;
    }
    
    public string[] CorpsPartsArray()
    {
        return targetCorpse.Split('<').Where(s => s.Contains("_")).ToArray();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log(CorpsMatch());
            Debug.Log(LibsMatchCounter());
            Debug.Log("Current"+CorpseEditorManager.Instance.MatchCorpString());
            Debug.Log("Target** "+targetCorpse);
        }
    }
}