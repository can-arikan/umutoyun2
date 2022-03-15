using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Text;
using TMPro;

public class userNameManager : MonoBehaviour
{
    public static userNameManager instance;
    public Button btn;
    public TextMeshProUGUI error;

   /* [HideInInspector]*/ public Text userName;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    UnityWebRequest Post(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        return request;
    }
    private struct MyClass {
        public string wallet;
        public string userName;
    };
    async public void GetUserName()
    {
        btn.enabled = false;
        int spaceCount = 0;
        for(int i = 0; i < userName.text.Length; i++)
        {
            if(userName.text.ToString()[i] == ' ')
            {
                spaceCount++;
            }
        }
        if (userName.text != "" && userName.text.IndexOf(" ") != 0 && spaceCount != userName.text.Length)
        {
            try
            {
                MyClass newObj = new MyClass();
                newObj.wallet = PlayerPrefs.GetString("Account");
                newObj.userName = userName.text;
                string jsonStringTrial = JsonUtility.ToJson(newObj);
                var resp = Post("https://xcodebackend.herokuapp.com/createOrLogin", jsonStringTrial);
                await resp.SendWebRequest();
                string res = resp.downloadHandler.text.ToString();
                if (resp.responseCode != 500) {
                    SceneManager.LoadScene(5);
                }
                else
                {
                    error.text = "Username Already Taken";
                    btn.enabled = true;
                    return;
                }
            }
            catch
            {
                error.text = "Username Already Taken";
                btn.enabled = true;
                return;
            }
        }
        else
        {
            error.text = "Invalid Username";
            btn.enabled = true;
            return;
        }
    }
}
