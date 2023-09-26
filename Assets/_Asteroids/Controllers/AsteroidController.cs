using System;
using AAA.Extensions;
using AAA.GlobalVariables.Variables;
using AAA.Utility;
using Asteroids.Views;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Asteroids.Controllers
{
    public class AsteroidController : MonoBehaviour
    {
        [SerializeField] private PrefabPool asteroidPool;

        [SerializeField] private float spawnRadius;
        [SerializeField] private int targetAsteroidAmount;
        [SerializeField] private IntVariable currentAsteroidAmount;

        public void Start()
        {
            for (var i = 0; i < targetAsteroidAmount; i++)
            {
                SpawnNewRandomizedAsteroid();
            }

            currentAsteroidAmount.OnChanged += OnAsteroidAmountChanged;
        }

        private void OnDestroy()
        {
            currentAsteroidAmount.OnChanged -= OnAsteroidAmountChanged;
        }

        private void OnAsteroidAmountChanged()
        {
            Debug.Log("onchanged");

            if (currentAsteroidAmount > targetAsteroidAmount)
                return;
            
            Debug.Log("Spawning");
            for (var i = 0; i < targetAsteroidAmount - currentAsteroidAmount; i++)
            {
                SpawnNewRandomizedAsteroid();
            }
        }

        private void SpawnNewRandomizedAsteroid()
        {
            var instance = asteroidPool.GetRandomInstanceFromPool();
            instance.transform.position = Random.insideUnitCircle.ToVector3XZ() * spawnRadius;
        }
    }
}