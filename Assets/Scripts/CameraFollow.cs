using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public enum State { Idle, Run }
    public State state;
    [Range(0.0f, 10.0f)]
    public float smoothness;
    public Vector3 offset, glideOffset;

    private Transform target;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, target.position.z) + offset;
        transform.position = desiredPosition;
    }
    private void LateUpdate()
    {

        if (GetComponent<Camera>().orthographicSize == 40&&MyGameManager.instance.gameState == MyGameManager.GameState.Playing)
        {
            StartCoroutine(SetCamSize(GetComponent<Camera>().orthographicSize));
            StartCoroutine(SetOffsetSize(offset.y));
        }
    }

    private void FixedUpdate()
    {
        if (MyGameManager.instance.gameState==MyGameManager.GameState.Playing)
        {
            //if (GetComponent<Camera>().orthographicSize == 40)
            //{
            //    StartCoroutine(SetCamSize(GetComponent<Camera>().orthographicSize));
            //}

      

            //if (offset.y != 0) 

            Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, target.position.z) + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothness);
        }
    }

    

    public IEnumerator SetCamSize(float value)
    {
        while (value > 15)
        {
            value = Mathf.Lerp(value, 14.8f, Time.deltaTime * 1);
            GetComponent<Camera>().orthographicSize = value;
            yield return null;
        }
    }
    public IEnumerator SetOffsetSize(float value)
    {
        while (value > 0)
        {
            value = Mathf.Lerp(value, -0.2f, Time.deltaTime * 1);
            offset = new Vector3(offset.x, value, offset.z);
            yield return null;
        }
    }
}
