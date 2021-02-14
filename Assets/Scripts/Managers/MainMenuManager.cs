using System;
using DG.Tweening;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private string sceneToLoad;

        [SerializeField] private Button newGameButton;
        [SerializeField] private Button howToButton;
        [SerializeField] private Button backToMenuButton;
        [SerializeField] private Button exitButton;

        [SerializeField] private CanvasGroup menuCanvasGroup;
        [SerializeField] private CanvasGroup howToCanvasGroup;

        [SerializeField] private Image VolumeOnIcon;
        [SerializeField] private Image VolumeMuteIcon;
        [SerializeField] private StudioEventEmitter MainMenuSoundtrack;


        [SerializeField] private float fadeTime = .5f;


        private void Awake()
        {
#if UNITY_WEBGL
            exitButton.gameObject.SetActive(false);
            ToggleAudio(); //set audio as "Suspended"
            MainMenuSoundtrack.PlayEvent = EmitterGameEvent.None;
#endif
        }


        private void Start()
        {
            FadeCanvasGroup(menuCanvasGroup, true, 0);
            FadeCanvasGroup(howToCanvasGroup, false, 0);
            newGameButton.onClick.AddListener(() => { ChangeSceneAndFadeManager.Instance.DoChangeScene(sceneToLoad); });

            howToButton.onClick.AddListener(() =>
            {
                FadeCanvasGroup(menuCanvasGroup, false, fadeTime);
                FadeCanvasGroup(howToCanvasGroup, true, fadeTime);
            });

            backToMenuButton.onClick.AddListener(() =>
            {
                FadeCanvasGroup(menuCanvasGroup, true, fadeTime);
                FadeCanvasGroup(howToCanvasGroup, false, fadeTime);
            });

            exitButton.onClick.AddListener(Application.Quit);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            VolumeMuteIcon.enabled = !_fmodEnabled;
            VolumeOnIcon.enabled = _fmodEnabled;
            
        }

        bool _fmodEnabled = true;

        public void ToggleAudio()
        {
            if (_fmodEnabled)
            {
                Debug.Log("Suspend");
                RuntimeManager.PauseAllEvents(true);
                var result = RuntimeManager.CoreSystem.mixerSuspend();
                Debug.Log(result);
            }
            else
            {
                Debug.Log("Resume");
                RuntimeManager.PauseAllEvents(false);
                var result = RuntimeManager.CoreSystem.mixerResume();
                Debug.Log(result);
#if UNITY_WEBGL
                if (!MainMenuSoundtrack.IsPlaying())
                {
                    MainMenuSoundtrack.Play();
                }
#endif
            }

            _fmodEnabled = !_fmodEnabled;

            VolumeMuteIcon.enabled = !_fmodEnabled;
            VolumeOnIcon.enabled = _fmodEnabled;
        }

        private void FadeCanvasGroup(CanvasGroup cg, bool fadeIn, float time)
        {
            cg.interactable = fadeIn;
            cg.blocksRaycasts = fadeIn;
            cg.DOFade(fadeIn ? 1 : 0, time);
        }
    }
}