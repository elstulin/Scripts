using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CombatSystem : NetworkBehaviour
{
    public Transform target;
    public StatsSystem targetss;
    public MoveControll MC;
    public LayerMask lm;
    public StatsSystem stats;
    public TargetSystem targetSystem;
    public List<GameObject> summonedCharacters = new List<GameObject>();
    public bool beliar;
    Transform _transform;
    public WeaponController weaponController;
    public GameObject arrow;
    public StatsSystem enemy;
    float atsd;
    //public bool hit;
    private SoundContoll soundContoll;
    private InventorySystem inventorySystem;
    NetworkIdentity networkIdentity;
    public Vector3 weaponForce;
    public bool allyMode;

    void Start()
    {
        
        _transform = transform;
        inventorySystem = GetComponent<InventorySystem>();
        weaponController = GetComponent<WeaponController>();
        soundContoll = GetComponent<SoundContoll>();
        networkIdentity = GetComponent<NetworkIdentity>();
        MC = GetComponent<MoveControll>();
        stats = GetComponent<StatsSystem>();
        targetSystem = GetComponent<TargetSystem>();
        if (!networkIdentity.isLocalPlayer) enabled = false;
    }
    void Update()
    {

        if (target && (stats.wpnrdy | !stats.humanoid) && Input.GetKey(KeyCode.Mouse0))
        {
            // MC.lookeTarget = target.position;
            if (targetss.minHealth <= 0)
                target = null;
        }
        else
        {
            // MC.lookeTarget = new Vector3(0,0,0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
           

            allyMode = !allyMode;
            if (allyMode)
            {
                ChatOmegaLul.instance.TakeSystemMessage("<color=#ff0008>[Система]: ПвП режим выключен</color>");
            }
            else
            {
                ChatOmegaLul.instance.TakeSystemMessage("<color=#ff0008>[Система]: ПвП режим включен</color>");
            }
        }
        //  weaponForce = (weaponController.weaponOldPos - (weaponController.wpn1Pos.position + weaponController.wpn1Pos.forward)).normalized;
        //  weaponController.weaponOldPos = Vector3.Lerp(weaponController.weaponOldPos, weaponController.wpn1Pos.position + weaponController.wpn1Pos.forward,0.8f);

    }
    public void DealDamage()
    {
        if (!networkIdentity.isLocalPlayer) return;
        enemy = null;
        if (stats.wpntype <= 1)
        {


            StatsSystem mainTargetStats = null;
            if (target)
                mainTargetStats = target.GetComponent<StatsSystem>();

            enemy = mainTargetStats;
            int dmg1 = 0;
            int dmg2 = 0;
            int dmg3 = 0;
            int dmg4 = 0;

            dmg1 = (int)((float)(stats.weaponDamage + stats.strength) * ((inventorySystem.EquipedItemWeapon.durability > 100) ? (inventorySystem.EquipedItemWeapon.durability / 100) : Mathf.Clamp((inventorySystem.EquipedItemWeapon.durability + 10) / 100, 0, 1)));
            dmg3 = (stats.weaponFireDamage);
            dmg4 = (stats.weaponMagicDamage);
            int critChance = 0;
            if (stats.wpntype == 0)
                critChance = stats.skill1h;
            if (stats.wpntype == 1)
                critChance = stats.skill2h;


            /*if (targetStats&&inventorySystem.EquipedItemWeapon.beliar )
            {
                if (Random.Range(0, 100) < 24)
                    targetStats.TakeBeliarHit();
            }*/

            int iHits = 0;
            for (int i = 0; i < GOManager.worldChars.Count; i++)
            {
                if (iHits >= 10) return;
                Transform trTarget = GOManager.worldChars[i].transform;

                StatsSystem targetStats = GOManager.worldChars[i];
                if ((allyMode && stats.type != targetStats.type) | (!allyMode))
                    if (targetStats.minHealth > 0)
                    {
                        float dis = Vector3.Distance(trTarget.position, _transform.position);
                        float dot = Vector3.Dot(trTarget.position - _transform.position, _transform.forward);
                        if (trTarget != _transform && dis <= stats.rangeAttack + 1.75f && dot > 0.5f)
                        {

                            dmg1 *= (int)(10f / (10f + iHits));
                            iHits++;
                            atsd -= 0.5f;
                            bool crit = false;
                            float crtforce = 1;
                            if (Random.Range(0, 101) < critChance)
                            {
                                crit = true;
                                crtforce = 1.1f;
                            }
                            if (targetStats.rigidbodyPart)
                            {
                                if (MC.animator.GetInteger("AttackRnd") == 2)
                                    targetStats.rigidbodyPart.AddForce(_transform.right * -dmg1 / targetStats.maxHealth * 600000 * crtforce, ForceMode.Force);
                                if (MC.animator.GetInteger("AttackRnd") == 1)
                                    targetStats.rigidbodyPart.AddForce(_transform.right * dmg1 / targetStats.maxHealth * 600000 * crtforce, ForceMode.Force);
                                if (MC.animator.GetInteger("AttackRnd") == 0)
                                    targetStats.rigidbodyPart.AddForce(_transform.forward * dmg1 / targetStats.maxHealth * 600000 * crtforce, ForceMode.Force);
                                // targetStats.rigidbodyPart.AddForce(weaponForce * -dmg1 / targetStats.maxHealth * 200000, ForceMode.Force);
                            }
                            for (int q = 0; q < targetStats.BodyPartColliders.Count; q++)
                            {
                                if (MC.animator.GetInteger("AttackRnd") == 2)
                                    targetStats.BodyPartColliders[q].attachedRigidbody.AddForce(_transform.right * -dmg1 / targetStats.maxHealth * 6000 * crtforce, ForceMode.Force);
                                if (MC.animator.GetInteger("AttackRnd") == 1)
                                    targetStats.BodyPartColliders[q].attachedRigidbody.AddForce(_transform.right * dmg1 / targetStats.maxHealth * 6000 * crtforce, ForceMode.Force);
                                if (MC.animator.GetInteger("AttackRnd") == 0)
                                    targetStats.BodyPartColliders[q].attachedRigidbody.AddForce(_transform.forward * dmg1 / targetStats.maxHealth * 6000 * crtforce, ForceMode.Force);
                                // targetStats.rigidbodyPart.AddForce(weaponForce * -dmg1 / targetStats.maxHealth * 200000, ForceMode.Force);
                            }
                            targetStats.CmdTakeDamage(dmg1, dmg2, dmg3, dmg4, crit, gameObject.GetComponent<NetworkIdentity>().netId);
                        }

                    }

            }
            // targetStats.CmdTakeDamage(dmg1, dmg2,dmg3, dmg4, gameObject.GetComponent<NetworkIdentity>().netId);
        }
        if (target)
        {
            if (stats.wpntype == 2)
            {
                for (int i = 0; i < inventorySystem.items.Count; i++)
                {
                    if (inventorySystem.items[i]._name == "Стрела")
                    {
                        inventorySystem.MinusCount(inventorySystem.items[i]);
                        break;
                    }
                }
                CmdSpawnArrow("Arrow", weaponController.wpn2.transform.position, Quaternion.Euler(weaponController.wpn2.transform.eulerAngles.x + 90, weaponController.wpn2.transform.eulerAngles.y + 90, weaponController.wpn2.transform.eulerAngles.z), _transform.position + GOManager.MainCamera.transform.forward * 44);

            }
            else
            if (stats.wpntype == 3)
            {
                int mana = inventorySystem.EquipedItemWeaponRune.mana;
                if (stats.minMana >= mana)
                {
                    stats.CmdManaLose();
                    byte effect = inventorySystem.EquipedItemWeaponRune.summonId;
                    byte effect2 = inventorySystem.EquipedItemWeaponRune.morphId;
                    if (effect > 0)
                    {
                        CmdSummon(effect);
                    }
                    else
                    {
                        if (effect2 > 0)
                        {
                            CmdMorphSpell(effect2);

                        }
                        else
                            CmdSpawnMagicProjectile("FireBall 1", weaponController.wpn3.transform.position, Quaternion.Euler(weaponController.wpn3.transform.eulerAngles.x + 90, weaponController.wpn3.transform.eulerAngles.y + 90, weaponController.wpn3.transform.eulerAngles.z), _transform.position + GOManager.MainCamera.transform.forward * 44);
                    }
                }

            }
        }
        else
        {
            if (stats.wpntype <= 1)
            {
                RaycastHit raycastHit;
                if (Physics.Linecast(weaponController.wpn1.transform.position, weaponController.wpn1.transform.position + weaponController.wpn1.transform.forward * weaponController.statsSystem.rangeAttack, out raycastHit))
                {
                    int check = soundContoll.CheckWorldMaterial(raycastHit);
                    if (check == 2)
                    {
                        GameObject go2 = Instantiate(Resources.Load("BulletImpactStoneEffect"), raycastHit.point, Quaternion.LookRotation(raycastHit.normal)) as GameObject;
                        stats.soundContoll.audioSource.PlayOneShot(Resources.Load("Sounds/SFX/CS_IHL_WO_ST") as AudioClip);
                        //hit = false;
                    }
                    if (check == 3)
                    {
                        GameObject go2 = Instantiate(Resources.Load("BulletImpactWoodEffect"), raycastHit.point, Quaternion.LookRotation(raycastHit.normal)) as GameObject;
                        stats.soundContoll.audioSource.PlayOneShot(Resources.Load("Sounds/SFX/CS_IHL_WO_WO") as AudioClip);
                        //hit = false;
                    }
                    if (check == 4)
                    {
                        GameObject go2 = Instantiate(Resources.Load("SparksEffect"), raycastHit.point, Quaternion.LookRotation(raycastHit.normal)) as GameObject;
                        stats.soundContoll.audioSource.PlayOneShot(Resources.Load("Sounds/SFX/CS_IHL_WO_ME") as AudioClip);
                        //hit = false;
                    }
                }
            }else
            if (stats.wpntype == 2)
            {
                for (int i = 0; i < inventorySystem.items.Count; i++)
                {
                    if (inventorySystem.items[i]._name == "Стрела")
                    {
                        inventorySystem.MinusCount(inventorySystem.items[i]);
                        break;
                    }
                }

                CmdSpawnArrow("Arrow", weaponController.wpn2.transform.position, Quaternion.Euler(weaponController.wpn2.transform.eulerAngles.x + 90, weaponController.wpn2.transform.eulerAngles.y + 90, weaponController.wpn2.transform.eulerAngles.z), GOManager.MainCamera.transform.position + GOManager.MainCamera.transform.forward * 44);
            }else
            if (stats.wpntype == 3)
            {
                int mana = inventorySystem.EquipedItemWeaponRune.mana;
                if (stats.minMana >= mana)
                {
                    stats.CmdManaLose();
                    byte effect = inventorySystem.EquipedItemWeaponRune.summonId;
                    byte effect2 = inventorySystem.EquipedItemWeaponRune.morphId;
                    if (effect > 0)
                    {
                        CmdSummon(effect);
                    }
                    else
                    {
                        if (effect2 > 0)
                        {
                            CmdMorphSpell(effect2);

                        }
                        else
                            CmdSpawnMagicProjectile("FireBall 1", weaponController.wpn3.transform.position, Quaternion.Euler(weaponController.wpn3.transform.eulerAngles.x + 90, weaponController.wpn3.transform.eulerAngles.y + 90, weaponController.wpn3.transform.eulerAngles.z), GOManager.MainCamera.transform.position + GOManager.MainCamera.transform.forward * 44);
                    }
                }
            }

        }

    }
    [Command(requiresAuthority = false)]
    public void CmdSpawnObjOnServer1(string name, Vector3 pos, Quaternion rot)
    {

        RpcSpawnObjOnServer1(name, pos, rot);
    }
    [ClientRpc]
    public void RpcSpawnObjOnServer1(string name, Vector3 pos, Quaternion rot)
    {
        GameObject gg = Resources.Load(name) as GameObject;
        GameObject go = Instantiate(gg, pos, rot);
        go.transform.LookAt(_transform.position + _transform.forward * 10 + new Vector3(0, 2, 0));
        ArrowMover am = go.GetComponent<ArrowMover>();
        am.go = gameObject;
    }
    [Command(requiresAuthority = false)]
    public void CmdSpawnArrow(string name, Vector3 pos, Quaternion rot, Vector3 targetPos)
    {

        RpcSpawnArrow(name, pos, rot, targetPos);
        GameObject gg = Resources.Load(name) as GameObject;
        GameObject go = Instantiate(gg, pos, rot);
        ArrowMover am = go.GetComponent<ArrowMover>();
        am.go = gameObject;
        am.dmg = stats.weaponDamage + stats.dex;
        am.skillbow = stats.skillbow;
        go.transform.LookAt(targetPos);
    }
    [ClientRpc]
    public void RpcSpawnArrow(string name, Vector3 pos, Quaternion rot, Vector3 targetPos)
    {
        GameObject gg = Resources.Load(name) as GameObject;
        GameObject go = Instantiate(gg, pos, rot);
        ArrowMover am = go.GetComponent<ArrowMover>();
        am.go = gameObject;
        //am.dmg = stats.weaponDamage + stats.dex;
        am.skillbow = stats.skillbow;
        go.transform.LookAt(targetPos); // + new Vector3(0, ds, 0)
    }
    [Command(requiresAuthority = false)]
    public void CmdSpawnMagicProjectile(string name, Vector3 pos, Quaternion rot, Vector3 targetPos)
    {

        RpcSpawnMagicProjectile(name, pos, rot, targetPos);
        GameObject gg = Resources.Load(name) as GameObject;
        GameObject go = Instantiate(gg, pos, rot);
        ArrowMover am = go.GetComponent<ArrowMover>();
        am.go = gameObject;
        am.dmgFire = stats.weaponFireDamage;
        go.transform.LookAt(targetPos);
    }
    [ClientRpc]
    public void RpcSpawnMagicProjectile(string name, Vector3 pos, Quaternion rot, Vector3 targetPos)
    {
        GameObject gg = Resources.Load(name) as GameObject;
        GameObject go = Instantiate(gg, pos, rot);
        ArrowMover am = go.GetComponent<ArrowMover>();
        am.go = gameObject;
        // am.dmgFire = stats.weaponFireDamage;
        go.transform.LookAt(targetPos); // + new Vector3(0, ds, 0)
    }
    [Command(requiresAuthority = false)]
    public void CmdSummon(byte id)
    {
        GameObject go = null;
        for (int i = 0; i < summonedCharacters.Count; i++)
        {
            if (summonedCharacters[i] == null)
            {
                summonedCharacters.Remove(summonedCharacters[i]);
            }
        }
        if (summonedCharacters.Count >= 3)
        {
            StatsSystem statsGo = summonedCharacters[0].GetComponent<StatsSystem>();
            if (statsGo.minHealth > 0)
                statsGo.TakeDamage(statsGo.maxHealth, statsGo.maxHealth, statsGo.maxHealth, statsGo.maxHealth, true, summonedCharacters[0]);
            summonedCharacters.Remove(summonedCharacters[0]);
        }
        if (id == 1)
        {
            go = Instantiate(Resources.Load("Малый скелет") as GameObject, _transform.position + _transform.forward + new Vector3(Random.Range(-0.2f, 0.2f), 0, Random.Range(-0.2f, 0.2f)), new Quaternion(0, 0, 0, 0));
        }
        else
        if (id == 2)
        {
            go = Instantiate(Resources.Load("Скелет") as GameObject, _transform.position + _transform.forward + new Vector3(Random.Range(-0.2f, 0.2f), 0, Random.Range(-0.2f, 0.2f)), new Quaternion(0, 0, 0, 0));
        }
        else
        if (id == 3)
        {
            go = Instantiate(Resources.Load("Скелет воин") as GameObject, _transform.position + _transform.forward + new Vector3(Random.Range(-0.2f, 0.2f), 0, Random.Range(-0.2f, 0.2f)), new Quaternion(0, 0, 0, 0));
        }
        else
        if (id == 4)
        {
            go = Instantiate(Resources.Load("Demon") as GameObject, _transform.position + _transform.forward + new Vector3(Random.Range(-0.2f, 0.2f), 0, Random.Range(-0.2f, 0.2f)), new Quaternion(0, 0, 0, 0));
        }

        go.GetComponent<AI>().companion = _transform;
        go.GetComponent<StatsSystem>().type = stats.type;
        summonedCharacters.Add(go);
        NetworkServer.Spawn(go);
        IEnumerator coroutine = destroySummon(go, 300);
        StartCoroutine(coroutine);

    }
    IEnumerator destroySummon(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        if (go)
        {
            summonedCharacters.Remove(go);
            StatsSystem statsGo = go.GetComponent<StatsSystem>();
            statsGo.TakeDamage(statsGo.maxHealth, statsGo.maxHealth, statsGo.maxHealth, statsGo.maxHealth, true, go);
        }
    }



    [Command(requiresAuthority = false)]
    public void CmdMorphSpell(byte id, NetworkConnectionToClient conn = null)
    {
        if (id == 1)
        {
            GameObject go = Instantiate(Resources.Load("ПадальщикMorph") as GameObject, _transform.position, _transform.rotation);
            float rot = GetComponent<MoveControll>().rotation;
            CombatSystem newCombatSystem = go.GetComponent<CombatSystem>();
            newCombatSystem.GetComponent<MoveControll>().rotation = rot;
            IEnumerator coroutine = newCombatSystem.returnToHuman(100, conn);
            newCombatSystem.StartCoroutine(coroutine);
            GameObject oldPlayer = conn.identity.gameObject;
            NetworkServer.ReplacePlayerForConnection(conn, go);
            NetworkServer.Destroy(oldPlayer);
        }
        if (id == 2)
        {
            GameObject go = Instantiate(Resources.Load("snapperMorph") as GameObject, _transform.position, _transform.rotation);
            float rot = GetComponent<MoveControll>().rotation;
            CombatSystem newCombatSystem = go.GetComponent<CombatSystem>();
            newCombatSystem.GetComponent<MoveControll>().rotation = rot;
            IEnumerator coroutine = newCombatSystem.returnToHuman(150, conn);
            newCombatSystem.StartCoroutine(coroutine);
            GameObject oldPlayer = conn.identity.gameObject;
            NetworkServer.ReplacePlayerForConnection(conn, go);
            NetworkServer.Destroy(oldPlayer);
        }
        if (id == 3)
        {
            GameObject go = Instantiate(Resources.Load("waranMorph") as GameObject, _transform.position, _transform.rotation);
            float rot = GetComponent<MoveControll>().rotation;
            CombatSystem newCombatSystem = go.GetComponent<CombatSystem>();
            newCombatSystem.GetComponent<MoveControll>().rotation = rot;
            IEnumerator coroutine = newCombatSystem.returnToHuman(100, conn);
            newCombatSystem.StartCoroutine(coroutine);
            GameObject oldPlayer = conn.identity.gameObject;
            NetworkServer.ReplacePlayerForConnection(conn, go);
            NetworkServer.Destroy(oldPlayer);
        }
    }
    public IEnumerator returnToHuman(float time, NetworkConnectionToClient conn)
    {
        yield return new WaitForSeconds(time);

        GameObject go = Instantiate(NetworkManager.singleton.playerPrefab, _transform.position, _transform.rotation);
        float rot = GetComponent<MoveControll>().rotation;
        CombatSystem newCombatSystem = go.GetComponent<CombatSystem>();
        newCombatSystem.GetComponent<MoveControll>().rotation = rot;
        GameObject oldPlayer = conn.identity.gameObject;
        NetworkServer.ReplacePlayerForConnection(conn, go);
        NetworkServer.Destroy(oldPlayer);
        newCombatSystem.TargetRpcSetPlayerStats(conn);

    }
    [TargetRpc]
    public void TargetRpcSetPlayerStats(NetworkConnection target)
    {

        Invoke("Vo", 0.1f);
    }
    void Vo()
    {
        InventorySystem inventorySystem = GOManager.player.GetComponent<InventorySystem>();
        StatsSystem ss = GOManager.player.GetComponent<StatsSystem>();
        ss.CmdGetStatsInDataBase(GOManager.AccountName);
        inventorySystem.CmdGetItemInDataBase(GOManager.AccountName);
    }
}
