using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampController : MonoBehaviour
{
    [SerializeField]
    private bool condition;

    private GameObject spriteOn;

    private void Start()
    {
        spriteOn = transform.GetChild(0).gameObject;
        UpdateSprite();
    }

    public void SwitchCondition()
    {
        condition = !condition;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        spriteOn.SetActive(condition);
    }

    public bool CheckCondition()
    {
        return condition;
    }

}
