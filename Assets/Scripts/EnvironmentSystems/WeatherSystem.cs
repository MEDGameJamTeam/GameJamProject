using System;
using System.Collections;
using UnityEngine;

namespace EnvironmentSystems
{
    public class WeatherSystem : MonoBehaviour
    {
        #region Static Variables

        public static WeatherSystem Weather { get; private set; }

        #endregion

        #region Public Enums

        public enum SnowfallType
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

        #endregion

        #region Modify Variables

        // Variables used to check for modification
        private WindTypes _mCurrentWindType;
        private SnowfallType _mCurrentSnowfallType;
        private Vector3 _mWindDirection;

        #endregion

        #region Private Variables

        [SerializeField] private GameObject[] snowParticlePrefabs;

        private ParticleSystem _currentSnowfallParticleSystem;

        private float WindStrength { get; set; }

        #endregion

        #region Public Variables

        public Vector3 WindDirection { get; set; }

        [field: SerializeField] public WindTypes CurrentWindType { get; private set; }

        [field: SerializeField] public SnowfallType CurrentSnowfallType { get; private set; }

        #endregion

        #region Private Methods

        private void OnWindTypeChange()
        {
            var forceOverLifetimeModule = _currentSnowfallParticleSystem.forceOverLifetime;

            switch (CurrentWindType)
            {
                case WindTypes.Still:
                    forceOverLifetimeModule.enabled = false;
                    WindStrength = 1;
                    break;
                case WindTypes.Breeze:
                    WindStrength = 1.2f;
                    forceOverLifetimeModule.enabled = true;
                    UpdateParticleSystemToMatchWindDirection();
                    break;
                case WindTypes.Strong:
                    WindStrength = 2.0f;
                    forceOverLifetimeModule.enabled = true;
                    UpdateParticleSystemToMatchWindDirection();
                    break;
                case WindTypes.Hurricane:
                    WindStrength = 5.5f;
                    forceOverLifetimeModule.enabled = true;
                    UpdateParticleSystemToMatchWindDirection();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateParticleSystemToMatchWindDirection()
        {
            var forceOverLifetimeModule = _currentSnowfallParticleSystem.forceOverLifetime;
            var normalizedWindDirection = WindDirection.normalized;
            var particleMultiplier = 3;

            if (!forceOverLifetimeModule.enabled) return;

            forceOverLifetimeModule.xMultiplier = normalizedWindDirection.x * WindStrength * particleMultiplier;
            forceOverLifetimeModule.zMultiplier = normalizedWindDirection.z * WindStrength * particleMultiplier;
        }

        private void OnSnowfallTypeChange()
        {
            var emissionModule = _currentSnowfallParticleSystem.emission;

            switch (CurrentSnowfallType)
            {
                case SnowfallType.None:
                    emissionModule.enabled = false;
                    break;
                case SnowfallType.Slight:
                    emissionModule.enabled = true;
                    emissionModule.rateOverTimeMultiplier = 5;
                    break;
                case SnowfallType.Medium:
                    emissionModule.enabled = true;
                    emissionModule.rateOverTimeMultiplier = 12;
                    break;
                case SnowfallType.Heavy:
                    emissionModule.enabled = true;
                    emissionModule.rateOverTimeMultiplier = 24;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private float GetSnowfallMultiplier()
        {
            switch (CurrentSnowfallType)
            {
                case SnowfallType.None:
                    return 0f;
                case SnowfallType.Slight:
                    return 10f;
                case SnowfallType.Medium:
                    return 20f;
                case SnowfallType.Heavy:
                    return 30f;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private GameObject GetSnowfallParticlePrefab()
        {
            switch (CurrentSnowfallType)
            {
                case SnowfallType.None:
                    return null;
                case SnowfallType.Slight:
                    return snowParticlePrefabs[0];
                case SnowfallType.Medium:
                    return snowParticlePrefabs[1];
                case SnowfallType.Heavy:
                    if (CurrentWindType == WindTypes.Hurricane) return snowParticlePrefabs[3];
                    return snowParticlePrefabs[2];

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IEnumerator SwitchSnowfallParticles(GameObject newSnowfallParticlesGameObject)
        {
            var newSnowfallParticleSystem = newSnowfallParticlesGameObject.GetComponent<ParticleSystem>();

            if (_currentSnowfallParticleSystem == null) _currentSnowfallParticleSystem = newSnowfallParticleSystem;

            if (_currentSnowfallParticleSystem != newSnowfallParticleSystem)
            {
                var currentEmissionModule = _currentSnowfallParticleSystem.emission;
                var newEmissionModule = newSnowfallParticleSystem.emission;

                while (currentEmissionModule.rateOverTimeMultiplier > 0)
                {
                    if (currentEmissionModule.rateOverTimeMultiplier > 0)
                        currentEmissionModule.rateOverTimeMultiplier -= 1;
                    if (newEmissionModule.rateOverTimeMultiplier < GetSnowfallMultiplier())
                        newEmissionModule.rateOverTimeMultiplier -= 1;

                    yield return new WaitForFixedUpdate();
                }

                _currentSnowfallParticleSystem = newSnowfallParticleSystem;
            }
        }

        #endregion

        #region Unity Event Functions

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
            CurrentSnowfallType = SnowfallType.Heavy;

            foreach (var prefab in snowParticlePrefabs)
            {
                Instantiate(prefab, transform);
                var emissionModule = prefab.GetComponent<ParticleSystem>().emission;
                emissionModule.rateOverTimeMultiplier = 0;
                emissionModule.enabled = false;
            }

            StartCoroutine(SwitchSnowfallParticles(GetSnowfallParticlePrefab()));
        }

        private void Update()
        {
            // Checks if variables have been modified since last update
            if (CurrentWindType != _mCurrentWindType)
            {
                _mCurrentWindType = CurrentWindType;
                OnWindTypeChange();
            }

            if (CurrentSnowfallType != _mCurrentSnowfallType)
            {
                _mCurrentSnowfallType = CurrentSnowfallType;
                OnSnowfallTypeChange();
                StartCoroutine(SwitchSnowfallParticles(GetSnowfallParticlePrefab()));
            }

            if (WindDirection != _mWindDirection)
            {
                _mWindDirection = WindDirection;
                UpdateParticleSystemToMatchWindDirection();
            }
        }

        #endregion
    }
}