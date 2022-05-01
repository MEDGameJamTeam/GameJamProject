using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class textScroller : MonoBehaviour
{
    public RectTransform[] text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate() 
    {
        foreach (RectTransform r in text)
        {
            r.position += new Vector3(0,1,0);
        }

        if (text[text.Length-1].position.y > 450)
        {
            SceneManager.LoadScene(0);
        }
    }
}
