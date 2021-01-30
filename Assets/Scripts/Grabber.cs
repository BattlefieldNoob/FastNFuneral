using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Grabber : MonoBehaviour
{
    private Grabbable GrabCandidate;

    private Grabbable Grabbed;

    private LinkPoint LinkCandidate;

    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grabbable") && Grabbed==null)
        {
            //Debug.Log("Collided with grabbable!");
            GrabCandidate = other.attachedRigidbody.GetComponent<Grabbable>();
        }
        else if (other.CompareTag("LinkPoint"))
        {
            Debug.Log("Collided with linkpoint!");
            LinkCandidate = other.GetComponent<LinkPoint>();
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
        Grabbed = GrabCandidate;
        GrabCandidate = null;
        Grabbed.Grabbed();
        Grabbed.transform.SetParent(transform);
        Grabbed.transform.localPosition = Vector3.zero;
        EventManager.Instance.OnGrab.Invoke();
        _collider.tag = "GrabbingHand";
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
        _collider.tag = "Hand";
        
        linkable.LinkTo(LinkCandidate);
        LinkCandidate = null;
    }

    private void ReleaseAndThrow()
    {
        Grabbed.transform.SetParent(null);
        Grabbed.Released();
        Grabbed.ApplyForce(transform.forward);
        Grabbed = null;
        _collider.enabled = true;
        EventManager.Instance.OnRelease.Invoke();
        _collider.tag = "Hand";
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