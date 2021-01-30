using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingManager : MonoBehaviour
{


    public void ShowResult()
    {
        
    }


    public void GoToMenu()
    {
        ChangeSceneAndFadeManager.Instance.DoChangeScene("MainMenu");
    }
}
