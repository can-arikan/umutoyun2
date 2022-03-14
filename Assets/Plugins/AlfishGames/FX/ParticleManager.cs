using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("AlfishGames/FX/Particle Manager")]
public class ParticleManager : MonoBehaviour
{
    public static ParticleManager instance;

    [SerializeField] private GameObject[] particles;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void InstantiateParticle(Particle particle)
    {
        GameObject go = Instantiate(particles[(int)particle], transform.position, Quaternion.identity);
    }
    public void InstantiateParticle(int index)
    {
        GameObject go = Instantiate(particles[index], transform.position, Quaternion.identity);
    }

    public void ParticleSetActive(Particle particle, bool value)
    {
        particles[(int)particle].SetActive(value);
    }

    public void ParticleSetActive(int index, bool value)
    {
        particles[index].SetActive(value);
    }

    public enum Particle { WinParticle }
}
