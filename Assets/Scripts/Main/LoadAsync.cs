using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadAsync : MonoBehaviour
{
    private AsyncOperation async;
    public float targetTime;
    private bool timerIsEnded = false;
    public int loadedSceneIndex;
    public bool isTriggerManual = false;
    public static LoadAsync GlobalAsync;

    // Use this for initialization
    void Start()
    {
        GlobalAsync = this;

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            StartCoroutine(LoadScene());
        }



    }

    public void TriggerManually()
    {

        StartCoroutine(LoadScene());
        isTriggerManual = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTriggerManual)
        {

            countDownTimer();

            if (async != null)
            {
                if (timerIsEnded)
                {


                    SwitchScene();

                }
            }

        }

    }

    IEnumerator LoadScene()
    {
        // loadingCanvas.gameObject.SetActive(true);
        async = SceneManager.LoadSceneAsync(loadedSceneIndex);
        async.allowSceneActivation = false;

        yield return async;
    }

    private void SwitchScene()
    {
        if (async != null)
            async.allowSceneActivation = true;
    }

    private void countDownTimer()
    {
        targetTime -= Time.deltaTime;

        if (targetTime <= 0.0f)
        {
            timerIsEnded = true;
        }
    }


}
