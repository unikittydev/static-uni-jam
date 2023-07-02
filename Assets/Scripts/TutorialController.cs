using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class TutorialController : MonoBehaviour
    {
        private const string TUT_WATCHED = "TUT_WATCHED_{0}";
        
        [SerializeField]
        private GameObject panel;
        [SerializeField]
        private TMP_Text label;

        [SerializeField] private Button leftButton, rightButton, proceedButton;

        [SerializeField] private GamePause gamePause;

        [SerializeField] private bool resetProgress;
        
        private TutorialInfo currentInfo;
        private int currentIndex;
        
        public void SetTutorialInfo(TutorialInfo info)
        {
            currentInfo = info;
            currentIndex = 0;
            proceedButton.gameObject.SetActive(false);
            
            if (resetProgress)
                PlayerPrefs.DeleteKey(string.Format(TUT_WATCHED, currentInfo.name));
        }

        public bool IsFirstLoad()
        {
            return !PlayerPrefs.HasKey(string.Format(TUT_WATCHED, currentInfo.name));
        }

        public void Show()
        {
            Time.timeScale = 0f;
            panel.SetActive(true);
            gamePause.enabled = false;
            currentIndex = 0;
            UpdateUI();
        }

        public void Hide()
        {
            Time.timeScale = 1f;
            panel.SetActive(false);
            gamePause.enabled = true;
        }

        public void NextPage()
        {
            currentIndex++;
            UpdateUI();
        }

        public void PreviousPage()
        {
            currentIndex--;
            UpdateUI();
        }

        private void UpdateUI()
        {
            label.text = currentInfo.labels[currentIndex];
            rightButton.gameObject.SetActive(currentIndex != currentInfo.labels.Length - 1);
            leftButton.gameObject.SetActive(currentIndex != 0);

            if (currentIndex == currentInfo.labels.Length - 1)
            {
                proceedButton.gameObject.SetActive(true);
                PlayerPrefs.SetInt(string.Format(TUT_WATCHED, currentInfo.name), 1);
            }
        }
    }
}
