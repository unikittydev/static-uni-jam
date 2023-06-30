using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game
{
    public class PipeGameController : PuzzleBase
    {
        private CellController[,] cells;

        [SerializeField]
        private int height = 20;

        [SerializeField]
        private int width = 12;

        [SerializeField]
        private float offsetX = 9.5f;

        [SerializeField]
        private float offsetY = 5.5f;

        private int startX;
        private int startY;

        private List<CellController> endCells = new List<CellController>();

        public static PipeGameController instance { get; private set; }

        private void Awake()
        {
            instance = this;
        }

        [ContextMenu("����� �����")]
        void Start()
        {
            cells = new CellController[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    cells[i, j] = null;
                }
            }

            foreach (Transform child in transform)
            {
                if (child.GetComponent<CellController>().isStart)
                {
                    startX = Mathf.RoundToInt(child.position.x + offsetX);
                    startY = Mathf.RoundToInt(child.position.y + offsetY);

                }
                if (child.GetComponent<CellController>().isEnd)
                {
                    endCells.Add(child.GetComponent<CellController>());

                }
                cells[Mathf.RoundToInt(child.position.x + offsetX), Mathf.RoundToInt(child.position.y + offsetY)] = child.GetComponent<CellController>();
            }
            SwitchAllConditions();
        }

        public void SwitchAllConditions()
        {
            foreach (CellController cell in cells)
            {
                if (cell == null) continue;
                cell.SwitchCondition(false);
                if (endCells.Contains(cell))
                {
                    //Debug.Log("Cancel");
                    CancelWin();
                }
            }
            SwitchCondition(startX, startY);
        }

        private void SwitchCondition(int x, int y)
        {
            cells[x, y].SwitchCondition(true);

            if (endCells.Contains(cells[x, y]))
            {
                CheckWin();
            }
            if (cells[x, y].ConnectUp())
            {
                if (cells[x, y + 1] != null)
                {
                    if (cells[x, y + 1].ConnectDown() && !cells[x, y + 1].isConnected)
                    {
                        //Debug.Log("Up");
                        SwitchCondition(x, y + 1);
                    }
                }

            }

            if (cells[x, y].ConnectLeft())
            {
                if (cells[x - 1, y] != null)
                {
                    if (cells[x - 1, y].ConnectRight() && !cells[x - 1, y].isConnected)
                    {
                        //Debug.Log("Left");
                        SwitchCondition(x - 1, y);
                    }
                }

            }

            if (cells[x, y].ConnectRight())
            {
                if (cells[x + 1, y] != null)
                {
                    
                    if (cells[x + 1, y].ConnectLeft() && !cells[x + 1, y].isConnected)
                    {
                        //Debug.Log("Right");
                        SwitchCondition(x + 1, y);
                    }
                }

            }

            if (cells[x, y].ConnectDown())
            {
                if (cells[x, y - 1] != null)
                {
                    if (cells[x, y - 1].ConnectUp() && !cells[x, y - 1].isConnected)
                    {
                        //Debug.Log("Down");
                        SwitchCondition(x, y - 1);
                    }
                }

            }
        }

        private void CheckWin()
        {
            foreach(var cell in endCells)
            {
                if (!cell.isConnected)
                {
                    return;
                }
            }
            TryWin();
        }

    }
}