using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class TutorialController : MonoBehaviour
    {
        [SerializeField]
        private GameObject panel;
        [SerializeField]
        private TMP_Text label;

        [SerializeField] private Button leftButton, rightButton, proceedButton;
        
        private TutorialInfo currentInfo;
        private int currentIndex;

        private bool canClose;

        private void Update()
        {
            if (canClose && Input.GetKeyDown(KeyCode.Escape))
                Hide();
        }

        public void SetTutorialInfo(TutorialInfo info)
        {
            currentInfo = info;
            currentIndex = 0;
            proceedButton.gameObject.SetActive(false);
        }

        public void Show()
        {
            panel.SetActive(true);
            currentIndex = 0;
            UpdateUI();
        }

        public void Hide()
        {
            panel.SetActive(false);
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
                canClose = true;
            }
        }
    }
}
