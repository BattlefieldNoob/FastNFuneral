using System;
using DG.Tweening;
using UnityEngine;

public class InteractableShelf : MonoBehaviour
{

    [SerializeField] private float defaultXValue;
    [SerializeField] private float openedXValue;
    [SerializeField] private float openCloseDuration=2;

    private Outline _outline;

    private bool isOpened = false;

    private void Start()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
        transform.position = new Vector3(defaultXValue, transform.position.y, transform.position.z);
    }

    public void Toggle()
    {
        if (isOpened)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    public void Open()
    {
        if(DOTween.IsTweening(transform))
            return;

        transform.DOMoveX(openedXValue,openCloseDuration).OnComplete(() => isOpened = true);;
    }
    
    public void Close()
    {
        if(DOTween.IsTweening(transform))
            return;

        transform.DOMoveX(defaultXValue, openCloseDuration).OnComplete(() => isOpened = false);
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
