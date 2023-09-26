using System;
using AAA.Editor.Runtime.Attributes;
using AAA.Utility.CustomUnityEvents;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Asteroids.Views
{
    public class LaserView : MonoBehaviour
    {
        [SerializeField, Self] private Rigidbody rbody;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float firingChargeTime;
        [SerializeField] private float laserWidth;
        [SerializeField] private float knockBackStrength;

        [SerializeField] private UnityEvent firedStarted;
        [SerializeField] private UnityEvent chargeReady;
        [SerializeField] private UnityEvent firedCompleted;
        [SerializeField] private UnityEvent firedCancelled;

        [ShowNonSerializedField, ReadOnly] private bool isFiring;
        [ShowNonSerializedField, ReadOnly] private bool hasIndicatedReady;
        [ShowNonSerializedField, ReadOnly] private float chargeTimeElapsed;


        public void FixedUpdate()
        {
            HandleFireInput();
        }

        private void HandleFireInput()
        {
            if (isFiring)
            {
                if (chargeTimeElapsed == 0)
                    firedStarted?.Invoke();
                
                chargeTimeElapsed += Time.fixedDeltaTime;
                if(chargeTimeElapsed >= firingChargeTime && !hasIndicatedReady)
                {
                    chargeReady?.Invoke();
                    hasIndicatedReady = true;
                }
            }
            else
            {
                if (chargeTimeElapsed >= firingChargeTime)
                {
                    OnFired();
                    isFiring = false;
                    chargeTimeElapsed = 0;

                    firedCompleted?.Invoke();
                }
                else if (chargeTimeElapsed != 0)
                {
                    chargeTimeElapsed = 0;
                    firedCancelled?.Invoke();
                }

                hasIndicatedReady = false;
            }
        }
        
        private void OnFired()
        {
            rbody.AddForce(transform.forward * (-1 * knockBackStrength), ForceMode.Impulse);
            //
            // var hits = Physics.SphereCastAll(transform.position, laserWidth, transform.forward, 10000, layerMask);
            // foreach (var hit in hits)
            // {
            //     var hittable = hit.collider.GetComponent<IHittable>();
            //     hittable?.Hit();
            // }
        }
        
        // Gets called by input system 
        public void Fire(InputAction.CallbackContext context)
        {
            isFiring = context.started || context.performed;
        }
    }
}