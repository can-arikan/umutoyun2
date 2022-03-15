using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarController : MonoBehaviour
{
    public ParticleSystem novaEffect;

    private void Update()
    {
        transform.Rotate(0, 5, 0);
    }
}
