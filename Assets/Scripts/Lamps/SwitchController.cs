using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] lamps;

    void OnMouseDown()
    {
        foreach (var l in lamps)
        {
            l.GetComponent<LampController>().SwitchCondition();
        }
        CheckWinLamps.instance.CheckWin();
    }
}
