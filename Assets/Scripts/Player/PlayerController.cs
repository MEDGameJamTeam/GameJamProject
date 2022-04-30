using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(NavmeshMaster))]
public class PlayerController : MonoBehaviour
{
    private Camera cam;
    private NavMeshAgent agent;
    private Rigidbody egg;
    public AudioClip[] footSteps;
    private AudioSource aus;
    public float maxFlickDistance;
    public float minFlickForce;
    public float maxFlickForce;

    private Animator anim;

    private NavmeshMaster navmas;
    // Start is called before the first frame update
    void Start()
    {
        aus = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        cam = FindObjectOfType<Camera>();
        egg = GameObject.FindGameObjectWithTag("Egg").GetComponent<Rigidbody>();
        navmas = GetComponent<NavmeshMaster>();
        anim = GetComponent<Animator>();
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

        if(Input.GetKeyDown("space") && !navmas.flicking)
        {            
            if (Vector3.Distance(transform.position, egg.transform.position) <= maxFlickDistance )
            {
                Vector3 v = new Vector3(egg.transform.position.x - transform.position.x, 2, egg.transform.position.z - transform.position.z);
                StartCoroutine(flickEgg(v));
            }
        }
        if (!aus.isPlaying && navmas.moving)
        {
            aus.clip = footSteps[1];
            aus.Play();
        }
    }


    IEnumerator flickEgg(Vector3 dir)
    {
        float flickForce = minFlickForce;
        transform.LookAt(new Vector3(egg.transform.position.x, 0, egg.transform.position.z));
        navmas.flicking = true;
        navmas.stop();
        agent.updateRotation = false;
        agent.velocity = Vector3.zero;
        anim.SetTrigger("flick");
        while(!anim.GetCurrentAnimatorStateInfo(0).IsName("flick")){yield return null;}
        yield return new WaitForSeconds(.3f);
        anim.speed = 0;
        while(Input.GetButton("Jump"))
        {
            if (flickForce < maxFlickForce)
            {
                flickForce += .5f;
            }
            yield return null;
        }
        anim.speed = 3;
        egg.AddForce(dir.normalized * flickForce);
        yield return new WaitForSeconds(.5f);
        anim.speed = 1;
        navmas.flicking = false;
        agent.updateRotation = true;
        yield return null;
    }
}
