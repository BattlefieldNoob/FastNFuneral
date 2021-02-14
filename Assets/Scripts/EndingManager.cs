using System;
using System.Collections;
using System.IO;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_WEBGL
using System.Runtime.InteropServices;
using TMPro;

#endif

public class EndingManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup screenshotButtonPanel;
    [SerializeField] private Button screenshotButton;
    [SerializeField] private CanvasGroup screenshotBlinkPanel;

#if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void DownloadFile(byte[] array, int byteLength, string fileName);
#endif


    private void Start()
    {
#if UNITY_WEBGL
        screenshotButton.GetComponentInChildren<TextMeshProUGUI>().text = "Take Screenshot and Download";
#endif
    }

    public void ShowScreenShotButton()
    {
        screenshotButtonPanel.interactable = true;
        screenshotButtonPanel.blocksRaycasts = true;
        screenshotButtonPanel.DOFade(1, 0.25f);
        screenshotButton.onClick.AddListener(() => StartCoroutine(Screenshot()));
    }


    private IEnumerator Screenshot()
    {
        var uiLayer = LayerMask.NameToLayer("UI");
        Debug.Log(uiLayer);
        var mainCamera = Camera.main;
        if (mainCamera == null)
            yield break;
        mainCamera.cullingMask &= ~(1 << uiLayer);
        yield return new WaitForEndOfFrame();
#if UNITY_WEBGL && !UNITY_EDITOR
        var bytes = ScreenCapture.CaptureScreenshotAsTexture().EncodeToPNG();
        Debug.Log(bytes.Length);
        var customFileName = $"Fast&Funeral_{DateTime.Now.ToLongTimeString().Replace(":", "_")}.png";
        Debug.Log(customFileName);
        DownloadFile(bytes, bytes.Length, customFileName);
#else
        var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        Debug.Log(folderPath);
        var completePath = Path.Combine(folderPath,
            $"Fast&Funeral_{DateTime.Now.ToLongTimeString().Replace(":", "_")}.png");
        Debug.Log(completePath);
        ScreenCapture.CaptureScreenshot(completePath);
#endif
        mainCamera.cullingMask |= (1 << uiLayer);
        yield return screenshotBlinkPanel.DOFade(1, 0.2f).SetLoops(2, LoopType.Yoyo).WaitForCompletion();
        screenshotButtonPanel.interactable = false;
        screenshotButtonPanel.blocksRaycasts = false;
        screenshotButtonPanel.DOFade(0, 0.25f);
    }

    public void GoToMenu()
    {
        ChangeSceneAndFadeManager.Instance.DoChangeScene("MainMenu");
    }
}