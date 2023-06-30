using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Game
{
    public class CellController : MonoBehaviour
    {
        private enum Types
        {
            None,
            Angle,
            Straight,
            Three,
            All
        }
        [SerializeField]
        private bool[] connections = { false, false, false, false };

        [SerializeField]
        private Types currentType;

        [SerializeField]
        private GameObject spriteOn;
        [SerializeField]
        private GameObject spriteOff;

        [SerializeField]
        private bool isLocked = false;

        [SerializeField]
        public bool isStart = false;

        [SerializeField]
        public bool isEnd = false;

        [SerializeField]
        private int angle = 0;

        private int angleChange = -90;

        public bool isConnected = false;

        [SerializeField] private UnityEvent onPipeRotate;
        [SerializeField] private UnityEvent onConnect;
        [SerializeField] private UnityEvent onDisconnect;
        
        private void Start()
        {
            //Debug.Log(angle);
            //PrintConnections();
            SwitchConnections();
            //Debug.Log(angle);
            //PrintConnections();
        }

        [ContextMenu("�����������")]
        private void ContextRotate()
        {
            transform.Rotate(0, 0, angleChange);
            angle = Mathf.RoundToInt(transform.eulerAngles.z);
            SwitchConnections();
        }

        private void Rotate()
        {
            if (isLocked) return;

            transform.Rotate(0, 0, angleChange);
            angle = Mathf.RoundToInt(transform.eulerAngles.z);
            SwitchConnections();
            
            onPipeRotate?.Invoke();
        }

        [ContextMenu("�������� ���")]
        public void SwitchCondition()
        {
            if (isStart) return;

            isConnected = !isConnected;

            spriteOn.SetActive(isConnected);
            spriteOff.SetActive(!isConnected);
        }

        public void SwitchCondition(bool cond)
        {
            if (isStart) return;

            isConnected = cond;

            spriteOn.SetActive(isConnected);
            spriteOff.SetActive(!isConnected);
        }

        private void SwitchConnections()
        {
            switch (currentType)
            {
                case Types.None:
                    connections = new[] { false, false, false, false };
                    return;
                case Types.Angle:
                    if (angle == 0)
                        connections = new[] { true, false, true, false };
                    else if (angle == 90)
                        connections = new[] { true, true, false, false };
                    else if (angle == 180)
                        connections = new[] { false, true, false, true };
                    else
                        connections = new[] { false, false, true, true };
                    break;
                case Types.Straight:
                    if (angle == 0 || angle == 180)
                        connections = new[] { true, false, false, true };
                    else
                        connections = new[] { false, true, true, false };
                    break;
                case Types.Three:
                    if (angle == 0)
                        connections = new[] { true, true, true, false };
                    else if (angle == 90)
                        connections = new[] { true, true, false, true };
                    else if (angle == 180)
                        connections = new[] { false, true, true, true };
                    else
                        connections = new[] { true, false, true, true };
                    break;
                case Types.All:
                    connections = new[] { true, true, true, true };
                    break;
            }
        }

        public bool CheckConnection(int dir)
        {
            return connections[dir];
        }

        public bool ConnectUp()
        {
            return connections[0];
        }
        public bool ConnectLeft()
        {
            return connections[1];
        }
        public bool ConnectRight()
        {
            return connections[2];
        }
        public bool ConnectDown()
        {
            return connections[3];
        }

        private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            
            Rotate();
            
            PipeGameController.instance.SwitchAllConditions();
        }

        public void PrintConnections()
        {
            Debug.Log(transform.position.x + " " + transform.position.y);
            Debug.Log("Up:" + connections[0] + " Left:" + connections[1] + " Right:" + connections[2] + " Down:" + connections[3]);
        }
    }
}
