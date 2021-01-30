using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : Singleton<EventManager>
{
    public class OnGrabEvent : UnityEvent
    {
        
    }
    
    public class OnReleaseEvent : UnityEvent
    {
        
    }
    
    //float contains time remained in seconds
    public class FloatEvent : UnityEvent<float>
    {
        
    }


    public FloatEvent OnCountdownEnd;
    public FloatEvent OnSpeedChange;
    public OnGrabEvent OnGrab;
    public OnReleaseEvent OnRelease;


    protected new void Awake()
    {
        base.Awake();
        OnCountdownEnd = new FloatEvent();
        OnSpeedChange = new FloatEvent();
        OnGrab = new OnGrabEvent();
        OnRelease = new OnReleaseEvent();
    }
    

}
