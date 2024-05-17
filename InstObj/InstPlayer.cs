
using UnityEngine;
using Mirror;
using Mirror.Authenticators;

public class InstPlayer : MonoBehaviour
{
    public delegate void PlayerInstance();
    public static event PlayerInstance OnPlayerInstanceEvent;
    void Start()
    {
        if (GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            
            GOManager.player = gameObject;
            GOManager.playerAnim = GetComponent<Animator>();
            GOManager.playerDS = GetComponent<DialogueSystem>();
            GOManager.playerTransform = GetComponent<Transform>();
            GOManager.playerSS = GetComponent<StatsSystem>();

            GOManager.AccountName = BasicAuthenticator.username;
            InventorySystem inventorySystem = GOManager.playerSS.inventorySystem;
            MoveControll moveControll = GOManager.playerSS.moveControll;
            GOManager.playerSS.CmdGetStatsInDataBase(GOManager.AccountName);
            GOManager.playerHpUIGo.SetActive(true);
            GOManager.playerSS.CmdSetNickName(GOManager.AccountName);
            inventorySystem.CmdGetItemInDataBase(GOManager.AccountName);
            GOManager.playerSS.CmdLoadCustomization(GOManager.AccountName);
            moveControll.CmdGetTransformInData(GOManager.AccountName);
            GOManager.minimap.SetActive(true);
            OnPlayerInstanceEvent?.Invoke();
        }
        Destroy(this);
    }

}
