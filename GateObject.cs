using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateObject : MonoBehaviour
{
    Transform _transform;
    Collider _collider;
    AudioSource audioSource;
    public AudioClip start;
    public AudioClip loop;
    public AudioClip end;
    float timer;
    bool trig;
    void Start()
    {
        _transform = transform;
        _collider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(start,1);
        audioSource.clip = loop;
        audioSource.Play();
    }

    void Update()
    {
        
       
        if (timer >= 11)
        {
            if (!trig)
            {
                Destroy(_collider);
                audioSource.Stop();
                audioSource.PlayOneShot(end, 1);
                Destroy(audioSource, end.length);
                Destroy(this, end.length);
                
                trig = true;
            }
        }
        else
        {
            timer += Time.deltaTime;
            _transform.position += new Vector3(0, timer * 0.003f, 0);
        }
    }
}
