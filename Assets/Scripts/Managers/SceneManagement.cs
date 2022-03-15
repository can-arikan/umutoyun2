using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagement : MonoBehaviour
{
  
    private struct Wallet{
        public string wallet;
    }

    public GameObject error;
    public GameObject inpu;
    UnityWebRequest Post(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        return request;
    }

    public void BackToLoginScreen()
    {
        SceneManager.LoadScene(0);
    }
    public void BackToHomeScreen()
    {
        SceneManager.LoadScene(5);
    }

    public async void delete() {
        string inUnity = PlayerPrefs.GetString("Account");
        char[] trims = { '\t', '\n', '\"', '\'', ' '};
        List<char> preUsersAns = new List<char>();
        foreach (char i in inpu.GetComponentsInChildren<TextMeshProUGUI>()[1].text.ToCharArray()){
            if (('a' <= i && i <= 'z') || ('A' <= i && i <= 'Z') || ('0' <= i && i <= '9'))
                preUsersAns.Add(i);
        }
        string usersAns = preUsersAns.ToArray().ArrayToString().ToLower();
        if (inUnity == usersAns){
            Wallet mwallet = new Wallet();
            mwallet.wallet = PlayerPrefs.GetString("Account");
            var prereq = Newtonsoft.Json.JsonConvert.SerializeObject(mwallet);
            var req = Post("https://xcodebackend.herokuapp.com/delses",prereq);
            await req.SendWebRequest();
            SceneManager.LoadScene(5);
        }
        else{
            var text = error.GetComponent<TextMeshProUGUI>();
            text.text = "You May Only Enter Your Signed in Account's Wallet !";
            text.color = Color.red;
        }
    }
}
