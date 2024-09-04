using System;
using System.Collections;
using System.Collections.Generic;
using MobaPrototype.Skills;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace MobaPrototype.SkillEntity
{
    public class TargetSkillPreviewer : MonoBehaviour
    {
        [SerializeField] private LayerMask targetLayerMask;
        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private LineRenderer lineRenderer;
        private Subject<(ITargetAble target, Vector3 direction)> _onCastingSkillToTarget = new();

        private ITargetAble currentTarget;
        private Camera mainCamera;
        private Vector3[] positions = new[] {Vector3.zero, Vector3.zero};
    
        public bool Enable { set => gameObject.SetActive(value); }
        public IObservable<(ITargetAble target, Vector3 direction)> OnCastingSkillToTarget => _onCastingSkillToTarget;
        
        private void Start()
        {
            mainCamera = Camera.main;
        }
    
        private void Update()
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            UpdateCastingTarget(ray);
            UpdateLineRenderer(ray);
        }

        private void UpdateLineRenderer(Ray ray)
        {
            if (Physics.Raycast(ray.origin, ray.direction * 1000.0f, out var rayCastHit))
            {
                positions[0] = transform.position;
                positions[1] = ray.origin + ray.direction * rayCastHit.distance;
                positions[1].y = positions[0].y;
                lineRenderer.SetPositions(positions);
            }
        }

        private void UpdateCastingTarget(Ray ray)
        {
            if (Physics.Raycast(ray.origin, ray.direction * 1000.0f, out var rayCastHit))
            {
                if (rayCastHit.collider.TryGetComponent<ITargetAble>(out var targetAble))
                {
                    UpdateCastingTarget(targetAble);
                    if (Input.GetMouseButtonUp(0))
                    {
                        _onCastingSkillToTarget.OnNext((currentTarget, (positions[1] - positions[0]).normalized));
                    }
                }
                else
                {
                    UpdateCastingTarget((ITargetAble)default);
                }
            }
            else
            {
                UpdateCastingTarget((ITargetAble)default);
            }
        }

        private void UpdateCastingTarget(ITargetAble target)
        {
            if (target == currentTarget) return; 
            target?.HighLight(true);
            currentTarget?.HighLight(false);
            currentTarget = target;
        }
    }
}