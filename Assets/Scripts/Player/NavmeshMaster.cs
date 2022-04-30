using EnvironmentSystems;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavmeshMaster : MonoBehaviour
{
    [HideInInspector] public bool flicking;

    public float coverDistance = 3;
    private NavMeshAgent agent;
    private bool isCovered;
    private bool moving;

    private float startSpeed;
    private WeatherSystem weather;

    private float windResistance;

    // Start is called before the first frame update
    private void Start()
    {
        weather = WeatherSystem.Weather;
        agent = GetComponent<NavMeshAgent>();
        startSpeed = agent.speed;
    }

    // Update is called once per frame
    private void Update()
    {
        if (moving)
            GetComponent<Animator>().SetBool("isMoving", true);
        else
            GetComponent<Animator>().SetBool("isMoving", false);
        windResistance = Mathf.Lerp(1f * weather.WindStrength, 1f / weather.WindStrength,
            Vector3.Angle(weather.WindDirection, agent.velocity) / 180);
        
        windResistance = 1;

        agent.speed = startSpeed * windResistance;
        RaycastHit hit;
        var ray = new Ray(transform.position, -weather.WindDirection);
        
        if (Physics.Raycast(ray, out hit, coverDistance))
            isCovered = true;
        else
            isCovered = false;
        if (!agent.pathPending)
            if (agent.remainingDistance < 0.5f)
                moving = false;

        if (!agent.pathPending)
            if (agent.remainingDistance <= agent.stoppingDistance)
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    agent.ResetPath();
                    if (!isCovered && !flicking)
                    {
                        agent.updateRotation = false;
                        agent.velocity = Vector3.Lerp(agent.velocity,
                            weather.WindDirection.normalized * (weather.WindStrength - 1) / 3, 0.01f);
                    }
                }

        ;
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