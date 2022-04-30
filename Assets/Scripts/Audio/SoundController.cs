using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnvironmentSystems;

public class SoundController : MonoBehaviour
{
    public AudioClip[] ambience;
    private WeatherSystem ws;
    private WeatherSystem.WindTypes currentWind;

    private AudioSource breeze, strong, hurricane;
    // Start is called before the first frame update
    void Start()
    {
        breeze = gameObject.AddComponent<AudioSource>();
        strong = gameObject.AddComponent<AudioSource>();
        hurricane = gameObject.AddComponent<AudioSource>();
        breeze.clip = ambience[0];
        strong.clip = ambience[1];
        hurricane.clip = ambience[2];
        breeze.loop = true;
        strong.loop = true;
        hurricane.loop = true;
        breeze.volume = 0;
        strong.volume = 0;
        hurricane.volume = 0;
        breeze.Play();
        strong.Play();
        hurricane.Play();
        ws = WeatherSystem.Weather;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWind != ws.CurrentWindType)
        {
            switch(ws.CurrentWindType)
            {
                case WeatherSystem.WindTypes.Still:
                    StartCoroutine(fadeout(breeze));
                    StartCoroutine(fadeout(strong));
                    StartCoroutine(fadeout(hurricane));
                break;
                case WeatherSystem.WindTypes.Breeze:
                    StartCoroutine(fadein(breeze));
                    StartCoroutine(fadeout(strong));
                    StartCoroutine(fadeout(hurricane));
                break;
                case WeatherSystem.WindTypes.Strong:
                    StartCoroutine(fadeout(breeze));
                    StartCoroutine(fadein(strong));
                    StartCoroutine(fadeout(hurricane));
                break;
                case WeatherSystem.WindTypes.Hurricane:
                    StartCoroutine(fadeout(breeze));
                    StartCoroutine(fadeout(strong));
                    StartCoroutine(fadein(hurricane));
                break;
                
            }
        }
        currentWind = ws.CurrentWindType;
    }

    IEnumerator fadeout(AudioSource aus)
    {
        while (aus.volume > 0.01f)
        {
            aus.volume -= 0.001f;
            yield return null;
        }
        aus.volume = 0f;
        yield return null;
    }

    IEnumerator fadein(AudioSource aus)
    {
        while (aus.volume < 0.99f)
        {
            aus.volume += 0.001f;
            yield return null;
        }
        aus.volume = 1f;
        yield return null;
    }
}
