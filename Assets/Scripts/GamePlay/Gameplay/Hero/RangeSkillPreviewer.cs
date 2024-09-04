using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace MobaPrototype.Hero
{
    public class RangeSkillPreviewer : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private LineRenderer lineRenderer;
        private Subject<Vector3> castingDirection = new(); 
        private Vector3[] positions = new[] {Vector3.zero, Vector3.zero};
        private float range;
        private Camera mainCamera;
        
        public IObservable<Vector3> CastingDirection => castingDirection;

        private void Start()
        {
            mainCamera = Camera.main;
        }

        public bool Enable
        {
            set => gameObject.SetActive(value);
        }
        
        public float Range
        {
            set => range = value / 100;
        }

        private void Update()
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction * 1000.0f, out var rayCastHit))
            {
                positions[0] = transform.position;
                var point = ray.origin + ray.direction * rayCastHit.distance;
                point.y = positions[0].y;
                positions[1] = positions[0] + (point - positions[0]).normalized * range;
                lineRenderer.SetPositions(positions);
            }

            if (Input.GetMouseButtonUp(0))
            {
                var direction = (positions[1] - positions[0]).normalized;
                castingDirection.OnNext(direction);
            }
        }
    }
}