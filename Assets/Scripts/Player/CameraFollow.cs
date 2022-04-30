using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 targetOffset;
    // Start is called before the first frame update
    void Start()
    {
        targetOffset -= target.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position+targetOffset, 0.001f);
    }
}
