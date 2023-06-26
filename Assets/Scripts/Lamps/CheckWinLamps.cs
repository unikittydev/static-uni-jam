using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckWinLamps : MonoBehaviour
{
    [SerializeField]
    private GameObject[] lamps;

    public static CheckWinLamps instance { get; private set; }
    private void Awake()
    {
        instance = this;
    }
    private void ConsoleWin()
    {
        Debug.Log("Win");
    }

    public void CheckWin()
    {
        foreach (var item in lamps)
        {
            if (!item.GetComponent<LampController>().CheckCondition())
            {
                
                return;
            }
        }
        ConsoleWin();
    }
}
