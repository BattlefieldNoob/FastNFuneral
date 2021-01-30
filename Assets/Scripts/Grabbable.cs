using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{

    private Outline _outline;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _outline = GetComponentInChildren<Outline>();
        _outline.enabled = false;
    }

    public void Grabbed()
    {
        _rigidbody.isKinematic = true;
        _outline.enabled = false;
    }

    public void Released()
    {
        _rigidbody.isKinematic = false;
    }

    public void ApplyForce(Vector3 direction)
    {
        _rigidbody.AddForce(direction*20,ForceMode.Impulse);
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Hand"))
            return;

        _outline.enabled = true;
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag("Hand"))
            return;

        _outline.enabled = false;
    }
}
