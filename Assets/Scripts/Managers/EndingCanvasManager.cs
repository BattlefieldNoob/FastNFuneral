using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    public class EndingCanvasManager : MonoBehaviour
    {
        [SerializeField] private Button backToMenu;
        [SerializeField] private string sceneToLoad;
        [SerializeField] private CanvasGroup scoreCanvas;
        [SerializeField] private TMP_Text scoreText;

        private void Start()
        {
            backToMenu.onClick.AddListener(() => SceneManager.LoadScene(sceneToLoad));
            scoreCanvas.alpha = 0;
        }

        public void ShowScoreCanvas()
        {
            //set score from gamemanager
            //play audio
            scoreCanvas.DOFade(1, .5f);
        }
    }
}