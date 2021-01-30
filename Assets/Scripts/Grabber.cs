using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Grabber : MonoBehaviour
{
    private Grabbable GrabCandidate;

    private Grabbable Grabbed;

    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Grabbable"))
            return;
        
        Debug.Log("Collided!");
        GrabCandidate = other.attachedRigidbody.GetComponent<Grabbable>();
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag("Grabbable"))
            return;
        
        Debug.Log("Removing!");
        GrabCandidate = null;
    }


    private void Grab()
    {
        Grabbed = GrabCandidate;
        GrabCandidate = null;
        _collider.enabled = false;
        Grabbed.Grabbed();
        Grabbed.transform.SetParent(transform);
        Grabbed.transform.localPosition=Vector3.zero;
    }

    private void ThrowGrabbed()
    {
        Grabbed.transform.SetParent(null);
        Grabbed.Released();
        Grabbed.ApplyForce(transform.forward);
        Grabbed = null;
        _collider.enabled = true;
    }
    
    private void Update()
    {
        if(!Input.GetMouseButtonDown(0))
            return;
        
        if (GrabCandidate != null && Grabbed is null)
        {
            Grab();
        }else if (Grabbed != null)
        {
            //check if is actually selected a link point, or throw away
            ThrowGrabbed();
        }
    }
}
