using UnityEngine;

public class Linkable : MonoBehaviour
{
    [SerializeField] private LinkPoint primaryLinkPoint;
    [SerializeField] private LinkPoint[] secondaryLinkPoints;
    private Rigidbody _rigidbody;


    private LinkPoint primaryLinkedWith;

    public bool IsLinked => primaryLinkedWith!=null || primaryLinkPoint!=null;

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
        primaryLinkedWith.SetAsLinked();
        transform.SetParent(primaryLinkedWith.GetAttachJoint());
        transform.localRotation = primaryLinkPoint.GetAttachJoint().localRotation;
        transform.localPosition = Vector3.zero;
        primaryLinkPoint.Disable();
        primaryLinkedWith.Disable();
        _rigidbody.isKinematic = true;
        
    }

    public void Unlink()
    {
        if(primaryLinkedWith==null)
            return;
        
        primaryLinkPoint.Enable();
        primaryLinkedWith.Enable();
        primaryLinkedWith.SetAsReleased();
        primaryLinkedWith = null;
        _rigidbody.isKinematic = false;
        transform.SetParent(null);
    }
    
}
