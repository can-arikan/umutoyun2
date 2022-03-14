using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

#if UNITY_WEBGL
public class WebLogin : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void Web3Connect();

    [DllImport("__Internal")]
    private static extern string ConnectAccount();

    [DllImport("__Internal")]
    public static extern void SetConnectAccount(string value);

    private int expirationTime;
    private string account; 

    private struct MyClass
    {
        public string wallet;
    };

    private async void Start()
    {
        var currentWallet = ConnectAccount();
        if (PlayerPrefs.GetString("Account") != "" && PlayerPrefs.GetString("Account") != "0x0000000000000000000000000000000000000001" && currentWallet != "" && currentWallet == PlayerPrefs.GetString("Account"))
        {
            Web3Connect();
            SetConnectAccount("");
            MyClass newObj = new MyClass();
            newObj.wallet = PlayerPrefs.GetString("Account");
            string jsonStringTrial = JsonUtility.ToJson(newObj);
            var resp = Post("https://xcodebackend.herokuapp.com/createOrLogin", jsonStringTrial);
            await resp.SendWebRequest();
            string res = resp.downloadHandler.text.ToString();
            // load next scene
            if (res == "\"Send userName\"")
            {
                SceneManager.LoadScene(4);
            }
            else if (res == "\"Can Pass\"" || res == "\"User Exist but no time\"")
            {
                SceneManager.LoadScene(5);
            }
            else
            {
                SceneManager.LoadScene(6);
            }
        }
    }

    public void OnLogin()
    {
        Web3Connect();
        OnConnected();
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

    async private void OnConnected()
    {
        account = ConnectAccount();
        while (account == "") {
            await new WaitForSeconds(1f);
            account = ConnectAccount();
        };
        // save account for next scene
        PlayerPrefs.SetString("Account", account);
        // reset login message
        SetConnectAccount("");
        MyClass newObj = new MyClass();
        newObj.wallet = account;
        string jsonStringTrial = JsonUtility.ToJson(newObj);
        var resp = Post("https://xcodebackend.herokuapp.com/createOrLogin", jsonStringTrial);
        await resp.SendWebRequest();
        string res = resp.downloadHandler.text.ToString();
        // load next scene
        if (res == "\"Send userName\"")
        {
            SceneManager.LoadScene(4);
        }
        else if (res == "\"Can Pass\"" || res == "\"User Exist but no time\"")
        {
            SceneManager.LoadScene(5);
        }
        else
        {
            SceneManager.LoadScene(6);
        }
    }

    //public void OnSkip()
    //{
    //    // burner account for skipped sign in screen
    //    PlayerPrefs.SetString("Account", "");
    //    // move to next scene
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    //}
}
#endif
