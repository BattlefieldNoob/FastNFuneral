using System;
using UnityEngine;

public class LinkPointCollisionHandler : MonoBehaviour
{
    public LinkPoint LinkCandidate;
    public Collider _Collider;

    private bool isEnabled = false;

    private void Start()
    {
        _Collider = GetComponent<Collider>();
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
