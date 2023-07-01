using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

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
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private GameObject videoOverlay;
        [SerializeField] private GamePause pause;
        
        [SerializeField] private Color completedLevelColor;
        [Header("Cursors")]
        [SerializeField] private CursorData menuCursor;
        [SerializeField] private CursorData gameCursor;
        
        private ProgressData progress;
        
        public static LevelLoader Instance { get; private set; }

        private LevelWorld currentWorld;
        private LevelData currentLevel;

        public bool resetProgressOnLoad, unlockLevels;
        
        private void Awake()
        {
            Instance = this;
            Cursor.SetCursor(menuCursor.cursor, menuCursor.hotSpot, CursorMode.Auto);
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

        public void QuitLevel(bool showVideo)
        {
            foreach (Transform children in levelListPanel)
                Destroy(children.gameObject);
            
            ClearWorldButtons();
            AddWorldButtons();
            AddLevelButtons(currentWorld);

            StartCoroutine(UnloadLevelCoroutine(showVideo));
        }
        
        public void CompleteLevel()
        {
            if (!progress.completedLevels.Contains(currentLevel))
            {
                progress.completedLevels.Add(currentLevel);
                PlayerPrefs.SetString(PROGRESS_DATA, JsonUtility.ToJson(progress));
            }
            QuitLevel(true);
        }

        public void RestartLevel()
        {
            StartCoroutine(RestartLevelCoroutine());
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

            if (progress.completedLevels.Contains(level))
            {
                ColorBlock colors = levelButton.button.colors;
                colors.normalColor = completedLevelColor;
                levelButton.button.colors = colors;
            }
            levelButton.button.onClick.AddListener(() => LoadLevelAdditive(world, level));
        }
        
        private void LoadLevelAdditive(LevelWorld world, LevelData level)
        {
            StartCoroutine(LoadLevelAdditiveCoroutine(world, level, LevelState.Play));
        }
        
        private IEnumerator LoadLevelAdditiveCoroutine(LevelWorld world, LevelData level, LevelState levelState)
        {
            Cursor.visible = false;
            Cursor.SetCursor(gameCursor.cursor, gameCursor.hotSpot, CursorMode.Auto);
            
            currentWorld = world;
            currentLevel = level;
            AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(level.buildIndex, LoadSceneMode.Additive);
            sceneLoad.allowSceneActivation = false;
            
            vhsOverlay.Play(level.name, levelState);
            menu.SetActive(false);
            
            noiseGenerator.enabled = true;
            yield return StartCoroutine(screenOverlay.SetFade(true));
            yield return new WaitForSecondsRealtime(1f);

            while (sceneLoad.progress < 0.9f)
                yield return null;

            vhsOverlay.Stop();
            sceneLoad.allowSceneActivation = true;
            
            yield return StartCoroutine(screenOverlay.SetFade(false));
            Cursor.visible = true;
            noiseGenerator.enabled = false;
            pause.enabled = true;
        }
        
        private IEnumerator UnloadLevelCoroutine(bool showVideo)
        {
            Time.timeScale = 0f;

            pause.enabled = false;
            Cursor.visible = false;
            Cursor.SetCursor(menuCursor.cursor, menuCursor.hotSpot, CursorMode.Auto);

            if (showVideo)
            {
                vhsOverlay.Play(menuName, LevelState.Complete);
                videoPlayer.clip = currentLevel.endVideo;
                videoPlayer.time = 0f;
                videoPlayer.Play();
                yield return null;
                videoOverlay.SetActive(true);
                yield return new WaitForSecondsRealtime(3f);
                videoPlayer.Pause();

                vhsOverlay.SetLevelStateName(LevelState.Stop);
                yield return new WaitForSecondsRealtime(2f);
            }
            else
                vhsOverlay.Play(menuName, LevelState.Stop);

            noiseGenerator.enabled = true;
            yield return StartCoroutine(screenOverlay.SetFade(true));

            if (showVideo)
            {
                videoPlayer.Stop();
                videoOverlay.SetActive(false);
            }

            AsyncOperation sceneUnload = SceneManager.UnloadSceneAsync(currentLevel.buildIndex);

            //yield return new WaitForSecondsRealtime(3f);
            
            while (!sceneUnload.isDone)
                yield return null;
            
            Cursor.visible = true;
            vhsOverlay.Stop();
            menu.SetActive(true);
            yield return StartCoroutine(screenOverlay.SetFade(false));
            noiseGenerator.enabled = false;

            Time.timeScale = 1f;
        }

        private IEnumerator RestartLevelCoroutine()
        {
            Time.timeScale = 0f;
            
            pause.enabled = false;
            Cursor.visible = false;
            Cursor.SetCursor(menuCursor.cursor, menuCursor.hotSpot, CursorMode.Auto);
            vhsOverlay.Play(currentLevel.name, LevelState.Restart);
            
            noiseGenerator.enabled = true;
            yield return StartCoroutine(screenOverlay.SetFade(true));
            
            AsyncOperation sceneUnload = SceneManager.UnloadSceneAsync(currentLevel.buildIndex);
            
            while (!sceneUnload.isDone)
                yield return null;
            
            AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(currentLevel.buildIndex, LoadSceneMode.Additive);
            vhsOverlay.SetLevelStateName(LevelState.Play);
            
            while (!sceneLoad.isDone)
                yield return null;
            
            vhsOverlay.Stop();
            
            yield return StartCoroutine(screenOverlay.SetFade(false));
            Cursor.visible = true;
            noiseGenerator.enabled = false;

            pause.enabled = true;
            Time.timeScale = 1f;
        }
    }
}