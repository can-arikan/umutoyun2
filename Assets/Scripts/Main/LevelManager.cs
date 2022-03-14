using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    int lastLevel;

    public int totalLevelCount;

    private void Awake()
    {
        PlayerPrefs.GetInt("LastLevel", 1);
        //PlayerPrefs.SetInt("CurrentLevel", 2);
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);

        if (currentLevel > totalLevelCount && PlayerPrefs.GetInt("IsRestart") == 1)
        {

            Instantiate(Resources.Load("Levels/" + "Level" + PlayerPrefs.GetInt("LastLevel")) as GameObject);
        }
        else if (currentLevel <= totalLevelCount)
        {
            Instantiate(Resources.Load("Levels/" + "Level" + currentLevel) as GameObject);
        }
        else
        {
            lastLevel = Random.Range(2, totalLevelCount + 1);
            Debug.Log("last" + lastLevel);
            PlayerPrefs.SetInt("LastLevel", lastLevel);
            Instantiate(Resources.Load("Levels/" + "Level" + lastLevel) as GameObject);
        }
    }
}
