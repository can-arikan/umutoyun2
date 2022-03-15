using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class scoreBoardHandler : MonoBehaviour
{
    public GameObject writings;
    public GameObject Panel;

    UnityWebRequest Post(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        return request;
    }

    public struct class1
    {
        public float score;
        public int session;
        public string player;
        public string wallet;
    };

    public struct class2
    {
        public List<class1> info;
    };

    public struct class3
    {
        public string selam;
    };

    public async void Start()
    {
        writings.SetActive(false);
        class3 req = new class3();
        req.selam = "selam kizlar";
        var strreq = JsonUtility.ToJson(req);
        var res = Post("https://xcodebackend.herokuapp.com/scoreBoard", strreq);
        await res.SendWebRequest();
        string baseform = res.downloadHandler.text;
        class2 dic = Newtonsoft.Json.JsonConvert.DeserializeObject<class2>(baseform);
        for (int i = 0; i < dic.info.Count; i++)
        {
            GameObject go = Instantiate(writings);
            var text = go.GetComponentsInChildren<TextMeshProUGUI>();
            go.transform.parent = Panel.transform;
            go.transform.localScale = new Vector3(1,1,1);
            var pos = go.transform.localPosition;
            pos.z = 1;
            go.transform.localPosition = pos;
            text[0].text = (i + 1).ToString()+"-";
            string shortName;
            if (dic.info[i].player.Length >= 14)
            {
                shortName = dic.info[i].player.Substring(0,12) + "...";
            }
            else
            {
                shortName = dic.info[i].player;
            }
            text[1].text = shortName;
            text[2].text = dic.info[i].session.ToString();
            text[3].text = dic.info[i].score.ToString("0.00");
            if (dic.info[i].wallet == PlayerPrefs.GetString("Account").ToLower())
            {
                text[0].color = Color.red;
                text[1].color = Color.red;
                text[2].color = Color.red;
                text[3].color = Color.red;
            }
            if (1 <= (i+1) && (i+1) <=3)
            {
                text[0].color = new Color(255 / 255f, 214 / 255f, 0);
                text[1].color = new Color(255 / 255f, 214 / 255f, 0);
                text[2].color = new Color(255 / 255f, 214 / 255f, 0);
                text[3].color = new Color(255 / 255f, 214 / 255f, 0);
            }
            go.SetActive(true);
        }
        Destroy(writings);
    }

    public void goManager()
    {
        SceneManager.LoadScene(5);
    }

}
