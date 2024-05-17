using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRainSpawner : MonoBehaviour
{
    public GameObject fireBall;
    public GameObject owner;
    public int rainCount = 20;
    public int radius;
    private int curRainCount;
    private float timer;
    private float time;
    void Start()
    {
        
    }

    void Update()
    {
        time += Time.deltaTime;
        timer += Time.deltaTime;
        if (timer > 0.1f/(1- time))
        {
            timer = 0;
            curRainCount++;
         GameObject go =    Instantiate(fireBall,transform.position + new Vector3(Random.Range(-radius, radius),20, Random.Range(-radius, radius)),Quaternion.Euler(-270,0,0));
           
            ArrowMover arrowMover = go.GetComponent<ArrowMover>();
            arrowMover.go = owner;
            if(time>2)
            arrowMover.dmg -= (int)(time*5);
        }
        if(curRainCount >= rainCount)
        {
            Destroy(gameObject);
        }
    }
}
