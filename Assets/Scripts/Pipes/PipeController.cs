using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PipeController : MonoBehaviour
{

    public bool bondUp;
    public bool bondDown;
    public bool bondLeft;
    public bool bondRight;

    public bool condition = false;

    [SerializeField]
    private GameObject spriteOn;
    [SerializeField]
    private GameObject spriteOff;

    [ContextMenu("SwitchCondition")]

    private void SwitchCondition()
    {
        condition = !condition;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        spriteOn.SetActive(condition);
        spriteOff.SetActive(!condition);
    }
    public bool CheckCondition()
    {
        return condition;
    }

    private void CheckConnection(GameObject pipe, int direction)
    {
        if (pipe.GetComponent<PipeController>().CheckCondition())
        {
            if (bondUp && pipe.GetComponent<PipeController>().bondDown && direction == 0)
            {
                SwitchCondition();
                return;
            }

            if (bondRight && pipe.GetComponent<PipeController>().bondLeft && direction == 1)
            {
                SwitchCondition();
                return;
            }

            if (bondDown && pipe.GetComponent<PipeController>().bondUp && direction == 2)
            {
                SwitchCondition();
                return;
            }

            if (bondLeft && pipe.GetComponent<PipeController>().bondRight && direction == 3)
            {
                SwitchCondition();
                return;
            }

            
            
        }
    }

}
