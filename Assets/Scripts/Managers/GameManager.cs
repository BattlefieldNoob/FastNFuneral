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
    // private readonly List<string> orderedPositioning = new List<string>{"on the head","on the right arm","on the left arm","on the left leg","on the right leg"};
    private readonly List<string> orderedPositioning = new List<string>{"on the right leg","on the left leg","on the left arm","on the right arm","on the head"};

    public List<string> OrderedPositioning => orderedPositioning;

    private string targetCorpse;
    private List<LimbScriptableObject> targetLimbs = new List<LimbScriptableObject>();
    private int currentLimbIndex = 0;
    private int lieCounter = 0;
    private CorpseEditorManager corpseManager;
    private float partialMatch = 0;

    public string TargetCorpse => targetCorpse;

    private void Start()
    {
        EventManager.Instance.OnCountdownEnd.AddListener(remaining =>
        {
            // Do Final Cutscene
            Debug.Log("[GameManager] Restart Game");
        });
        if (SceneManager.GetActiveScene().name == "Main")
        {
            SetupGame();
        }
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            if (scene.name != "Main")
            {
                return;
            }
            SetupGame();
        };
        
    }

    private void SetupGame()
    {
        corpseManager = FindObjectOfType<CorpseEditorManager>();
        maxLimbs = Mathf.Clamp(maxLimbs, 1, branchLimbs.Count + leafLimbs.Count - 1);
        GenerateTargetCorpse();
        Debug.Log("TARGET: " + targetCorpse);
        //ShuffleLimbs();
        CountdownManager.Instance.StartCountdown();
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
            start += limbsGenerated;
        }

        targetCorpse = start;
        ShuffleLimbs();
    }

    private Tuple<string, int> GenerateRecursive(int currentLimbs, int limbIndex, int layer = 0)
    {
        if (layer >= maxDepth) return new Tuple<string, int>("", currentLimbs);
        currentLimbs++;
        if (Random.Range(0, 2) == 0)
        {
            if (branchLimbs.Count <= 0)
            {
                return new Tuple<string, int>("", currentLimbs);
            }

            var branch = branchLimbs.FirstOrDefault();
            branch.SetPositioning(limbIndex);
            targetLimbs.Add(branch);
            branchLimbs.Remove(branch);
            var matchTree = $"{layer}: <_{branch.GetName()}<";
            for (var i = 0; i < branch.GetLinkNumber(); i++)
            {
                if(currentLimbs>=maxLimbs || Random.Range(0, 2) == 0) continue;
                var ret = GenerateRecursive(currentLimbs, limbIndex, layer+1);
                currentLimbs = ret.Item2;
                matchTree = matchTree + ret.Item1;
            }

            return new Tuple<string, int>(matchTree, currentLimbs);
        }
        else
        {
            if (leafLimbs.Count <= 0)
            {
                return new Tuple<string, int>("", currentLimbs);
            }

            var leaf = leafLimbs.FirstOrDefault();
            leaf.SetPositioning(limbIndex);
            targetLimbs.Add(leaf);
            leafLimbs.Remove(leaf);
            var matchTree = $"{layer}: <_{leaf.GetName()}<";
            return new Tuple<string, int>(matchTree, currentLimbs);
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
        );
    }

    private void ShuffleLimbs()
    {
        currentLimbIndex = 0;
        targetLimbs = targetLimbs.OrderBy(a => new Guid()).ToList();
    }
    
    public bool CorpsMatch(string currentTree)
    {
        return NormalizedString(targetCorpse).Equals(currentTree);
    }
    
    public string CorpsMatchSubTree(string currentCorpsTree)
    {
        float subtreeCounter = 0;
        var subtrees = NormalizedString(targetCorpse).Split('0').Where(s => s.Contains("_")).ToArray();
        foreach (var subtree in subtrees)
        {
            if (currentCorpsTree.Contains(subtree)){ subtreeCounter++; }
        }

        partialMatch += subtreeCounter / subtrees.Length;
        return $"{subtreeCounter} on {subtrees.Length}";
    }

    private string NormalizedString(string str)
    {
        return str.Replace(" ", "").ToLower();
    }
    
    public string LibsMatchCounter(string currentCorpsTree)
    {
        float counter = 0;
        var parts = CorpsPartsArray();
        foreach (var part in parts)
        {
            if (currentCorpsTree.Contains(part)) { counter++; };
        }

        partialMatch += counter / parts.Length;
        return $"{counter} on {parts.Length}";
    }

    public string[] CorpsPartsArray()
    {
        return NormalizedString(targetCorpse).Split('<').Where(s => s.Contains("_")).ToArray();
    }

    public string GetScore()
    {
        var result = "";
        string currentCorpsTree = NormalizedString(corpseManager.MatchCorpString());
        if (CorpsMatch(currentCorpsTree)) result += "<b>Well Done!</b><br>";
        result += "You got " + LibsMatchCounter(currentCorpsTree) + " limbs!<br>";
        result += "and " + CorpsMatchSubTree(currentCorpsTree) + " in the right position!<br>";
        EventManager.Instance.OnScoreCalculated.Invoke(partialMatch/2);
        return result;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.U))
        {
            string currentCorpsTree = NormalizedString(corpseManager.MatchCorpString());
            Debug.Log("Corps"+CorpsMatch(currentCorpsTree));
            Debug.Log("SingleLibs"+LibsMatchCounter(currentCorpsTree));
            Debug.Log("SubTree"+CorpsMatchSubTree(currentCorpsTree));
            Debug.Log("Current"+corpseManager.MatchCorpString());
            Debug.Log("Target** "+targetCorpse);
        }
#endif
    }
}