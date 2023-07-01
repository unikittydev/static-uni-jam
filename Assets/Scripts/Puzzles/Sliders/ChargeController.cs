using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    public Rigidbody2D Rb => rb;

    private bool canMove = true;

    public Collision2D collision;

    [SerializeField]
    private float charge = 0.0f;

    void OnMouseDown()
    {
        //canMove = true;
        //SlidergameController.instance.SetAllDynamic(gameObject);
    }
    void OnMouseDrag()
    {
        //Debug.Log("drag");
        //rb.bodyType = RigidbodyType2D.Dynamic;
        if (canMove) transform.parent.gameObject.GetComponent<SliderController>().ChangePositions();
    }
    void OnMouseUp()
    {
        //rb.bodyType = RigidbodyType2D.Static;
        //SlidergameController.instance.SetAllStatic();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        this.collision = collision;
        //canMove = false;
        //Debug.Log("Enter");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        this.collision = null;
        //canMove = true;
        //Debug.Log("Exit");
    }
}
