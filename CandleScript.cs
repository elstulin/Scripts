using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CandleScript : MonoBehaviour
{
    //public float LightRange;
    public Light _light;
    public GameObject particle;
    public GameObject particle2;
    public GameObject particle3;
    new Transform transform;
    bool visible;

    private void OnEnable()
    {
        InstPlayer.OnPlayerInstanceEvent += OnPlayerInstance;
        NetworkManager.OnServerStart += OnServerStart;

    }
    void Awake()
    {
        visible = false;
        
        
        transform = GetComponent<Transform>();
        particle = transform.GetChild(0).gameObject;
        particle2 = transform.GetChild(1).gameObject;
        if (transform.childCount > 2)
            particle3 = transform.GetChild(2).gameObject;
        particle.SetActive(false);
        particle2.SetActive(false);
        if (particle3)
            particle3.SetActive(false);

    }
    void OnServerStart()
    {
#if !UNITY_EDITOR
        enabled = false;
#endif
    }
    void OnPlayerInstance()
    {
        StartCoroutine(DistanceCheck());
    }
    IEnumerator DistanceCheck()
    {
        byte i = 1;
        while (true)
        {
            yield return new WaitForSeconds(i);
            i++;
            if (i > 5) i = 5;
            short distanceToPlayer = (short)(((short)GOManager.playerTransform.position.x + (short)GOManager.playerTransform.position.z)
                - ((short)transform.position.x + (short)transform.position.z));
            if (distanceToPlayer < 0)
                distanceToPlayer = (short)-distanceToPlayer;


            if (distanceToPlayer <= 20)
            {
                if (!visible)
                {
                    particle.SetActive(true);
                    particle2.SetActive(true);
                    if (particle3)
                        particle3.SetActive(true);
                    // _light.enabled = true;
                    visible = true;
                    i = 1;
                }
            }
            else
            {
                if (visible)
                {
                    particle.SetActive(false);
                    particle2.SetActive(false);
                    if (particle3)
                        particle3.SetActive(false);
                    // _light.enabled = false;
                    visible = false;
                    i = 1;
                }
            }
            
        }

    }
}
