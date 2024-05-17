using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;
public class AI : NetworkBehaviour
{
    public LayerMask layerMask;
    public Transform companion;
    public Transform dialogueTarget;
    public DialogueTarget _dialogueTarget;
    public StatsSystem target;
    public StatsSystem target2;
    Animator targetAnimator;
    public List<StatsSystem> worldChars;
    public Transform _transform;
    public Animator animator;
    public Vector3 startPos;
    public Vector3 startPosRnd;
    public Vector3 eventPos;
    public Vector3 startRot;
    public bool randomWay;
    float randomWayTimer;
    public bool angry;
    public StatsSystem stats;
    public float agroTimer;
    public float agroTime;
    public CharacterController characterController;
    public float attackTimer;
    public float reactionTime;
    //public float backTimer;
    public int evadeChanceMin;
    public int evadeChance;
    public GameObject wpn1;
    public MeleeWeaponTrail mwt;
    public Transform hip;
    public Transform hand;
    public float attackspeed = 1;

    public int indexWinner2 = -1;
    float oldY;
    float idleTime;
    public bool trig1;
    public bool trig2;
    bool trig3;
    bool trig4;

    float targetTimer;
    public bool AFK;
    int atkrnd;
    bool landing;
    float fallTime;
    bool drawe;
    public SoundContoll soundContoll;
    float weapondownTimer;
    float weapondownTimer2;
    int animDraw1;
    int animDraw2;
    int animDraw3;
    int animDraw4;
    int anim1;
    int anim2;
    int anim3;
    int anim4;
    int anim5;
    int anim6;

    int anim7;
    int anim8;
    int anim9;
    float wpnrdytimer;
    Transform playerTransform;
    bool trig5;
    bool trig6;
    bool trig7;
    bool trig8;
    bool trig10;
    bool trig11;
    CombatSystem CombatSystemComp;
    //public AnimatorSyncNet animatorSyncNet;
    public NetworkTransformUnreliable syncTransorm;
    public InteractiveChair chair;
    public bool freefightBool;
    public ParticleSystem attackEffect;
    private bool RoutineBool;
    public float backTimer;
    public float attackTimerr;
    public float strafeTimer;
    public float drinkTimer;
    public float routineTimer;
    public StatsSystem routineDialogueTarget;
    public float smallTalkTimer;
    public bool social = true;
    void Start()
    {
        layerMask = LayerMask.GetMask("Default", "Terrain", "Player", "wef");
        _transform = transform;
        syncTransorm = GetComponent<NetworkTransformUnreliable>();
        _dialogueTarget = GetComponent<DialogueTarget>();
        if (eventPos.magnitude > 0)
        {
            startPos = eventPos;
            startPosRnd = startPos;
        }
        else
        {
            startPos = _transform.position;
            startPosRnd = startPos;
        }
        stats = GetComponent<StatsSystem>();
        startRot = _transform.eulerAngles;
        evadeChance = 100 - Random.Range(evadeChanceMin, evadeChance);
        trig4 = true;
        animator = GetComponent<Animator>();
        soundContoll = GetComponent<SoundContoll>();
        animDraw1 = Animator.StringToHash("Up.equip");
        animDraw2 = Animator.StringToHash("Up.equip2");

        animDraw3 = Animator.StringToHash("Bottom.1h3draw");
        animDraw4 = Animator.StringToHash("Bottom.1h3draw2");

        anim1 = Animator.StringToHash("New Layer.AttackMove");
        anim2 = Animator.StringToHash("Base Layer.Attack");
        anim3 = Animator.StringToHash("Up.MoveAttack");

        anim4 = Animator.StringToHash("Bottom.1h3attackright");
        anim5 = Animator.StringToHash("Bottom.1h3attackfront");
        anim6 = Animator.StringToHash("Bottom.1h3attackleft");

        anim7 = Animator.StringToHash("Bottom.1h3run");
        anim8 = Animator.StringToHash("Bottom.1h3runstart");
        anim9 = Animator.StringToHash("Bottom.1h3back");
        // animatorSyncNet = GetComponent<AnimatorSyncNet>();
        characterController = GetComponent<CharacterController>();
    }
    public void UpdateDamageTake()
    {

        indexWinner2 = -1;
        int maxDmg = 0;
        for (int i = 0; i < worldChars.Count; i++)
        {
            if (worldChars[i].minHealth > 0)
            {

                int dmg = 0;
                int indx = stats.damagedFrom.IndexOf(worldChars[i]);
                if (indx > -1)
                {
                    dmg = stats.damageFrom[indx];
                }
                float dis = Vector3.Distance(worldChars[i].transform.position, _transform.position);
                if (dis <= 10 && dmg > maxDmg)
                {
                    maxDmg = dmg;
                    indexWinner2 = i;
                }
            }
        }
    }

