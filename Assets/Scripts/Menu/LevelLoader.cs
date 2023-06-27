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

        public static LevelLoader Instance { get; private set; }

        private int levelIndex;

        public bool resetProgressOnLoad, unlockLevels;
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            if (resetProgressOnLoad)
                PlayerPrefs.SetInt(LAST_COMPLETED_LEVEL, levelIndices[0]);
            AddLevelButtons();
        }
        
        public void LoadLevelAdditive(int index, string levelName)
        {
            StartCoroutine(LoadLevelAdditiveCoroutine(index, levelName, LevelState.Play));
        }

        public void CompleteLevel()
        {
            int lastCompletedLevel = PlayerPrefs.GetInt(LAST_COMPLETED_LEVEL, levelIndices[0]) + 1;
            PlayerPrefs.SetInt(LAST_COMPLETED_LEVEL, Mathf.Max(lastCompletedLevel, levelIndex));

            foreach (Transform children in levelListPanel)
                Destroy(children.gameObject);
            AddLevelButtons();

            StartCoroutine(UnloadLevelCoroutine());
        }

        private void AddLevelButtons()
        {
            int lastLevel = PlayerPrefs.GetInt(LAST_COMPLETED_LEVEL, levelIndices[0]);
            
            for (int i = 0; i < levelIndices.Length; i++)
            {
                int level = levelIndices[i];
                if (!unlockLevels && level > lastLevel)
                    return;

                LevelButton levelButton = Instantiate(levelButtonPrefab, levelListPanel);
                levelButton.text.text = string.Format(LEVEL_NAME_FORMAT, level);
                levelButton.button.onClick.AddListener(() => LoadLevelAdditive(level, levelButton.text.text));
            }
        }

        private IEnumerator LoadLevelAdditiveCoroutine(int index, string levelName, LevelState levelState)
        {
            levelIndex = index;
            AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
            sceneLoad.allowSceneActivation = false;
            
            vhsOverlay.Play(levelName, levelState);
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
        
        private IEnumerator UnloadLevelCoroutine()
        {
            AsyncOperation sceneUnload = SceneManager.UnloadSceneAsync(levelIndex);
            
            vhsOverlay.Play("МЕНЮ", LevelState.Stop);
            
            noiseGenerator.enabled = true;
            yield return StartCoroutine(screenOverlay.SetFade(true));
            
            vhsOverlay.Stop();
            
            while (!sceneUnload.isDone)
                yield return null;
            
            menu.SetActive(true);
            yield return StartCoroutine(screenOverlay.SetFade(false));
            noiseGenerator.enabled = false;
        }
    }
}