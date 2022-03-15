using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject settingsButton,settingsPanel;


    public void Opensettings()
    {
        Time.timeScale = 0;
        settingsButton.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        Time.timeScale = 1;
        settingsPanel.SetActive(false);
        settingsButton.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void GoHome()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(5);
    }
}
