using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip battle;
    public AudioClip idle;
    public float timer;
    public bool trig;
    public float volume = 0.05f;
    private void OnEnable()
    {
        InstPlayer.OnPlayerInstanceEvent += InstancePlayer;
    }
    void InstancePlayer()
    {
        audioSource.clip = idle;
        audioSource.Play();
    }
    void Update()
    {
        if (Time.frameCount % 2 != 0) return;

        if (timer >= volume)
        {
            if (GOManager.attacked > 0)
            {
                if (!trig)
                {
                    audioSource.clip = battle;
                    audioSource.Play();
                    timer = 0;
                    trig = true;
                }
            }
            else
            if (trig)
            {
                audioSource.clip = idle;
                audioSource.Play();
                timer = 0;
                trig = false;
            }
        }
        else
        {
            timer += Time.deltaTime * 0.5f;
            audioSource.volume = Mathf.Clamp(timer, 0, volume);
        }

    }
}
