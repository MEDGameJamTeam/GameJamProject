using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        private List<GameObject> _snowfallParticleInstances;

        private ParticleSystem _currentSnowfallParticleSystem;

        #endregion

        #region Public Variables

        public Vector3 WindDirection { get; set; }

        public float WindStrength { get; private set; }


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
            switch (CurrentSnowfallType)
            {
                case SnowfallType.None:
                    break;
                case SnowfallType.Slight:
                    StartCoroutine(SwapActiveSnowfallParticleInstance(_snowfallParticleInstances[0]));
                    break;
                case SnowfallType.Medium:
                    StartCoroutine(SwapActiveSnowfallParticleInstance(_snowfallParticleInstances[1]));
                    break;
                case SnowfallType.Heavy:
                    if (CurrentWindType == WindTypes.Hurricane)
                        StartCoroutine(SwapActiveSnowfallParticleInstance(_snowfallParticleInstances[3]));
                    else
                        StartCoroutine(SwapActiveSnowfallParticleInstance(_snowfallParticleInstances[2]));
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///     Swaps the current and new particles for a smooth transition.
        /// </summary>
        /// <param name="newSnowfallParticlesGameObject"></param>
        private IEnumerator SwapActiveSnowfallParticleInstance(GameObject newSnowfallParticlesGameObject)
        {
            var activeSnowfallParticleInstances = _snowfallParticleInstances
                .Where(snowfallParticleInstance => snowfallParticleInstance.activeInHierarchy).ToList();

            _currentSnowfallParticleSystem = newSnowfallParticlesGameObject.GetComponent<ParticleSystem>();

            newSnowfallParticlesGameObject.SetActive(true);

            yield return new WaitForSeconds(5);

            foreach (var activeSnowfallParticleInstance in activeSnowfallParticleInstances)
                activeSnowfallParticleInstance.SetActive(false);
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
            _snowfallParticleInstances = new List<GameObject>();

            foreach (var prefab in snowParticlePrefabs) _snowfallParticleInstances.Add(Instantiate(prefab, transform));

            foreach (var snowParticleChild in _snowfallParticleInstances) snowParticleChild.SetActive(false);
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