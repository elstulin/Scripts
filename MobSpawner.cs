using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class MobSpawner : NetworkBehaviour
{
    public GameObject mobPrefab;
    private float timer;
    public GameObject currentMob;
    public float respawnTime = 1;
    public bool rndRotation = false;
    public int number;
    public Vector3[] rndPosition;
    void Start()
    {
        
        timer = respawnTime;

    }
    void Update()
    {
        if (!currentMob)
        {
            timer += Time.deltaTime;
            if(timer >= respawnTime)
            {
                currentMob = GameObject.Instantiate(mobPrefab,transform.position, (rndRotation) ? Quaternion.Euler(0,Random.Range(1,360),0) : transform.rotation);
                if (rndPosition.Length > 0)
                {
                    AIMonster aIMonster = currentMob.GetComponent<AIMonster>();
                    if(aIMonster)
                    aIMonster.rndPosition = rndPosition;
                }

                NetworkServer.Spawn(currentMob);
                timer = 0;
                StatsSystem ss = currentMob.GetComponent<StatsSystem>();
                if (ss)
                {
                    string newName = " id (" + number + ")";
                    ss._name += newName;
                }
            }
        }
    }
}
