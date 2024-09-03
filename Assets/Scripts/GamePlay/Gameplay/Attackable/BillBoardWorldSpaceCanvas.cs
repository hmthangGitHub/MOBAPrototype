using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobaPrototype.Dummy
{
    public class BillBoardWorldSpaceCanvas : MonoBehaviour
    {
        private Camera camera;
        private void Start()
        {
            camera = Camera.main;
        }

        void Update()
        {
            this.transform.forward = camera.transform.forward;
        }
    }
}