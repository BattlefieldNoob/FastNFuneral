using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Limb", menuName = "ScriptableObjects/LimbScriptableObject", order = 1)]
public class LimbScriptableObject : ScriptableObject
{
    /*enum LimbType
    {
        Head,
        RightLeg,
        LeftLeg,
        RightArm,
        LeftArm
    }
    [SerializeField]
    private LimbType limbType;*/
    [SerializeField] private string adjective;
    [SerializeField] private string objectName;
    [SerializeField] private string[] hazyDescriptions;
    [SerializeField] private string[] hazyAdjectives;
    [SerializeField] private int linkNumber;

    public string GetName()
    {
        return objectName;
    }

    public int GetLinkNumber()
    {
        return linkNumber;
    }

    public string RandomDescription(bool hazy = true)
    {
        if (hazy && hazyDescriptions.Length > 0)
        {
            return hazyDescriptions[Random.Range(0, hazyDescriptions.Length - 1)];
        }

        return objectName;
    }

    public string RandomAdjective(bool hazy = true)
    {
        if (hazy && hazyAdjectives.Length > 0)
        {
            return hazyAdjectives[Random.Range(0, hazyAdjectives.Length - 1)];
        }

        return adjective;
    }
}