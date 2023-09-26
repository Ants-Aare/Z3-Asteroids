using AAA.Extensions;
using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;
using AAA.Editor.Runtime.Attributes;
using UnityEngine.Events;

namespace Asteroids.Views
{
    [RequireComponent(typeof(Rigidbody))]
    public class RocketView : MonoBehaviour
    {
        [SerializeField, ReadOnly, Self] private Rigidbody rbody;
        [SerializeField] private Camera cam;

        [SerializeField] private float moveSpeedMultiplier;
        [SerializeField] private float rotationSpeedMultiplier;

        [SerializeField] private UnityEvent onMovingStarted;
        [SerializeField] private UnityEvent onMovingEnded;
        
        [ShowNonSerializedField, ReadOnly] private Vector2 moveInputRaw;
        [ShowNonSerializedField, ReadOnly] private Vector2 lookInputRaw;
        [ShowNonSerializedField, ReadOnly] private Vector3 moveSpeed;
        [ShowNonSerializedField, ReadOnly] private bool isMoving;

        private Quaternion lookRotation;
        private Plane plane;

        private void OnValidate()
        {
            if (cam == null)
                cam = Camera.main;
        }

        private void Start()
        {
            plane = new Plane(Vector3.up, Vector3.zero);
            if (cam == null)
                cam = Camera.main;
        }

        public void FixedUpdate()
        {
            HandleLookInput();

            HandleMovingInput();
        }

        private void HandleLookInput()
        {
            var screenPointToRay = cam.ScreenPointToRay(lookInputRaw.ToVector3XY());
            if (plane.Raycast(screenPointToRay, out var enter))
            {
                var vector3 = screenPointToRay.GetPoint(enter);
                lookRotation = Quaternion.LookRotation(vector3 - transform.position);
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, rotationSpeedMultiplier);
        }

        private void HandleMovingInput()
        {
            if (isMoving)
            {
                moveSpeed = transform.forward.normalized * (Time.fixedDeltaTime * moveSpeedMultiplier);
                rbody.AddForce(moveSpeed, ForceMode.VelocityChange);
            }
        }

        // Gets called by input system 
        public void Move(InputAction.CallbackContext context)
        {
            if (context.performed || context.started)
            {
                // plume.Play();
                Debug.Log("Started");
                onMovingStarted?.Invoke();
            }

            if (context.canceled)
            {
                // plume.Stop();

                onMovingEnded?.Invoke();
            }

            isMoving = context.started || context.performed;
        }
        
        // Gets called by input system 
        public void Look(InputAction.CallbackContext context)
        {
            lookInputRaw = context.ReadValue<Vector2>();
        }
    }
}