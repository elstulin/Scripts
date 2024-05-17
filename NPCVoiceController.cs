using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCVoiceController : MonoBehaviour
{
   public float timer;


    void Update()
    {
        for(int i = 0;i< GOManager.npcVoiceArea.Count;i++)
        {
            GOManager.npcVoiceArea[i].timer += Time.deltaTime;
            if(GOManager.npcVoiceArea[i].timer >=3)
            {
                GOManager.npcVoiceArea.RemoveAt(i);
                break;
            }
        }
    }
}
