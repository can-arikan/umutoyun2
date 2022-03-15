using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;


    private MultipleTargetCamera multiCam;
    private CameraFollow normalCam;


    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        GetVariablesOnStart();
    }

    private void GetVariablesOnStart()
    {
        multiCam = GetComponent<MultipleTargetCamera>();
        normalCam = GetComponent<CameraFollow>();
    }

    public void ChangeCamera(GameObject obj)
    {
        normalCam.enabled = false;
        multiCam.enabled = true;

        multiCam.targets.Add(obj.transform);
    }
}
