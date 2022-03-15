using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("AlfishGames/Camera/Multi Target Camera")]
[RequireComponent(typeof(Camera))]
public class MultiTargetCamera : MonoBehaviour
{
    public static MultiTargetCamera instance;

    [Header("Camera Target Value")]
    [SerializeField] private Transform cameraPositionsParent;
    [Header("Camera Position Values")]
    [SerializeField] bool instantPoseAtStart;
    [SerializeField] int cameraIndex;
    [Header("Camera Movement Values")]
    [SerializeField] private float smoothTime;
    [SerializeField] private float angleSpeed;

    private Vector3 velocity;
    private Transform target;
    private Transform[] camPoses;
    private int posLenght;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        GetVariablesOnStart();
        if (instantPoseAtStart) StartPosition();
    }

    private void Update()
    {
        CameraFunction();
    }

    private void GetVariablesOnStart()
    {
        // Variables
        camPoses = new Transform[cameraPositionsParent.childCount];

        int length = camPoses.Length;

        // For Loops
        for (int i = 0; i < length; i++)
        {
            camPoses[i] = cameraPositionsParent.GetChild(i);
        }

        // Values
        posLenght = length;

        if (smoothTime == 0) smoothTime = 1;
        if (angleSpeed == 0) angleSpeed = 1;
    }

    private void StartPosition()
    {
        target = camPoses[cameraIndex];
        transform.position = target.position;
        transform.rotation = target.rotation;
    }

    private void CameraFunction()
    {
        target = camPoses[cameraIndex];

        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * angleSpeed);
    }

    #region CameraChangers

    public void ChangeCameraIndex(int index)
    {
        cameraIndex = index;
    }
    public void ChangeCameraIndex(int index, float delayTime)
    {
        StartCoroutine(ChangeCameraIndexIE(index, delayTime));
    }
    public void NextCameraIndex()
    {
        if (cameraIndex < posLenght)
            cameraIndex++;
    }
    public void NextCameraIndex(float delayTime)
    {
        StartCoroutine(NextCameraIndexIE(delayTime));
    }
    public void PreviousCameraIndex()
    {
        if (cameraIndex > 0)
            cameraIndex--;
    }
    public void PreviousCameraIndex(float delayTime)
    {
        StartCoroutine(PreviousCameraIndexIE(delayTime));
    }
    private IEnumerator ChangeCameraIndexIE(int index, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        cameraIndex = index;
    }
    private IEnumerator NextCameraIndexIE(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (cameraIndex < posLenght)
            cameraIndex++;
    }
    private IEnumerator PreviousCameraIndexIE(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (cameraIndex > 0)
            cameraIndex--;
    }

    #endregion
}
