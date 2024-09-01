using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UIView
{
    public class UIViewButtonExtension : Button
    {
        [SerializeField] private UnityEvent onHoverEvent = new();
        [SerializeField] private UnityEvent onHoverExitEvent = new();
        
        public UnityEvent OnHoverEvent
        {
            get => onHoverEvent;
            set => onHoverEvent = value;
        }
        
        public UnityEvent OnHoverExitEvent
        {
            get => onHoverExitEvent;
            set => onHoverExitEvent = value;
        }
        
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            onHoverEvent.Invoke();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            onHoverExitEvent.Invoke();
        }
    }
}