using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{

    private Outline _outline;
    private Rigidbody _rigidbody;

    public bool grabbed = false;

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
        grabbed = true;
    }

    public void Released()
    {
        _rigidbody.isKinematic = false;
        grabbed = false;
    }

    public void ApplyForce(Vector3 direction)
    {
        _rigidbody.AddForce(direction*10,ForceMode.Impulse);
    }

    public void DoNotHighLight()
    {
        _outline.enabled = false;
    }

    public void HighLight()
    {
        _outline.enabled = true;
    }
}
