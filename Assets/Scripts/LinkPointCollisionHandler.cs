using UnityEngine;

public class LinkPointCollisionHandler : MonoBehaviour
{
    public LinkPoint LinkCandidate;

    private bool isEnabled = false;
    
    public void Enable()
    {
        isEnabled = true;
    }
    
    public void Disable()
    {
        isEnabled = false;
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
