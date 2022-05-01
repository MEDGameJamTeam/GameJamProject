using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class slideImages : MonoBehaviour
{
    private Image img;
    private Sprite current;
    private Vector3 translation;
    private RectTransform rt;
    private Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        rt = this.GetComponent<RectTransform>();
        startPos = rt.position;
        img = GetComponent<Image>();
        current = img.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (current != img.sprite)
        {
            translation = new Vector3(Random.Range(-0.04f,0.04f), Random.Range(-0.04f,0.04f), 0);
            rt.position = startPos;
            current = img.sprite;
        }
        rt.position += translation;
    }
}
