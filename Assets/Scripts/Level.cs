using UnityEngine;

namespace Game
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private PuzzleBase[] puzzleParts;

        private int partsCompleted;
        
        private void OnEnable()
        {
            foreach (PuzzleBase part in puzzleParts)
            {
                part.OnPuzzleCompleted.AddListener(IncrementCompletedCounter);
                part.OnPuzzleBroke.AddListener(DecrementCompletedCounter);
            }
        }

        private void OnDisable()
        {
            foreach (PuzzleBase part in puzzleParts)
            {
                part.OnPuzzleCompleted.RemoveListener(IncrementCompletedCounter);
                part.OnPuzzleCompleted.RemoveListener(DecrementCompletedCounter);
            }
        }

        private void IncrementCompletedCounter()
        {
            partsCompleted++;
        }
        
        private void DecrementCompletedCounter()
        {
            partsCompleted--;
        }
    }
}