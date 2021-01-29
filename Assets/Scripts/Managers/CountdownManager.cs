using Managers;
using UnityEngine;

public class CountdownManager : Singleton<CountdownManager>
{
    [SerializeField] private float countdownSeconds = 60;

    private bool running;
    private float actual;

    public void StartCountdown()
    {
        Debug.Log("[CountdownManager] Countdown start");
        actual = countdownSeconds;
        running = true;
    }

    public void StopCountdown()
    {
        Debug.Log("[CountdownManager] Countdown stop");
        running = false;
    }

    public void StopCountdownAndInvokeEvent()
    {
        Debug.Log("[CountdownManager] Countdown stop with event");
        running = false;
        EventManager.Instance.OnCountdownEnd.Invoke(actual);
    }

    private void Update()
    {
        if(!running)
            return;

        actual -= Time.deltaTime;

        if (actual <= 0)
        {
            StopCountdownAndInvokeEvent();
        }

    }
}
