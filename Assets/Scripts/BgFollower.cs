using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgFollower : MonoBehaviour
{
    //[SerializeField] Transform target;
    public Transform target;
    public float offSet;

    private void Update()
    {
        transform.position = new Vector3(target.position.x, target.position.y + offSet, transform.position.z);
    }
}
