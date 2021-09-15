using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundController : MonoBehaviour
{
    public AudioClip jump;

    private AudioSource audioPlayer;
    
    public AudioClip scoreHighlight;

    public void PlayScoreHighlight()
    {
    audioPlayer.PlayOneShot(scoreHighlight);
    }

    private void Start()
    {
    audioPlayer = GetComponent<AudioSource>();
    }

    public void PlayJump()
    {
    audioPlayer.PlayOneShot(jump);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
