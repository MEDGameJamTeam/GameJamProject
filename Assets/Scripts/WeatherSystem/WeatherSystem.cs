using UnityEngine;

namespace WeatherSystem
{
    public class WeatherSystem : MonoBehaviour
    {
        public static WeatherSystem Weather { get; private set; }
        public Vector3 WindDirection { get; private set; }
        public float WindStrenght { get; private set; }

        // Start is called before the first frame update
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}