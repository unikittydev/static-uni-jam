using Game;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeController : MonoBehaviour
{

    public Collision2D collision;

    [SerializeField]
    private SpriteRenderer sprite;

    [SerializeField]
    private float charge = 0.0f;

    public float Charge => charge;

    private float maxCharge = 1.0f;
    private float minCharge = -1.0f;

    [SerializeField]
    private float changeAmount = 0.5f;

    public float ChangeAmount => changeAmount;

    private bool canChange = true;

    public bool CanChange => canChange;

    private bool canMove = true;

    public bool CanMove => canMove;

    private bool needForWin = false;

    public bool NeedForWin => needForWin;

    [SerializeField]
    private float winAmountDown = 0.0f;

    [SerializeField]
    private float winAmountUp = 0.0f;

    private bool canWin = false;

    public bool CanWin => canWin;

    private void Start()
    {
        charge = Mathf.Max(minCharge, Mathf.Min(charge, maxCharge));
        sprite.color = new Color(Mathf.Abs(charge + 1) / 2, Mathf.Abs(Mathf.Abs(charge) - 1) / 2, Mathf.Abs(charge - 1) / 2);
    }

    public void ChangeCharge(float change)
    {
        if (!canChange) return;

        charge = Mathf.Max(minCharge, Mathf.Min(charge + change * Time.deltaTime, maxCharge));
        sprite.color = new Color(Mathf.Abs(charge + 1) / 2, Mathf.Abs(Mathf.Abs(charge) - 1) / 2, Mathf.Abs(charge - 1) / 2);

        if (needForWin)
        {
            canWin = charge <= winAmountUp && charge >= winAmountDown;
            SlidergameController.instance.CheckWin();
        }
    }

    void OnMouseDown()
    {
        if (!canMove) return;
        SlidergameController.instance.ChangeAllMaxPoints();
    }
    void OnMouseDrag()
    {
        if (!canMove) return;
        transform.parent.gameObject.GetComponent<SliderController>().ChangePositions();
    }
    void OnMouseUp()
    {
        if (!canMove) return;
    }
}
