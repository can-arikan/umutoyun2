using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Text;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public TextMeshProUGUI timeText,bestTimeTime;
    public static float timer;
    public GameObject panel;
    public Text timeTextPanel, bestTimeTextPanel;

    private float score,bestScore,starCount;
    private TimeSpan newTimer2;

    UnityWebRequest Post(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        return request;
    }

    public struct yolla
    {
        public int sessionNum;
    };

    public struct al
    {
        public float best;
    };

    public float getStars()
    {
        return this.starCount;
    }

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    private void Start()
    {
        HighScoreText();
    }

    private void Update()
    {
        if(MyGameManager.instance.gameState == MyGameManager.GameState.Playing)
        {
            newTimer2 = DateTime.Now - MyGameManager.instance.zaman;
            timer = DateToSecond(newTimer2.ToString());
            int minutes = Mathf.FloorToInt(timer / 60F);
            int seconds = Mathf.FloorToInt(timer - minutes * 60);
            string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);

            timeText.text = niceTime;
            score = timer;
            timeTextPanel.text = niceTime; 
        }
    }
    
    private int DateToSecond(string x)
    {
        var hour = x.Substring(0, 2);
        var minute = x.Substring(3, 2);
        var second = x.Substring(6, 2);
        int skor = int.Parse(hour) * 3600 + int.Parse(minute) * 60 + int.Parse(second);
        return skor;
    }

    public float GetScore()
    {
        return score;
    }

    public void SetTimerToZero()
    {
        timer = 0;
    }

    public async void BestTimeChanger()
    {
        yolla Yolla = new yolla();
        Debug.Log(PlayerPrefs.GetInt("GameId"));
        Yolla.sessionNum = PlayerPrefs.GetInt("GameId");
        string yollayici = JsonUtility.ToJson(Yolla);
        var req = Post("https://xcodebackend.herokuapp.com/bestscore",yollayici);
        await req.SendWebRequest();
        var alici = req.downloadHandler.text;
        var dic = Newtonsoft.Json.JsonConvert.DeserializeObject<al>(alici);
        var SCORE = dic.best;
        int minutes = Mathf.FloorToInt(SCORE / 60);
        int seconds = Mathf.FloorToInt(SCORE % 60);
        string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);

        starCount = PlayerController.instance.GetStarCount() + 1;

        bestTimeTime.text = niceTime;


        PlayerPrefs.SetFloat("score", SCORE);
        PlayerPrefs.SetFloat("star", starCount);
        ufff();
    }

    public void ufff()
    {
        bestTimeTextPanel.text = bestTimeTime.text;
    }

    private void HighScoreText()
    {
        int minutes = Mathf.FloorToInt(PlayerPrefs.GetFloat("score") / 60F);
        int seconds = Mathf.FloorToInt(PlayerPrefs.GetFloat("score") - minutes * 60);
        string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);
        bestTimeTime.text = niceTime;
    }

    public void AddStarCount()
    {
        starCount++;
        if (starCount > 5) starCount = 5;
    }

}
