using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Limb", menuName = "ScriptableObjects/LimbScriptableObject", order = 1)]
public class LimbScriptableObject : ScriptableObject
{
    //[SerializeField] private string adjective;
    [SerializeField] private string objectName;

    [SerializeField] private string[] hazyDescriptions;

    //[SerializeField] private string[] hazyAdjectives;
    [SerializeField] private int linkNumber;
    private string _positioning = "";

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

    public void SetPositioning(int index)
    {
        _positioning = GameManager.Instance.OrderedPositioning.Count < index ? "" : GameManager.Instance.OrderedPositioning[index];
    }

    public string GetPositioning()
    {
        return _positioning;
    }

    /*public string RandomAdjective(bool hazy = true)
    {
        if (hazy && hazyAdjectives.Length > 0)
        {
            return hazyAdjectives[Random.Range(0, hazyAdjectives.Length - 1)];
        }

        return adjective;
    }*/
}