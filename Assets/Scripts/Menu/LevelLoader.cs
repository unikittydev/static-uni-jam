using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class LevelLoader : MonoBehaviour
    {
        private const string PROGRESS_DATA = "PROGRESS_DATA";
        private const string LEVEL_NAME_FORMAT = "{0} {1:00}";
        private const string backButtonText = "НАЗАД";
        private const string menuName = "МЕНЮ";
        
        [SerializeField] private LevelWorld[] worlds;
        
        [SerializeField] private LevelButton levelButtonPrefab;
        [SerializeField] private GameObject separatorPrefab;
        [SerializeField] private Transform worldListPanel;
        [SerializeField] private Transform levelListPanel;

        [SerializeField] private GameObject menu;
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private MenuNavigation navigation;
        [SerializeField] private WhiteNoiseCreator noiseGenerator;
        [SerializeField] private ImageFader screenOverlay;
        [SerializeField] private VHSOverlay vhsOverlay;

        private ProgressData progress;
        
        public static LevelLoader Instance { get; private set; }

        private LevelWorld currentWorld;
        private LevelData currentLevel;

        public bool resetProgressOnLoad, unlockLevels;
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            if (resetProgressOnLoad)
                PlayerPrefs.DeleteKey(PROGRESS_DATA);

            if (PlayerPrefs.HasKey(PROGRESS_DATA))
            {
                string progressJson = PlayerPrefs.GetString(PROGRESS_DATA);
                progress = JsonUtility.FromJson<ProgressData>(progressJson);
            }
            else
                progress = new ProgressData();
        }

        public void CompleteLevel()
        {
            if (!progress.completedLevels.Contains(currentLevel))
            {
                progress.completedLevels.Add(currentLevel);
                PlayerPrefs.SetString(PROGRESS_DATA, JsonUtility.ToJson(progress));
            }

            foreach (Transform children in levelListPanel)
                Destroy(children.gameObject);
            
            ClearWorldButtons();
            AddWorldButtons();
            AddLevelButtons(currentWorld);

            StartCoroutine(UnloadLevelCoroutine());
        }

        private void AddWorldButtons()
        {
            ClearWorldButtons();
            
            for (int i = 0; i < worlds.Length; i++)
            {
                LevelWorld world = worlds[i];
                if (!unlockLevels && !world.IsUnlocked(progress.completedLevels))
                    continue;
                
                LevelButton worldButton = Instantiate(levelButtonPrefab, worldListPanel);
                worldButton.text.text = world.worldName;
                worldButton.button.onClick.AddListener(() => AddLevelButtons(world));
            }

            Instantiate(separatorPrefab, worldListPanel);
            LevelButton backButton = Instantiate(levelButtonPrefab, worldListPanel);
            backButton.text.text = backButtonText;
            backButton.button.onClick.AddListener(() => navigation.SetActivePanel(mainMenuPanel));
        }

        private void ClearWorldButtons()
        {
            foreach (Transform button in worldListPanel)
                Destroy(button.gameObject);
            ClearLevelButtons();
        }

        private void ClearLevelButtons()
        {
            foreach (Transform button in levelListPanel)
                Destroy(button.gameObject);
        }
        
        private void AddLevelButtons(LevelWorld world)
        {
            ClearLevelButtons();
            bool enumerateFlag = true;
            for (int i = 0; enumerateFlag && i < world.levelIndices.Length; i++)
            {
                LevelData level = world.levelIndices[i];
                if (!unlockLevels && !progress.completedLevels.Contains(level))
                    enumerateFlag = false;
                AddLevelButton(world, level);
            }
        }

        private void AddLevelButton(LevelWorld world, LevelData level)
        {
            LevelButton levelButton = Instantiate(levelButtonPrefab, levelListPanel);
            levelButton.text.text = level.name;
            levelButton.button.onClick.AddListener(() => LoadLevelAdditive(world, level));
        }
        
        private void LoadLevelAdditive(LevelWorld world, LevelData level)
        {
            StartCoroutine(LoadLevelAdditiveCoroutine(world, level, LevelState.Play));
        }
        
        private IEnumerator LoadLevelAdditiveCoroutine(LevelWorld world, LevelData level, LevelState levelState)
        {
            currentWorld = world;
            currentLevel = level;
            AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(level.buildIndex, LoadSceneMode.Additive);
            sceneLoad.allowSceneActivation = false;
            
            vhsOverlay.Play(level.name, levelState);
            menu.SetActive(false);
            
            noiseGenerator.enabled = true;
            yield return StartCoroutine(screenOverlay.SetFade(true));
            yield return new WaitForSecondsRealtime(3f);

            while (sceneLoad.progress < 0.9f)
                yield return null;

            vhsOverlay.Stop();
            sceneLoad.allowSceneActivation = true;
            
            yield return StartCoroutine(screenOverlay.SetFade(false));
            noiseGenerator.enabled = false;
        }
        
        private IEnumerator UnloadLevelCoroutine()
        {
            vhsOverlay.Play(menuName, LevelState.Stop);
            
            noiseGenerator.enabled = true;
            yield return StartCoroutine(screenOverlay.SetFade(true));
            
            AsyncOperation sceneUnload = SceneManager.UnloadSceneAsync(currentLevel.buildIndex);
            yield return new WaitForSecondsRealtime(3f);
            
            while (!sceneUnload.isDone)
                yield return null;
            
            vhsOverlay.Stop();
            menu.SetActive(true);
            yield return StartCoroutine(screenOverlay.SetFade(false));
            noiseGenerator.enabled = false;
        }
    }
}