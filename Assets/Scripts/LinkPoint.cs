using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class LinkPoint : MonoBehaviour
{
    private Collider _collider;
    private CanvasGroup _canvasGroup;
    [SerializeField] private Image highlight;

    private Linkable _linkable;

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
        if(_linkable.IsLinked)
            _canvasGroup.alpha = 1;
    }

    public void Disable()
    {
        _collider.enabled = false;
    }
    
    public void Enable()
    {
        _collider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("GrabbingHand"))
            return;
        
        Debug.Log("Show can grab");
        highlight.color=Color.red;
    }
    private void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag("GrabbingHand"))
            return;
        
        Debug.Log("Hide can grab");
        highlight.color=Color.white;
    }
}
