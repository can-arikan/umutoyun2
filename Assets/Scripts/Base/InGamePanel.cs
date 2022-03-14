using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InGamePanel : MonoBehaviour
{
    [SerializeField] GameObject  tutorialPanelGo,textObj;

    private DOTweenAnimation fadeOutAnim, tutorialFadeAnim;


    private void Start()
    {
        //fadeOutAnim = textObj.GetComponents<DOTweenAnimation>()[1];
        fadeOutAnim = textObj.GetComponents<DOTweenAnimation>()[0];
        tutorialFadeAnim = tutorialPanelGo.GetComponent<DOTweenAnimation>();

        MyGameManager.instance.OnGameStart += OpenPanel;
        //MyGameManager.instance.OnGameFail += ClosePanel;
    }

    private void OpenPanel()
    {
        //inGamePanelGo.SetActive(true);
        tutorialFadeAnim.DOPlay();
        fadeOutAnim.DOPlay();
    }

    //private void ClosePanel()
    //{
    //    fadeOutAnim.DOPlay();
    //}
}
