using UnityEngine;

namespace EnvironmentSystems
{
    public class WeatherTrigger : MonoBehaviour
    {
        [SerializeField] private WeatherSystem.WindTypes targetWindType;
        [SerializeField] private WeatherSystem.SnowfallType targetSnowfallType;
        [SerializeField] private Vector3 targetWindDirection;
        [SerializeField] private float targetWindStrength;
        [SerializeField] private float targetFogDensity;
        
        private Collider _playerCollider;
        private WeatherSystem _weatherSystem;

        private void Start()
        {
            _playerCollider = FindObjectOfType<PlayerController>().GetComponent<Collider>();
            _weatherSystem = WeatherSystem.Weather;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Hit by " + other.name);
            if (other != _playerCollider) return;
            
            _weatherSystem.CurrentWindType = targetWindType;
            _weatherSystem.CurrentSnowfallType = targetSnowfallType;
            _weatherSystem.WindDirection = targetWindDirection;
            _weatherSystem.WindStrength = targetWindStrength;
            RenderSettings.fogDensity = targetFogDensity;
            gameObject.SetActive(false);
        }
    }
}