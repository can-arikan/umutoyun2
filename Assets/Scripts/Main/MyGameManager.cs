using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Text;

public class MyGameManager : MonoBehaviour
{
    public static MyGameManager instance;

    [HideInInspector] public bool startGame, failed, successed, paused;
    [HideInInspector] public DateTime zaman;
   /* [HideInInspector]*/ public GameState gameState;
    public GameObject successPanel, failPanel, startGamePanel;
    public event Action OnGameStart, OnGamePause, OnGameResume, OnGameFail, OnGameSuccess;

    UnityWebRequest Post(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        return request;
    }
    private struct ReqClass
    {
        public int sessionNum;
        public string wallet;
        public float star;
    };

    public class ScoreBE
    {
        public string resp { get; set; }
        public float score { get; set; }
    }

    public struct GetBest
    {
        public int sessionNum;
    }

    public class GetBest2
    {
        public float best { get; set; }
    }

    private void Awake()
    {
        Application.targetFrameRate = 61;
        if (instance == null) instance = this;
    }

    private void Start()
    {
        ScoreManager.instance.BestTimeChanger();
    }

    public async void StartGame()
    {
        if (!startGame)
        {
            ReqClass reqC = new ReqClass();
            reqC.wallet = PlayerPrefs.GetString("Account");
            reqC.sessionNum = PlayerPrefs.GetInt("GameId");
            var reqJson = JsonUtility.ToJson(reqC);
            var req = Post("https://xcodebackend.herokuapp.com/gameStart", reqJson);
            await req.SendWebRequest();
            string resp = req.downloadHandler.text;
            if (resp == "\"Game has began\"") {
                startGame = true;
                gameState = GameState.Playing;
                if (startGamePanel != null)
                    startGamePanel.SetActive(false);

                PlayerController.instance.StartPlayer();

                zaman = DateTime.Now;

                OnGameStart?.Invoke();
            }
            else if (resp == "\"Session Time was END\"")
            {
                SceneManager.LoadScene(0);
            }

        }
    }

    public void StartGame(float time)
    {
        StartCoroutine(StartIE(time));
    }

    public void Fail()
    {
        if (!failed && !successed)
        {
            successed = true;
            gameState = GameState.Successed;
            successPanel.SetActive(true);
            PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel", 1) + 1);
            ScoreManager.instance.panel.SetActive(true);
            ScoreManager.instance.bestTimeTextPanel.text = ScoreManager.instance.bestTimeTime.text;
            ScoreManager.instance.timeTextPanel.text = ScoreManager.instance.timeText.text;

            ScoreManager.instance.timeText.gameObject.SetActive(false);
            ScoreManager.instance.bestTimeTime.gameObject.SetActive(false);

            OnGameSuccess?.Invoke();
        }
    }

    public void PauseGame()
    {
        if (!failed && !successed && startGame && !paused)
        {
            paused = true;
            gameState = GameState.Paused;
            Time.timeScale = 0;

            OnGamePause?.Invoke();
        }
    }

    public void ResumeGame()
    {
        if (!failed && !successed && startGame && paused)
        {
            paused = failed;
            gameState = GameState.Playing;
            Time.timeScale = 1;

            OnGameResume?.Invoke();
        }
    }

    public void Fail(float time)
    {
        StartCoroutine(FailIE(time));
    }

    public async void Success()
    {
        if (!failed && !successed)
        {
            ReqClass reqC = new ReqClass();
            reqC.wallet = PlayerPrefs.GetString("Account"); 
            reqC.sessionNum = PlayerPrefs.GetInt("GameId"); 
            reqC.star = ScoreManager.instance.getStars();
            var reqJson = JsonUtility.ToJson(reqC);
            var req = Post("https://xcodebackend.herokuapp.com/gameEnd", reqJson);
            await req.SendWebRequest();
            string resp = req.downloadHandler.text;
            ScoreBE dict = Newtonsoft.Json.JsonConvert.DeserializeObject<ScoreBE>(resp);
            if (dict.resp != "\"Session Time Over\"")
            {
                successed = true;
                gameState = GameState.Successed;
                successPanel.SetActive(true);
                PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel", 1) + 1);


                GetBest req2js = new GetBest();
                req2js.sessionNum = PlayerPrefs.GetInt("GameId");
                var kucukCan = JsonUtility.ToJson(req2js);
                var req2 = Post("https://xcodebackend.herokuapp.com/bestscore",kucukCan);
                await req2.SendWebRequest();
                var resp2 = req2.downloadHandler.text;
                var data2 = Newtonsoft.Json.JsonConvert.DeserializeObject<GetBest2>(resp2);

                ScoreManager.instance.BestTimeChanger();
                ScoreManager.instance.panel.SetActive(true);
                ScoreManager.instance.timeTextPanel.text = ScoreManager.instance.timeText.text;
                ScoreManager.instance.bestTimeTextPanel.text = ScoreManager.instance.bestTimeTime.text;


                ScoreManager.instance.timeText.gameObject.SetActive(false);
                ScoreManager.instance.bestTimeTime.gameObject.SetActive(false);

                OnGameSuccess?.Invoke();
            }
            else
            {
                SceneManager.LoadScene(1);
            }
        }
    }

    public void Success(float time)
    {
        StartCoroutine(SuccessIE(time));
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(1);
        ScoreManager.instance.SetTimerToZero();
    }
    public void HomeLevel()
    {
        SceneManager.LoadScene(5);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public GameState CurrentGameState()
    {
        return gameState;
    }

    private IEnumerator FailIE(float time)
    {
        yield return new WaitForSeconds(time);
        Fail();
    }
    private IEnumerator SuccessIE(float time)
    {
        yield return new WaitForSeconds(time);
        Success();
    }

    private IEnumerator StartIE(float time)
    {
        yield return new WaitForSeconds(time);
        StartGame();
    }

    

    public enum GameState { Null, Playing, Paused, Successed, Failed }
}
