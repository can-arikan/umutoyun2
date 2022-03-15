using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadFill : MonoBehaviour
{
    [SerializeField] Image fill;
    [SerializeField] Text description;

    private float fillAmount, speed;
    private bool isFillFull;

    private void Update()
    {
        if (!isFillFull)
        {
            if (fillAmount > 1)
            {
                isFillFull = true;
                fillAmount = 1;
            }

            speed = 0.4f;

            if (fillAmount > 0.2f)
            {
                description.text = "Loading World.";
                speed = 1.8f;
            }

            if (fillAmount > 0.6f)
            {
                description.text = "Spawning Player.";
                speed = 0.8f;
            }

            if (fillAmount > 0.9f)
            {
                description.text = "Initializing Level " + PlayerPrefs.GetInt("CurrentLevel", 1) + ".";
            }

            fillAmount += Time.deltaTime * speed;
            fill.fillAmount = fillAmount;
        }
    }
}
