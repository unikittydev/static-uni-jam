using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{

    public class SlidergameController : PuzzleBase
    {
        public static SlidergameController instance { get; private set; }
        private void Awake()
        {
            instance = this;

        }
        public void CheckWin()
        {
            foreach (Transform child in transform)
            {
                try
                {
                    if (!child.GetComponent<ChargeController>().CanWin)
                    {
                        CancelWin();
                        return;
                    }
                }
                catch
                {
                    continue;
                }
                
                
            }
            TryWin();
        }

        public void ChangeAllMaxPoints()
        {
            foreach (Transform child in transform)
            {
                try
                {
                    child.GetComponent<SliderController>().ChangeMaxPoints();
                }
                catch
                {
                    continue;
                }
            }
        }

    }
}