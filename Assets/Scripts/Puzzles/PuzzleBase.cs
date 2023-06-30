using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class PuzzleBase : MonoBehaviour
    {
        [SerializeField] private UnityEvent onPuzzleCompleted;
        public UnityEvent OnPuzzleCompleted => onPuzzleCompleted;
        
        [SerializeField] private UnityEvent onPuzzleBroke;
        public UnityEvent OnPuzzleBroke => onPuzzleBroke;

        private bool completed;
        
        protected void TryWin()
        {
            if (completed)
                return;
            completed = true;
            onPuzzleCompleted.Invoke();
        }

        protected void CancelWin()
        {
            if (!completed)
                return;
            completed = false;
            onPuzzleBroke.Invoke();
        }
    }
}