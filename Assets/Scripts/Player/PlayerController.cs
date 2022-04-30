using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(NavmeshMaster))]
public class PlayerController : MonoBehaviour
{
    public Camera cam;
    private NavMeshAgent agent;
    private NavmeshMaster navmas;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        navmas = GetComponent<NavmeshMaster>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition),out hit))
            {
                navmas.setDestination(hit.point);
            }             
        }
    }
}
