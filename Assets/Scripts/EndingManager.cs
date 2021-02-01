using System;
using System.IO;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_WEBGL
using System.Runtime.InteropServices;
#endif

public class EndingManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup screenshotButtonPanel;
    [SerializeField] private Button screenshotButton;
    [SerializeField] private Button backToMenuButton;

#if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void DownloadFile(byte[] array, int byteLength, string fileName);
#endif

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
        
#if UNITY_WEBGL
        var bytes = ScreenCapture.CaptureScreenshotAsTexture().EncodeToJPG();
        var customFileName = $"Fast_&_Funeral_{DateTime.Now.ToLongTimeString().Replace(":", "_")}.jpg";
        DownloadFile(bytes, bytes.Length, customFileName);
#else
        var folderPath = Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        var completePath = Path.Combine(folderPath,
            $"Fast_&_Funeral_{DateTime.Now.ToLongTimeString().Replace(":", "_")}.png");
        Debug.Log(completePath);
        ScreenCapture.CaptureScreenshot(completePath);
#endif
        screenshotButtonPanel.alpha = 1;
        backToMenuButton.enabled = true;
    }

    public void GoToMenu()
    {
        ChangeSceneAndFadeManager.Instance.DoChangeScene("MainMenu");
    }
}