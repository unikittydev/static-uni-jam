using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

namespace Game
{
    public class SliderController : MonoBehaviour
    {
        private Vector3 pointCharge;
        public Vector3 PointCharge
        {
            get { return pointCharge; }
            private set { pointCharge = value; }
        }
        [SerializeField]
        private Vector3 pointFirst;
        public Vector3 PointFirst
        {
            get { return pointFirst; }
            private set { pointFirst = value; }
        }
        [SerializeField]
        private Vector3 pointSecond;
        public Vector3 PointSecond
        {
            get { return pointSecond; }
            private set { pointSecond = value; }
        }
        [SerializeField]
        private Vector3 pointMouse;
        public Vector3 PointMouse
        {
            get { return pointMouse; }
            private set { pointMouse = value; }
        }

        private Vector3 pointFirstMax;
        public Vector3 PointFirstMax
        {
            get { return pointFirstMax; }
            private set { pointFirstMax = value; }
        }
        [SerializeField]
        private Vector3 pointSecondMax;
        public Vector3 PointSecondMax
        {
            get { return pointSecondMax; }
            private set { pointSecondMax = value; }
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
            line.gameObject.SetActive(true);
            charge.gameObject.SetActive(true);

            pointFirst = firstPoint.transform.position;
            pointSecond = secondPoint.transform.position;

            pointFirstMax = pointFirst;
            pointSecondMax = pointSecond;

            line.positionCount = 2;
            line.SetPosition(0, PointFirst);
            line.SetPosition(1, PointSecond);

            PointCharge = Vector3.Lerp(pointFirst, pointSecond, 0.5f);
            charge.transform.position = PointCharge;
        }
        
        public void ChangePositions()
        {
            PointMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10));

            if (Mathf.Pow(Vector3.Distance(PointFirstMax, PointMouse), 2) >= Mathf.Pow(Vector3.Distance(PointFirstMax, PointSecondMax), 2) + Mathf.Pow(Vector3.Distance(PointMouse, PointSecondMax), 2)){
                PointCharge = PointSecondMax;
            }
            else if (Mathf.Pow(Vector3.Distance(PointSecondMax, PointMouse), 2) >= Mathf.Pow(Vector3.Distance(PointFirstMax, PointSecondMax), 2) + Mathf.Pow(Vector3.Distance(PointMouse, PointFirstMax), 2))
            {
                PointCharge = PointFirstMax;
            }
            else
            {
                PointCharge = Vector3.Project(PointMouse - PointFirstMax, PointSecondMax - PointFirstMax) + PointFirstMax;
            }
            PointCharge = new Vector3(PointCharge.x, PointCharge.y, 0);
            charge.transform.position = PointCharge;
            FindClosestCharge();
        }

        public void ChangeMaxPoints()
        {

            var firstPointHits = Physics2D.CircleCastAll(charge.transform.position, charge.gameObject.GetComponent<CircleCollider2D>().radius, PointFirst - PointSecond);
            if (firstPointHits.Length > 1)
            {
                PointFirstMax = Vector3.Lerp(2 * (new Vector3(firstPointHits[1].point.x, firstPointHits[1].point.y, 0)) - firstPointHits[1].collider.gameObject.transform.position, PointSecond, 0.01f);
            }
            else
            {
                PointFirstMax = PointFirst;
            }

            var secondPointHits = Physics2D.CircleCastAll(charge.transform.position, charge.gameObject.GetComponent<CircleCollider2D>().radius, PointSecond - PointFirst);

            if (secondPointHits.Length > 1)
            {
                
                PointSecondMax = Vector3.Lerp(2 * (new Vector3(secondPointHits[1].point.x, secondPointHits[1].point.y, 0)) - secondPointHits[1].collider.gameObject.transform.position, PointFirst, 0.01f);
            }
            else
            {
                PointSecondMax = PointSecond;
            }
        }

        public void FindClosestCharge()
        {
            var charges = Physics2D.OverlapCircleAll(charge.transform.position, 0.55f);
            if (charges.Length < 2) return;

            var closestCharge = charges[1].gameObject.GetComponent<ChargeController>();

            if (closestCharge.Charge == charge.Charge || Mathf.Abs(closestCharge.Charge - charge.Charge) < 0.01) return;

            if (closestCharge.Charge > charge.Charge)
            {
                charge.ChangeCharge(charge.ChangeAmount);
                closestCharge.ChangeCharge(-charge.ChangeAmount);
            }
            else
            {
                charge.ChangeCharge(-charge.ChangeAmount);
                closestCharge.ChangeCharge(charge.ChangeAmount);
            }
        }
    }
}