using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyRoutine : MonoBehaviour
{
    private AI ai;
    public Transform[] patrolWays;
    private float patrolTimer;
    private uint patrolWayIndex;
    void Start()
    {
        ai = GetComponent<AI>();
    }

    void Update()
    {
        if (patrolTimer > 6)
        {
            patrolWayIndex++;
            if (patrolWayIndex >= patrolWays.Length)
                patrolWayIndex = 0;
            patrolTimer = 0;
            ai.startPosRnd = patrolWays[patrolWayIndex].position;
        }
        else
            patrolTimer += Time.deltaTime;
    }
}
