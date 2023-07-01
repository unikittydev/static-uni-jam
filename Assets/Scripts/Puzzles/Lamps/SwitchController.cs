using Game;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SwitchController : MonoBehaviour
{
    [SerializeField]
    private LampController[] lamps;

    [SerializeField]
    private GameObject onModel, offModel;

    [SerializeField] private UnityEvent switchOn;
    [SerializeField] private UnityEvent switchOff;
    
    private bool switchEnabled;
    
    void OnMouseDown()
    {
        if (GamePause.GamePaused || EventSystem.current.IsPointerOverGameObject())
            return;
        
        foreach (var l in lamps)
            l.SwitchCondition();

        switchEnabled = !switchEnabled;
        onModel.SetActive(switchEnabled);
        offModel.SetActive(!switchEnabled);
        
        if (switchEnabled)
            switchOn?.Invoke();
        else
            switchOff.Invoke();
        
        CheckWinLamps.instance.CheckWin();
    }
}
