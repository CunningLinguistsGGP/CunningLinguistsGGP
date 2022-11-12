using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    ScoreSystem scoreSystem;

    [SerializeField] int score;
    [SerializeField] List<int> EnemmyKillValues;

    float max_time, min_time;

    int multipler;

    bool timeremaining = false;

    private void Start()
    {
        scoreSystem = GetComponent<ScoreSystem>();
    }

    void Set_ScoreValues(int maxTime,int minTime, int score_Multipler)
    {
        max_time = maxTime;

        min_time = minTime;

        multipler = score_Multipler;

        timeremaining = true;
    }

    private void Update()
    {
        if (timeremaining)
        {
            max_time -= Time.deltaTime;

            if(max_time == 0) timeremaining = false;
        }
    }

    public int Score()
    {
        return score;
    }

    public void ScoreSet(int value)
    {
        score = score + EnemmyKillValues[value];

        Score_Save();
    }

    public void OngameComplete()
    {
        if (max_time <= min_time)
        {
            score = score * multipler;
        }

        Score_Save();
    }

    private void Score_Reset()
    {
        PlayerPrefs.SetInt("HighScore", score);

        score = 0;

        Score_Save();
    }

    private void Score_Save()
    {
        PlayerPrefs.SetInt("Score", score);

        if (PlayerPrefs.GetInt("Score") < score)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }

}
