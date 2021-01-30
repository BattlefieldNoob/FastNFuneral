using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class LinkPoint : MonoBehaviour
{
    private Collider _collider;
    private CanvasGroup _canvasGroup;
    [SerializeField] private Image highlight;
    [SerializeField] private GameObject Joint;

    private Linkable _linkable;

    private bool _isLinked;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _canvasGroup = GetComponentInChildren<CanvasGroup>();
        _linkable = GetComponentInParent<Linkable>();
        EventManager.Instance.OnGrab.AddListener(ShowAttachPoint);
        EventManager.Instance.OnRelease.AddListener(HideAttachPoint);
        HideAttachPoint();
    }

    private void HideAttachPoint()
    {
        _canvasGroup.alpha = 0;
    }

    private void ShowAttachPoint()
    {
        if(!_linkable.IsLinked && !_isLinked)
            _canvasGroup.alpha = 1;
    }
    
    public void SetAsLinked()
    {
        _isLinked = true;
    }
    
    public void SetAsReleased()
    {
        _isLinked = false;
    }

    public void Disable()
    {
        _collider.enabled = false;
    }
    
    public void Enable()
    {
        _collider.enabled = true;
    }

    public Transform GetAttachJoint() => Joint.transform;

    public void DoNotHighLight()
    {
        highlight.color = Color.white;
    }
    
    public void HighLight()
    {
        highlight.color = Color.red;
    }
}
