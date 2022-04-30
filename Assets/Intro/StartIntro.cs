using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartIntro : MonoBehaviour
{
    private StoryDisplay _storyDisplay;
    private CanvasGroup fadePanel;
    void Start()
    {
        fadePanel = GameObject.Find("FadePanel").GetComponent<CanvasGroup>();
        _storyDisplay = GameObject.Find("StoryCollider").GetComponent<StoryDisplay>();
        _storyDisplay.SetStartSequence(true);
    }

    private void Update()
    {
        if (_storyDisplay.GetDone())
        {
            StartCoroutine(FadeScreen(1));
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }
    }
    
    public IEnumerator FadeScreen(int num)
    {
        while(fadePanel.alpha < 1)
        {
            fadePanel.alpha += Time.deltaTime * 4f;
            yield return new WaitForSeconds (0.05f);
        }
        fadePanel.alpha = 1;
        if(num == 1){
            SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if(num == 0){
            Application.Quit();
        }

    }
}
