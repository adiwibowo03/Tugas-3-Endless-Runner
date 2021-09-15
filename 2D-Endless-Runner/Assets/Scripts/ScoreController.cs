﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [Header("Score Highlight")]
    public int scoreHighlightRange;
    public CharacterSoundController sound;

private int lastScoreHighlight = 0;
    private int currentScore = 0;
    // Start is called before the first frame update
    private void Start()
{
    // reset
    currentScore = 0;
    lastScoreHighlight = 0;
}

public float GetCurrentScore()
{
    return currentScore;
}

public void IncreaseCurrentScore(int increment)
{
    currentScore += increment;

    if (currentScore - lastScoreHighlight > scoreHighlightRange)
    {
        sound.PlayScoreHighlight();
        lastScoreHighlight += scoreHighlightRange;
    }
}

public void FinishScoring()
{
    // set high score
    if (currentScore > ScoreData.highScore)
    {
        ScoreData.highScore = currentScore;
    }
}

    // Update is called once per frame
    void Update()
    {
        
    }
}
