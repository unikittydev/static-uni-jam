using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidergameController : MonoBehaviour
{
    public static SlidergameController instance { get; private set; }
    private void Awake()
    {
        instance = this;
    }

    public void SetAllDynamic(GameObject exception)
    {
        foreach(Transform child in transform)
        {
            if (child.GetChild(0).gameObject == exception) continue;
            child.GetChild(0).transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
    }

    public void SetAllStatic()
    {
        foreach (Transform child in transform)
        {
            child.GetChild(0).GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }

}
