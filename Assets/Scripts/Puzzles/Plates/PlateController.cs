using Game;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
        private Sprite emptySprite;
        [SerializeField]
        private Sprite lightSprite;
        [SerializeField]
        private Sprite wallSprite;
        [SerializeField]
        private Sprite directionSprite;

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

        private void Awake()
        {
            Initialize(Direction.None);
        }

        public void ChangeType(Types type, Direction dir)
        {
            currentType = type;
            Initialize(dir);
        }

        [ContextMenu("Переопределить")]
        private void Initialize(Direction dir)
        {
            switch (currentType)
            {
                case Types.Empty:
                    GetComponent<SpriteRenderer>().sprite = emptySprite;
                    CanEnd = false;
                    CanPass = true;
                    CurrentDir = Direction.None;
                    break;
                case Types.Light:
                    GetComponent<SpriteRenderer>().sprite = lightSprite;
                    CanEnd = true;
                    CanPass = false;
                    CurrentDir = Direction.None;
                    break;
                case Types.Wall:
                    GetComponent<SpriteRenderer>().sprite = wallSprite;
                    CanEnd = true;
                    CanPass = false;
                    CurrentDir = Direction.None;
                    break;
                case Types.Direction:
                    GetComponent<SpriteRenderer>().sprite = directionSprite;
                    CanEnd = false;
                    CanPass = true;
                    CurrentDir = dir;
                    break;
            }
        }

        private void OnMouseDown()
        {
            if (currentType != Types.Direction) return;

            PlateGameController.instance.ReleaseRay(this);
        }
    }
}