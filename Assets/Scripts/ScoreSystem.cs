using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using UnityEngine;
using TMPro;
using LootLocker.Requests;

public class ScoreSystem : MonoBehaviour
{
    ScoreSystem scoreSystem;

    string PlayerID;
    string LeaderboardID;
    [SerializeField] TextMeshProUGUI PlayerNames;
    [SerializeField] TextMeshProUGUI PlayersScores;

    [SerializeField] GameObject scoreText;
    [SerializeField] GameObject TimeText;

    [SerializeField] int score;
    [SerializeField] List<int> EnemmyKillValues;

    float max_time, min_time;

    int multipler;

    bool timeremaining = false;

    private void Start()
    {
        scoreSystem = GetComponent<ScoreSystem>();
        StartCoroutine(Setup_LeaderBoard());
        LeaderboardID = "9376";
    }



    public void Set_ScoreValues(int maxTime,int minTime, int score_Multipler)
    {
        max_time = maxTime;

        min_time = minTime;

        multipler = score_Multipler;

        timeremaining = true;

        scoreText.GetComponent<TMP_Text>().text = "";
        TimeText.GetComponent<TMP_Text>().text = "";
    }

    private void Update()
    {
        if (timeremaining)
        {
            max_time -= Time.deltaTime;

            string temp;

            TimeText.GetComponent<TMP_Text>().text = "Time Remaining: " + Mathf.RoundToInt(max_time).ToString();

            temp = "Score : " + Score();

            scoreText.GetComponent<TMP_Text>().text = temp;

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

        StartCoroutine(Submit_Score());

        if (PlayerPrefs.GetInt("Score") < score)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }

    #region leaderboard

    IEnumerator Setup_LeaderBoard()
    {
        yield return StartLoginRequest();
        yield return GetTopHighScores();
    }

    IEnumerator StartLoginRequest()
    {
        bool Status = false;

        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                UnityEngine.Debug.Log("Server Connected");
                PlayerID = response.player_id.ToString();
                PlayerPrefs.SetString("PlayerID", PlayerID);
                Status = true;
            }
            else
            {
                UnityEngine.Debug.Log("Server Not Connected!!!");
                Status = true;
            }

        });

        yield return new WaitWhile(() => Status == false);

    }

    IEnumerator Submit_Score()
    {
        bool Status = false;

        LootLockerSDKManager.SubmitScore(PlayerID, score, LeaderboardID, (response) =>
        {
            if (response.success)
            {
                UnityEngine.Debug.Log("Score Submited!!");
                Status = true;
            }
            else
            {
                UnityEngine.Debug.Log("Score Submission Failed!!");
                Status = true;
            }
        });

        yield return new WaitWhile(() => Status == false);

    }

    IEnumerator GetTopHighScores()
    {
        bool Status = false;

        LootLockerSDKManager.GetScoreList(LeaderboardID, 10, 0, (response) =>
        {
            if (response.success)
            {
                string tempPlayerNames = "Names\n";
                string tempPlayerScores = "Scores\n";

                LootLockerLeaderboardMember[] members = response.items;

                for(int i = 0; i < members.Length; i++)
                {
                    tempPlayerNames += members[i].rank + ". ";
                    if (members[i].player.name != "")
                    {
                        tempPlayerNames += members[i].player.name;
                    }
                    else
                    {
                        tempPlayerNames += members[i].player.id;
                    }
                    tempPlayerScores += members[i].score + "\n";
                    tempPlayerNames += "\n";
                }

                PlayerNames.text = tempPlayerNames;
                PlayersScores.text = tempPlayerScores;
                Status = true;
            }
            else
            {
                UnityEngine.Debug.Log("Learderboard Failed!!");
                Status = true;
            }
        });

        yield return new WaitWhile(() => Status == false);

    }
    #endregion



}
