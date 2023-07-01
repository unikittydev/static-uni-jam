using System;
using UnityEngine;

namespace Game
{
    public class GamePause : MonoBehaviour
    {
        [SerializeField] private VHSOverlay vhsOverlay;
        [SerializeField] private LevelLoader levelLoader;
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject levelPanel;
        [SerializeField] private GameObject menu;
        [SerializeField] private GameObject pauseButtons;
        
        private static bool gamePaused;

        public static bool GamePaused => gamePaused;

        private void OnEnable()
        {
            pauseButtons.SetActive(true);
        }

        private void OnDisable()
        {
            pauseButtons.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                TogglePause();
        }

        public void TogglePause()
        {
            if (gamePaused)
                Resume();
            else
                Pause();
        }
        
        public void Resume()
        {
            gamePaused = false;
            Time.timeScale = 1f;
            vhsOverlay.Stop();
            menu.SetActive(false);
            pauseButtons.SetActive(true);
            levelPanel.SetActive(true);
            pausePanel.SetActive(false);
        }

        public void RestartLevel()
        {
            gamePaused = false;
            levelLoader.RestartLevel();
            menu.SetActive(false);
            levelPanel.SetActive(true);
            pausePanel.SetActive(false);
        }
        
        public void Pause()
        {
            gamePaused = true;
            Time.timeScale = 0f;
            vhsOverlay.Play(string.Empty, LevelState.Pause);
            
            menu.SetActive(true);
            pauseButtons.SetActive(false);
            levelPanel.SetActive(false);
            pausePanel.SetActive(true);
        }

        public void QuitLevel()
        {
            levelLoader.QuitLevel(false);
            gamePaused = false;
            
            menu.SetActive(false);
            levelPanel.SetActive(true);
            pausePanel.SetActive(false);
        }
    }
}
