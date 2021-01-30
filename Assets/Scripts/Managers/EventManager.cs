using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : Singleton<EventManager>
{

    
    //float contains time remained in seconds
    public class FloatEvent : UnityEvent<float>
    {
        
    }


    public FloatEvent OnCountdownEnd;
    public FloatEvent OnSpeedChange;


    protected new void Awake()
    {
        base.Awake();
        OnCountdownEnd = new FloatEvent();
        OnSpeedChange = new FloatEvent();
    }
    

}
