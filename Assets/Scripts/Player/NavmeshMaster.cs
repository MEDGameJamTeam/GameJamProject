using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshMaster : MonoBehaviour
{
    private NavMeshAgent agent;
    private WeatherSystem.WeatherSystem weather;
    private float windResistance;

    private float startSpeed;
    // Start is called before the first frame update
    void Start()
    {
        weather = WeatherSystem.WeatherSystem.Weather;
        agent = GetComponent<NavMeshAgent>();
        startSpeed = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        windResistance = Mathf.Lerp(.5f,1.5f,(Vector3.Angle(weather.WindDirection, agent.velocity)/180));
        agent.speed = startSpeed * (windResistance*weather.WindStrenght);
    }
}
