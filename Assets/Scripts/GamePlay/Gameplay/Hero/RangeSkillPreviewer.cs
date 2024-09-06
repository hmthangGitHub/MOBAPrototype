using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlasticGui.WorkspaceWindow.QueryViews;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace MobaPrototype.Hero
{
    public class RangeSkillPreviewer : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float projectileAngleOffset;
        [SerializeField] private LineRenderer[] lineRenderer;
        private Subject<(Vector3 heroDirection, Vector3[] projectTileDirection)> castingDirection = new();
        private Vector3[][] lineRendererPositions;
        private Vector3 heroDirection;
        private float range;
        private Camera mainCamera;
        [SerializeField] private int numberOfProjectile = 3;

        public IObservable<(Vector3 heroDirection, Vector3[] projectTileDirection)> CastingDirection => castingDirection;
        
        public bool Enable
        {
            set => gameObject.SetActive(value);
        }
        
        public float Range
        {
            set => range = value / 100;
        }

        public int NumberOfProjectile
        {
            get => numberOfProjectile;
            set
            {
                numberOfProjectile = value;
                for (int i = 0; i < lineRenderer.Length; i++)
                {
                    lineRenderer[i].gameObject.SetActive(i <= numberOfProjectile - 1);
                }
            }
        }

        private void Start()
        {
            mainCamera = Camera.main;
            lineRendererPositions = lineRenderer.Select(x => new[] {Vector3.zero, Vector3.zero}).ToArray();
        }

        private void Update()
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction * 1000.0f, out var rayCastHit))
            {
                var rayCastPoint = CalculateHeroDirectionAndRayCastPoint(ray, rayCastHit);
                for (int i = 0; i < numberOfProjectile; i++)
                {
                    CalculatePositionProjectile(rayCastPoint, i);
                }

                if (Input.GetMouseButtonUp(0))
                {
                    castingDirection.OnNext((heroDirection, lineRendererPositions.Take(numberOfProjectile).Select(x => (x[1] - x[0].normalized)).ToArray()));
                }
            }
        }

        private Vector3 CalculateHeroDirectionAndRayCastPoint(Ray ray, RaycastHit rayCastHit)
        {
            var rayCastPoint = ray.origin + ray.direction * rayCastHit.distance;
            rayCastPoint.y = transform.position.y;
            heroDirection = (rayCastPoint - transform.position).normalized;
            return rayCastPoint;
        }

        private void CalculatePositionProjectile(Vector3 rayCastPoint, int positionIndex)
        {
            lineRendererPositions[positionIndex][0] = transform.position;
            var direction = (rayCastPoint - lineRendererPositions[positionIndex][0]).normalized;
            direction = Quaternion.Euler(new Vector3(0, projectileAngleOffset * (positionIndex - (numberOfProjectile * 0.5f - 0.5f)), 0.0f)) * direction;
            lineRendererPositions[positionIndex][1] = lineRendererPositions[positionIndex][0] + direction * range;
            lineRenderer[positionIndex].SetPositions(lineRendererPositions[positionIndex]);
        }
    }
}