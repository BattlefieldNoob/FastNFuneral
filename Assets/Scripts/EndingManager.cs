using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup screenshotButtonPanel;
    [SerializeField] private Button screenshotButton;
    [SerializeField] private Button backToMenuButton;

    public void ShowScreenShotButton()
    {
        screenshotButtonPanel.interactable = true;
        screenshotButtonPanel.blocksRaycasts = true;
        screenshotButtonPanel.DOFade(1, 0.25f);
        screenshotButton.onClick.AddListener(Screenshot);
    }


    private void Screenshot()
    {
        screenshotButtonPanel.alpha = 0;
        backToMenuButton.enabled = false;
        screenshotButton.interactable = false;
        var folderPath = Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        var completePath = Path.Combine(folderPath, $"Fast_&_Funeral_{DateTime.Now.ToLongTimeString().Replace(":","_")}.png");
        Debug.Log(completePath);
        ScreenCapture.CaptureScreenshot(completePath);
        screenshotButtonPanel.alpha = 1;
        backToMenuButton.enabled = true;
    }

    public void GoToMenu()
    {
        ChangeSceneAndFadeManager.Instance.DoChangeScene("MainMenu");
    }
}
