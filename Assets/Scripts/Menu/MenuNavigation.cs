using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MenuNavigation : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> activePanels = new();

        public void AddActivePanel(GameObject panel)
        {
            panel.SetActive(true);
            activePanels.Add(panel);
        }

        public void SetActivePanel(GameObject panel)
        {
            foreach (GameObject activePanel in activePanels)
                activePanel.SetActive(false);
            activePanels.Clear();
            AddActivePanel(panel);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}