using UnityEngine;

public class LampController : MonoBehaviour
{
    [SerializeField]
    private bool condition = true;

    [SerializeField]
    private GameObject spriteOn;

    [SerializeField]
    private GameObject spriteOff;

    [ContextMenu("SwitchCondition")]

    private void Start()
    {
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
        spriteOff.SetActive(!condition);
    }

    public bool CheckCondition()
    {
        return condition;
    }

}
