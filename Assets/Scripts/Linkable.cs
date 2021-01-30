using UnityEngine;

public class Linkable : MonoBehaviour
{
    [SerializeField] private LinkPoint primaryLinkPoint;
    [SerializeField] private LinkPoint[] secondaryLinkPoints;
    private Rigidbody _rigidbody;


    private LinkPoint primaryLinkedWith;

    public bool IsLinked => primaryLinkedWith != null || primaryLinkPoint==null;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    //Link Primary to another gameObject
    public void LinkTo(LinkPoint linkToObject)
    {
        if(primaryLinkPoint==null)
            return;
        
        primaryLinkedWith = linkToObject;
        transform.SetParent(linkToObject.transform);
        transform.localRotation = primaryLinkPoint.transform.localRotation;
        transform.localPosition = transform.localRotation*(-primaryLinkPoint.transform.localPosition);
        primaryLinkPoint.Disable();
        primaryLinkedWith.Disable();
        _rigidbody.isKinematic = true;
        
    }

    public void Unlink()
    {
        if(primaryLinkPoint!=null)
            return;
        
        primaryLinkPoint.Enable();
        primaryLinkedWith.Enable();
        primaryLinkedWith = null;
        _rigidbody.isKinematic = false;
        transform.SetParent(null);
    }
    
}
