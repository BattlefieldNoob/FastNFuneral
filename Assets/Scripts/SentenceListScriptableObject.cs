using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SentenceList", menuName = "ScriptableObjects/SentenceListScriptableObject", order = 2)]
public class SentenceListScriptableObject : ScriptableObject
{
    [SerializeField]
    [Tooltip("Valid sentence must contain \"{0}\" that will be replaced with some description and optionally \"{1}\" for adjectives")]
    private string[] sentences;

    public string[] ValidList()
    {
        return sentences.Where(sentence => sentence.Contains("{0}")).ToArray();
    }
}
