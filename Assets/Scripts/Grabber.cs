using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Grabber : MonoBehaviour
{

    [SerializeField] private LinkPointCollisionHandler _linkPointCollisionHandler;
    
    private Grabbable GrabCandidate;

    private Grabbable Grabbed;

    private LinkPoint LinkCandidate=>_linkPointCollisionHandler.LinkCandidate;

    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _linkPointCollisionHandler.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grabbable") && Grabbed==null)
        {
            //Debug.Log("Collided with grabbable!");
            GrabCandidate = other.attachedRigidbody.GetComponent<Grabbable>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Grabbable"))
            return;

        //Debug.Log("Removing!");
        GrabCandidate = null;
    }


    private void Grab()
    {
        var linkable = GrabCandidate.GetComponent<Linkable>();
        if (linkable != null)
        {
            linkable.Unlink();
        }
        Grabbed = GrabCandidate;
        GrabCandidate = null;
        Grabbed.Grabbed();
        Grabbed.transform.SetParent(transform);
        Grabbed.transform.localPosition = Vector3.zero;
        EventManager.Instance.OnGrab.Invoke();
        _collider.enabled = false;
        _linkPointCollisionHandler.Enable();
    }

    private void ReleaseAndLink()
    {
        var linkable=Grabbed.GetComponent<Linkable>();
        if (linkable == null)
        {
            Debug.Log("Cannot find linkable!");
            ReleaseAndThrow();
            return;
        }
        
        Grabbed.transform.SetParent(null);
        Grabbed.Released();
        Grabbed = null;
        EventManager.Instance.OnRelease.Invoke();
        _collider.enabled = true;
        
        linkable.LinkTo(LinkCandidate);
        _linkPointCollisionHandler.Disable();
    }

    private void ReleaseAndThrow()
    {
        Grabbed.transform.SetParent(null);
        Grabbed.Released();
        Grabbed.ApplyForce(transform.forward);
        Grabbed = null;
        _collider.enabled = true;
        EventManager.Instance.OnRelease.Invoke();
        _collider.enabled = true;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if (GrabCandidate != null && Grabbed is null)
        {
            Grab();
        }
        else if (Grabbed != null && LinkCandidate != null)
        {
            Debug.Log("Release And Link!");
            ReleaseAndLink();
        }
        else if (Grabbed != null)
        {
            Debug.Log("Release And Throw!");
            ReleaseAndThrow();
        }
    }
}