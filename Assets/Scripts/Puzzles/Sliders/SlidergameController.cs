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
    public void ChangeAllMaxPoints()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<SliderController>().ChangeMaxPoints();
        }
    }

}
