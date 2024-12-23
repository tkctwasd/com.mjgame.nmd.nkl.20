using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJGame
{

    public class CameraHandle : TickBehaviour
    {
        public float panSpeed = 0.015f;
        public float smoothTime = 0.1f;

        private Vector3 lastMousePosition;
        private Vector3 targetPosition;
        private Vector3 velocity = Vector3.zero;

        private void Start()
        {
            targetPosition = transform.position;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (Input.GetMouseButtonDown(0))
            {
                lastMousePosition = Input.mousePosition;
            }
            if (Input.GetMouseButton(0))
            {
                Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
                lastMousePosition = Input.mousePosition;

                Vector3 move = new Vector3(-mouseDelta.x * panSpeed, -mouseDelta.y * panSpeed, 0);
                targetPosition += move;
            }

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}