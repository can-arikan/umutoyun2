using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("AlfishGames/Managers/Tutorial Manager")]
public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    [SerializeField] GameObject[] tutorials;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void TutorialSetActive(Tutorial tutorial, bool value)
    {
        tutorials[(int)tutorial].SetActive(value);
    }

    public void TutorialSetActive(int index, bool value)
    {
        tutorials[index].SetActive(value);
    }

    public enum Tutorial { Swipe, Tap }
}
