using Game;
using UnityEngine;

public class CheckWinLamps : PuzzleBase
{
    [SerializeField]
    private GameObject[] lamps;

    public static CheckWinLamps instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    public void CheckWin()
    {
        foreach (var item in lamps)
            if (!item.GetComponent<LampController>().CheckCondition())
            {
                CancelWin();
                return;
            }

        TryWin();
    }
}
