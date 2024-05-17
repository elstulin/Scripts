using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SummonCharacter : NetworkBehaviour
{
    public GameObject summonedCharacter;
    public Transform summoner;
    [Command(requiresAuthority = false)]
    public void CmdSummon(byte id)
    {
        if (id == 0)
        {
            summonedCharacter = GameObject.Instantiate(Resources.Load("Скелет воин") as GameObject, transform.position, transform.rotation);
        }
        summonedCharacter.GetComponent<AI>().companion = summoner;
        summonedCharacter.GetComponent<StatsSystem>().type = summoner.GetComponent<StatsSystem>().type;
        NetworkServer.Spawn(summonedCharacter);
    }
}
