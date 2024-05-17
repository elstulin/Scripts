using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public int id;
    public List<StatsSystem> statsSystems;
    void Start()
    {
        
    }
    void Update()
    {
        for(int i = 0; i < statsSystems.Count; i++)
        {
            if (statsSystems[i].minHealth < 1)
            {
                statsSystems.Remove(statsSystems[i]);
            }
        }
        if (id == 0)
        {
            if (statsSystems.Count == 0)
            {
                QuestSystem.BdtScavengersQuestSuff = true;
            }
        }
    }
}
