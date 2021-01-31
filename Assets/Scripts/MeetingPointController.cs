using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MeetingPointController : MonoBehaviour
{
    [SerializeField] private GameObject infoObject;
    [SerializeField] private float fadeInTime;
    [SerializeField] private float timeToLive;
    private TMP_Text _infoText;
    private CanvasGroup _infoCanvasGroup;
    private Transform _player;
    private bool isShowing;

    private void Start()
    {
        _infoText = infoObject.GetComponentInChildren<TMP_Text>();
        _infoCanvasGroup = infoObject.GetComponentInChildren<CanvasGroup>();
        _player = GameObject.FindWithTag("Player").transform;
        if (_infoText == null || _infoCanvasGroup == null)
        {
            Destroy(gameObject);
            return;
        }

        FadeCanvasGroup(_infoCanvasGroup, false, 0);
    }

    private void FadeCanvasGroup(CanvasGroup cg, bool fadeIn, float time)
    {
        cg.interactable = fadeIn;
        cg.blocksRaycasts = fadeIn;
        cg.DOFade(fadeIn ? 1 : 0, time).OnComplete(()=>isShowing = fadeIn);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || isShowing) return;
        isShowing = true;
        var phrase = GameManager.Instance.RandomSentence();
        _infoText.text = phrase;
        FadeCanvasGroup(_infoCanvasGroup, true, fadeInTime);
        StartCoroutine(WaitAndHide());
    }
    
    private IEnumerator WaitAndHide()
    {
        yield return new WaitForSeconds(timeToLive);
        FadeCanvasGroup(_infoCanvasGroup, false, fadeInTime * 2);
    }

    private void Update()
    {
        if (_infoCanvasGroup.alpha > 0)
        {
            var position = _player.position;
            position.y = infoObject.transform.position.y;
            infoObject.transform.LookAt(position);
        }
    }
}