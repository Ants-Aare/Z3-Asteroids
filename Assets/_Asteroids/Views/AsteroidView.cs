using AAA.Editor.Runtime.Attributes;
using AAA.Extensions;
using AAA.GlobalVariables.Variables;
using AAA.Utility;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace Asteroids.Views
{
    [RequireComponent(typeof(Rigidbody))]
    public class AsteroidView : MonoBehaviour, IHittable
    {
        [ReadOnly] public int currentPhase;

        [SerializeField, ReadOnly, Self] private Rigidbody rbody;
        [SerializeField] private IntReference score;
        [SerializeField] private IntReference currentAsteroidAmount;
        [SerializeField] private int[] childAsteroidPhases;
        [SerializeField] private float[] sizeAsteroidPhases;
        [SerializeField] private float moveSpeed;

        [SerializeField] private PrefabPool asteroidPool;

        [SerializeField] private UnityEvent onHit;

        [ShowNonSerializedField, ReadOnly] private float birthTime;
        private const float MinLifeThreshold = 0.4f;

        public void Initialize()
        {
            birthTime = Time.time;
            currentAsteroidAmount++;
            rbody.AddForce(Random.insideUnitSphere * moveSpeed, ForceMode.VelocityChange);
        }

        public void OnReturn()
        {
            transform.localScale = Vector3.one;
            currentPhase = 0;
            rbody.velocity = Vector3.zero;
            rbody.ResetInertiaTensor();
        }

        [Button]
        public void Hit()
        {
            // Don't destroy if this is a newly created asteroid that is still inside of the laser trigger 
            if (birthTime + MinLifeThreshold > Time.time)
                return;

            var childAsteroidAmount = currentPhase < childAsteroidPhases.Length
                ? childAsteroidPhases[currentPhase]
                : 0;

            var targetPhase = currentPhase + 1;
            for (var i = 0; i < childAsteroidAmount; i++)
            {
                var targetPosition = transform.position + Random.insideUnitSphere;
                var instance = asteroidPool.GetRandomInstanceFromPool();
                var childAsteroidView = instance.GetComponent<AsteroidView>();
                childAsteroidView.currentPhase = targetPhase;
                instance.transform.position = targetPosition;
                instance.transform.localScale = sizeAsteroidPhases.GetClamped(targetPhase).ToVector3XYZ();
            }

            currentAsteroidAmount--;
            score++;

            onHit?.Invoke();
        }
    }
}