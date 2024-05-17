using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript1 : MonoBehaviour
{
    public float timer;
    public Transform _transform;
    public ParticleSystem _particleSystem;
    void Start()
    {
        _transform = transform;
        Debug.Log(name);
    }

    void Update()
    {
        timer += Time.deltaTime;
        _transform.localScale += new Vector3(0.1f* timer, 0.1f * timer, 0.1f * timer);
        _particleSystem.emissionRate = 100+ timer * 1000;

    }
}
