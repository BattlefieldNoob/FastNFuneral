using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    public class EndingCanvasManager : MonoBehaviour
    {
        [SerializeField] private Button backToMenu;
        [SerializeField] private string sceneToLoad;

        private void Start()
        {
            backToMenu.onClick.AddListener(() => SceneManager.LoadScene(sceneToLoad));
        }
    }
}