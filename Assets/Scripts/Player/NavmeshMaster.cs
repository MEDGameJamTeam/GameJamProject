using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EnvironmentSystems;

[RequireComponent(typeof(NavMeshAgent))]
public class NavmeshMaster : MonoBehaviour
{
    private NavMeshAgent agent;
    private WeatherSystem weather;
    private float windResistance;
    private bool moving;
    private bool isCovered;
    public float coverDistance = 3;

    private float startSpeed;
    // Start is called before the first frame update
    void Start()
    {
        weather = WeatherSystem.Weather;
        print(weather);
        agent = GetComponent<NavMeshAgent>();
        startSpeed = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        windResistance = Mathf.Lerp(1f * weather.WindStrength,1f / weather.WindStrength,(Vector3.Angle(weather.WindDirection, agent.velocity)/180));
        agent.speed = startSpeed * windResistance;
        RaycastHit hit;
        Ray ray = new Ray(transform.position, -weather.WindDirection);
        if (Physics.Raycast(ray,out hit, coverDistance))
        {
            isCovered = true;
        }
        else
        {
            isCovered = false;
        }
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    agent.ResetPath();
                    if (!isCovered)
                    {
                        agent.velocity = weather.WindDirection.normalized * (weather.WindStrength - 1);
                    }
                    moving = false;
                }
            }
        };
    }


    public void setDestination(Vector3 des)
    {
        agent.destination = des;
        moving = true;
    }
}
