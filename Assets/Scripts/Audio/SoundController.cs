using System.Collections;
using EnvironmentSystems;
using UnityEngine;

namespace Audio
{
    public class SoundController : MonoBehaviour
    {
        public AudioClip[] ambience;
        private WeatherSystem _ws;
        private WeatherSystem.WindTypes _currentWind;

        private AudioSource _breeze, _strong, _hurricane;
        // Start is called before the first frame update
        void Start()
        {
            _breeze = gameObject.AddComponent<AudioSource>();
            _strong = gameObject.AddComponent<AudioSource>();
            _hurricane = gameObject.AddComponent<AudioSource>();
            _breeze.clip = ambience[0];
            _strong.clip = ambience[1];
            _hurricane.clip = ambience[2];
            _breeze.loop = true;
            _strong.loop = true;
            _hurricane.loop = true;
            _breeze.volume = 0;
            _strong.volume = 0;
            _hurricane.volume = 0;
            _breeze.Play();
            _strong.Play();
            _hurricane.Play();
            _ws = WeatherSystem.Weather;
        }

        // Update is called once per frame
        void Update()
        {
            if (_currentWind != _ws.CurrentWindType)
            {
                switch(_ws.CurrentWindType)
                {
                    case WeatherSystem.WindTypes.Still:
                        StartCoroutine(Fadeout(_breeze));
                        StartCoroutine(Fadeout(_strong));
                        StartCoroutine(Fadeout(_hurricane));
                        break;
                    case WeatherSystem.WindTypes.Breeze:
                        StartCoroutine(Fadein(_breeze));
                        StartCoroutine(Fadeout(_strong));
                        StartCoroutine(Fadeout(_hurricane));
                        break;
                    case WeatherSystem.WindTypes.Strong:
                        StartCoroutine(Fadeout(_breeze));
                        StartCoroutine(Fadein(_strong));
                        StartCoroutine(Fadeout(_hurricane));
                        break;
                    case WeatherSystem.WindTypes.Hurricane:
                        StartCoroutine(Fadeout(_breeze));
                        StartCoroutine(Fadeout(_strong));
                        StartCoroutine(Fadein(_hurricane));
                        break;
                
                }
            }
            _currentWind = _ws.CurrentWindType;
        }

        IEnumerator Fadeout(AudioSource aus)
        {
            while (aus.volume > 0.01f)
            {
                aus.volume -= 0.03f;
                yield return null;
            }
            aus.volume = 0f;
            yield return null;
        }

        IEnumerator Fadein(AudioSource aus)
        {
            while (aus.volume < 0.40f)
            {
                aus.volume += 0.001f;
                yield return null;
            }
            aus.volume = 0.4f;
            yield return null;
        }
    }
}
