using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobaPrototype.Hero
{
    public class AOESkillPreviewer : MonoBehaviour
    {
        public float Aoe
        {
            set => this.transform.localScale = Vector3.one * value / 100.0f;
        }

        public bool Enable
        {
            set => gameObject.SetActive(value);
        }
    }
}