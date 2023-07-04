using Game;
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
    private float changeAmount = 0.01f;

    public float ChangeAmount => changeAmount;

    private void Start()
    {
        charge = Mathf.Max(minCharge, Mathf.Min(charge, maxCharge));
        sprite.color = new Color(Mathf.Abs(charge + 1) / 2, Mathf.Abs(Mathf.Abs(charge) - 1) / 2, Mathf.Abs(charge - 1) / 2);
    }

    public void ChangeCharge(float change)
    {
        charge = Mathf.Max(minCharge, Mathf.Min(charge + change, maxCharge));
        
        sprite.color = new Color(Mathf.Abs(charge + 1) / 2, Mathf.Abs(Mathf.Abs(charge) - 1) / 2, Mathf.Abs(charge - 1) / 2);
    }

    void OnMouseDown()
    {
        SlidergameController.instance.ChangeAllMaxPoints();
    }
    void OnMouseDrag()
    {
        transform.parent.gameObject.GetComponent<SliderController>().ChangePositions();
    }
    void OnMouseUp()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enter");
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Exit");
    }
}
