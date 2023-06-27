using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class PuzzleBase : MonoBehaviour
    {
        [SerializeField] protected UnityEvent onPuzzleCompleted;
        public UnityEvent OnPuzzleCompleted => onPuzzleCompleted;
        
        [SerializeField] protected UnityEvent onPuzzleBroke;
        public UnityEvent OnPuzzleBroke => onPuzzleBroke;
    }
}