using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Text;
using System.Collections.Generic;

#if UNITY_WEBGL
public class BuyManager : MonoBehaviour
{
    public Button button;

    private int myNum;

    UnityWebRequest Post(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        return request;
    }

    private struct Wallet
    {
        public string wallet;
    }

    private struct gameId
    {
        public int GameID { get; set; }
    }

    private struct WalletGameID
    {
        public string wallet;
        public int gameId;
    }

    private struct Root
    {
        public string start { get; set; }
        public int gameId { get; set; }
    };

    private struct verylast {
        public List<int> waitings;
    };
    
    private async void Start(){
        if (!PlayerPrefs.HasKey("PendingSessions")) {
            PlayerPrefs.SetInt("PendingSessions",0);
            myNum = 0;
        } else {
            PlayerPrefs.SetInt("PendingSessions",PlayerPrefs.GetInt("PendingSessions")+1);
            myNum = PlayerPrefs.GetInt("PendingSessions");
        }
        Wallet newWallet = new Wallet();
        newWallet.wallet = PlayerPrefs.GetString("Account");
        var reqjs = Newtonsoft.Json.JsonConvert.SerializeObject(newWallet);
        var req = Post("https://xcodebackend.herokuapp.com/waitings", reqjs);
        await req.SendWebRequest();
        var rawres = req.downloadHandler.text;
        var res1 = Newtonsoft.Json.JsonConvert.DeserializeObject<verylast>(rawres);
        if (res1.waitings.Count != 0){
            button.enabled = false;
        }
    }

    public async void BuyEntry()
    {
        Web3GL.buying();
        button.enabled = false;
        bool enter = false;
        // smart contract method to call
        string method = "payment";
        // abi in json format
        string abi = "[{\"inputs\":[],\"stateMutability\":\"payable\",\"type\":\"constructor\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"newPrice\",\"type\":\"uint256\"}],\"name\":\"changePrice\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address payable\",\"name\":\"_withdrawaddress\",\"type\":\"address\"}],\"name\":\"changeWithDrawAddress\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes1\",\"name\":\"b\",\"type\":\"bytes1\"}],\"name\":\"char\",\"outputs\":[{\"internalType\":\"bytes1\",\"name\":\"c\",\"type\":\"bytes1\"}],\"stateMutability\":\"pure\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address[]\",\"name\":\"payerlist\",\"type\":\"address[]\"},{\"internalType\":\"uint256[]\",\"name\":\"gamelist\",\"type\":\"uint256[]\"}],\"name\":\"deletePlayers\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getBalance\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"owner\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"name\":\"payers\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"game\",\"type\":\"uint256\"}],\"name\":\"payment\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"payable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"price\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"x\",\"type\":\"address\"}],\"name\":\"toAsciiString\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"pure\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"withDraw\",\"outputs\":[],\"stateMutability\":\"payable\",\"type\":\"function\"}]";
        string contract = "0x4e7410E2A32e2fbFC9831c5baAe33E36c935DDB7";
        // array of arguments for contract
        string args = "[]";
        // value in wei
        string response;
        try
        {
            response = await EVM.Call("polygon", "mainnet", contract, abi, "price", args);
            response = (float.Parse(response) / Mathf.Pow(10, 18)).ToString();
        }
        catch
        {
            button.enabled = true;
            return;
        }
        gameId res1 = new gameId();
        UnityWebRequest req = new UnityWebRequest();
        try
        {
            Wallet newWallet = new Wallet();
            newWallet.wallet = PlayerPrefs.GetString("Account");
            var reqjs = Newtonsoft.Json.JsonConvert.SerializeObject(newWallet);
            req = Post("https://xcodebackend.herokuapp.com/prepayment", reqjs);
            await req.SendWebRequest();
            var rawres = req.downloadHandler.text;
            res1 = Newtonsoft.Json.JsonConvert.DeserializeObject<gameId>(rawres);
            PlayerPrefs.SetInt("MyNum"+myNum.ToString(),res1.GameID);
            SceneManager.LoadScene(5);
            if (req.responseCode == 200)
            {
                Int64 bigint = (Int64)(double.Parse(response) * (Int64)Math.Pow(10, 18));
                string value = bigint.ToString();
                string gasLimit = "";
                int[] obj = { res1.GameID };
                args = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                string gasPrice = await EVM.GasPrice("polygon", "mainnet", "https://polygon-rpc.com/");
                try
                {
                    Debug.Log("My Num: " + myNum + " my del ses is " + PlayerPrefs.GetInt("MyNum"+myNum).ToString());
                    var txcontract = await Web3GL.SendContract(method, abi, contract, args, value, gasLimit, gasPrice);
                    var res2 = await EVM.TxStatus("polygon", "mainnet", txcontract);
                    if (res2.responseCode != 200){
                        Debug.Log("My Num: " + myNum + " my del ses is " + PlayerPrefs.GetInt("MyNum"+myNum).ToString() + " i am deleting now 1");
                        WalletGameID x = new WalletGameID();
                        x.gameId = PlayerPrefs.GetInt("MyNum"+myNum);
                        x.wallet = PlayerPrefs.GetString("Account");
                        var y = Newtonsoft.Json.JsonConvert.SerializeObject(x);
                        req = Post("https://xcodebackend.herokuapp.com/delses", y);
                        await req.SendWebRequest();
                        button.enabled = true;
                        SceneManager.LoadScene(5);
                        return;
                    }
                }
                catch {
                    enter = true;
                    Debug.Log("My Num: " + myNum + " my del ses is " + PlayerPrefs.GetInt("MyNum"+myNum).ToString() + " i am deleting now 2");
                    WalletGameID x = new WalletGameID();
                    x.gameId = PlayerPrefs.GetInt("MyNum"+myNum);
                    x.wallet = PlayerPrefs.GetString("Account");
                    var y = Newtonsoft.Json.JsonConvert.SerializeObject(x);
                    req = Post("https://xcodebackend.herokuapp.com/delses", y);
                    await req.SendWebRequest();
                    button.enabled = true;
                    SceneManager.LoadScene(5);
                    return;
                }
                SceneManager.LoadScene(5);
                return;
            }
        }
        catch
        {
            if(enter == false) {
                Debug.Log("My Num: " + myNum + " my del ses is " + PlayerPrefs.GetInt("MyNum"+myNum).ToString() + " i am deleting now 3");
                WalletGameID x = new WalletGameID();
                x.gameId = PlayerPrefs.GetInt("MyNum"+myNum);
                x.wallet = PlayerPrefs.GetString("Account");
                var y = Newtonsoft.Json.JsonConvert.SerializeObject(x);
                req = Post("https://xcodebackend.herokuapp.com/delses", y);
                await req.SendWebRequest();
                button.enabled = true;
                SceneManager.LoadScene(5);
                return;
            }
        }
    }

    public void BackToHome()
    {
        SceneManager.LoadScene(5);
    }
}
#endif
