using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class LevelLoader : MonoBehaviour
    {
        private const string LAST_COMPLETED_LEVEL = "LAST_COMPLETED_LEVEL";
        private const string LEVEL_NAME_FORMAT = "Уровень {0}";
        
        [SerializeField] private int[] levelIndices;

        [SerializeField] private LevelButton levelButtonPrefab;
        [SerializeField] private Transform levelListPanel;

        [SerializeField]
        private GameObject menu;
        [SerializeField] private WhiteNoiseCreator noiseGenerator;
        [SerializeField] private ImageFader screenOverlay;
        
        private void Start()
        {
            AddLevelButtons();
        }

        private void AddLevelButtons()
        {
            int lastLevel = PlayerPrefs.GetInt(LAST_COMPLETED_LEVEL, levelIndices[0]);

            for (int i = 0; i < levelIndices.Length; i++)
            {
                int level = levelIndices[i];
                if (level > lastLevel)
                    return;

                LevelButton levelButton = Instantiate(levelButtonPrefab, levelListPanel);
                levelButton.text.text = string.Format(LEVEL_NAME_FORMAT, level);
                levelButton.button.onClick.AddListener(() => LoadLevelAdditive(level));
            }
        }

        public void LoadLevelAdditive(int index)
        {
            StartCoroutine(LoadLevelAdditiveCoroutine(index));
        }

        private IEnumerator LoadLevelAdditiveCoroutine(int index)
        {
            AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
            sceneLoad.allowSceneActivation = false;

            noiseGenerator.enabled = true;
            yield return StartCoroutine(screenOverlay.SetFade(true));
            noiseGenerator.enabled = false;
            menu.SetActive(false);

            while (sceneLoad.progress < 0.9f)
                yield return null;

            sceneLoad.allowSceneActivation = true;
            
            yield return StartCoroutine(screenOverlay.SetFade(false));
        }
    }
}