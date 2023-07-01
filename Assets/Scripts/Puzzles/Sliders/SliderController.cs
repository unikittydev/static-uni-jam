using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class SliderController : MonoBehaviour
    {
        private Vector3 pointCharge = new Vector3(1, -1, 0);
        public Vector3 PointCharge
        {
            get { return pointCharge; }
            private set { pointCharge = value; }
        }
        [SerializeField]
        private Vector3 pointFirst = new Vector3(1, -5, 0);
        public Vector3 PointFirst
        {
            get { return pointFirst; }
            private set { pointFirst = value; }
        }
        [SerializeField]
        private Vector3 pointSecond = new Vector3(5, -1, 0);
        public Vector3 PointSecond
        {
            get { return pointSecond; }
            private set { pointSecond = value; }
        }
        [SerializeField]
        private Vector3 pointMouse = new Vector3(5, -1, 0);
        public Vector3 PointMouse
        {
            get { return pointMouse; }
            private set { pointMouse = value; }
        }

        [SerializeField]
        private LineRenderer line;
        [SerializeField]
        private ChargeController charge;

        [SerializeField]
        private GameObject firstPoint;
        [SerializeField]
        private GameObject secondPoint;

        [ContextMenu("Вектор")]
        private void Start()
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);

            Debug.Log(Vector3.Project(PointCharge - PointFirst, PointSecond - PointFirst) + PointFirst);

            firstPoint.transform.position += new Vector3(0, 0, -2);
            secondPoint.transform.position += new Vector3(0, 0, -2);
            pointFirst = firstPoint.transform.position;
            pointSecond = secondPoint.transform.position;

            line = transform.GetChild(1).GetComponent<LineRenderer>();
            line.positionCount = 2;
            line.SetPosition(0, PointFirst + new Vector3(0, 0, 2));
            line.SetPosition(1, PointSecond + new Vector3(0, 0, 2));

            PointCharge = Vector3.Lerp(pointFirst, pointSecond, 0.5f) + new Vector3(0, 0, -3);
            charge.transform.position = PointCharge;
        }
        
        public void ChangePositions()
        {
            PointMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10));

            if (Mathf.Pow(Vector3.Distance(PointFirst, PointMouse), 2) >= Mathf.Pow(Vector3.Distance(PointFirst, PointSecond), 2) + Mathf.Pow(Vector3.Distance(PointMouse, PointSecond), 2)){
                PointCharge = PointSecond;
            }
            else if (Mathf.Pow(Vector3.Distance(PointSecond, PointMouse), 2) >= Mathf.Pow(Vector3.Distance(PointFirst, PointSecond), 2) + Mathf.Pow(Vector3.Distance(PointMouse, PointFirst), 2))
            {
                PointCharge = PointFirst;
            }
            else
            {
                PointCharge = Vector3.Project(PointMouse - PointFirst, PointSecond - PointFirst) + PointFirst;
            }
            //charge.transform.position = PointCharge;
            
            charge.Rb.MovePosition(PointCharge);
        }
    }
}