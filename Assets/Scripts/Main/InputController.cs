using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        
    }

    public float MouseAxis(string axis)
    {
        if (axis == "X")
        {
            return Input.GetAxis("Mouse X");
        }

        if (axis == "Y")
        {
            return Input.GetAxis("Mouse Y");
        }

        return 0;
    }

    public void StartGameOnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MyGameManager.instance.StartGame();
        }
    }

    public void StartGameOnClick(float time)
    {
        if (Input.GetMouseButtonDown(0))
        {
            MyGameManager.instance.StartGame(time);
        }
    }
}
