using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class SessionHandler : MonoBehaviour
{
    public GameObject insideButtonGreen;
    public GameObject insideButtonRed;
    public GameObject insideButtonWaiting;
    public GameObject Panel;
    private List<GameObject> butons = new List<GameObject>();
    private List<GameObject> ActiveButons = new List<GameObject>();

    public GameObject[] stars = new GameObject[5];
    public TextMeshProUGUI highestScore;
    private float score;
    UnityWebRequest Post(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        return request;
    }
    private struct MyClass
    {
        public string wallet;
    };

    private struct MyScore
    {
        public float bestScore;
        public int star;
    };

    public class Button2
    {
        public int id { get; set; }
        public bool end { get; set; }
        public float score { get; set; }
        public string timeRemain { get; set; }
    }

    public class Root
    {
        public List<int> gamesIDS { get; set; }
        public List<Button2> buttons { get; set; }

        public override string ToString()
        {
            string str = "{gamesIDS: [ ";
            foreach (var id in gamesIDS)
                str += " Id: " + id.ToString() + ", ";
            str = str.Substring(0, str.Length - 2);
            str += "], buttons: [";
            foreach (var button in buttons)
                str += "{" + button.id + ": " + button.end + "}, ";
            str = str.Substring(0, str.Length - 2);
            str += "] }";

            return str;
        }
    }

    public class bestsessend
    {
        public int sessionNum { get; set; }
    };

    public class bestsesget
    {
        public float best { get; set; }
    };

    private struct verylast {
        public List<int> waitings;
    };

    void onClickHandler(int gameId)
    {
        PlayerPrefs.SetInt("GameId", gameId);
        SceneManager.LoadScene(1);
    }

    private List<int> gamesIDS = new List<int>();
    private verylast realres = new verylast();
    private List<GameObject> waitingButtons = new List<GameObject>();

    public async void Start()
    {
        insideButtonGreen.SetActive(false);
        insideButtonRed.SetActive(false);
        insideButtonWaiting.SetActive(false);
        MyClass walletAddress = new MyClass();
        walletAddress.wallet = PlayerPrefs.GetString("Account");
        var reqdata = Newtonsoft.Json.JsonConvert.SerializeObject(walletAddress);
        var req = Post("https://xcodebackend.herokuapp.com/waitings", reqdata);
        await req.SendWebRequest();
        var resr = req.downloadHandler.text;
        realres = Newtonsoft.Json.JsonConvert.DeserializeObject<verylast>(resr);
        for (int i = 0; i < realres.waitings.Count; i++)
        {
            GameObject go;
            go = Instantiate(insideButtonWaiting);
            var text = go.GetComponentInChildren<TextMeshProUGUI>();
            text.text = "Waiting Session: " + realres.waitings[i].ToString(); 
            Text index = go.GetComponentInChildren<Text>();
            index.text = realres.waitings[i].ToString();
            go.transform.parent = Panel.transform;
            go.transform.localScale = new Vector3(1, 1, 1);
            go.transform.localScale.Set(1, 1, 1);
            Vector3 localpos = go.transform.localPosition;
            localpos.z = 1;
            go.transform.localPosition = localpos;
            go.SetActive(true);
            waitingButtons.Add(go);
        }
        MyClass newObj = new MyClass();
        newObj.wallet = PlayerPrefs.GetString("Account");

        string jsonStringTrial = JsonUtility.ToJson(newObj);
        var resp = Post("https://xcodebackend.herokuapp.com/sessions", jsonStringTrial);
        await resp.SendWebRequest();
        string res = resp.downloadHandler.text;
        Root dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Root>(res);
        gamesIDS = dict.gamesIDS;
        for (int i = 0; i < dict.gamesIDS.Count ; i++)
        {
            GameObject go;
            if (dict.buttons[i].end == true)
            {
                go = Instantiate(insideButtonRed);
                var scaler = go.GetComponent<RectTransform>();
                scaler.localScale = new Vector3(1, 1, 1);
                var text = go.GetComponentInChildren<TextMeshProUGUI>();
                text.text = "Session: " + dict.gamesIDS[i].ToString();
                text.text += " Score: " + dict.buttons[i].score.ToString("n2");
            }
            else
            {
                go = Instantiate(insideButtonGreen);
                var scaler = go.GetComponent<RectTransform>();
                scaler.localScale = new Vector3(1, 1, 1);
                var text = go.GetComponentInChildren<TextMeshProUGUI>();
                text.text = "Session: " + dict.gamesIDS[i].ToString();
                text.text += " Time Left: " + (Mathf.FloorToInt(float.Parse(dict.buttons[i].timeRemain)/60)).ToString("00") + ":" + (Mathf.FloorToInt(float.Parse(dict.buttons[i].timeRemain)%60)).ToString("00");
            }
            Text index = go.GetComponentInChildren<Text>();
            index.text = i.ToString();
            go.transform.parent = Panel.transform;
            go.SetActive(true);
            butons.Add(go);
        }
        for (var j = 0; j < butons.Count; j++)
        {
            var btn = butons[j];
            var rbtn = btn.GetComponent<Button>();
            if (!dict.buttons[j].end)
            {
                rbtn.onClick.AddListener(delegate { onClickHandler(dict.gamesIDS[int.Parse(btn.GetComponentsInChildren<Text>()[0].text)]); });
                btn.GetComponentsInChildren<Text>()[1].text = dict.buttons[j].timeRemain;
                ActiveButons.Add(btn);
            }
            btn.transform.localScale = new Vector3(1, 1, 1);
            btn.transform.localScale.Set(1, 1, 1);
            Vector3 localpos = btn.transform.localPosition;
            localpos.z = 1;
            btn.transform.localPosition = localpos;
        }

        MyClass nreq = new MyClass();
        nreq.wallet = PlayerPrefs.GetString("Account");
        var esreq = JsonUtility.ToJson(nreq);
        var esereq = Post("https://xcodebackend.herokuapp.com/starscore", esreq);
        await esereq.SendWebRequest();
        var fres = esereq.downloadHandler.text;
        var ares = Newtonsoft.Json.JsonConvert.DeserializeObject<MyScore>(fres);
        highestScore.text += ares.bestScore;
        for (int i = 0; i < 5; i++)
        {
            stars[i].SetActive(false);
        }
        for (int i = 0; i < ares.star; i++)
        {
            stars[i].SetActive(true);
        }
    }

    private void Update()
    {
        foreach (var abtn in ActiveButons)
        {
            var time = float.Parse(abtn.GetComponentsInChildren<Text>()[1].text);
            time -= Time.deltaTime;
            string ntext;
            if (time >= 0)
            {
                abtn.GetComponentsInChildren<Text>()[1].text = time.ToString();
                ntext = (Mathf.FloorToInt(time / 60)).ToString("00") + ":" + (Mathf.FloorToInt(time % 60)).ToString("00");
                abtn.GetComponentsInChildren<TextMeshProUGUI>()[0].text = abtn.GetComponentsInChildren<TextMeshProUGUI>()[0].text.Substring(0, abtn.GetComponentsInChildren<TextMeshProUGUI>()[0].text.LastIndexOf(' ') + 1) + ntext;
            }
            else
            {
                ActiveButons.Remove(abtn);
                StartIE(abtn);
            }
        }

        foreach(var abtn in waitingButtons)
        {
            waitingButtons.Remove(abtn);
            StartIE2(abtn);
        }
    }

    private struct enough
    {
        public int game_id;
        public string wallet;
        public enough(int x, string y)
        {
            this.game_id = x;
            this.wallet = y;
        }
    }

    private struct pain
    {
        public bool res;
    }

    private async void StartIE2(GameObject abtn)
    {
        Text index = abtn.GetComponentInChildren<Text>();
        enough newEn = new enough(int.Parse(index.text),PlayerPrefs.GetString("Account"));
        var req = Post("https://xcodebackend.herokuapp.com/ispaid", Newtonsoft.Json.JsonConvert.SerializeObject(newEn));
        await req.SendWebRequest();
        var rawres = req.downloadHandler.text;
        var rres = Newtonsoft.Json.JsonConvert.DeserializeObject<pain>(rawres);
        if (rres.res == false)
        {
            await new WaitForSecondsRealtime(5);
            waitingButtons.Add(abtn);
        }
        else
        {
            if (SceneManager.GetActiveScene().buildIndex != 1 && SceneManager.GetActiveScene().buildIndex != 2)
            {
                SceneManager.LoadScene(5);
            }
        }
    }

    private async void StartIE(GameObject abtn)
    {
        GameObject go = Instantiate(insideButtonRed);
        bestsessend sender = new bestsessend();
        sender.sessionNum = gamesIDS[int.Parse(abtn.GetComponentInChildren<Text>().text)];
        var senderjson = Newtonsoft.Json.JsonConvert.SerializeObject(sender);
        var req = Post("https://xcodebackend.herokuapp.com/bestscore", senderjson);
        await req.SendWebRequest();
        var res = req.downloadHandler.text;
        var cvp = Newtonsoft.Json.JsonConvert.DeserializeObject<bestsesget>(res);
        var text = go.GetComponentInChildren<TextMeshProUGUI>();
        text.text = "Session: " + sender.sessionNum.ToString();
        text.text += " Score: " + cvp.best.ToString("n2");
        go.transform.parent = Panel.transform;
        go.transform.localScale = new Vector3(1, 1, 1);
        go.transform.localScale.Set(1, 1, 1);
        Vector3 localpos = go.transform.localPosition;
        localpos.z = 1;
        go.transform.localPosition = localpos;
        go.transform.SetAsFirstSibling(); 
        foreach (var btn in ActiveButons)
        {
            btn.transform.SetAsFirstSibling();
        }
        foreach (var btn in waitingButtons)
        {
            btn.transform.SetAsFirstSibling();
        }
        go.SetActive(true);
        Destroy(abtn);
    }

    public void seceneLoader()
    {
        SceneManager.LoadScene(3);
    }
}
