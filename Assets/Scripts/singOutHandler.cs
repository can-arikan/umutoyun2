<<<<<<< HEAD
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
=======
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
>>>>>>> 08ffa6b6b1dfe570855efe224370f176c495fd57
}