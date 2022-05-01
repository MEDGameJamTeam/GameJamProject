using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class fadeoutending : MonoBehaviour
{
    private float faded = 0.25f;
    public Image img;
    public int wait;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(fade());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator fade()
    {
        yield return new WaitForSeconds(wait);
        while (faded < 1)
        {
            print(faded);
            img.color = new Color(img.color.r, img.color.g, img.color.b, faded);
            faded += .0002f;
            yield return null;
        }
        SceneManager.LoadScene(5);
        yield return null;
    }
}
