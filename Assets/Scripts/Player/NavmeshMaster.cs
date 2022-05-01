using EnvironmentSystems;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavmeshMaster : MonoBehaviour
{
    [HideInInspector] public bool flicking;

    public float coverDistance = 3;

    [HideInInspector] public bool moving;

    private NavMeshAgent _agent;
    private bool _isCovered;

    private float _startSpeed;
    private WeatherSystem _weather;

    private float _windResistance;

    // Start is called before the first frame update
    private void Start()
    {
        _weather = WeatherSystem.Weather;
        _weather.WindDirection = new Vector3(1, 0, 0);
        _agent = GetComponent<NavMeshAgent>();
        _startSpeed = _agent.speed;
    }

    // Update is called once per frame
    private void Update()
    {
        if (moving)
            GetComponent<Animator>().SetBool("isMoving", true);
        else
            GetComponent<Animator>().SetBool("isMoving", false);
        _windResistance = Mathf.Lerp(1f * _weather.WindStrength, 1f / _weather.WindStrength,
            Vector3.Angle(_weather.WindDirection, _agent.velocity) / 180);

        _windResistance = 1;

        _agent.speed = _startSpeed * _windResistance;
        RaycastHit hit;
        var ray = new Ray(transform.position, -_weather.WindDirection);

        if (Physics.Raycast(ray, out hit, coverDistance))
            _isCovered = true;
        else
            _isCovered = false;
        if (!_agent.pathPending)
            if (_agent.remainingDistance < 0.5f)
                moving = false;

        if (_agent.pathPending) return;
        if (!(_agent.remainingDistance <= _agent.stoppingDistance)) return;
        if (_agent.hasPath && _agent.velocity.sqrMagnitude != 0f) return;
        
        _agent.ResetPath();

        if (_isCovered || flicking) return;
        
        _agent.updateRotation = false;
        _agent.velocity = Vector3.Lerp(_agent.velocity,
            _weather.WindDirection.normalized * (_weather.WindStrength - 1) / 3, 0.01f);
    }


    public void SetDestination(Vector3 des)
    {
        _agent.updateRotation = true;
        _agent.destination = des;
        moving = true;
    }

    public void Stop()
    {
        _agent.ResetPath();
    }
}