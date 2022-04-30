using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    
    private CanvasGroup fadePanel;
    void Start()
    {
        fadePanel = GameObject.Find("FadePanel").GetComponent<CanvasGroup>();
    }
    
    // Start is called before the first frame update
    public void PlayGame()
    {
        print("play");
        StartCoroutine(FadeScreen(1));
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }


    public void Quit()
    {
        StartCoroutine(FadeScreen(0));
    }

    public void Back()
    {
        
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
