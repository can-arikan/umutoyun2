using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpObjectsHandler : MonoBehaviour
{
    public static JumpObjectsHandler instance;

    /*[HideInInspector]*/ public int lightedJumpObjectCount;

    [SerializeField] List<GameObject> jumpsObjects = new List<GameObject>();

    private int totalCount;

    private void Awake()
    {
        if (instance == null) instance = this;
    } 

    private void Start()
    {
        jumpsObjects.AddRange(GameObject.FindGameObjectsWithTag("JumpObjects"));

        totalCount = jumpsObjects.Count;
    }

    public void FinishGameChecker()
    {
        if (lightedJumpObjectCount + 1 == totalCount) CameraManager.instance.ChangeCamera(jumpsObjects[0]);

        if(lightedJumpObjectCount == totalCount) MyGameManager.instance.Success();
    }

    public void GetOutFromList(GameObject obj)
    {
        if(jumpsObjects.Contains(obj))
            jumpsObjects.Remove(obj);
        //Debug.Log("asd");
    }
}
