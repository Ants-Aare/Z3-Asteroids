using System;
using AAA.Extensions;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Asteroids.Views
{
    public class ScreenLoopPosition : MonoBehaviour
    {
        [SerializeField, ReadOnly] private Camera cam;
        [SerializeField] private float threshold;
        [SerializeField] private bool syncPhysicsTransforms;

        private void OnValidate()
        {
            if (cam == null)
                cam = Camera.main;
        }

        private void Start()
        {
            if (cam == null)
                cam = Camera.main;
        }

        private void FixedUpdate()
        {
            if (cam == null)
                return;

            var screenPoint = cam.WorldToViewportPoint(transform.position);
            var isOutOfFrustum = false;

            if (screenPoint.x >= 1 + threshold)
            {
                screenPoint.x -= 1 + threshold + threshold;
                isOutOfFrustum = true;
            }
            else if (screenPoint.x < -threshold)
            {
                screenPoint.x += 1 + threshold + threshold;
                isOutOfFrustum = true;
            }

            if (screenPoint.y >= 1 + threshold)
            {
                screenPoint.y -= 1 + threshold + threshold;
                isOutOfFrustum = true;
            }
            else if (screenPoint.y < -threshold)
            {
                screenPoint.y += 1 + threshold + threshold;
                isOutOfFrustum = true;
            }

            if (isOutOfFrustum)
            {
                transform.position = cam.ViewportToWorldPoint(screenPoint);
                if (syncPhysicsTransforms)
                    Physics.SyncTransforms();
            }
        }
    }
}