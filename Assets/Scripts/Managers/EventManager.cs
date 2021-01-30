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
    public class CountdownEndEvent : UnityEvent<float>
    {
        
    }


    public CountdownEndEvent OnCountdownEnd;
    public OnGrabEvent OnGrab;
    public OnReleaseEvent OnRelease;


    protected new void Awake()
    {
        base.Awake();
        OnCountdownEnd = new CountdownEndEvent();
        OnGrab = new OnGrabEvent();
        OnRelease = new OnReleaseEvent();
    }
    

}
