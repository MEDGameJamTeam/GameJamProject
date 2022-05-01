using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryDisplay : MonoBehaviour
{
    public Story story;
    public TextMeshProUGUI storyText;

    public Image background;

    public AudioSource playerAudioSource;
    public GameObject player;
    private float _time;
    private bool _timerStart;
    private int _index = 0;
    private bool _done = false;

    [SerializeField] private GameObject removeThings;
   

    public bool startSequence;

    void Start()
    {
        background.sprite = story.backgroundImage[0];
    }

    private void Update()
    {
        if (startSequence)
        {
            StartSequence();
            startSequence = false;
        }
        

        if (_timerStart)
        {
            _time += Time.deltaTime;

            if (_index < story.storyMessage.Length-1)
            {
                if (_time >= story.storyVoiceOver[_index].length)
                {
                    _index++;
                    _timerStart = false;
                    ShowStory(_index);
                }
            }
            else
            {
                if (_time >= story.storyVoiceOver[_index].length)
                {
                    _timerStart = false;
                    _done = true;
                    removeThings.SetActive(false);
                }
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        _index = 0;
        if (other.name == player.name)
        {
            ShowStory(_index);
        }
    }

    void ShowStory(int i)
    {
        _time = 0;
        removeThings.SetActive(true);
        storyText.text = story.storyMessage[i];
        background.sprite = story.backgroundImage[i];
        playerAudioSource.clip = story.storyVoiceOver[i];
        playerAudioSource.Play();
        _timerStart = true;
    }

    private void StartSequence()
    {
        _index = 0;
        ShowStory(_index);
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.name == player.name)
        {
            _timerStart = false;
            removeThings.SetActive(false);
            _index = 0;
        }
    }

    public void SetStartSequence(bool start)
    {
        startSequence = start;
    }

    public bool GetDone()
    {
        return _done;
    }
    
    
}