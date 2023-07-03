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

    [SerializeField]
    private float changeCharge = 0.01f;

    private void Start()
    {
        sprite.color = new Color(charge, 0, 1 - charge);
    }

    public void ChangeCharge()
    {
        if (charge < 1.0f)
        {
            charge += changeCharge;
        }

        sprite.color = new Color(charge, 0, 1 - charge);
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

    public void PrintCharge()
    {
        Debug.Log(charge);
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
