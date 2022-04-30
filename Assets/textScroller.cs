using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textScroller : MonoBehaviour
{
    public RectTransform text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate() 
    {
        text.position += new Vector3(0,1,0);
    }
}
