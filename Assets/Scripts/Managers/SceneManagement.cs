using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
  
    public void BackToLoginScreen()
    {
        SceneManager.LoadScene(0);
    }
    public void BackToHomeScreen()
    {
        SceneManager.LoadScene(5);
    }
}
