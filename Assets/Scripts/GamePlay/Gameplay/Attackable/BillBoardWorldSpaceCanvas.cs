using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobaPrototype.Dummy
{
    public class BillBoardWorldSpaceCanvas : MonoBehaviour
    {
        private Camera mainCamera;
        
        private void Start()
        {
            mainCamera = Camera.main;
        }

        void Update()
        {
            this.transform.forward = mainCamera.transform.forward;
        }
    }
}