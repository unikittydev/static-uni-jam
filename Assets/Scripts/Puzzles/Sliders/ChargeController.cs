using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    private bool canMove = true;

    void OnMouseDown()
    {
        canMove = true;
        //SlidergameController.instance.SetAllDynamic(gameObject);
    }
    void OnMouseDrag()
    {
        //Debug.Log("drag");
        rb.bodyType = RigidbodyType2D.Dynamic;
        transform.parent.gameObject.GetComponent<SliderController>().ChangePositions();
    }
    void OnMouseUp()
    {
        rb.bodyType = RigidbodyType2D.Static;
        //SlidergameController.instance.SetAllStatic();
    }

    public Rigidbody2D Rb => rb;
}
