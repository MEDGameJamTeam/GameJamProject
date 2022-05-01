using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Story Element", menuName = "Story Element")]
public class Story : ScriptableObject
{
    public string[] storyMessage;
    public AudioClip[] storyVoiceOver;
    public Sprite[] backgroundImage;
    
    //public GameObject textBoxPrefab;

}
