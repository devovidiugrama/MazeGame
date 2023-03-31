using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Visuals
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera cineMachineVirtualCamera;
        [Range(10, 100)] [SerializeField] private int moveSpeed = 10;
        [Range(1, 10)] [SerializeField] private int dragPanSpeed = 2;
        [Range(50, 200)] [SerializeField] private int rotateSpeed = 50;
        [Range(5, 50)] [SerializeField] private int zoomSpeed = 5;

        private float targetFieldOfView = 50;
        
        Vector3 inputDir = Vector3.zero;
        private Vector2 lastMousePosition;
        private bool dragPanActive;

        private void Update()
        {
            Move();
            Rotate();
            Zoom();
        }
        
        private void Move()
        {
            inputDir = Vector3.zero;
            
            if (Keyboard.current[Key.W].isPressed)
            {
                inputDir.z += 1;
            }
            if (Keyboard.current[Key.S].isPressed)
            {
                inputDir.z -= 1;
            }
            if (Keyboard.current[Key.A].isPressed)
            {
                inputDir.x -= 1;
            }
            if (Keyboard.current[Key.D].isPressed)
            {
                inputDir.x += 1;
            }
            
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                lastMousePosition = Mouse.current.position.ReadValue();
                dragPanActive = true;
            }

            if (Mouse.current.rightButton.wasReleasedThisFrame)
            {
                lastMousePosition = Mouse.current.position.ReadValue();
                dragPanActive = false;
            }

            if (dragPanActive)
            {
                Vector2 mouseMoveDelta = Mouse.current.position.ReadValue() - lastMousePosition;
                inputDir.x = -mouseMoveDelta.x * dragPanSpeed * Time.deltaTime;
                inputDir.z = -mouseMoveDelta.y * dragPanSpeed * Time.deltaTime;
            }

            Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }

        private void Rotate()
        {
            float rotateDirection = 0;
            if (Keyboard.current[Key.Q].isPressed)
            {
                rotateDirection = 1;
            }
            if (Keyboard.current[Key.E].isPressed)
            {
                rotateDirection = -1;
            }

            transform.eulerAngles += new Vector3(0, rotateDirection * rotateSpeed * Time.deltaTime, 0);
        }
        
        private void Zoom()
        {
            if (Mouse.current.scroll.ReadValue().y < 0)
            {
                targetFieldOfView += 5;
            }

            if (Mouse.current.scroll.ReadValue().y > 0)
            {
                targetFieldOfView -= 5;
            }

            targetFieldOfView = Mathf.Clamp(targetFieldOfView, 10, 50);

            if (cineMachineVirtualCamera != null)
            {
                cineMachineVirtualCamera.m_Lens.FieldOfView = Mathf.Lerp(cineMachineVirtualCamera.m_Lens.FieldOfView, targetFieldOfView, Time.deltaTime * zoomSpeed);
            }
        }
    }
}
