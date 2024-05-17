using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNpcInst : MonoBehaviour
{
    void Start()
    {
        StatsSystem ss = GetComponent<StatsSystem>();
        string nme = ss._name;
        if (nme == "Браго")
        {
            QuestSystem.Brago = ss;
        }

    }
}
