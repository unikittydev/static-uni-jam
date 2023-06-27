using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PipeController : MonoBehaviour
{
    [SerializeField]
    public bool bondUp;
    [SerializeField]
    public bool bondDown;
    [SerializeField]
    public bool bondLeft;
    [SerializeField]
    public bool bondRight;

    [SerializeField]
    private bool condition;

    [SerializeField]
    private GameObject spriteOn;
    [SerializeField]
    private GameObject spriteOff;

    public void SwitchCondition()
    {
        condition = !condition;
        SwitchSprite();
    }
    private void SwitchSprite()
    {
        spriteOn.SetActive(condition);
        spriteOff.SetActive(!condition);
    }
}
