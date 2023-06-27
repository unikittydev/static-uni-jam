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

        [SerializeField] private GameObject menu;
        [SerializeField] private WhiteNoiseCreator noiseGenerator;
        [SerializeField] private ImageFader screenOverlay;
        [SerializeField] private VHSOverlay vhsOverlay;
            
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
                //if (level > lastLevel)
                //    return;

                LevelButton levelButton = Instantiate(levelButtonPrefab, levelListPanel);
                levelButton.text.text = string.Format(LEVEL_NAME_FORMAT, level);
                levelButton.button.onClick.AddListener(() => LoadLevelAdditive(level, levelButton.text.text));
            }
        }

        public void LoadLevelAdditive(int index, string levelName)
        {
            StartCoroutine(LoadLevelAdditiveCoroutine(index, levelName));
        }

        private IEnumerator LoadLevelAdditiveCoroutine(int index, string levelName)
        {
            AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
            sceneLoad.allowSceneActivation = false;
            
            vhsOverlay.Play(levelName, LevelState.Play);
            menu.SetActive(false);
            
            noiseGenerator.enabled = true;
            yield return StartCoroutine(screenOverlay.SetFade(true));
            
            vhsOverlay.Stop();

            while (sceneLoad.progress < 0.9f)
                yield return null;

            sceneLoad.allowSceneActivation = true;
            
            yield return StartCoroutine(screenOverlay.SetFade(false));
            noiseGenerator.enabled = false;
        }
    }
}