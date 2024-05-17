using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class MegaEventController : NetworkBehaviour
{
    public GameObject[] mobPrefabs;
    public List<Transform> mobList;
    public int stage;
    private float timer;
    private float timer2;
    public bool eventEnable;
    public Transform startPosition;
    private Transform _transform;
    public int monstersCount;
    public ChatOmegaLul chat;
    public int lethatCount;
    public int waves = 3;
    void Start()
    {
        _transform = transform;
    }
    [ServerCallback]
    void Update()
    {
        if (!eventEnable) return;
        if (stage < waves)
        {
            timer += Time.deltaTime;
            timer2 += Time.deltaTime;
            if (timer2 > stage * 30 && timer >= 1f)
            {
                GameObject currentMob = Instantiate(mobPrefabs[stage], startPosition.position + new Vector3(Random.Range(-1,1),0, Random.Range(-1, 1)), _transform.rotation);
                //mobList.Add(currentMob.transform);
                AI ai = currentMob.GetComponent<AI>();
                ai.eventPos = _transform.position;
                NetworkServer.Spawn(currentMob);
                timer = 0;
                monstersCount++;
            }

            if (monstersCount >= 1f + 0.05f * stage)
            {
                monstersCount = 0;
                stage++;
            }
        }
        if(stage >= waves && mobList.Count == 0)
        {
            eventEnable = false;
            chat.RpcSendMessageInGlobalChat(uint.MaxValue, "","<color=red>Вы защитили город!</color>");
        }
       /* for(int i = 0; i < mobList.Count; i++)
        {
            if (!mobList[i])
                mobList.RemoveAt(i);
            if (Vector3.Distance(mobList[i].position, _transform.position) < 1)
            {
                NetworkServer.Destroy(mobList[i].gameObject);
                NetworkServer.UnSpawn(mobList[i].gameObject);
                Destroy(mobList[i].gameObject);
                mobList.RemoveAt(i);
                lethatCount++;
                break;
            }
        } */
        if (lethatCount > 60)
        {
            eventEnable = false;
            chat.RpcSendMessageInGlobalChat(uint.MaxValue, "", "<color=red>Орки захватили город!</color>");
        }

    }
    public void StartEvent()
    {
        eventEnable = true;
        timer = 0;
        timer2 = 0;
        lethatCount = 0;
        stage = 0;
        monstersCount = 0;
        chat.RpcSendMessageInGlobalChat(uint.MaxValue, "", "<color=red>Орки нападают на город!</color>");
    }
}
