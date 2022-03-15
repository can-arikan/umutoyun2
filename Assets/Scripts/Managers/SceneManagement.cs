using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
  
    private struct Wallet{
        public string wallet;
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

    public void BackToLoginScreen()
    {
        SceneManager.LoadScene(0);
    }
    public void BackToHomeScreen()
    {
        SceneManager.LoadScene(5);
    }

    public async void delete(){
        Wallet mwallet = new Wallet();
        mwallet.wallet = PlayerPrefs.GetString("Account");
        var prereq = Newtonsoft.Json.JsonConvert.SerializeObject(mwallet);
        var req = Post("https://xcodebackend.herokuapp.com/delses",prereq);
        await req.SendWebRequest();
        SceneManager.LoadScene(5);
    }
}
