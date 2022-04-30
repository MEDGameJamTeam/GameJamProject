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
    [HideInInspector]
    public bool moving;
    [HideInInspector]
    public bool flicking;
    private bool isCovered;
    public float coverDistance = 3;

    private float startSpeed;
    // Start is called before the first frame update
    void Start()
    {
        weather = WeatherSystem.Weather;
        weather.WindDirection = new Vector3(1,0,0);
        agent = GetComponent<NavMeshAgent>();
        startSpeed = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if(moving)
        {
            this.GetComponent<Animator>().SetBool("isMoving", true);
        }
        else
        {
            this.GetComponent<Animator>().SetBool("isMoving", false);
        }
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
            if (agent.remainingDistance < 0.5f)
            {
                moving = false;
            }
        }

        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    agent.ResetPath();
                    if (!isCovered && !flicking)
                    {
                        agent.updateRotation = false;
                        agent.velocity = Vector3.Lerp(agent.velocity, weather.WindDirection.normalized * (weather.WindStrength - 1)/3, 0.01f); 
                    }
                }
            }
        };
    }


    public void setDestination(Vector3 des)
    {
        agent.updateRotation = true;
        agent.destination = des;
        moving = true;
    }
    public void stop()
    {
        agent.ResetPath();
    }
}