    void LateUpdate()
    {

        /* if (isServer) {
             if (NetworkClient.ready)
             {
                 playerTransform = GOManager.playerTransform;

                 int distanceToPlayer = ((int)playerTransform.position.x + (int)playerTransform.position.y + (int)playerTransform.position.z)
                     - ((int)_transform.position.x + (int)_transform.position.y + (int)_transform.position.z);
                 if (distanceToPlayer < 0)
                     distanceToPlayer = -distanceToPlayer;
                 if (distanceToPlayer >= 50)
                 {
                     if (!trig8)
                     {
                        // Destroy(animatorSyncNet);
                         animator.enabled = false;
                         trig8 = true;
                     }
                     return;
                 }
                 else
                 {
                     if (trig8)
                     {
                         gameObject.AddComponent<AnimatorSyncNet>();
                         animator.enabled = true;
                         trig8 = false;
                     }
                 }
             }
             return; 
         }*/
        if (!isServer) enabled = false;
       //if (Time.frameCount % 2 != 0) return;


        worldChars = GOManager.worldChars;
        float deltaTime = Time.deltaTime;
        if (characterController.enabled)
        {
            characterController.Move(new Vector3(0, -9.8f * fallTime * deltaTime, 0));
            if (landing)
                characterController.Move(new Vector3(0, -11 * deltaTime, 0));
            landing = characterController.isGrounded;

            if (!landing)
            {

                if (!trig7)
                {
                    trig7 = true;
                    trig6 = false;
                    animator.SetBool("Landing", false);
                    //  animatorSyncNet.RpcAnimatorSetBool("Landing", false);
                }
                fallTime += deltaTime;
                if (fallTime > 0.2f)
                {
                    if (!trig5)
                    {
                        // Debug.Log("fall");
                        trig5 = true;
                        animator.SetBool("Fall", true);
                        // animatorSyncNet.RpcAnimatorSetBool("Fall", true);

                    }
                }
            }
            else
            {

                if (!trig6)
                {
                    animator.SetBool("Landing", true);
                    animator.SetBool("Fall", false);
                    //animatorSyncNet.RpcAnimatorSetBool("Landing", true);
                    //  animatorSyncNet.RpcAnimatorSetBool("Fall", false);
                    trig6 = true;
                    trig5 = false;
                    trig7 = false;
                }
                if (fallTime > 1.1f)
                {
                    int dmg1 = (int)(fallTime * stats.maxHealth / 2);
                    int dmg2 = 0;
                    int dmg3 = 0;
                    int dmg4 = 0;
                    stats.CmdTakeDamage(dmg1, dmg2, dmg3, dmg4, true, gameObject.GetComponent<Mirror.NetworkIdentity>().netId);
                }
                fallTime = 0;

            }
        }
        if (AFK) return;










        if (animator.GetBool("Guard") == true)
        {
            animator.SetBool("Guard", false);
            //  animatorSyncNet.RpcAnimatorSetBool("Guard", false);
        }
        if (animator.GetBool("Run") == true)
        {
            animator.SetBool("Run", false);
            // animatorSyncNet.RpcAnimatorSetBool("Run", false);
        }
        if (animator.GetBool("Block") == true)
        {
            animator.SetBool("Block", false);
            // animatorSyncNet.RpcAnimatorSetBool("Block", false);
        }
        if (animator.GetBool("Back") == true)
        {
            animator.SetBool("Back", false);
            //  animatorSyncNet.RpcAnimatorSetBool("Back", false);
        }
        if (animator.GetBool("TurnL") == true)
        {
            animator.SetBool("TurnL", false);
            // animatorSyncNet.RpcAnimatorSetBool("TurnL", false);
        }
        if (animator.GetBool("TurnR") == true)
        {
            animator.SetBool("TurnR", false);
            // animatorSyncNet.RpcAnimatorSetBool("TurnR", false);
        }
        if (animator.GetInteger("AttackRnd") > -1)
        {
            animator.SetInteger("AttackRnd", -1);
            //animatorSyncNet.RpcAnimatorSetInteger("AttackRnd", -1);
        }
        if (animator.GetBool("Guard") == true)
        {
            animator.SetBool("Guard", false);
            //  animatorSyncNet.RpcAnimatorSetBool("Guard", false);
        }
        if (animator.GetBool("AttackMove") == true)
        {
            animator.SetBool("AttackMove", false);
            // animatorSyncNet.RpcAnimatorSetBool("AttackMove", false);
        }
        if (animator.GetBool("Attack") == true)
        {
            animator.SetBool("Attack", false);
            //animatorSyncNet.RpcAnimatorSetBool("Attack", false);
        }
        strafeTimer -= Time.deltaTime;
        if (strafeTimer <= 0)
        {
            if (animator.GetBool("StrafeLeft") == true)
            {
                animator.SetBool("StrafeLeft", false);
                //  animatorSyncNet.RpcAnimatorSetBool("StrafeLeft", false);
            }
            if (animator.GetBool("StrafeRight") == true)
            {
                animator.SetBool("StrafeRight", false);
                // animatorSyncNet.RpcAnimatorSetBool("StrafeRight", false);
            }
            strafeTimer = 0;
        }

        if (stats.minHealth <= 0) return;
        worldChars = GOManager.worldChars;

        if (!target)
        {
            int indexWinner = -1;
            float mindis = 100000;

            for (int i = 0; i < worldChars.Count; i++)
            {
                StatsSystem worldChar = worldChars[i];
                if (worldChar.minHealth > 0)
                {
                    bool perm = InteractionOfTypesPattern(worldChar);
                    if (perm)
                    {
                        float dis = Vector3.Distance(worldChar.transform.position, _transform.position);
                        if (Vector3.Dot((worldChar.transform.position - _transform.position).normalized, _transform.forward) > 0)
                        {
                            if (dis <= 10 && dis < mindis)
                            {
                                mindis = dis;
                                indexWinner = i;
                            }

                        }
                        else
                        if (dis <= 5 && dis < mindis)
                        {
                            mindis = dis;
                            indexWinner = i;
                        }
                    }
                }
            }
            if (indexWinner > -1)
            {
                RaycastHit raycastHit;
                if (Physics.Linecast(_transform.position + new Vector3(0, 1.75f, 0) + _transform.forward, worldChars[indexWinner].transform.position + new Vector3(0, 1f, 0), out raycastHit, layerMask))
                {
                    if (raycastHit.collider.name == worldChars[indexWinner].name)
                    {
                        target = worldChars[indexWinner];
                        target2 = null;
                    }
                }


            }
            if (!companion && !target2)
                for (int i = 0; i < worldChars.Count; i++)
                {
                    StatsSystem worldChar = worldChars[i];
                    if (worldChar.minHealth > 0)
                    {
                        float dis = Vector3.Distance(worldChar.transform.position, _transform.position);
                        if (worldChar.type != stats.type && worldChar.wpnrdy && dis < 3)
                        {


                            target2 = worldChars[i];

                            break;
                        }

                    }
                }
        }

        if (indexWinner2 > -1 && indexWinner2 < worldChars.Count && worldChars[indexWinner2].minHealth > 0 && Vector3.Distance(worldChars[indexWinner2].transform.position, _transform.position) < 40)
        {
            target = worldChars[indexWinner2];
            agroTimer = 3;
        }
        else
        {
            indexWinner2 = -1;

        }
        AnimatorStateInfo state33 = new AnimatorStateInfo();
        if (stats.humanoid)
        {

            state33 = animator.GetCurrentAnimatorStateInfo(7);
            var state44 = animator.GetCurrentAnimatorStateInfo(0);
            bool draw = (state33.fullPathHash == animDraw1 | state44.fullPathHash == animDraw3);
            bool undraw = (state33.fullPathHash == animDraw2 | state44.fullPathHash == animDraw4);
            if (draw)
            {
                drawe = true;
                if (animator.GetBool("Ready") == false)
                {
                    animator.SetBool("Ready", true);
                    //animatorSyncNet.RpcAnimatorSetBool("Ready", true);
                }
            }
            if (undraw)
            {
                drawe = false;
                if (animator.GetBool("Ready") == true)
                {
                    animator.SetBool("Ready", false);
                    // animatorSyncNet.RpcAnimatorSetBool("Ready", false);
                }
            }
        }
        if (!target && target2 && stats.humanoid)
        {
            Vector3 targetpos = target2.transform.position;
            float dis = Vector3.Distance(targetpos, _transform.position);
            targetTimer = 1;
            wpnrdytimer += deltaTime;
            if (wpnrdytimer >= 0.5f)
            {
                if (wpnrdytimer >= 5)
                {
                    target = target2;
                    target2 = null;
                }
                if (!trig3)
                {
                    if (animator.GetBool("freefight"))
                    {
                        animator.SetBool("freefight", false);
                        // animatorSyncNet.RpcAnimatorSetBool("freefight", false);
                    }
                    if (GOManager.GetVoicePermit(_transform.position) && soundContoll)
                    {
                        soundContoll.WeaponDown();
                        GOManager.npcVoiceArea.Add(new NpcVoiceArea(_transform.position));
                    }

                    stats.SetBoolAsTrigger("Draw");
                    // animatorSyncNet.RpcAnimatorResetTrigger("UnDraw");
                    // animatorSyncNet.RpcAnimatorSetTrigger("Draw");
                    targetAnimator = target2.GetComponent<Animator>();
                    trig3 = true;
                    if (trig4)
                        trig4 = false;
                }
                _transform.rotation = Quaternion.Euler(0, Quaternion.Slerp(_transform.rotation, Quaternion.LookRotation(targetpos - _transform.position), 0.3f).eulerAngles.y, 0);
                if (!target2.wpnrdy)
                {
                    if (GOManager.GetVoicePermit(_transform.position) && soundContoll)
                    {
                        soundContoll.WiseMove();
                        GOManager.npcVoiceArea.Add(new NpcVoiceArea(_transform.position));
                    }
                    target2 = null;
                    weapondownTimer = 0;
                    weapondownTimer2 = 0;
                }
                else
                {
                    weapondownTimer += deltaTime;
                    weapondownTimer2 += deltaTime;
                    if (weapondownTimer >= 4)
                    {
                        if (GOManager.GetVoicePermit(_transform.position) && soundContoll)
                        {
                            soundContoll.WeaponDown2();
                            GOManager.npcVoiceArea.Add(new NpcVoiceArea(_transform.position));
                        }
                        weapondownTimer = -100;
                    }
                    if (weapondownTimer2 >= 9)
                    {
                        if (GOManager.GetVoicePermit(_transform.position) && soundContoll)
                        {
                            soundContoll.WillStop();
                            GOManager.npcVoiceArea.Add(new NpcVoiceArea(_transform.position));
                        }
                        weapondownTimer2 = -100;
                    }
                }
            }
            if (target2 && target2.type == stats.type && dis > 15)
                target2 = null;
        }
        else
        {
            wpnrdytimer = 0;
            targetTimer -= deltaTime;
            if (targetTimer <= 0) targetTimer = 0;
            if (targetTimer == 0)
            {
                if (!trig4)
                {
                    stats.SetBoolAsTrigger("UnDraw");
                    // animatorSyncNet.RpcAnimatorResetTrigger("Draw");
                    // animatorSyncNet.RpcAnimatorSetTrigger("UnDraw");
                    trig4 = true;
                    if (trig3)
                        trig3 = false;
                }


            }

            if (target)
            {
                Vector3 targetpos = target.transform.position;

                targetTimer = 1;
                float dis = Vector3.Distance(targetpos, _transform.position);
                if (target.minHealth > 0)
                {
                    agroTimer += deltaTime;
                    if (dis < 3)
                    {
                        agroTimer = agroTime;
                    }

                    // _transform.rotation = Quaternion.Euler(0, Quaternion.Slerp(_transform.rotation, Quaternion.LookRotation(target.transform.position - _transform.position), 0.3f).eulerAngles.y, 0);
                    NavMeshPath path = new NavMeshPath();

                    if (dis > 10)
                    {
                        stats.damagedFrom.RemoveRange(0, stats.damagedFrom.Count);
                        stats.damageFrom.RemoveRange(0, stats.damageFrom.Count);
                        target = null;
                        return;
                    }



                    if (!trig3)
                    {
                        if (Random.Range(0, 4) == 0 && stats.humanoid && GOManager.GetVoicePermit(_transform.position) && (!target.humanoid | target.orc)  && soundContoll)
                        {
                            soundContoll.DieMonster();
                            GOManager.npcVoiceArea.Add(new NpcVoiceArea(_transform.position));
                        }
                        stats.SetBoolAsTrigger("Draw");
                        // animatorSyncNet.RpcAnimatorSetTrigger("Draw");
                        targetAnimator = target.GetComponent<Animator>();
                        trig3 = true;
                        if (trig4)
                            trig4 = false;
                    }
                    if (agroTimer >= agroTime)
                    {
                        NavMesh.CalculatePath(_transform.position, targetpos, NavMesh.AllAreas, path);
                        if (path.corners.Length > 1)
                        {

                            _transform.rotation = Quaternion.Euler(0, Quaternion.Slerp(_transform.rotation, Quaternion.LookRotation(path.corners[1] - _transform.position), 0.3f).eulerAngles.y, 0);
                        }
                        else
                            _transform.rotation = Quaternion.Euler(0, Quaternion.Slerp(_transform.rotation, Quaternion.LookRotation(targetpos - _transform.position), 0.3f).eulerAngles.y, 0);
                        /*if (target == GOManager.player.GetComponent<StatsSystem>())
                        {
                            
                            if (!trig1)
                            {
                                    trig2 = true;
                                GOManager.attacked++;
                                trig1 = true;
                            }
                        }*/
                        if (dis > stats.rangeAttack)
                        {
                            int rndrun = 2;
                            if (dis < stats.rangeAttack * 2)
                            {
                                rndrun = Random.Range(0, 20);
                                if (rndrun == 0 && strafeTimer == 0 && attackTimerr == 0)
                                {
                                    strafeTimer = Random.Range(0.5f, 1f);
                                    if (animator.GetBool("StrafeLeft") == false)
                                    {
                                        animator.SetBool("StrafeLeft", true);
                                        //  animatorSyncNet.RpcAnimatorSetBool("StrafeLeft", true);
                                    }
                                }
                                if (rndrun == 1 && strafeTimer == 0 && attackTimerr == 0)
                                {
                                    strafeTimer = Random.Range(0.5f, 1f);
                                    if (animator.GetBool("StrafeRight") == false)
                                    {
                                        animator.SetBool("StrafeRight", true);
                                        // animatorSyncNet.RpcAnimatorSetBool("StrafeRight", true);
                                    }
                                }
                            }
                            if (rndrun >= 2)
                                if (animator.GetBool("Run") == false)
                                {
                                    animator.SetBool("Run", true);
                                    // animatorSyncNet.RpcAnimatorSetBool("Run", true);
                                }
                        }

                        var state = animator.GetCurrentAnimatorStateInfo(1);
                        var state2 = animator.GetCurrentAnimatorStateInfo(0);
                        bool attackmove = (state.fullPathHash == anim1 || state33.fullPathHash == anim3);
                        bool attack = (state2.fullPathHash == anim2);
                        bool run = (state2.fullPathHash == anim7 || state2.fullPathHash == anim8);
                        bool back = (state2.fullPathHash == anim9);
                        if (stats.humanoid)
                        {
                            attackmove = (state.fullPathHash == anim3);
                            attack = (state2.fullPathHash == anim4 || state2.fullPathHash == anim5 || state2.fullPathHash == anim6);
                        }

                        backTimer -= Time.deltaTime;
                        attackTimerr -= Time.deltaTime ;
                        if (backTimer <= 0) backTimer = 0;
                        if (attackTimerr <= 0) attackTimerr = 0;

                        if (attackTimer == 0)
                        {


                            attackTimer = Random.Range(attackspeed *0.9f, attackspeed * 1.1f);
                            if (!attackmove && dis < stats.rangeAttack)
                            {
                                animator.SetInteger("AttackRnd", atkrnd);
                                atkrnd++;
                                if (atkrnd > 2) atkrnd = 0;
                                stats.SetBoolAsTrigger("AttackTrigger");
                                animator.SetBool("Attack", true);
                                // animatorSyncNet.RpcAnimatorSetInteger("AttackRnd", atkrnd);
                                // animatorSyncNet.RpcAnimatorSetTrigger("AttackTrigger");
                                // animatorSyncNet.RpcAnimatorSetBool("Attack", true);
                                attackTimerr = 1;
                            }

                            if (dis > stats.rangeAttack && dis < stats.rangeAttack + 2.5f)
                            {
                                if (!attack && backTimer == 0 && strafeTimer == 0)
                                {
                                    stats.SetBoolAsTrigger("AttackTrigger");
                                    animator.SetInteger("AttackRnd", 0);
                                    animator.SetBool("AttackMove", true);
                                    //  animatorSyncNet.RpcAnimatorSetTrigger("AttackTrigger");
                                    //  animatorSyncNet.RpcAnimatorSetInteger("AttackRnd", 0);
                                    // animatorSyncNet.RpcAnimatorSetBool("AttackMove", true);
                                    attackTimerr = 1;
                                }
                            }
                        }
                        if (!attack && !attackmove && dis <= 0.1f && attackTimerr == 0)
                        {
                            if (animator.GetBool("Back") == false)
                            {
                                //    animatorSyncNet.RpcAnimatorSetBool("Back", true);
                                animator.SetBool("Back", true);
                                backTimer = 1;
                            }

                        }

                        if (dis <= target.rangeAttack + 1.2f)
                        {
                            bool attackt = false;
                            if (targetAnimator)
                            {

                                if (stats.humanoid)
                                {
                                    if (!attackmove && target.wpntype < 2 && drawe && Vector3.Dot((_transform.position - targetpos).normalized, target.transform.forward) > 0.5f)
                                    {
                                        var state3 = targetAnimator.GetCurrentAnimatorStateInfo(0);
                                        attackt = (state3.fullPathHash == anim4 || state3.fullPathHash == anim5 || state3.fullPathHash == anim6);

                                    }
                                }
                                else
                                {
                                    if (!attackmove && target.wpntype < 2 && Vector3.Dot((_transform.position - targetpos).normalized, target.transform.forward) > 0.5f)
                                    {

                                        var state3 = targetAnimator.GetCurrentAnimatorStateInfo(0);
                                        attackt = (state3.fullPathHash == anim4 || state3.fullPathHash == anim5 || state3.fullPathHash == anim6);

                                    }
                                }
                            }
                            if (attackt)
                            {
                                if (reactionTime == 0)
                                    reactionTime = Random.Range(0, 100);
                                if (reactionTime > evadeChance)
                                {

                                    if (!attackmove && !attack && attackTimerr == 0)
                                    {
                                        int rnddef = 0;
                                        if (stats.humanoid && target.humanoid)
                                        {
                                            rnddef = Random.Range(0, 3);
                                        }
                                        if (rnddef == 0)
                                        {
                                            if (animator.GetBool("Back") == false)
                                            {
                                                // animatorSyncNet.RpcAnimatorSetBool("Back", true);
                                                animator.SetBool("Back", true);
                                                backTimer = 1;
                                            }
                                        }
                                        else
                                        {
                                            if (animator.GetBool("Block") == false)
                                            {
                                                int blockrnd = Random.Range(0, 3);
                                                //  animatorSyncNet.RpcAnimatorSetBool("Block", true);
                                                //  animatorSyncNet.RpcAnimatorSetInteger("BlockRnd", blockrnd);
                                                animator.SetBool("Block", true);
                                                animator.SetInteger("BlockRnd", blockrnd);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                reactionTime = 0;
                            }


                        }

                    }
                    else
                    {
                        _transform.rotation = Quaternion.Euler(0, Quaternion.Slerp(_transform.rotation, Quaternion.LookRotation(targetpos - _transform.position), 0.3f).eulerAngles.y, 0);
                        if (dis > 10)
                        {
                            stats.damagedFrom.RemoveRange(0, stats.damagedFrom.Count);
                            stats.damageFrom.RemoveRange(0, stats.damageFrom.Count);
                            target = null;
                        }
                    }


                }
                else
                {
                    if (Random.Range(0, 4) == 0 && stats.humanoid && GOManager.GetVoicePermit(_transform.position))
                    {
                        if (target.orc)
                            soundContoll.MonsterKilled();
                        else
                        if (target.humanoid)
                            soundContoll.EnemyKilled();
                        else
                            soundContoll.MonsterKilled();
                        GOManager.npcVoiceArea.Add(new NpcVoiceArea(_transform.position));
                    }
                    stats.damagedFrom.RemoveRange(0, stats.damagedFrom.Count);
                    stats.damageFrom.RemoveRange(0, stats.damageFrom.Count);
                    target = null;
                }
                if (animator.GetBool("freefight"))
                {
                    animator.SetBool("freefight", false);
                    //animatorSyncNet.RpcAnimatorSetBool("freefight", false);
                }
            }
            else
            {


                if (trig2)
                {

                    if (trig1)
                        trig1 = false;

                    trig2 = false;
                    GOManager.attacked--;
                }

                targetTimer -= deltaTime;
                if (targetTimer <= 0) targetTimer = 0;
                if (targetTimer == 0)
                {
                    if (!trig4)
                    {
                        stats.SetBoolAsTrigger("UnDraw");
                        // animatorSyncNet.RpcAnimatorSetTrigger("UnDraw");
                        trig4 = true;
                        if (trig3)
                            trig3 = false;
                    }
                }
                agroTimer = 0;
                drinkTimer -= deltaTime;
                if (drinkTimer <= 0) drinkTimer = 0;
                if (stats.humanoid && stats.type != StatsSystem.Type.Orc && stats.minHealth < stats.maxHealth && stats.itemDrops[1].countMin > 0)
                {
                    if (drinkTimer == 0)
                    {
                        drinkTimer = 1.21f;
                        Transform potionOffsetTransform = GetComponent<WeaponController>().hand3.GetChild(0);
                        GameObject go = Instantiate(stats.itemDrops[1].item.WorldItem, potionOffsetTransform.position, potionOffsetTransform.rotation, potionOffsetTransform);
                        DestroyImmediate(go.GetComponent<WorldItem>());
                        Destroy(go, 1.2f);
                        stats.itemDrops[1].countMin--;
                        stats.itemDrops[1].countMax--;
                        stats.CmdHeal(50);
                        stats.SetBoolAsTrigger("DrinkPotion");
                        stats.audioSource.PlayOneShot(Resources.Load("Sounds/SFX/DRINK_POTION") as AudioClip);
                    }
                }
                else
                {
                    animator.SetBool("freefight", freefightBool);
                    // animatorSyncNet.RpcAnimatorSetBool("freefight", freefightBool);
                    if (freefightBool)
                    {
                        if (!trig11)
                        {
                            stats.SetBoolAsTrigger("Draw");
                            //animatorSyncNet.RpcAnimatorSetTrigger("Draw");
                            trig11 = true;
                        }



                    }
                    else
                    {
                        if (trig11)
                        {
                            //animator.ResetTrigger("UnDraw");
                            //animator.SetTrigger("UnDraw");
                            stats.SetBoolAsTrigger("UnDraw");
                            // animatorSyncNet.RpcAnimatorSetTrigger("UnDraw");
                            trig11 = false;
                        }
                    }
                    if (!freefightBool)
                        if (companion)
                        {
                            if (!trig10)
                            {
                                CombatSystemComp = companion.GetComponent<CombatSystem>();
                                trig10 = true;
                            }
                            if (CombatSystemComp.enemy && CombatSystemComp.enemy.minHealth > 0)
                                target = CombatSystemComp.enemy;
                            float dis = Vector3.Distance(companion.transform.position, _transform.position);
                            // _transform.rotation = Quaternion.Euler(0, Quaternion.Slerp(_transform.rotation, Quaternion.LookRotation(companion.transform.position - _transform.position), 0.2f).eulerAngles.y, 0);
                            NavMeshPath path = new NavMeshPath();
                            NavMesh.CalculatePath(_transform.position, companion.transform.position, NavMesh.AllAreas, path);
                            if (path.corners.Length > 1)
                            {

                                _transform.rotation = Quaternion.Euler(0, Quaternion.Slerp(_transform.rotation, Quaternion.LookRotation(path.corners[1] - _transform.position), 0.3f).eulerAngles.y, 0);
                            }

                            if (dis > 2)
                            {
                                animator.SetBool("Run", true);
                                // animatorSyncNet.RpcAnimatorSetBool("Run", true);
                            }
                            else
                            {
                                float dot = _transform.rotation.eulerAngles.y - oldY;
                                animator.SetBool("TurnL", dot < -0.2f);
                                animator.SetBool("TurnR", dot > 0.2f);
                                animator.SetFloat("TurnSpeed", Mathf.Abs(dot) * 0.3f);
                                //  animatorSyncNet.RpcAnimatorSetBool("TurnL", dot < -0.2f);
                                // animatorSyncNet.RpcAnimatorSetBool("TurnR", dot > 0.2f);
                                // animatorSyncNet.RpcAnimatorSetFloat("TurnSpeed", Mathf.Abs(dot) * 0.3f);

                            }
                        }
                        else
                        {
                            if (trig10)
                            {
                                trig10 = false;
                            }
                            if (chair)
                            {
                                if (!animator.GetBool("Seet"))
                                {
                                    animator.SetBool("Seet", true);
                                    //animatorSyncNet.CmdAnimatorSetBool("Seet", true);
                                    chair.transform.GetChild(1).GetComponent<Collider>().enabled = false;
                                    _transform.position = chair.transform.GetChild(2).position;
                                    float rotation = Quaternion.LookRotation(chair.transform.position - _transform.position, Vector3.up).eulerAngles.y;
                                    _transform.rotation = Quaternion.Euler(0, rotation, 0);
                                }

                            }
                            else
                            {
                                if (animator.GetBool("Seet"))
                                {
                                    animator.SetBool("Seet", false);
                                    // animatorSyncNet.CmdAnimatorSetBool("Seet", false);
                                    chair.transform.GetChild(1).GetComponent<Collider>().enabled = true;
                                }
                            }
                            if (!chair)
                                if (dialogueTarget)
                                {
                                    float dis = Vector3.Distance(dialogueTarget.transform.position, _transform.position);
                                    _transform.rotation = Quaternion.Euler(0, Quaternion.Slerp(_transform.rotation, Quaternion.LookRotation(dialogueTarget.transform.position - _transform.position), 0.2f).eulerAngles.y, 0);
                                    {
                                        float dot = _transform.rotation.eulerAngles.y - oldY;
                                        animator.SetBool("TurnL", dot < -0.15f);
                                        animator.SetBool("TurnR", dot > 0.15f);
                                        animator.SetFloat("TurnSpeed", Mathf.Abs(dot) * 0.3f);
                                        // animatorSyncNet.RpcAnimatorSetBool("TurnL", dot < -0.15f);
                                        // animatorSyncNet.RpcAnimatorSetBool("TurnR", dot > 0.15f);
                                        // animatorSyncNet.RpcAnimatorSetFloat("TurnSpeed", Mathf.Abs(dot) * 0.3f);

                                    }

                                }
                                else
                                {
                                    float dis = Vector3.Distance(startPosRnd, _transform.position);
                                    if (randomWay)
                                    {
                                        randomWayTimer += deltaTime;
                                        if (randomWayTimer >= 5)
                                        {
                                            randomWayTimer = 0;
                                            startPosRnd = startPos + new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
                                            RaycastHit raycastHit;
                                            if (Physics.Raycast(startPosRnd + new Vector3(0, 2, 0), new Vector3(0, -2, 0), out raycastHit, 4))
                                                startPosRnd = new Vector3(startPosRnd.x, raycastHit.point.y + 0.1f, startPosRnd.z);
                                            else
                                                startPosRnd = startPos;
                                        }
                                    }
                                    if (dis > 2)
                                    {


                                        NavMeshPath path = new NavMeshPath();
                                        NavMesh.CalculatePath(_transform.position, startPosRnd, NavMesh.AllAreas, path);

                                        if (path.corners.Length > 1)
                                        {
                                            _transform.rotation = Quaternion.Euler(0, Quaternion.Slerp(_transform.rotation, Quaternion.LookRotation(path.corners[1] - _transform.position), 0.3f).eulerAngles.y, 0);
                                            // for (int i = 0; i < path.corners.Length - 1; i++)
                                            //  Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
                                            animator.SetBool("Run", true);
                                            //  animatorSyncNet.RpcAnimatorSetBool("Run", true);
                                        }

                                    }

                                    else
                                    {
                                        RoutineBool = true;



                                        routineTimer -= deltaTime;
                                        if (social && routineTimer <= 0)
                                        {
                                            routineTimer = 30;
                                            for (int i = 0; i < worldChars.Count; i++)
                                            {
                                                float disTo = Vector3.Distance(worldChars[i].transform.position, _transform.position);
                                                if (disTo < 3 && worldChars[i].type == stats.type && worldChars[i] != stats && !worldChars[i].PLAYER)
                                                {
                                                    routineDialogueTarget = worldChars[i];
                                                    if (Random.Range(0, 2) == 0)
                                                        break;
                                                }
                                            }
                                        }


                                        if (routineDialogueTarget)
                                        {
                                            float disToDialogueTarget = Vector3.Distance(routineDialogueTarget.transform.position, _transform.position);

                                            _transform.rotation = Quaternion.Euler(0, Quaternion.Slerp(_transform.rotation, Quaternion.LookRotation(routineDialogueTarget.transform.position - _transform.position), 0.3f).eulerAngles.y, 0);

                                            float dot = _transform.rotation.eulerAngles.y - oldY;
                                            animator.SetBool("TurnL", dot < -0.15f);
                                            animator.SetBool("TurnR", dot > 0.15f);
                                            animator.SetFloat("TurnSpeed", Mathf.Abs(dot) * 0.3f);
                                            //  animatorSyncNet.RpcAnimatorSetBool("TurnL", dot < -0.15f);
                                            //animatorSyncNet.RpcAnimatorSetBool("TurnR", dot > 0.15f);
                                            //  animatorSyncNet.RpcAnimatorSetFloat("TurnSpeed", Mathf.Abs(dot) * 0.3f);

                                            smallTalkTimer += deltaTime;

                                            if (smallTalkTimer >= 4)
                                            {
                                                if (Random.Range(0, 5) == 0)
                                                {
                                                    if (soundContoll)
                                                        soundContoll.SmallTalk();
                                                    int value = Random.Range(0, 11);
                                                    animator.SetTrigger("Dialogue");
                                                    animator.SetInteger("DialogueRnd", value);
                                                    // animatorSyncNet.RpcAnimatorSetTrigger("Dialogue");
                                                    // animatorSyncNet.RpcAnimatorSetInteger("DialogueRnd", value);
                                                }
                                                smallTalkTimer = 0;
                                            }
                                            if (disToDialogueTarget > 3) routineDialogueTarget = null;
                                        }
                                        else
                                        {
                                            if (startPosRnd == startPos)
                                                _transform.rotation = Quaternion.Euler(0, Quaternion.Slerp(_transform.rotation, Quaternion.Euler(0, startRot.y, 0), 0.2f).eulerAngles.y, 0);
                                            else
                                                _transform.rotation = Quaternion.Euler(0, Quaternion.Slerp(_transform.rotation, Quaternion.LookRotation(startPosRnd - _transform.position), 0.3f).eulerAngles.y, 0);
                                            float dot = _transform.rotation.eulerAngles.y - oldY;
                                            animator.SetBool("TurnL", dot < -0.15f);
                                            animator.SetBool("TurnR", dot > 0.15f);
                                            animator.SetFloat("TurnSpeed", Mathf.Abs(dot) * 0.3f);
                                            // animatorSyncNet.RpcAnimatorSetBool("TurnL", dot < -0.15f);
                                            // animatorSyncNet.RpcAnimatorSetBool("TurnR", dot > 0.15f);
                                            //  animatorSyncNet.RpcAnimatorSetFloat("TurnSpeed", Mathf.Abs(dot) * 0.3f);
                                            animator.SetBool("Guard", true);
                                            //  animatorSyncNet.RpcAnimatorSetBool("Guard", true);
                                        }

                                    }
                                }
                        }
                }
                oldY = _transform.rotation.eulerAngles.y;
            }
        }
        attackTimer -= deltaTime;
        if (attackTimer <= 0) attackTimer = 0;
        animator.SetBool("Angry", agroTimer >= agroTime);
        // animatorSyncNet.RpcAnimatorSetBool("Angry", agroTimer >= agroTime);
    }

    public void DealDamage()
    {
        if (isServer)
        {
            int iHits = 0;
            if (!target) return;
            if (!(Vector3.Distance(_transform.position, target.transform.position) <= stats.rangeAttack)) return;

            for (int i = 0; i < worldChars.Count; i++)
            {
                StatsSystem targetStats = worldChars[i].GetComponent<StatsSystem>();
                Transform trTarget = targetStats.transform;
                float dis = Vector3.Distance(trTarget.position, _transform.position);
                float dot = Vector3.Dot(trTarget.position - _transform.position, _transform.forward);
                bool perm = true;
                if (targetStats.type == stats.type)
                    perm = false;
                if (target == targetStats)
                    perm = true;
                if (targetStats.minHealth > 0 && perm && trTarget != _transform && dis <= stats.rangeAttack && dot > 0.5f)
                {

                    int dmg1 = (stats.weaponDamage + stats.strength);
                    dmg1 /= (int)(10f / (10f - iHits));
                    iHits++;
                    bool crit = false;
                    int dmg2 = 0;
                    int dmg3 = stats.weaponFireDamage;
                    int dmg4 = stats.weaponMagicDamage;
                    float crtforce = 1;
                    if (stats.humanoid)
                    {
                        if (Random.Range(0, 101) < stats.skill1h)
                        {
                            crit = true;
                            crtforce = 2;
                        }
                    }
                    else
                    {
                        crit = true;
                    }
                    if (targetStats.rigidbodyPart)
                    {
                        if (atkrnd == 2)
                            targetStats.rigidbodyPart.AddForce(_transform.right * -dmg1 / targetStats.maxHealth * 80000 * crtforce, ForceMode.Force);
                        if (atkrnd == 1)
                            targetStats.rigidbodyPart.AddForce(_transform.right * dmg1 / targetStats.maxHealth * 80000 * crtforce, ForceMode.Force);
                        if (atkrnd == 0)
                            targetStats.rigidbodyPart.AddForce(_transform.forward * dmg1 / targetStats.maxHealth * 80000 * crtforce, ForceMode.Force);
                        // targetStats.rigidbodyPart.AddForce(weaponForce * -dmg1 / targetStats.maxHealth * 200000, ForceMode.Force);
                    }
                    targetStats.TakeDamage(dmg1, dmg2, dmg3, dmg4, crit, gameObject);
                }
            }
        }
    }
    public void Equip()
    {
        wpn1.transform.SetParent(hand);
        wpn1.transform.localPosition = new Vector3(0f, 0f, 0f);
        wpn1.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
    }
    public void UnEquip()
    {
        wpn1.transform.SetParent(hip);
        wpn1.transform.localPosition = new Vector3(0f, 0f, 0f);
        wpn1.transform.localEulerAngles = new Vector3(0f, 0f, 0f);




    }
    /* public void TrailEmitOn()
     {
         mwt.Emit = true;

     }
     public void TrailEmitOff()
     {
         mwt.Emit = false;
     }*/

    public void AttackFireWaranEffect()
    {
        attackEffect.Play();
    }


    bool InteractionOfTypesPattern(StatsSystem ss)
    {
        bool perm = false;

        //for Neutral
        if (stats.type == StatsSystem.Type.Neutral && ss.type == StatsSystem.Type.Bandit)
            perm = true;
        if (stats.type == StatsSystem.Type.Neutral && ss.type == StatsSystem.Type.Creature)
            perm = true;
        if (stats.type == StatsSystem.Type.Neutral && ss.type == StatsSystem.Type.Orc)
            perm = true;
        if (stats.type == StatsSystem.Type.Neutral && ss.type == StatsSystem.Type.Undead)
            perm = true;
        if (stats.type == StatsSystem.Type.Neutral && ss.type == StatsSystem.Type.Golem)
            perm = true;
        //for Paladin
        if (stats.type == StatsSystem.Type.Paladin && ss.type == StatsSystem.Type.Bandit)
            perm = true;
        if (stats.type == StatsSystem.Type.Paladin && ss.type == StatsSystem.Type.Creature)
            perm = true;
        if (stats.type == StatsSystem.Type.Paladin && ss.type == StatsSystem.Type.Orc)
            perm = true;
        if (stats.type == StatsSystem.Type.Paladin && ss.type == StatsSystem.Type.Undead)
            perm = true;
        if (stats.type == StatsSystem.Type.Paladin && ss.type == StatsSystem.Type.Golem)
            perm = true;

        //for Mage
        if (stats.type == StatsSystem.Type.Mage && ss.type == StatsSystem.Type.Bandit)
            perm = true;
        if (stats.type == StatsSystem.Type.Mage && ss.type == StatsSystem.Type.Creature)
            perm = true;
        if (stats.type == StatsSystem.Type.Mage && ss.type == StatsSystem.Type.Orc)
            perm = true;
        if (stats.type == StatsSystem.Type.Mage && ss.type == StatsSystem.Type.Undead)
            perm = true;
        if (stats.type == StatsSystem.Type.Mage && ss.type == StatsSystem.Type.Golem)
            perm = true;

        //for Mercenary
        if (stats.type == StatsSystem.Type.Mercenary && ss.type == StatsSystem.Type.Bandit)
            perm = true;
        if (stats.type == StatsSystem.Type.Mercenary && ss.type == StatsSystem.Type.Creature)
            perm = true;
        if (stats.type == StatsSystem.Type.Mercenary && ss.type == StatsSystem.Type.Orc)
            perm = true;
        if (stats.type == StatsSystem.Type.Mercenary && ss.type == StatsSystem.Type.Undead)
            perm = true;
        if (stats.type == StatsSystem.Type.Mercenary && ss.type == StatsSystem.Type.Golem)
            perm = true;

        // for Orc
        if (stats.type == StatsSystem.Type.Orc && ss.type == StatsSystem.Type.Bandit)
            perm = true;
        if (stats.type == StatsSystem.Type.Orc && ss.type == StatsSystem.Type.Paladin)
            perm = true;
        if (stats.type == StatsSystem.Type.Orc && ss.type == StatsSystem.Type.Mercenary)
            perm = true;
        if (stats.type == StatsSystem.Type.Orc && ss.type == StatsSystem.Type.Undead)
            perm = true;
        if (stats.type == StatsSystem.Type.Orc && ss.type == StatsSystem.Type.Golem)
            perm = true;
        if (stats.type == StatsSystem.Type.Orc && ss.type == StatsSystem.Type.Neutral)
            perm = true;
        if (stats.type == StatsSystem.Type.Orc && ss.type == StatsSystem.Type.Mage)
            perm = true;

        // for Creature
        if (stats.type == StatsSystem.Type.Creature && ss.type == StatsSystem.Type.Bandit)
            perm = true;
        if (stats.type == StatsSystem.Type.Creature && ss.type == StatsSystem.Type.Paladin)
            perm = true;
        if (stats.type == StatsSystem.Type.Creature && ss.type == StatsSystem.Type.Mercenary)
            perm = true;
        if (stats.type == StatsSystem.Type.Creature && ss.type == StatsSystem.Type.Undead)
            perm = true;
        if (stats.type == StatsSystem.Type.Creature && ss.type == StatsSystem.Type.Golem)
            perm = true;
        if (stats.type == StatsSystem.Type.Creature && ss.type == StatsSystem.Type.Neutral)
            perm = true;
        if (stats.type == StatsSystem.Type.Creature && ss.type == StatsSystem.Type.Mage)
            perm = true;

        // for Bandit
        if (stats.type == StatsSystem.Type.Bandit && ss.type == StatsSystem.Type.Creature)
            perm = true;
        if (stats.type == StatsSystem.Type.Bandit && ss.type == StatsSystem.Type.Paladin)
            perm = true;
        if (stats.type == StatsSystem.Type.Bandit && ss.type == StatsSystem.Type.Mercenary)
            perm = true;
        if (stats.type == StatsSystem.Type.Bandit && ss.type == StatsSystem.Type.Undead)
            perm = true;
        if (stats.type == StatsSystem.Type.Bandit && ss.type == StatsSystem.Type.Golem)
            perm = true;
        if (stats.type == StatsSystem.Type.Bandit && ss.type == StatsSystem.Type.Neutral)
            perm = true;
        if (stats.type == StatsSystem.Type.Bandit && ss.type == StatsSystem.Type.Mage)
            perm = true;
        if (stats.type == StatsSystem.Type.Bandit && ss.type == StatsSystem.Type.Orc)
            perm = true;
        if (stats.humanoid && _dialogueTarget.ID == 1013 && !QuestSystem._DialogueBndt1013Angry)
            perm = false;

        // for Golem
        if (stats.type == StatsSystem.Type.Golem && ss.type == StatsSystem.Type.Creature)
            perm = true;
        if (stats.type == StatsSystem.Type.Golem && ss.type == StatsSystem.Type.Paladin)
            perm = true;
        if (stats.type == StatsSystem.Type.Golem && ss.type == StatsSystem.Type.Mercenary)
            perm = true;
        if (stats.type == StatsSystem.Type.Golem && ss.type == StatsSystem.Type.Undead)
            perm = true;
        if (stats.type == StatsSystem.Type.Golem && ss.type == StatsSystem.Type.Bandit)
            perm = true;
        if (stats.type == StatsSystem.Type.Golem && ss.type == StatsSystem.Type.Neutral)
            perm = true;
        if (stats.type == StatsSystem.Type.Golem && ss.type == StatsSystem.Type.Mage)
            perm = true;
        if (stats.type == StatsSystem.Type.Golem && ss.type == StatsSystem.Type.Orc)
            perm = true;

        // for Undead
        if (stats.type == StatsSystem.Type.Undead && ss.type == StatsSystem.Type.Creature)
            perm = true;
        if (stats.type == StatsSystem.Type.Undead && ss.type == StatsSystem.Type.Paladin)
            perm = true;
        if (stats.type == StatsSystem.Type.Undead && ss.type == StatsSystem.Type.Mercenary)
            perm = true;
        if (stats.type == StatsSystem.Type.Undead && ss.type == StatsSystem.Type.Golem)
            perm = true;
        if (stats.type == StatsSystem.Type.Undead && ss.type == StatsSystem.Type.Bandit)
            perm = true;
        if (stats.type == StatsSystem.Type.Undead && ss.type == StatsSystem.Type.Neutral)
            perm = true;
        if (stats.type == StatsSystem.Type.Undead && ss.type == StatsSystem.Type.Mage)
            perm = true;
        if (stats.type == StatsSystem.Type.Undead && ss.type == StatsSystem.Type.Orc)
            perm = true;


        return perm;
    }
}
