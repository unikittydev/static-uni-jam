using System.Collections;
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
        private int currentLevelIndex;

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
            progress.completedLevels.Add(currentLevelIndex);
            PlayerPrefs.SetString(PROGRESS_DATA, JsonUtility.ToJson(progress));

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
                int index = world.levelIndices[i];
                if (!unlockLevels && !progress.completedLevels.Contains(index))
                    enumerateFlag = false;
                AddLevelButton(world, index);
            }
        }

        private void AddLevelButton(LevelWorld world, int level)
        {
            LevelButton levelButton = Instantiate(levelButtonPrefab, levelListPanel);
            levelButton.text.text = string.Format(LEVEL_NAME_FORMAT, world.worldName, level);
            levelButton.button.onClick.AddListener(() => LoadLevelAdditive(world, level, levelButton.text.text));
        }
        
        private void LoadLevelAdditive(LevelWorld world, int index, string levelName)
        {
            StartCoroutine(LoadLevelAdditiveCoroutine(world, index, levelName, LevelState.Play));
        }
        
        private IEnumerator LoadLevelAdditiveCoroutine(LevelWorld world, int index, string levelName, LevelState levelState)
        {
            currentWorld = world;
            currentLevelIndex = index;
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
            AsyncOperation sceneUnload = SceneManager.UnloadSceneAsync(currentLevelIndex);
            
            vhsOverlay.Play(menuName, LevelState.Stop);
            
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