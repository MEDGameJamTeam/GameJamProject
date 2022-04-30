using System;
using UnityEngine;

namespace EnvironmentSystems
{
    public class WeatherSystem : MonoBehaviour
    {
        public enum Snowfall
        {
            None,
            Slight,
            Medium,
            Heavy
        }

        public enum WindTypes
        {
            Still,
            Breeze,
            Strong,
            Hurricane
        }

        [SerializeField] private ParticleSystem snowParticleSystem;

        public static WeatherSystem Weather { get; private set; }

        public Vector3 WindDirection { get; set; }
        public float WindStrength { get; private set; }
        public WindTypes CurrentWindType { get; private set; }
        public Snowfall CurrentSnowfall { get; private set; }


        private void UpdateWindStrength()
        {
            var forceOverLifetimeModule = snowParticleSystem.forceOverLifetime;
            var normalizedWindDirection = WindDirection.normalized;
            var particleMultiplier = 3;

            switch (CurrentWindType)
            {
                case WindTypes.Still:
                    forceOverLifetimeModule.enabled = false;
                    WindStrength = 1;
                    break;
                case WindTypes.Breeze:
                    WindStrength = 1.2f;
                    forceOverLifetimeModule.enabled = true;
                    forceOverLifetimeModule.xMultiplier = normalizedWindDirection.x * WindStrength * particleMultiplier;
                    forceOverLifetimeModule.zMultiplier = normalizedWindDirection.z * WindStrength * particleMultiplier;
                    break;
                case WindTypes.Strong:
                    WindStrength = 2.0f;
                    forceOverLifetimeModule.enabled = true;
                    forceOverLifetimeModule.xMultiplier = normalizedWindDirection.x * WindStrength * particleMultiplier;
                    forceOverLifetimeModule.zMultiplier = normalizedWindDirection.z * WindStrength * particleMultiplier;
                    break;
                case WindTypes.Hurricane:
                    WindStrength = 5.5f;
                    forceOverLifetimeModule.enabled = true;
                    forceOverLifetimeModule.xMultiplier = normalizedWindDirection.x * WindStrength * particleMultiplier;
                    forceOverLifetimeModule.zMultiplier = normalizedWindDirection.z * WindStrength * particleMultiplier;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateSnowfallAmount()
        {
            var emissionModule = snowParticleSystem.emission;

            switch (CurrentSnowfall)
            {
                case Snowfall.None:
                    emissionModule.enabled = false;
                    break;
                case Snowfall.Slight:
                    emissionModule.enabled = true;
                    emissionModule.rateOverTimeMultiplier = 5;
                    break;
                case Snowfall.Medium:
                    emissionModule.enabled = true;
                    emissionModule.rateOverTimeMultiplier = 12;
                    break;
                case Snowfall.Heavy:
                    emissionModule.enabled = true;
                    emissionModule.rateOverTimeMultiplier = 24;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #region UnityEventFunctions

        private void Awake()
        {
            // If there is an instance, and it's not me, delete myself.
            if (Weather != null && Weather != this)
                Destroy(this);
            else
                Weather = this;
        }

        private void Start()
        {
            CurrentWindType = WindTypes.Hurricane;
            CurrentSnowfall = Snowfall.Heavy;
        }

        // Update is called once per frame
        private void Update()
        {
            UpdateWindStrength();
            UpdateSnowfallAmount();
        }

        #endregion
    }
}