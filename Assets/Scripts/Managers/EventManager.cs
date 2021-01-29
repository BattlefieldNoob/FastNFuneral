using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : Singleton<EventManager>
{

    
    //float contains time remained in seconds
    public class CountdownEndEvent : UnityEvent<float>
    {
        
    }


    public CountdownEndEvent OnCountdownEnd;


    protected new void Awake()
    {
        base.Awake();
        OnCountdownEnd = new CountdownEndEvent();
    }
    

}
