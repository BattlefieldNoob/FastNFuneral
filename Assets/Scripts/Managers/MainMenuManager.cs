using System;
using DG.Tweening;
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

        [SerializeField] private float fadeTime = .5f;

        private void Start()
        {
            
            FadeCanvasGroup(menuCanvasGroup,true);
            FadeCanvasGroup(howToCanvasGroup,false);
            newGameButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(sceneToLoad);
            });
            
            howToButton.onClick.AddListener(() =>
            {
                FadeCanvasGroup(menuCanvasGroup,false);
                FadeCanvasGroup(howToCanvasGroup,true);
            });

            backToMenuButton.onClick.AddListener(() =>
            {
                FadeCanvasGroup(menuCanvasGroup,true);
                FadeCanvasGroup(howToCanvasGroup,false);

            });
            
            exitButton.onClick.AddListener(Application.Quit);
        }

        private void FadeCanvasGroup(CanvasGroup cg, bool fadeIn)
        {
            cg.interactable = fadeIn;
            cg.blocksRaycasts = fadeIn;
            cg.DOFade(fadeIn ? 1 : 0, fadeTime);
        }
    }
}
