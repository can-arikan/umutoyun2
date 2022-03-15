using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class singOutHandler : MonoBehaviour
{

    public void signFuckenOut()
    {
        WebLogin.SetConnectAccount("");
        PlayerPrefs.SetString("Account", "0x0000000000000000000000000000000000000001");
        Web3GL.disconnect();
        SceneManager.LoadScene(0);
    }

    public void scoreFuckenBoard()
    {
        SceneManager.LoadScene(7);
    }
}