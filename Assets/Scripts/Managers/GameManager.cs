using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private void Start()
    {
        CountdownManager.Instance.StartCountdown();
        EventManager.Instance.OnCountdownEnd.AddListener(remaining =>
        {
            // Do Final Cutscene
            Debug.Log("[GameManager] Restart Game");
        });
    }
}
