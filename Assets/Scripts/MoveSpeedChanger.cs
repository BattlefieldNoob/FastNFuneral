using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpeedChanger : MonoBehaviour
{
    [SerializeField] private float targetSpeed = 5;

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player")) return;
        EventManager.Instance.OnSpeedChange.Invoke(targetSpeed);
    }
}
