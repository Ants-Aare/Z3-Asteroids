using System;
using UnityEngine;

namespace Asteroids.Views
{
    public class LaserTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            // var hits = Physics.SphereCastAll(transform.position, laserWidth, transform.forward, 10000, layerMask);
            // foreach (var hit in hits)
            // {
            var hittable = other.GetComponent<IHittable>();
            hittable?.Hit();
            // }
        }
    }
}