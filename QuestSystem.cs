using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class QuestSystem
{
    public static bool _DialogueKsardas1 = false;
    public static bool _DialogueKsardas2 = false;
    public static bool _DialogueKsardas3 = false;
    public static bool _DialogueKsardas4 = false;
    public static bool _DialogueKsardas5 = false;
    public static bool Bdt1 = false;
    public static bool Bdt2 = false;
    public static bool BdtFollowe = false;
    public static bool BdtScavengersQuestSuff = false;
    public static bool BdtScavengersQuestSuffD = false;


    public static bool guard1 = false;
    public static bool guard2 = false;
    public static bool guard3 = false;

    public static bool guard10 = false;






    public static bool _meme1 = false;
    public static bool _meme2 = false;
    public static bool _meme3 = false;
    public static bool _meme4 = false;
    public static bool _meme5 = false;
    public static bool _meme6 = false;
    public static bool _meme7 = false;
    public static bool _meme8 = false;
    public static bool _meme9 = false;
    public static bool _meme10 = false;




    public static bool _DialogueMaleth1 = false;
    public static bool _DialogueMaleth2 = false;
    public static bool _DialogueMaleth3 = false;
    public static bool _DialogueMaleth4 = false;
    public static bool _DialogueMaleth5 = false;
    public static bool _DialogueMaleth6 = false;
    public static bool _DialogueMaleth7 = false;
    public static bool _DialogueMaleth8 = false;
    public static bool _DialogueMaleth9 = false;
    public static bool _DialogueMaleth10 = false;
    public static bool _DialogueMalethKnowWhereBandits = false;
    public static string MalethBanditsLOG = "";
    public static StatsSystem Brago;


    public static bool _DialogueCavalorn1 = false;
    public static bool _DialogueCavalorn2 = false;
    public static bool _DialogueCavalorn3 = false;
    public static bool _DialogueCavalorn4 = false;
    public static bool _DialogueCavalorn5 = false;
    public static bool _DialogueCavalorn6 = false;
    public static bool _DialogueCavalorn7 = false;
    public static bool _DialogueCavalorn8 = false;
    public static bool _DialogueCavalorn9 = false;
    public static bool _DialogueCavalorn10 = false;
    public static bool _DialogueCavalorn11 = false;
    public static bool _DialogueCavalorn12 = false;
    public static bool _DialogueCavalorn13 = false;
    public static bool _DialogueCavalorn14 = false;

    public static bool _DialogueBndt1013Angry = false;
    public static bool Npc_IsDead(string name)
    {
        if(name == "Браго")
        {
            if (Brago)
            {
                if (Brago.minHealth <= 0)
                    return true;
            }
            else return true;

        }
        return false;
    }
}
