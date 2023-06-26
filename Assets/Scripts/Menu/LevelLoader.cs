using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public class LevelLoader : MonoBehaviour
    {
        private const string LAST_COMPLETED_LEVEL = "LAST_COMPLETED_LEVEL";
        private const string LEVEL_NAME_FORMAT = "Уровень {0}";
        
        [SerializeField] private int[] levelIndices;

        [SerializeField] private LevelButton levelButtonPrefab;
        [SerializeField] private Transform levelListPanel;
        
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

            while (!sceneLoad.isDone)
                yield return null;
        }
    }
}