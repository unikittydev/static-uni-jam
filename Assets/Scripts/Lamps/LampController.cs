using UnityEngine;

public class LampController : MonoBehaviour
{
    [SerializeField]
    private bool condition;

    [SerializeField]
    private GameObject spriteOn;

    private void Start()
    {
        UpdateSprite();
    }

    private void OnValidate()
    {
        SwitchCondition();
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
