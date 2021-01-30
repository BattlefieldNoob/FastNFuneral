using System.Collections.Generic;
using UnityEngine;

public class Linkable : MonoBehaviour
{
    [SerializeField] private LinkPoint primaryLinkPoint;
    [SerializeField] private LimbScriptableObject limbInfo;
    private List<Linkable> _linkables = new List<Linkable>();
    private Rigidbody _rigidbody;

    public List<Linkable> Linkables => _linkables;

    private LinkPoint primaryLinkedWith;

    public bool IsLinked => primaryLinkedWith != null || primaryLinkPoint != null;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        var parent = GetComponentInParent<LinkPoint>();
        if (parent != null)
        {
            LinkTo(parent);
        }
    }

    public void AddLinked(Linkable linkedObject)
    {
        _linkables.Add(linkedObject);
    }

    public void RemoveLinked(Linkable linkedObject)
    {
        if (_linkables.Contains(linkedObject))
            _linkables.Remove(linkedObject);
    }


    //Link Primary to another gameObject
    public void LinkTo(LinkPoint linkToObject)
    {
        if (primaryLinkPoint == null)
            return;

        primaryLinkedWith = linkToObject;
        primaryLinkedWith.SetAsLinked();
        transform.SetParent(primaryLinkedWith.GetAttachJoint());
        transform.localRotation = primaryLinkPoint.GetAttachJoint().localRotation;
        transform.localPosition = Vector3.zero;
        primaryLinkPoint.Disable();
        primaryLinkedWith.Disable();
        _rigidbody.isKinematic = true;
        linkToObject.GetComponentInParent<Linkable>().AddLinked(this);
    }

    public void Unlink()
    {
        if (primaryLinkedWith == null)
            return;
        
        foreach (var linkable in _linkables)
        {
            linkable.Unlink();
        }

        _linkables.Clear();
        //primaryLinkedWith.GetComponentInParent<Linkable>().RemoveLinked(this);
        primaryLinkPoint.Enable();
        primaryLinkedWith.Enable();
        primaryLinkedWith.SetAsReleased();
        primaryLinkedWith = null;
        _rigidbody.isKinematic = false;
        transform.SetParent(null);
        
    }

    public string PrintMatchTree(int layer = 0)
    {
        var matchTree = $"{layer}: _" +gameObject.name;
        if (Linkables.Count < 1)
        {
            return matchTree;
        }
        matchTree = matchTree + "[";
        foreach (var linkable in Linkables)
        {
            matchTree += linkable.PrintMatchTree(layer+1)+",";
        }
        matchTree = matchTree + "]";
        return matchTree;
    }
}