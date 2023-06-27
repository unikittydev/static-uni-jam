using UnityEngine;

public class SwitchController : MonoBehaviour
{
    [SerializeField]
    private LampController[] lamps;

    [SerializeField]
    private GameObject onModel, offModel;
    
    private bool switchEnabled;
    
    void OnMouseDown()
    {
        foreach (var l in lamps)
            l.SwitchCondition();

        switchEnabled = !switchEnabled;
        onModel.SetActive(switchEnabled);
        offModel.SetActive(!switchEnabled);
        
        CheckWinLamps.instance.CheckWin();
    }
}
