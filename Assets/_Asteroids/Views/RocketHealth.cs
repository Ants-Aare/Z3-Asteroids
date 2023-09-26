using System;
using AAA.GlobalVariables.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace Asteroids.Views
{
    public class RocketHealth : MonoBehaviour, IHittable
    {
        [SerializeField] private IntRangeReference health;

        [SerializeField] private UnityEvent onHit;
        [SerializeField] private UnityEvent onDeath;

        private void Start()
        {
            health.Variable.OnChanged += OnHealthChanged;
        }

        private void OnDestroy()
        {
            health.Variable.OnChanged -= OnHealthChanged;
        }

        public void Hit() => health.Value.Value--;

        private void OnHealthChanged()
        {
            onHit?.Invoke();
            
            if (health.Value.Value <= 0)
            {
                onDeath?.Invoke();
            }
        }
    }
}