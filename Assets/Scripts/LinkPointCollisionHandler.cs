using System;
using UnityEngine;

public class LinkPointCollisionHandler : MonoBehaviour
{
    public LinkPoint LinkCandidate;
    public Collider _Collider;

    private bool isEnabled = false;
    
    private Transform cameraTransform;


    private void Start()
    {
        _Collider = GetComponent<Collider>();
        cameraTransform = Camera.main.transform;
    }

    public void Enable()
    {
        isEnabled = true;
        _Collider.enabled = true;
    }
    
    public void Disable()
    {
        isEnabled = false;
        _Collider.enabled = false;
        LinkCandidate = null;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(!isEnabled)
            return;
        
        if (other.CompareTag("LinkPoint"))
        {
            //Debug.Log("Collided with linkpoint!");
            var newLink=other.GetComponent<LinkPoint>();
            
            
            if(newLink==null)
                return;

            if (LinkCandidate != null && LinkCandidate != newLink)
            {
                var cameraPosition = cameraTransform.position;
                var actualCandidateDistance = Vector3.Distance(cameraPosition, LinkCandidate.transform.position);

                var newCandidateDistance = Vector3.Distance(cameraPosition, newLink.transform.position);

                if (actualCandidateDistance < newCandidateDistance)
                    //dont change the actual candidate!
                    return;
                
                
                LinkCandidate.DoNotHighLight();
                LinkCandidate = null;
            }

            LinkCandidate = newLink;
            LinkCandidate.HighLight();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(!isEnabled)
            return;
        
        if (other.CompareTag("LinkPoint"))
        {
            if(LinkCandidate==null)
                return;
            //Debug.Log("Removing linkpoint!");
            LinkCandidate.DoNotHighLight();
            LinkCandidate = null;
        }
    }
}
