using UnityEngine;

public class Linkable : MonoBehaviour
{
    [SerializeField] private LinkPoint primaryLinkPoint;
    [SerializeField] private LinkPoint[] secondaryLinkPoints;


    private LinkPoint primaryLinkedWith;
    
    //Link Primary to another gameObject
    public void LinkTo(LinkPoint linkToObject)
    {
        primaryLinkedWith = linkToObject;
        transform.position = linkToObject.transform.position - primaryLinkPoint.transform.position;
        primaryLinkPoint.Disable();
        primaryLinkedWith.Disable();
    }

    public void Unlink()
    {
        primaryLinkPoint.Enable();
        primaryLinkedWith.Enable();
        primaryLinkedWith = null;
    }
    
}
