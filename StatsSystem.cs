using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
public class StatsSystem : NetworkBehaviour
{
    [System.Serializable]
    public class ItemDrop
    {
        public Item item;
        public float chanceToDrop = 50;
        public int countMin = 1;
        public int countMax = 1;
        public ItemDrop(Item item, float chanceToDrop, int countMin, int countMax)
        {
            this.item = item;
            this.chanceToDrop = chanceToDrop;
            this.countMin = countMin;
            this.countMax = countMax;
        }

    }
    public string _name;
    public int minHealth;
    public int health;
    public int maxHealth;
    public int minMana;
    public int maxMana;
    public int strength;
    public int dex;
    public int weaponDamage;
    public int weaponFireDamage;
    public int weaponMagicDamage;
    public int[] ringStr = new int[2];
    public int[] ringDex = new int[2];
    public int[] ringHp = new int[2];
    public int[] ringMp = new int[2];
    public int skill1h;
    public int skill2h;
    public int skillbow;
    public int skillcrossbow;
    public int armor;
    public int arrowdef;
    public int firedef;
    public int magicdef;
    public float rangeAttack;
    public int exp;
    public int expToLvl;
    public int lvl;
    public int lvlPoints;
    public Animator animator;
    public enum Type
    {
        Neutral,
        Mercenary,
        Paladin,
        Mage,
        Bandit,
        BanditQuest,
        Creature,
        Orc,
        Undead,
        Golem,
        GodMode,
    };
    public Type type;
    public bool evade;
    public bool block;
    public AudioSource audioSource;
    public AudioClip[] hitSounds;
    public SoundContoll soundContoll;
    public MoveControll moveControll;
    public SoundMonsters soundMonsters;
    public InventorySystem inventorySystem;
    public WeaponController weaponController;
    public NetworkIdentity networkIdentity;
    public NetworkAnimator networkAnimator;
    public AudioClip audioLvlUp;
    public List<ItemDrop> itemDrops;
    public List<Item> itemsDrop;
    public List<StatsSystem> damagedFrom;
    public List<int> damageFrom;
    public bool humanoid;
    public bool blood = true;
    public bool PLAYER;
    public int wpntype;
    public bool wpnrdy;
    public int[] wpnEqpdType = new int[2];
    public bool beliar;
    private bool trig1 = true;
    public Transform _transform;
    private Transform Player;
    int anim1 = Animator.StringToHash("Bottom.1h3attackleft");
    int anim2 = Animator.StringToHash("Bottom.1h3attackfront");
    int anim3 = Animator.StringToHash("Bottom.1h3attackcombo1");
    int anim4 = Animator.StringToHash("Bottom.1h3attackcombo2");
    int anim5 = Animator.StringToHash("Bottom.1h3attackcombo3");
    private float respawnTimer;
    private float deathTimer;
    [SyncVar] public bool god;
    public bool orc;
    public List<Collider> BodyPartColliders = new List<Collider>();
    public Rigidbody rigidbodyPart;
    private Rigidbody rigidbody;
    public GameObject[] SeveredParts;
    public Transform[] SeveredPartsChar;
    public Transform[] SeveredPartsChar2;
    public float RegenTime;
    public float weight = 0;
    public float size = 1;
    public List<FatnesMesh> fatnesMeshes = new List<FatnesMesh>();
    public GameObject[] BodyMeshes;
    public byte currentBody;
    public GameObject[] HeadMeshes;
    public byte currentHead;
    public static bool firstLogin = true;
    public uint partyId;
    public uint guildId;
    CharacterController characterController;
    public StatsSystem Copy()
    {
        return (StatsSystem)this.MemberwiseClone();
    }
    private void OnDestroy()
    {
        GOManager.worldChars.Remove(this);
        int inx = GOManager.enemysPos.IndexOf(_transform);
        if (inx > -1)
        {
            Destroy(GOManager.enemysPosUI[inx].gameObject);
            GOManager.enemysPosUI.RemoveAt(inx);
            GOManager.enemysPos.RemoveAt(inx);
        }
    }
    private void Update()
    {

        if (Time.frameCount % 2 != 0) return;
        if (isServer)
        {

            if (minHealth <= 0)
            {
                respawnTimer += Time.deltaTime * 2;
                if (respawnTimer >= 60)
                    DestroyItemOnServer(this.GetComponent<NetworkIdentity>().netId);
            }
            else
            {
                RegenTime += Time.deltaTime * 2;
                if (blood && RegenTime >= 500f / maxHealth)
                {
                    minHealth += 1;
                    if (minHealth >= maxHealth)
                    {
                        minHealth = maxHealth;
                    }
                    RpcHpChange(minHealth, maxHealth);
                    RegenTime = 0;
                }
            }
        }
        /*if (GOManager.player)
        {
            short distanceToPlayer = (short)(((short)GOManager.playerTransform.position.x + (short)GOManager.playerTransform.position.z)
               - ((short)_transform.position.x + (short)_transform.position.z));
            if (distanceToPlayer < 0)
                distanceToPlayer = (short)-distanceToPlayer;


            if (distanceToPlayer <= 30)
            {
                if (!trig1)
                {
                    animator.enabled = true;
                    trig1 = true;
                }
            }
            else
            {
                if (trig1)
                {
                    animator.enabled = false;
                    trig1 = false;
                }
            }
        }*/



        if (networkIdentity.isLocalPlayer)
        {

            if (minHealth <= 0)
            {
                respawnTimer += Time.deltaTime * 2;
                if (respawnTimer >= 4)
                {
                    minHealth = maxHealth / 4;
                    CmdRespawnPlayer1();
                }
            }
        }

    }
    [Command(requiresAuthority = false)]
    public void CmdWpnReadySync(bool wpnrdy)
    {
        this.wpnrdy = wpnrdy;
    }
    [Command(requiresAuthority = false)]
    public void CmdRespawnPlayer1()
    {
        minHealth = maxHealth / 4;
        RpcHpChange(minHealth, maxHealth);
        RpcRespawnPlayer();
        animator.enabled = true;

        characterController.enabled = true;

        //transform.position = new Vector3(-1021.396f, 8.473858f, 40.92551f);
        //transform.position += new Vector3(0, 0.2f, 0);


    }
    public void Teleportation(Vector3 position, float rotation)
    {
        _transform.position = position;
        _transform.rotation = Quaternion.Euler(0, rotation, 0);
        moveControll.rotation = rotation;
        CameraController.instance.cinemachineFreeLook.enabled = false;
        CameraController.instance._transform.position = _transform.position + _transform.TransformDirection(new Vector3(0, 5, -10));
        CameraController.instance._transform.LookAt(_transform.position);
        CameraController.instance.cinemachineFreeLook.enabled = true;
    }
    [ClientRpc]
    public void RpcRespawnPlayer()
    {
        animator.enabled = true;

        characterController.enabled = true;
        for (int i = 0; i < BodyPartColliders.Count; i++)
        {
            BodyPartColliders[i].isTrigger = true;
            BodyPartColliders[i].attachedRigidbody.isKinematic = true;

        }
        



        if (netIdentity.isLocalPlayer)
        {
            if (wpnrdy)
            {
                weaponController.UnEquip();
            }
            SetBoolAsTrigger("Resurrect", 1f);
            
            Teleportation(new Vector3(-720.37f, -45.55659f, -98.47f), -90f);
           moveControll.enabled = true;
            if (moveControll.swim is true)
            {
                moveControll.swim = false;
                animator.SetBool("Swimming", false);
            }
        }
    }
    [ClientRpc]
    public void RpcHpChange(int hp, int maxhp)
    {

        minHealth = hp;
        maxHealth = maxhp;
        //Debug.Log(hp + "/" + maxhp);
        if (networkIdentity.isLocalPlayer && PLAYER)
            GOManager.playerHpUIRectTransform.sizeDelta = new Vector2(((float)minHealth / maxHealth) * 186, 16);
    }
    [ClientRpc]
    public void RpcMpChange(int mp, int maxmp)
    {
        minMana = mp;
        maxMana = maxmp;
        if (networkIdentity.isLocalPlayer && PLAYER)
            GOManager.playerMpUI.sizeDelta = new Vector2(((float)minMana / maxMana) * 186, 16);
    }
    [Command(requiresAuthority = false)]
    public void CmdManaLose()
    {
        string name = moveControll.inventorySystem.EquipedWeaponRune.name;
        int manacost = (Resources.Load("Items/WorldItems/" + name.Remove(name.Length - 7, 7)) as GameObject).GetComponent<WorldItem>().item.mana;
        minMana -= manacost;
        RpcMpChange(minMana, maxMana);
    }
    [Command(requiresAuthority = false)]
    public void CmdSetRingHp(int[] hp)
    {
        ringHp[0] = hp[0];
        ringHp[1] = hp[1];
        minHealth += ringHp[0] + ringHp[1];
        maxHealth = health + ringHp[0] + ringHp[1] + 12 * lvl;
        RpcHpChange(minHealth, maxHealth);
    }
    public void Updatewpnstate()
    {

        if (orc) return;
        if (humanoid)
        {

            animator.SetBool("Bow", false);
            if (wpntype == 0)
            {
                animator.SetLayerWeight(1, 1);
                animator.SetLayerWeight(2, 0);
                animator.SetLayerWeight(3, 0);
                animator.SetLayerWeight(4, 0);
                animator.SetLayerWeight(5, 0);
                animator.SetLayerWeight(6, 0);
                animator.SetLayerWeight(8, 1);
                animator.SetLayerWeight(9, 0);
                animator.SetLayerWeight(10, 0);
                animator.SetLayerWeight(11, 0);
                animator.SetLayerWeight(12, 0);
                animator.SetLayerWeight(13, 0);
                animator.SetInteger("ComboCount", 2);

                if (skill1h >= 60)
                {
                    animator.SetLayerWeight(1, 0);
                    animator.SetLayerWeight(2, 0);
                    animator.SetLayerWeight(3, 1);
                    animator.SetInteger("ComboCount", 4);
                }
                else
                    if (skill1h >= 30)
                {
                    animator.SetLayerWeight(1, 0);
                    animator.SetLayerWeight(2, 1);
                    animator.SetLayerWeight(3, 0);
                    animator.SetInteger("ComboCount", 4);
                }


            }
            if (wpntype == 1)
            {
                animator.SetLayerWeight(1, 0);
                animator.SetLayerWeight(2, 0);
                animator.SetLayerWeight(3, 0);
                animator.SetLayerWeight(4, 1);
                animator.SetLayerWeight(5, 0);
                animator.SetLayerWeight(6, 0);
                animator.SetLayerWeight(8, 0);
                animator.SetLayerWeight(9, 1);
                animator.SetLayerWeight(10, 0);
                animator.SetLayerWeight(11, 0);
                animator.SetLayerWeight(12, 0);
                animator.SetLayerWeight(13, 0);
                animator.SetInteger("ComboCount", 2);
                if (skill2h >= 60)
                {
                    animator.SetLayerWeight(4, 0);
                    animator.SetLayerWeight(5, 0);
                    animator.SetLayerWeight(6, 1);
                    animator.SetInteger("ComboCount", 4);
                }
                else
                    if (skill2h >= 30)
                {
                    animator.SetLayerWeight(4, 0);
                    animator.SetLayerWeight(5, 1);
                    animator.SetLayerWeight(6, 0);
                    animator.SetInteger("ComboCount", 3);
                }
            }
            if (wpntype == 2)
            {
                animator.SetLayerWeight(1, 0);
                animator.SetLayerWeight(2, 0);
                animator.SetLayerWeight(3, 0);
                animator.SetLayerWeight(4, 0);
                animator.SetLayerWeight(5, 0);
                animator.SetLayerWeight(6, 0);
                animator.SetLayerWeight(8, 0);
                animator.SetLayerWeight(9, 0);
                animator.SetLayerWeight(10, 1);
                animator.SetLayerWeight(11, 1);
                animator.SetLayerWeight(12, 0);
                animator.SetLayerWeight(13, 0);
                animator.SetInteger("ComboCount", 1);
                animator.SetBool("Bow", true);
            }

            if (wpntype == 3)
            {
                animator.SetLayerWeight(1, 0);
                animator.SetLayerWeight(2, 0);
                animator.SetLayerWeight(3, 0);
                animator.SetLayerWeight(4, 0);
                animator.SetLayerWeight(5, 0);
                animator.SetLayerWeight(6, 0);
                animator.SetLayerWeight(8, 0);
                animator.SetLayerWeight(9, 0);
                animator.SetLayerWeight(10, 0);
                animator.SetLayerWeight(11, 0);
                animator.SetLayerWeight(12, 1);
                animator.SetLayerWeight(13, 1);
                animator.SetInteger("ComboCount", 1);
                animator.SetBool("Bow", true);
                if (GetComponent<InventorySystem>())
                    animator.SetInteger("CastType", GetComponent<InventorySystem>().EquipedItemWeaponRune.castType + 1);
            }
            else
                animator.SetInteger("CastType", 0);
        }


    }
    [Command(requiresAuthority = false)]
    public void CmdUpdateName()
    {

        RpcSetNickName(_name);
    }
    [Command(requiresAuthority = false)]
    public void CmdSetNickName(string nm)
    {
        _name = nm;
        RpcSetNickName(_name);
    }
    [ClientRpc]
    public void RpcSetNickName(string nm)
    {
        _name = nm;
    }
    private void Start()
    {

        weaponController = GetComponent<WeaponController>();
        animator = GetComponent<Animator>();
        networkAnimator = GetComponent<NetworkAnimator>();
        if (!orc && humanoid && animator)
        {

            Updatewpnstate();
        }

        ringStr = new int[2];
        ringDex = new int[2];
        ringHp = new int[2];
        ringMp = new int[2];

        GOManager.worldChars.Add(this);
        _transform = transform;
        moveControll = GetComponent<MoveControll>();
        wpnEqpdType = new int[3];
        soundContoll = GetComponent<SoundContoll>();
        rigidbody = GetComponent<Rigidbody>();
        soundMonsters = GetComponent<SoundMonsters>();
        inventorySystem = GetComponent<InventorySystem>();
        audioSource = GetComponent<AudioSource>();
        characterController = GetComponent<CharacterController>();
        if (hitSounds.Length == 0)
            hitSounds = new AudioClip[]
            {
            Resources.Load("Sounds/SFX/CS_IAM_UD_FL_01") as AudioClip,
            Resources.Load("Sounds/SFX/CS_IAM_UD_FL_02") as AudioClip,
            Resources.Load("Sounds/SFX/CS_IAM_UD_FL_03") as AudioClip,
            Resources.Load("Sounds/SFX/CS_IAM_UD_FL_04") as AudioClip,
            Resources.Load("Sounds/SFX/CS_IAM_UD_FL_05") as AudioClip
            }

            ;
        networkIdentity = GetComponent<Mirror.NetworkIdentity>();
        if (type == Type.Neutral)
        {
            GOManager.enemysPos.Add(_transform);
            GOManager.enemysPosUI.Add((Instantiate(Resources.Load("UI/NeutralOnMinimap"), new Vector3(0, 0, 0), Quaternion.identity, GOManager.minimap.transform) as GameObject).GetComponent<RectTransform>());
        }
        else
        if (type == Type.Mage | type == Type.Paladin | type == Type.Mercenary)
        {
            GOManager.enemysPos.Add(_transform);
            GOManager.enemysPosUI.Add((Instantiate(Resources.Load("UI/AllyOnMinimap"), new Vector3(0, 0, 0), Quaternion.identity, GOManager.minimap.transform) as GameObject).GetComponent<RectTransform>());
        }
        else
        {
            GOManager.enemysPos.Add(_transform);
            GOManager.enemysPosUI.Add((Instantiate(Resources.Load("UI/EnemyOnMinimap"), new Vector3(0, 0, 0), Quaternion.identity, GOManager.minimap.transform) as GameObject).GetComponent<RectTransform>());
        }
        if (humanoid && !PLAYER)
        {
            Updatewpnstate();
        }
        if (PLAYER)
            CmdUpdateName();

        // itemDrops.AddRange(Item.DropSetup((uint)lvl));
        for (int i = 0; i < itemDrops.Count; i++)
        {
            if (Random.Range(0f, 100f) <= itemDrops[i].chanceToDrop)
            {
                Item item = Instantiate(itemDrops[i].item.gameObject, _transform).GetComponent<Item>();
                item.count = (uint)Random.Range(itemDrops[i].countMin, itemDrops[i].countMax);
                itemsDrop.Add(item);
            }
        }

        if (netIdentity.isLocalPlayer)
        {

        }
        else
        {
            Invoke(nameof(Initialize),0.1f);
        }
    }
    void Initialize()
    {
        CmdInitialize();
    }
    [Command(requiresAuthority = false)]
    public void CmdAddStr(int value)
    {
        if (lvlPoints >= value)
        {
            lvlPoints -= value;
            strength += value;
        }
    }
    [ClientRpc]
    public void RpcAddStr(int str, int lvlPoints)
    {
        strength = str;
        this.lvlPoints = lvlPoints;
        if (GOManager.statsPanel.open)
            GOManager.statsPanel.TextUpdate();
        CmdSetStatsInDataBase(GOManager.AccountName);
    }
    [Command(requiresAuthority = false)]
    public void CmdAddDex(int value)
    {
        if (lvlPoints >= value)
        {
            lvlPoints -= value;
            dex += value;
        }
    }
    [ClientRpc]
    public void RpcAddDex(int dex, int lvlPoints)
    {
        this.dex = dex;
        this.lvlPoints = lvlPoints;
        if (GOManager.statsPanel.open)
            GOManager.statsPanel.TextUpdate();
        CmdSetStatsInDataBase(GOManager.AccountName);
    }
    [Command(requiresAuthority = false)]
    public void CmdAddSkill1h(int value)
    {
        if (lvlPoints >= value)
        {
            lvlPoints -= value;
            skill1h += value;
        }
    }
    [ClientRpc]
    public void RpcAddSkill1h(int skill1h, int lvlPoints)
    {
        this.skill1h = skill1h;
        this.lvlPoints = lvlPoints;
        if (GOManager.statsPanel.open)
            GOManager.statsPanel.TextUpdate();
        Updatewpnstate();
        CmdSetStatsInDataBase(GOManager.AccountName);
    }
    [Command(requiresAuthority = false)]
    public void CmdAddSkill2h(int value)
    {
        if (lvlPoints >= value)
        {
            lvlPoints -= value;
            skill2h += value;

        }
    }
    [ClientRpc]
    public void RpcAddSkill2h(int skill2h, int lvlPoints)
    {
        this.skill2h = skill2h;
        this.lvlPoints = lvlPoints;
        if (GOManager.statsPanel.open)
            GOManager.statsPanel.TextUpdate();
        Updatewpnstate();
        CmdSetStatsInDataBase(GOManager.AccountName);
    }
    [Command(requiresAuthority = false)]
    public void CmdUpdateWpnType(int type)
    {
        wpntype = type;
        RpcUpdateWpnType(type);
    }
    [ClientRpc]
    public void RpcUpdateWpnType(int type)
    {

        wpntype = type;
    }
    public void TakeExp(int exp, NetworkConnectionToClient conn = null)
    {
        this.exp += exp;
        if (this.exp >= expToLvl)
        {
            lvl++;
            expToLvl = (lvl + 1) * (lvl + 1) * 100 + 500;
            maxHealth = health + ringHp[0] + ringHp[1] + 12 * lvl;

            lvlPoints += 10;
            minHealth += 12;
        }
        if (conn is not null)
            TargetRpcTakeExp(conn, exp);
    }
    [TargetRpc]
    public void TargetRpcTakeExp(NetworkConnection conn, int exp)
    {
        this.exp += exp;
        GameObject expText = Instantiate(Resources.Load<GameObject>("ExpText"), GameObject.Find("Canvas").transform);
        expText.GetComponent<Text>().text = "Опыт: +" + exp;
        if (this.exp >= expToLvl)
        {
            lvl++;
            expToLvl = (lvl + 1) * (lvl + 1) * 100 + 500;
            maxHealth = health + ringHp[0] + ringHp[1] + 12 * lvl;

            lvlPoints += 10;
            minHealth += 12;
            audioSource.PlayOneShot(audioLvlUp);


        }
        CmdSetStatsInDataBase(GOManager.AccountName);
    }
    [Command(requiresAuthority = false)]
    public void CmdHeal(int heal)
    {
        minHealth += heal;
        if (minHealth > maxHealth)
        {
            minHealth = maxHealth;

        }
        RpcHpChange(minHealth, maxHealth);

    }
    [Command(requiresAuthority = false)]
    public void CmdGiveMana(int mana)
    {
        minMana += mana;
        if (minMana > maxMana)
        {
            minMana = maxMana;

        }
        RpcMpChange(minMana, maxMana);
    }
    [ClientRpc]
    public void RpcTakeDamage(int dmg, int dmgArrow, int dmgFire, int dmgMagic, uint id)
    {
        /* if (inventorySystem && inventorySystem.EquipedArmor)
         {
             inventorySystem.CreateNewItemOnHit(inventorySystem.EquipedItemArmor);
             inventorySystem.EquipedItemArmor.durability -= 0.005f;
         }*/

        GameObject go = null;
        NetworkIdentity[] identity = GameObject.FindObjectsOfType<NetworkIdentity>();
        for (int i = 0; i < identity.Length; i++)
        {
            if (identity[i].netId == id)
            {
                go = identity[i].gameObject;
                break;
            }
        }
        StatsSystem ss = go.GetComponent<StatsSystem>();
        int damage = dmg;
        if (dmgFire / 3 >= 10)
        {
            InFlameController inFlameController = GetComponent<InFlameController>();
            if (inFlameController)
            {
                inFlameController.flameTime.Add(10);
                inFlameController.flameDamage.Add(dmgFire / 3);
                inFlameController.audioSource.PlayOneShot(inFlameController.audioSource.clip);
            }
        }
        if (dmg + dmgArrow > 0)
            if (blood)
            {
                float intensyBlood = (float)damage / maxHealth * 10;
                if (intensyBlood > 1) intensyBlood = 1;
                if (damage <= 5) intensyBlood = 0;
                SpawnObjOnClient("BloodFX_impact_col", transform.position + new Vector3(0, 1.25f, 0), (go.transform.position - transform.position).magnitude == 0 ? Quaternion.Euler(0, 0, 0) : Quaternion.LookRotation(go.transform.position - transform.position), intensyBlood);
                int rndd = Random.Range(0, 4);
                string str = "DecalBlood";
                if (rndd > 0)
                    str += " " + rndd;
                SpawnObjOnClient(str, transform.position, Quaternion.Euler(90, 0, 0));
            }
        if (dmg + dmgArrow > 0)
            audioSource.PlayOneShot(hitSounds[Random.Range(0, hitSounds.Length)]);
        if (soundContoll && dmg + dmgArrow + dmgFire + dmgMagic > 0)
            soundContoll.Hit();
        if (soundMonsters && dmg + dmgArrow + dmgFire + dmgMagic > 0)
            soundMonsters.Hit();
        if (go != gameObject)
        {
            int indx = damagedFrom.IndexOf(ss);
            if (indx == -1)
            {
                damagedFrom.Add(ss);
                damageFrom.Add(damage);
            }
            else
            {
                damageFrom[indx] += damage;
            }
        }
        int hitrnd = -1;
        if (Vector3.Dot((go.transform.position - transform.position).normalized, transform.forward) > 0)
            hitrnd = 0;
        else
            hitrnd = 1;
        var state = animator.GetCurrentAnimatorStateInfo(0);
        bool hittbl = (state.fullPathHash == anim1) | (state.fullPathHash == anim2) | (state.fullPathHash == anim3) | (state.fullPathHash == anim4) | (state.fullPathHash == anim5);
        if (!hittbl && dmg + dmgArrow + dmgFire + dmgMagic > 0)
        {
            animator.SetInteger("HitRnd", hitrnd);
            SetBoolAsTrigger("Hit");
            // animator.ResetTrigger("Hit");
            //  animator.SetTrigger("Hit");
        }

    }
    public void SetBoolAsTrigger(string name)
    {
        StartCoroutine(SetBoolAsTriggerCorotinue(name));
    }
    IEnumerator SetBoolAsTriggerCorotinue(string name)
    {
        animator.SetBool(name, true);
        yield return new WaitForSeconds(0.21f);
        animator.SetBool(name, false);
    }
    public void SetBoolAsTrigger(string name, float time)
    {
        StartCoroutine(SetBoolAsTriggerCorotinue(name, time));
    }
    IEnumerator SetBoolAsTriggerCorotinue(string name, float time)
    {
        animator.SetBool(name, true);
        yield return new WaitForSeconds(time);
        animator.SetBool(name, false);
    }
    [ClientRpc]
    void RpcInstSpark()
    {
        if (moveControll)
        {
            Instantiate(Resources.Load("SparksEffect"), moveControll.combatSystem.weaponController.wpn1.transform.position + moveControll.combatSystem.weaponController.wpn1.transform.GetChild(2).localPosition / 2, Quaternion.Euler(0, 0, 0));
            audioSource.PlayOneShot(Resources.Load("Sounds/SFX/CS_IHL_WO_ME") as AudioClip);
        }
        else
        {
            Instantiate(Resources.Load("SparksEffect"), BodyPartColliders[Random.Range(0, BodyPartColliders.Count)].transform.position, Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
            audioSource.PlayOneShot(Resources.Load("Sounds/SFX/CS_IHL_WO_ME") as AudioClip);
        }
    }
    [Command(requiresAuthority = false)]
    public void CmdTakeDamage(int dmg, int dmgArrow, int dmgFire, int dmgMagic, bool crit, uint id)
    {
        // inventorySystem.CreateNewItemOnHit(inventorySystem.EquipedItemArmor);
        /*if (inventorySystem && inventorySystem.EquipedItemArmor)
            inventorySystem.EquipedItemArmor.durability -= 0.005f;*/
        GameObject go = null;
        NetworkIdentity[] identity = GameObject.FindObjectsOfType<NetworkIdentity>();
        for (int i = 0; i < identity.Length; i++)
        {
            if (identity[i].netId == id)
            {
                go = identity[i].gameObject;
                break;
            }
        }
        StatsSystem ss = go.GetComponent<StatsSystem>();
        if (ss.wpntype < 2 && evade) return;
        if (ss.humanoid && block)
        {

            RpcInstSpark();
            return;
        }
        float damageReduction = 1 - weight * 10;
        int damage = Mathf.Clamp(dmg - armor, 0, 1000) + Mathf.Clamp(dmgArrow - arrowdef, 0, 1000) + Mathf.Clamp(dmgFire - firedef, 0, 1000) + Mathf.Clamp(dmgMagic - magicdef, 0, 1000);
        if (inventorySystem && inventorySystem.EquipedItemArmor)
            damage = Mathf.Clamp(dmg - (int)((float)armor * ((inventorySystem.EquipedItemArmor.durability / 100) * (2 - (inventorySystem.EquipedItemArmor.durability / 100)))), 0, 1000) + Mathf.Clamp(dmgArrow - arrowdef, 0, 1000) + Mathf.Clamp(dmgFire - firedef, 0, 1000) + Mathf.Clamp(dmgMagic - magicdef, 0, 1000);
        else
            damage = Mathf.Clamp(dmg - (int)((float)armor * (1 * (2 - 1))), 0, 1000) + Mathf.Clamp(dmgArrow - arrowdef, 0, 1000) + Mathf.Clamp(dmgFire - firedef, 0, 1000) + Mathf.Clamp(dmgMagic - magicdef, 0, 1000);
        if (!crit)
            damage /= 10;
        damage = Mathf.Clamp((int)((float)damage * damageReduction), 5, maxHealth);
        if (!god)
            minHealth -= damage;
        RpcTakeDamage(damage, dmgArrow, dmgFire, dmgMagic, id);
        RpcHpChange(minHealth, maxHealth);
        /*if (dmgFire/10 > 10)
        {
            InFlameController inFlameController = GetComponent<InFlameController>();
            if (inFlameController)
            {
                inFlameController.flameTime.Add(10);
                inFlameController.flameDamage.Add(dmgFire / 10);
                inFlameController.audioSource.PlayOneShot(inFlameController.audioSource.clip);
            }
        }*/
        /* if (dmg + dmgArrow > 0)
         {
             if (blood)
             {
                 float intensyBlood = (float)damage / maxHealth * 10;
                 if (intensyBlood > 1) intensyBlood = 1;
                 if (damage <= 5) intensyBlood = 0;
                 SpawnObjOnClient("BloodFX_impact_col", transform.position + new Vector3(0, 1.25f, 0), (go.transform.position - transform.position).magnitude == 0 ? Quaternion.Euler(0, 0, 0) : Quaternion.LookRotation(go.transform.position - transform.position),intensyBlood);
                 int rndd = Random.Range(0, 4);
                 string str = "DecalBlood";
                 if (rndd > 0)
                     str += " " + rndd;
                 SpawnObjOnClient(str, transform.position, Quaternion.Euler(0, 0, 0));
             }
         }*/
        if (dmg + dmgArrow > 0)
            audioSource.PlayOneShot(hitSounds[Random.Range(0, hitSounds.Length)]);
        if (soundContoll && dmg + dmgArrow + dmgFire + dmgMagic > 0)
            soundContoll.Hit();
        if (soundMonsters && dmg + dmgArrow + dmgFire + dmgMagic > 0)
            soundMonsters.Hit();
        if (go != gameObject)
        {
            int indx = damagedFrom.IndexOf(ss);
            if (indx == -1)
            {
                damagedFrom.Add(ss);
                damageFrom.Add(damage);
            }
            else
            {
                damageFrom[indx] += damage;
            }
        }
        int hitrnd = -1;
        if (Vector3.Dot((go.transform.position - transform.position).normalized, transform.forward) > 0)
            hitrnd = 0;
        else
            hitrnd = 1;
        var state = animator.GetCurrentAnimatorStateInfo(0);
        bool hittbl = (state.fullPathHash == anim1) | (state.fullPathHash == anim2) | (state.fullPathHash == anim3) | (state.fullPathHash == anim4) | (state.fullPathHash == anim5);
        if (!hittbl && dmg + dmgArrow + dmgFire + dmgMagic > 0)
        {
            animator.SetInteger("HitRnd", hitrnd);
            SetBoolAsTrigger("Hit");
            // animator.ResetTrigger("Hit");
            //animator.SetTrigger("Hit");
        }
        if (minHealth <= 0)
        {

            respawnTimer = 0;
            RpcSetDeath((byte)go.GetComponent<StatsSystem>().animator.GetInteger("AttackRnd"), damage, go.GetComponent<NetworkIdentity>().netId);
            characterController.enabled = false;
            animator.enabled = false;
            // Destroy(GetComponent<DialogueTarget>());
            if (go != gameObject)
                go.GetComponent<StatsSystem>().TakeExp(lvl * 10, go.GetComponent<NetworkIdentity>().connectionToClient);



        }


    }
    [ServerCallback]
    public void TakeDamage(int dmg, int dmgArrow, int dmgFire, int dmgMagic, bool crit, GameObject go)
    {
        StatsSystem ss = go.GetComponent<StatsSystem>();
        if (ss.wpntype < 2 && evade) return;

        if (ss.humanoid && block)
        {
            RpcInstSpark();
            return;
        }
        float damageReduction = 1 - weight * 10;
        int damage = Mathf.Clamp(dmg - armor, 0, 1000) + Mathf.Clamp(dmgArrow - arrowdef, 0, 1000) + Mathf.Clamp(dmgFire - firedef, 0, 1000) + Mathf.Clamp(dmgMagic - magicdef, 0, 1000);
        if (!crit)
            damage /= 10;
        damage = Mathf.Clamp((int)((float)damage * damageReduction), 5, maxHealth);
        if (!god)
            minHealth -= damage;
        RpcTakeDamage(damage, dmgArrow, dmgFire, dmgMagic, go.GetComponent<NetworkIdentity>().netId);
        RpcHpChange(minHealth, maxHealth);
        /*if (dmgFire > 10)
        {
            InFlameController inFlameController = GetComponent<InFlameController>();
            if (inFlameController)
            {
                inFlameController.flameTime.Add(10);
                inFlameController.audioSource.PlayOneShot(inFlameController.audioSource.clip);
            }
        }*/
        /*if (dmg + dmgArrow > 0)
            if (blood)
            {
                float intensyBlood = (float)damage / maxHealth *10;
                if (intensyBlood > 1) intensyBlood = 1;
                if (damage <= 5) intensyBlood = 0;
                SpawnObjOnClient("BloodFX_impact_col", transform.position + new Vector3(0, 1.25f, 0), (go.transform.position - transform.position).magnitude == 0 ? Quaternion.Euler(0, 0, 0) : Quaternion.LookRotation(go.transform.position - transform.position), intensyBlood);
                int rndd = Random.Range(0, 4);
                string str = "DecalBlood";
                if (rndd > 0)
                    str += " " + rndd;
                SpawnObjOnClient(str, transform.position, Quaternion.Euler(0, 0, 0));
            }*/
        if (dmg + dmgArrow > 0)
            audioSource.PlayOneShot(hitSounds[Random.Range(0, hitSounds.Length)]);
        if (soundContoll && dmg + dmgArrow + dmgFire + dmgMagic > 0)
            soundContoll.Hit();
        if (soundMonsters && dmg + dmgArrow + dmgFire + dmgMagic > 0)
            soundMonsters.Hit();
        if (go != gameObject)
        {
            int indx = damagedFrom.IndexOf(ss);
            if (indx == -1)
            {
                damagedFrom.Add(ss);
                damageFrom.Add(damage);
            }
            else
            {
                damageFrom[indx] += damage;
            }
        }

        int hitrnd = -1;
        if (Vector3.Dot((go.transform.position - transform.position).normalized, transform.forward) > 0)
            hitrnd = 0;
        else
            hitrnd = 1;
        var state = animator.GetCurrentAnimatorStateInfo(0);
        bool hittbl = (state.fullPathHash == anim1) | (state.fullPathHash == anim2) | (state.fullPathHash == anim3) | (state.fullPathHash == anim4) | (state.fullPathHash == anim5);
        if (!hittbl && dmg + dmgArrow + dmgFire + dmgMagic > 0)
        {
            animator.SetInteger("HitRnd", hitrnd);
            SetBoolAsTrigger("Hit");
            //animator.ResetTrigger("Hit");
            // animator.SetTrigger("Hit");
        }
        if (minHealth <= 0)
        {
            respawnTimer = 0;
            RpcSetDeath((byte)go.GetComponent<StatsSystem>().animator.GetInteger("AttackRnd"), damage, go.GetComponent<NetworkIdentity>().netId);
            animator.enabled = false;
            characterController.enabled = false;
            Destroy(GetComponent<DialogueTarget>());
            if (go != gameObject)
                go.GetComponent<StatsSystem>().TakeExp(lvl * 10);

        }


    }
    [ClientRpc]
    void RpcSetDeath(byte dir, int str, uint id)
    {
        int inx = GOManager.enemysPos.IndexOf(_transform);
        if (inx > -1)
        {
            Destroy(GOManager.enemysPosUI[inx].gameObject);
            GOManager.enemysPosUI.RemoveAt(inx);
            GOManager.enemysPos.RemoveAt(inx);

        }

        respawnTimer = 0;
        GameObject go = null;
        NetworkIdentity[] identity = GameObject.FindObjectsOfType<NetworkIdentity>();
        for (int i = 0; i < identity.Length; i++)
        {
            if (identity[i].netId == id)
            {
                go = identity[i].gameObject;
                break;
            }
        }
        DeathTrigger(dir, str, go.transform);
        Destroy(GetComponent<DialogueTarget>());
    }
    public void TakeBeliarHit()
    {
        minHealth -= 100;
        if (minHealth < 0)
            minHealth = 1;
        GameObject go = Instantiate(Resources.Load("BeliarLight"), transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        Destroy(go, 7);
    }
    [Command(requiresAuthority = false)]
    public void CmdInitialize(NetworkConnectionToClient conn = null)
    {
        TargetRpcUpdateDeathState(conn, minHealth);
        if (inventorySystem)
        {
            inventorySystem.TargetRpcEquipWeapon(conn, inventorySystem.equipedItemsID[0]);
            inventorySystem.TargetRpcEquipWeaponBow(conn, inventorySystem.equipedItemsID[1]);
            inventorySystem.TargetRpcEquipWeaponRune(conn, inventorySystem.equipedItemsID[2]);
            inventorySystem.TargetRpcEquipArmor(conn, inventorySystem.equipedItemsID[4]);
        }
        if (PLAYER)
            TargetRpcLoadCustomization(conn, _name, weight, size, currentBody, currentHead, false);
    }
    [TargetRpc]
    public void TargetRpcUpdateDeathState(NetworkConnection conn, int minHp)
    {
        minHealth = minHp;
        if (minHealth <= 0)
        {
            animator.enabled = false;
            if (rigidbody && !PLAYER)
                rigidbody.isKinematic = false;
            for (int i = 0; i < BodyPartColliders.Count; i++)
            {
                BodyPartColliders[i].isTrigger = false;
                BodyPartColliders[i].attachedRigidbody.isKinematic = false;
            }
        }
    }
    public void DeathTrigger(byte dir, int str, Transform tr)
    {
        if (soundContoll)
            soundContoll.Dead();
        characterController.enabled = false;
        animator.enabled = false;
        if(!ReferenceEquals(moveControll,null))
        moveControll.enabled = false;
        for (int i = 0; i < BodyPartColliders.Count; i++)
        {
            BodyPartColliders[i].isTrigger = false;
            BodyPartColliders[i].attachedRigidbody.isKinematic = false;
        }
        if (rigidbodyPart)
        {
            if (dir == 2)
                rigidbodyPart.AddForce(tr.right * -str / maxHealth * 10000, ForceMode.Force);
            if (dir == 1)
                rigidbodyPart.AddForce(tr.right * str / maxHealth * 10000, ForceMode.Force);
            if (dir == 0)
                rigidbodyPart.AddForce(tr.forward * str / maxHealth * 10000, ForceMode.Force);
            // targetStats.rigidbodyPart.AddForce(weaponForce * -dmg1 / targetStats.maxHealth * 200000, ForceMode.Force);
        }
        for (int q = 0; q < BodyPartColliders.Count; q++)
        {
            if (dir == 2)
                BodyPartColliders[q].attachedRigidbody.AddForce(tr.right * -str / maxHealth * 1000, ForceMode.Force);
            if (dir == 1)
                BodyPartColliders[q].attachedRigidbody.AddForce(tr.right * str / maxHealth * 1000, ForceMode.Force);
            if (dir == 0)
                BodyPartColliders[q].attachedRigidbody.AddForce(tr.forward * str / maxHealth * 1000, ForceMode.Force);
            // targetStats.rigidbodyPart.AddForce(weaponForce * -dmg1 / targetStats.maxHealth * 200000, ForceMode.Force);
        }
        /*if (!PLAYER&&weaponController)
        {
            
            Transform wpn = weaponController.wpn1.transform.GetChild(2);
            wpn.GetComponent<WorldItem>().enabled = true;
            wpn.parent = null;
            wpn.GetChild(2).GetComponent<MeshCollider>().enabled = true;
            wpn.GetComponent<Rigidbody>().isKinematic = false;
            wpn.GetChild(2).GetComponent<Rigidbody>().isKinematic = false;
            itemDrops.RemoveAt(0);
        }*/
        if (_name == "Ящик")
        {
            GetComponent<ArionDigital.CrashCrate>().Test();
        }
    }
    public void EvadeOn()
    {
        if (!evade)
        {
            evade = true;
            CmdEvadeOn();
        }
    }
    public void EvadeOff()
    {
        if (evade)
        {
            evade = false;
            CmdEvadeOff();
        }
    }
    public void BlockOn()
    {
        if (!block)
        {
            block = true;
            CmdBlockOn();
        }
    }
    public void BlockOff()
    {
        if (block)
        {
            block = false;
            CmdBlockOff();
        }
    }
    [Command(requiresAuthority = false)]
    public void CmdBlockOff()
    {
        block = false;
    }
    [Command(requiresAuthority = false)]
    public void CmdBlockOn()
    {
        block = true;
        Invoke(nameof(BlockOffDelay), 1f);
    }
    [Command(requiresAuthority = false)]
    public void CmdEvadeOff()
    {
        evade = false;
    }
    [Command(requiresAuthority = false)]
    public void CmdEvadeOn()
    {
        evade = true;
        Invoke(nameof(EvadeOffDelay), 1f);
    }
    void EvadeOffDelay()
    {
        evade = false;
    }
    void BlockOffDelay()
    {
        block = false;
    }
    public void ItemsChestUpdate()
    {
        for (int i = 0; i < itemsDrop.Count; i++)
        {

            itemsDrop[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(-1000, -1000, 0);
        }
    }



    [Command(requiresAuthority = false)]
    public void CmdSpawnObjOnServer(string name, Vector3 pos, Quaternion rot)
    {

        RpcSpawnObjOnServer(name, pos, rot);
    }
    [ClientRpc]
    public void RpcSpawnObjOnServer(string name, Vector3 pos, Quaternion rot)
    {
        GameObject gg = Resources.Load(name) as GameObject;
        GameObject itm = Instantiate(gg, pos, rot);

    }
    public void SpawnObjOnClient(string name, Vector3 pos, Quaternion rot)
    {
        GameObject gg = Resources.Load(name) as GameObject;
        GameObject itm = Instantiate(gg, pos, rot);

    }

    public void SpawnObjOnClient(string name, Vector3 pos, Quaternion rot, float intensy)
    {

        GameObject gg = Resources.Load(name) as GameObject;
        GameObject itm = Instantiate(gg, pos, rot);
        itm.GetComponent<BloodIntensy>().intensy = intensy;
    }

    [Command(requiresAuthority = false)]
    public void CmdSaveCustomization(string accname)
    {
        string filepath = "DataBase/Accounts/" + accname + "/Characters/1/Customization.xml";
        XmlDocument xmlDoc = new XmlDocument();
        XmlNode rootNode = null;
        if (!File.Exists(filepath))
        {
            rootNode = xmlDoc.CreateElement("Data");
            xmlDoc.AppendChild(rootNode);
        }
        else
        {
            xmlDoc.Load(filepath);
            rootNode = xmlDoc.DocumentElement;
        }
        rootNode.RemoveAll();
        XmlElement element;
        element = xmlDoc.CreateElement("Customization");
        element.SetAttribute("Name", _name);
        element.SetAttribute("Customizable", false.ToString());
        element.SetAttribute("Weight", weight.ToString());
        element.SetAttribute("Size", size.ToString());
        element.SetAttribute("Body", currentBody.ToString());
        element.SetAttribute("Head", currentHead.ToString());

        rootNode.AppendChild(element);
        xmlDoc.Save(filepath);
    }
    [Command(requiresAuthority = false)]
    public void CmdLoadCustomization(string accname, NetworkConnectionToClient conn = null)
    {
        string filepath = "DataBase/Accounts/" + accname + "/Characters/1/Customization.xml";
        if (File.Exists(filepath))
        {
            XmlTextReader reader = new XmlTextReader(filepath);
            while (reader.Read())
            {
                if (reader.IsStartElement("Customization"))
                {
                    _name = reader.GetAttribute("Name");
                    firstLogin = bool.Parse(reader.GetAttribute("Customizable"));
                    weight = float.Parse(reader.GetAttribute("Weight"));
                    size = float.Parse(reader.GetAttribute("Size"));
                    currentBody = byte.Parse(reader.GetAttribute("Body"));
                    currentHead = byte.Parse(reader.GetAttribute("Head"));

                }
            }


            reader.Close();
        }
        TargetRpcLoadCustomization(conn, _name, weight, size, currentBody, currentHead, firstLogin);
    }
    [TargetRpc]
    public void TargetRpcLoadCustomization(NetworkConnection conn, string name, float weight, float size, byte body, byte head, bool firstLogin)
    {
        _name = name;
        CmdSetNickName(name);
        StatsSystem.firstLogin = firstLogin;
        if (firstLogin)
        {
            CharacterChanger.instance.SwitchActive();
        }
        this.weight = weight;
        this.size = size;
        transform.localScale = Vector3.one * size;
        for (int i = 0; i < fatnesMeshes.Count; i++)
        {
            fatnesMeshes[i].UpdateD();
        }
        currentBody = body;
        currentHead = head;
        for (int i = 0; i < BodyMeshes.Length; i++)
        {
            GameObject curBody = BodyMeshes[i];
            if (currentBody == i)
            {
                if (!curBody.activeSelf)
                    curBody.SetActive(true);
            }
            else if (curBody.activeSelf)
                curBody.SetActive(false);
        }
        for (int i = 0; i < HeadMeshes.Length; i++)
        {
            GameObject curHead = HeadMeshes[i];
            if (currentHead == i)
            {
                if (!curHead.activeSelf)
                    curHead.SetActive(true);
            }
            else if (curHead.activeSelf)
                curHead.SetActive(false);
        }
        moveControll.headEmotions = HeadMeshes[currentHead].GetComponent<HeadEmotions>();
    }
    [Command(requiresAuthority = false)]
    public void CmdSetStatsInDataBase(string accname)
    {
        string filepath = "DataBase/Accounts/" + accname + "/Characters/1/Characteristics.xml";
        XmlDocument xmlDoc = new XmlDocument();
        XmlNode rootNode = null;
        if (!File.Exists(filepath))
        {
            rootNode = xmlDoc.CreateElement("Data");
            xmlDoc.AppendChild(rootNode);
        }
        else
        {
            xmlDoc.Load(filepath);
            rootNode = xmlDoc.DocumentElement;
        }
        rootNode.RemoveAll();
        XmlElement element;
        element = xmlDoc.CreateElement("Characteristics");
        element.SetAttribute("Str", strength.ToString());
        element.SetAttribute("Dex", dex.ToString());
        element.SetAttribute("MinHp", minHealth.ToString());
        element.SetAttribute("MaxHp", maxHealth.ToString());
        element.SetAttribute("MinMana", minMana.ToString());
        element.SetAttribute("MaxMana", maxMana.ToString());
        element.SetAttribute("Lvl", lvl.ToString());
        element.SetAttribute("Exp", exp.ToString());
        element.SetAttribute("SkillPoints", lvlPoints.ToString());
        element.SetAttribute("Skill1h", skill1h.ToString());
        element.SetAttribute("Skill2h", skill2h.ToString());
        element.SetAttribute("SkillBow", skillbow.ToString());
        element.SetAttribute("SkillCrossbow", skillcrossbow.ToString());
        rootNode.AppendChild(element);
        xmlDoc.Save(filepath);
    }

    [Command(requiresAuthority = false)]
    public void CmdGetStatsInDataBase(string accname, NetworkConnectionToClient conn = null)
    {
        string filepath = "DataBase/Accounts/" + accname + "/Characters/1/Characteristics.xml";
        if (File.Exists(filepath))
        {
            XmlTextReader reader = new XmlTextReader(filepath);
            while (reader.Read())
            {
                if (reader.IsStartElement("Characteristics"))
                {

                    strength = int.Parse(reader.GetAttribute("Str"));
                    dex = int.Parse(reader.GetAttribute("Dex"));
                    minHealth = int.Parse(reader.GetAttribute("MinHp"));
                    maxHealth = int.Parse(reader.GetAttribute("MaxHp"));
                    minMana = int.Parse(reader.GetAttribute("MinMana"));
                    maxMana = int.Parse(reader.GetAttribute("MaxMana"));
                    lvl = int.Parse(reader.GetAttribute("Lvl"));
                    exp = int.Parse(reader.GetAttribute("Exp"));
                    lvlPoints = int.Parse(reader.GetAttribute("SkillPoints"));
                    skill1h = int.Parse(reader.GetAttribute("Skill1h"));
                    skill2h = int.Parse(reader.GetAttribute("Skill2h"));
                    skillbow = int.Parse(reader.GetAttribute("SkillBow"));
                    skillcrossbow = int.Parse(reader.GetAttribute("SkillCrossbow"));
                    expToLvl = (lvl + 1) * (lvl + 1) * 100 + 500;
                    TargetRpcGetStatsInDataBase(conn, strength, dex, minHealth, maxHealth, minMana, maxMana, lvl, exp, lvlPoints, skill1h, skill2h, skillbow, skillcrossbow);
                    RpcHpChange(minHealth, maxHealth);
                    RpcMpChange(minMana, maxMana);
                }
            }


            reader.Close();
        }
    }
    [TargetRpc]
    public void TargetRpcGetStatsInDataBase(NetworkConnection conn, int str, int dex, int minHp, int maxHp, int minMp, int maxMp, int lvl, int exp, int skillPoints, int skill1h, int skill2h, int skillBow, int skillCrossbow)
    {
        strength = str;
        //inventorySystem.bodyMesh.GetComponent<FatnesMesh>().UpdateD();
        this.dex = dex;
        minHealth = minHp;
        maxHealth = maxHp;
        maxMana = maxMp;
        minMana = minMp;
        this.lvl = lvl;
        this.exp = exp;
        lvlPoints = skillPoints;
        this.skill1h = skill1h;
        this.skill2h = skill2h;
        skillbow = skillBow; ;
        skillcrossbow = skillCrossbow;
        expToLvl = (lvl + 1) * (lvl + 1) * 100 + 500;
    }
    public void DestroyItemOnServer(uint id)
    {
        RpcDeleteObj(id);
        NetworkIdentity[] go = GameObject.FindObjectsOfType<NetworkIdentity>();
        for (int i = 0; i < go.Length; i++)
        {
            if (go[i].netId == id)
            {
                NetworkServer.Destroy(go[i].gameObject);
                NetworkServer.UnSpawn(go[i].gameObject);
                Destroy(go[i].gameObject);
                break;
            }
        }

    }
    [ClientRpc]
    public void RpcDeleteObj(uint id)
    {

        NetworkIdentity[] go = GameObject.FindObjectsOfType<NetworkIdentity>();
        for (int i = 0; i < go.Length; i++)
        {
            if (go[i].netId == id)
            {
                Destroy(go[i].gameObject);
                break;
            }
        }
    }
    [Command(requiresAuthority = false)]
    public void CmdHeadSync(byte currentHead)
    {
        this.currentHead = currentHead;

        RpcHeadSync(currentHead);
    }

    [ClientRpc]
    public void RpcHeadSync(byte currentHead)
    {
        this.currentHead = currentHead;
        for (int i = 0; i < HeadMeshes.Length; i++)
        {
            GameObject curHead = HeadMeshes[i];
            if (currentHead == i)
            {
                if (!curHead.activeSelf)
                    curHead.SetActive(true);
            }
            else if (curHead.activeSelf)
                curHead.SetActive(false);

        }
        moveControll.headEmotions = HeadMeshes[currentHead].GetComponent<HeadEmotions>();
    }
    [Command(requiresAuthority = false)]
    public void CmdBodySync(byte currentBody)
    {
        this.currentBody = currentBody;

        RpcBodySync(currentBody);
    }

    [ClientRpc]
    public void RpcBodySync(byte currentBody)
    {
        this.currentBody = currentBody;
        for (int i = 0; i < BodyMeshes.Length; i++)
        {
            GameObject curBody = BodyMeshes[i];
            if (currentBody == i)
            {
                if (!curBody.activeSelf)
                    curBody.SetActive(true);
            }
            else if (curBody.activeSelf)
                curBody.SetActive(false);
        }
    }
    [Command(requiresAuthority = false)]
    public void CmdSizeSync(float size)
    {
        this.size = size;

        RpcSizeSync(size);
    }

    [ClientRpc]
    public void RpcSizeSync(float size)
    {
        this.size = size;
        transform.localScale = Vector3.one * size;
    }
    [TargetRpc]
    public void TargetRpcSizeSync(NetworkConnection conn, float size)
    {
        this.size = size;
        transform.localScale = Vector3.one * size;
    }
    [Command(requiresAuthority = false)]
    public void CmdFatnessSync(float weight)
    {
        this.weight = weight;

        RpcFatnessSync(weight);
    }

    [ClientRpc]
    public void RpcFatnessSync(float weight)
    {
        this.weight = weight;

        for (int i = 0; i < fatnesMeshes.Count; i++)
        {
            fatnesMeshes[i].UpdateD();
        }

    }
    [TargetRpc]
    public void TargetRpcFatnessSync(NetworkConnection conn, float weight)
    {
        this.weight = weight;
        for (int i = 0; i < fatnesMeshes.Count; i++)
        {
            fatnesMeshes[i].UpdateD();
        }
    }
    [TargetRpc]
    public void TargetRpcGetAttacked(NetworkConnection conn, short value)
    {
        GOManager.attacked += value;
    }
}








