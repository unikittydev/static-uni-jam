using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Game
{
    public class PlateController : MonoBehaviour
    {
        public enum Types
        {
            Empty,
            Light,
            Wall,
            Direction
        }

        [SerializeField]
        private Color emptyColor;
        [SerializeField] 
        private Color lightColor;
        [SerializeField] 
        private Color wallColor;
        [SerializeField] 
        private Color directionColor;

        [SerializeField]
        private Types currentType = Types.Empty;

        public Types CurrentType
        {
            get { return currentType; }
            private set { currentType = value; }
        }

        public enum Direction
        {
            None,
            Up,
            Down,       
            Left,
            Right
        }

        private Direction currentDir = Direction.None;

        public Direction CurrentDir
        {
            get { return currentDir; }
            private set { currentDir = value; }
        }

        [SerializeField]
        private bool canPass;
        public bool CanPass
        {
            get
            {
                return canPass;
            }
            private set
            {
                canPass = value;
            }
        }

        private bool canEnd;
        public bool CanEnd
        {
            get
            {
                return canEnd;
            }
            private set
            {
                canEnd = value;
            }
        }

        [SerializeField] private UnityEvent onRayRelease;
        
        private void Awake()
        {
            Initialize(Direction.None);
        }

        [ContextMenu("fwfw")]
        private void PrintPosition()
        {
            Debug.Log(transform.position);
        }

        public void ChangeType(Types type, Direction dir)
        {
            currentType = type;
            Initialize(dir);
        }

        private void Initialize(Direction dir)
        {
            switch (currentType)
            {
                case Types.Empty:
                    GetComponent<SpriteRenderer>().color = emptyColor;
                    CanEnd = false;
                    CanPass = true;
                    CurrentDir = Direction.None;
                    break;
                case Types.Light:
                    GetComponent<SpriteRenderer>().color = lightColor;
                    CanEnd = true;
                    CanPass = false;
                    CurrentDir = Direction.None;
                    break;
                case Types.Wall:
                    GetComponent<SpriteRenderer>().color = wallColor;
                    CanEnd = true;
                    CanPass = false;
                    CurrentDir = Direction.None;
                    break;
                case Types.Direction:
                    GetComponent<SpriteRenderer>().color = directionColor;
                    CanEnd = false;
                    CanPass = true;
                    CurrentDir = dir;
                    break;
            }
        }

        private void OnMouseDown()
        {
            if (GamePause.GamePaused || EventSystem.current.IsPointerOverGameObject())
                return;
            
            if (currentType != Types.Direction) return;

            PlateGameController.instance.ReleaseRay(this);
            
            onRayRelease?.Invoke();
        }
    }
}