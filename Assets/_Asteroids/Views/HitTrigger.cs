using UnityEngine;
using UnityEngine.Events;

namespace Asteroids.Views
{
    public class HitTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var hittable = other.GetComponent<IHittable>();
            hittable?.Hit();
        }
    }
}