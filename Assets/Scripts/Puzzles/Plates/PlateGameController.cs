using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlateGameController : PuzzleBase
    {
        private PlateController[,] cells;

        private int height = 20;
        private int width = 12;
        private float offsetX = 9.5f;
        private float offsetY = 5.5f;

        [SerializeField]
        private GameObject start;

        private int startX;
        private int startY;

        private int positionX;
        private int positionY;

        public static PlateGameController instance { get; private set; }
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            cells = new PlateController[height, width];
            startX = Mathf.RoundToInt(start.transform.position.x + offsetX);
            startY = Mathf.RoundToInt(start.transform.position.y + offsetY);

            positionX = startX;
            positionY = startY;

            foreach (Transform child in transform)
            {
                cells[Mathf.RoundToInt(child.position.x + offsetX), Mathf.RoundToInt(child.position.y + offsetY)] = child.GetComponent<PlateController>();
            }

            AddDirections();
        }

        public void ReleaseRay(PlateController plate)
        {
            if (cells[positionX, positionY + 1] != null && cells[positionX, positionY + 1].CanPass && plate.CurrentDir != PlateController.Direction.Up)
            {
                cells[positionX, positionY + 1].ChangeType(PlateController.Types.Empty, PlateController.Direction.None);
            }
            if (cells[positionX, positionY - 1] != null && cells[positionX, positionY - 1].CanPass && plate.CurrentDir != PlateController.Direction.Down)
            {
                cells[positionX, positionY - 1].ChangeType(PlateController.Types.Empty, PlateController.Direction.None);
            }
            if (cells[positionX - 1, positionY] != null && cells[positionX - 1, positionY].CanPass && plate.CurrentDir != PlateController.Direction.Left)
            {
                cells[positionX - 1, positionY].ChangeType(PlateController.Types.Empty, PlateController.Direction.None);
            }
            if (cells[positionX + 1, positionY] != null && cells[positionX + 1, positionY].CanPass && plate.CurrentDir != PlateController.Direction.Right)
            {
                cells[positionX + 1, positionY].ChangeType(PlateController.Types.Empty, PlateController.Direction.None);
            }
            switch (plate.CurrentDir)
            {
                case PlateController.Direction.Up:
                    positionY += 1;
                    while (cells[positionX, positionY + 1] != null && cells[positionX, positionY + 1].CanPass)
                    {
                        cells[positionX, positionY].ChangeType(PlateController.Types.Light, PlateController.Direction.None);
                        positionY += 1;
                    }
                    break; 
                case PlateController.Direction.Down:
                    positionY -= 1;
                    while (cells[positionX, positionY - 1] != null && cells[positionX, positionY - 1].CanPass)
                    {
                        cells[positionX, positionY].ChangeType(PlateController.Types.Light, PlateController.Direction.None);
                        positionY -= 1;
                    }
                    break;
                case PlateController.Direction.Left:
                    positionX -= 1;
                    while (cells[positionX - 1, positionY] != null && cells[positionX - 1, positionY].CanPass)
                    {
                        cells[positionX, positionY].ChangeType(PlateController.Types.Light, PlateController.Direction.None);
                        positionX -= 1;
                    }
                    break;  
                case PlateController.Direction.Right:
                    positionX += 1;
                    while (cells[positionX + 1, positionY] != null && cells[positionX + 1, positionY].CanPass)
                    {
                        cells[positionX, positionY].ChangeType(PlateController.Types.Light, PlateController.Direction.None);
                        positionX += 1;
                    }
                    break;
            }

            AddDirections();
            CheckWin();
        }

        [ContextMenu("Начать заново")]
        private void Restart()
        {
            positionX = startX;
            positionY = startY;
            foreach (var cell in cells)
            {
                if (cell != null && cell.CurrentType != PlateController.Types.Wall)
                {
                    cell.ChangeType(PlateController.Types.Empty, PlateController.Direction.None);
                }
            }
            
            AddDirections();
            CancelWin();
        }

        private void AddDirections()
        {
            cells[positionX, positionY].ChangeType(PlateController.Types.Light, PlateController.Direction.None);

            if (cells[positionX, positionY + 1] != null && cells[positionX, positionY + 1].CanPass) 
            {
                cells[positionX, positionY + 1].ChangeType(PlateController.Types.Direction, PlateController.Direction.Up);
            }
            if (cells[positionX, positionY - 1] != null && cells[positionX, positionY - 1].CanPass)
            {
                cells[positionX, positionY - 1].ChangeType(PlateController.Types.Direction, PlateController.Direction.Down);
            }
            if (cells[positionX - 1, positionY] != null && cells[positionX - 1, positionY].CanPass)
            {
                cells[positionX - 1, positionY].ChangeType(PlateController.Types.Direction, PlateController.Direction.Left);
            }
            if (cells[positionX + 1, positionY] != null && cells[positionX + 1, positionY].CanPass)
            {
                cells[positionX + 1, positionY].ChangeType(PlateController.Types.Direction, PlateController.Direction.Right);
            }
        }

        private void CheckWin()
        {
            foreach (PlateController cell in cells)
            {
                if (cell != null)
                {
                    if (!cell.CanEnd)
                    {
                        return;
                    }
                }
            }
            TryWin();
            //Debug.Log("Win");
        }

        
    }
}
