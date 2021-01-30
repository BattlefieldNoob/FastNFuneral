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

    private LinkPoint LinkCandidate => _linkPointCollisionHandler.LinkCandidate;

    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _linkPointCollisionHandler.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Grabbable") ||
             (other.attachedRigidbody != null && other.attachedRigidbody.CompareTag("Grabbable"))) && Grabbed == null)
        {
            var newGrabbable=other.attachedRigidbody.GetComponent<Grabbable>();
            
            if(newGrabbable==null)
                return;

            if (GrabCandidate != null && GrabCandidate != newGrabbable)
            {
                GrabCandidate.DoNotHighLight();
                GrabCandidate = null;
            }

            GrabCandidate = newGrabbable;
            GrabCandidate.HighLight();
            //Debug.Log("Collided with grabbable!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grabbable"))
        {
            if(GrabCandidate==null)
                return;
            //Debug.Log("Removing linkpoint!");
            GrabCandidate.DoNotHighLight();
            GrabCandidate = null;
        }
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
        Debug.Log("ReleaseAndLink");
        var linkable = Grabbed.GetComponent<Linkable>();
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
        var chargeAmount = Time.time - throwChargeStart;

        chargeAmount = Mathf.Clamp(chargeAmount,0, 2);
        
        Grabbed.transform.SetParent(null);
        Grabbed.Released();
        _collider.enabled = true;
        EventManager.Instance.OnRelease.Invoke();
        _collider.enabled = true;

        //Debug.Log("Throw distance:"+chargeAmount);
        if (chargeAmount > 0.5f)
        {
            //Throw!!!!
            Grabbed.ApplyForce(transform.forward*chargeAmount);
        }
        
        Grabbed = null;
        throwChargeStart = 0;
    }

    private void Update()
    {

        if (inThrowCharge && Input.GetMouseButtonUp(0))
        {
            ReleaseAndThrow();
            inThrowCharge = false;
            return;
        }

        if (!Input.GetMouseButtonDown(0))
            return;

        if (GrabCandidate != null && Grabbed is null)
        {
            Grab();
        }
        else if (Grabbed != null && LinkCandidate != null && LinkCandidate.GetComponentInParent<Grabber>()==null)
        {
            //Debug.Log("Release And Link!");
            ReleaseAndLink();
        }
        else if (Grabbed != null)
        {
            //Debug.Log("Release And Throw!");
            StartThrowCharge();
        }
    }

    private float throwChargeStart = 0;
    private bool inThrowCharge;

    private void StartThrowCharge()
    {
        throwChargeStart = Time.time;
        inThrowCharge = true;
    }
}