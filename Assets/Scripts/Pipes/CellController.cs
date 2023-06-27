using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellController : MonoBehaviour
{
    private enum Types
    {
        Angle,
        Straight,
        Three,
        All
    }
    [SerializeField]
    private bool[] connections = { false, false, false, false };

    [SerializeField]
    private GameObject connectedPipeUp;
    [SerializeField]
    private GameObject connectedPipeRight;
    [SerializeField]
    private GameObject connectedPipeDown;
    [SerializeField]
    private GameObject connectedPipeLeft;

    [SerializeField]
    private Types currentType;

    [SerializeField]
    private GameObject spriteOn;
    [SerializeField]
    private GameObject spriteOff;

    [SerializeField]
    private bool isLocked = false;

    private int angle = 0;

    private int angleChange = -90;

    private bool isConnected = false;

    private void Start()
    {
        SwitchConnections();
    }


    [ContextMenu("Перевернуть")]
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
    }

    [ContextMenu("Поменять ток")]
    private void SwitchCondition()
    {
        isConnected = !isConnected;

        spriteOn.SetActive(isConnected);
        spriteOff.SetActive(!isConnected);
    }

    private void SwitchConnections()
    {
        switch(currentType)
        {
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
                    connections = new[] { true, false, true, true };
                else if (angle == 180)
                    connections = new[] { false, true, true, true };
                else
                    connections = new[] { true, true, false, true };
                break;
            case Types.All:
                connections = new[] { true, true, true, true };
                break;
        }
    }

    private void OnMouseDown()
    {
        //Rotate();
        //SwitchCondition();
    }
}
