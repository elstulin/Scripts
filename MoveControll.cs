using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using System.Xml;
using System.IO;
using UnityEngine.Rendering.HighDefinition;
public class MoveControll : NetworkBehaviour
{
    public Animator animator;
    public NetworkAnimator networkAnimator;
    public HeadEmotions headEmotions;
    public float rotation;
    public float inertX;
    bool landing;
    public float fallTime;
    public Vector3 jumpvelocity;
    public new Transform transform;
    public float jumpTimer;
    public CharacterController cc;
    public StatsSystem stats;
    public float sadw;
    public InventorySystem IS;
    public DialogueSystem DS;
    public CombatSystem combatSystem;
    public InventorySystem inventorySystem;
    public Vector3 lookTarget;
    public Vector3 lookeTarget;
    public Vector3 lookDialogTarget;
    float attackTimer;
    float equipTimer;
    bool failAttack;
    public LadderObject targetLadder;
    public float moveSpeed;
    WeaponController weaponController;
    int animDraw1;
    int animDraw2;
    int animDraw3;
    int animDraw4;
    float JumpDis;
    Vector3 oldPos;
    float oldDeltaTime;
    Vector3 inert;
    float mouseX;
    int anim1 = Animator.StringToHash("Bottom.Jump");
    int anim2 = Animator.StringToHash("Bottom.RunJump");
    int anim3 = Animator.StringToHash("Bottom.JumpUpLow");
    int anim4 = Animator.StringToHash("Bottom.JumpUpMid");
    int anim5 = Animator.StringToHash("Bottom.LadderUp");
    int anim6 = Animator.StringToHash("Bottom.LadderIdle");
    int anim7 = Animator.StringToHash("Bottom.LadderDown");
    int anim8 = Animator.StringToHash("Bottom.LadderStart2");
    int anim9 = Animator.StringToHash("Bottom.LadderEnd");
    int anim10 = Animator.StringToHash("Bottom.LadderStart");
    int anim11 = Animator.StringToHash("Bottom.LadderStart2");
    int anim12 = Animator.StringToHash("Bottom.1h3idle");
    int anim13 = Animator.StringToHash("Bottom.1h3attackfront");
    public float lerpRota;
    public float timerUpdateInBase;
    public Vector3 lastPos;
    private bool enduranceUIActive;
    public bool controllBlock
    {
        get { return (ChatOmegaLul.active || MenuController.active || InventorySystem.UIActive || CharacterChanger.UIActive); }
    }
    public Vector3 jumpTarget;
    public Transform BodyBone;
    public Transform BodyBone2;
    public Transform BodyBone3;
    bool block;
    int combo;
    int maxCombo = 4;
    public static bool classicControll;
    bool strafeR;
    bool strafeL;
    bool run;
    bool back;
    bool sprint;
    float endurance = 100;
    bool sitGround;
    public bool dance;
    public WaterSurface waterSurface;
    private Transform waterSurfaceTransform;
    private WaterSearchParameters search;
    WaterSearchResult searchResult;
    public bool swim;
    private bool inWater;
    public float inWaterDelayTimer;
    private bool rippleOn;
    private float swimDamageUpdateTimer;
    private int swimDamageCount;
    public GameObject rippleSpawnerGo;
    Transform rippleSpawnerTransform;
    void Start()
    {


        // Application.targetFrameRate = 60;
        combatSystem = GetComponent<CombatSystem>();
        weaponController = GetComponent<WeaponController>();
        inventorySystem = GetComponent<InventorySystem>();
        animator = GetComponent<Animator>();
        networkAnimator = GetComponent<NetworkAnimator>();
        networkAnimator.animator = animator;
        transform = GetComponent<Transform>();
        cc = GetComponent<CharacterController>();
        stats = GetComponent<StatsSystem>();
        IS = GetComponent<InventorySystem>();
        DS = GetComponent<DialogueSystem>();
        animDraw1 = Animator.StringToHash("Up.equip");
        animDraw2 = Animator.StringToHash("Up.equip2");
        animDraw3 = Animator.StringToHash("Bottom.1h3draw");
        animDraw4 = Animator.StringToHash("Bottom.1h3draw2");
        anim1 = Animator.StringToHash("Bottom.Jump");
        anim2 = Animator.StringToHash("Bottom.RunJump");
        anim3 = Animator.StringToHash("Bottom.JumpUpLow");
        anim4 = Animator.StringToHash("Bottom.JumpUpMid");
        anim5 = Animator.StringToHash("Bottom.LadderUp");
        anim6 = Animator.StringToHash("Bottom.LadderIdle");
        anim7 = Animator.StringToHash("Bottom.LadderDown");
        anim8 = Animator.StringToHash("Bottom.LadderStart2");
        anim9 = Animator.StringToHash("Bottom.LadderEnd");

        anim10 = Animator.StringToHash("Bottom.LadderStart");
        anim11 = Animator.StringToHash("Bottom.LadderStart2");
        animator.SetBool("Angry", true);
        if (netIdentity.isLocalPlayer)
        {
            waterSurface = GameObject.Find("Ocean").GetComponent<WaterSurface>();
            waterSurfaceTransform = waterSurface.transform;
            GOManager.waterHeight = waterSurfaceTransform.position.y;
        }
        rippleSpawnerTransform = rippleSpawnerGo.transform;
    }
    void LateUpdate()
    {
        float str = (CameraController.instance.cinemachineFreeLook.m_YAxis.Value - 0.5f) * 45;
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if ((state.fullPathHash == anim12 | state.fullPathHash == anim13) && stats.wpnrdy)
        {
            if (stats.wpntype == 2)
                BodyBone.localEulerAngles = new Vector3(0, str, -str);
            if (stats.wpntype == 3)
                BodyBone.localEulerAngles = new Vector3(0, 0, -str);
        }

        if (controllBlock)
        {
            if (Cursor.lockState != CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else
        {
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                
            }
        }
    }
    public IEnumerator DialogueRnd()
    {
        while (true)
        {
            animator.SetInteger("DialogueRnd", Random.Range(0, 11));
            yield return new WaitForSeconds(0.5f);
        }

    }
    float GetAtkspd()
    {
        float atspd = 1;
        if (stats.wpntype == 0)
        {

            if (stats.skill1h >= 60)
            {

                if (combo == 0)
                {
                    atspd = 0.76f;
                }
                else if (combo == 1)
                {
                    atspd = 1.04f;
                }
                else if (combo == 2)
                {
                    atspd = 1.08f;
                }
                else if (combo == 3)
                {
                    atspd = 0.72f;
                }
                maxCombo = 3;
            }
            else
                if (stats.skill1h >= 30)
            {

                if (combo == 0)
                {
                    atspd = 1.16f;
                }
                else if (combo == 1)
                {
                    atspd = 1.12f;
                }
                else if (combo == 2)
                {
                    atspd = 1.12f;
                }
                else if (combo == 3)
                {
                    atspd = 0.6f;
                }
                maxCombo = 3;
            }
            else
            {

                if (combo == 0)
                {
                    atspd = 0.72f;
                }
                else if (combo == 1)
                {
                    atspd = 0.52f;
                }
                else if (combo == 2)
                {
                    atspd = 0.52f;
                }
                else if (combo == 3)
                {
                    atspd = 0.52f;
                }
                maxCombo = 1;
            }
        }
        else if (stats.wpntype == 1)
        {

            if (stats.skill2h >= 60)
            {

                if (combo == 0)
                {
                    atspd = 1.36f;
                }
                else if (combo == 1)
                {
                    atspd = 1.2f;
                }
                else if (combo == 2)
                {
                    atspd = 1.64f;
                }
                else if (combo == 3)
                {
                    atspd = 1f;
                }
                maxCombo = 3;
            }
            else
                if (stats.skill2h >= 30)
            {

                if (combo == 0)
                {
                    atspd = 1.4f;
                }
                else if (combo == 1)
                {
                    atspd = 0.72f;
                }
                else if (combo == 2)
                {
                    atspd = 0.76f;
                }
                else if (combo == 3)
                {
                    atspd = 0.76f;
                }
                maxCombo = 2;
            }
            else
            {

                if (combo == 0)
                {
                    atspd = 0.72f;
                }
                else if (combo == 1)
                {
                    atspd = 0.8f;
                }
                else if (combo == 2)
                {
                    atspd = 0.8f;
                }
                else if (combo == 3)
                {
                    atspd = 0.8f;
                }
                maxCombo = 1;
            }
        }
        return atspd;
    }
    void Update()
    {
        if (!networkAnimator.netIdentity.isLocalPlayer) enabled = false;

        waterSurface.deformationAreaOffset = new Vector2(transform.position.x - waterSurfaceTransform.position.x, transform.position.z - waterSurfaceTransform.position.z);
        /* var state33 = animator.GetCurrentAnimatorStateInfo(7);
        var state44 = animator.GetCurrentAnimatorStateInfo(0);
       bool draw = (state33.fullPathHash == animDraw1 | state44.fullPathHash == animDraw3);
        bool undraw = (state33.fullPathHash == animDraw2 | state44.fullPathHash == animDraw4);
        if (draw)
        {
            animator.SetBool("Ready", true);
            //CmdAnimatorSetBool("Ready", true);
        }
        if (undraw)
        {
            animator.SetBool("Ready", false);
            // CmdAnimatorSetBool("Ready", false);
        }*/
        float deltaTime = Time.deltaTime;

        bool mouse0 = false;
        bool alpha1 = false;
        bool w = false;
        bool s = false;
        bool a = false;
        bool d = false;
        bool e = false;
        bool q = false;
        bool space = false;
        alpha1 = Input.GetKey(KeyCode.Alpha1);
        if (stats.god && Input.GetKey(KeyCode.F))
        {
            cc.Move((transform.forward + transform.up * 0.5f) * 15 * (1 + moveSpeed) * deltaTime);
            if (transform.position.y > 100) transform.position = new Vector3(transform.position.x, 100, transform.position.z);
            fallTime = 0.16f;
            landing = false;
        }
        if (inWaterDelayTimer <= 0f)
        {
            if (transform.position.y <= GOManager.waterHeight - 1f)
            {
                if (!swim && !Physics.Linecast(new Vector3(transform.position.x, Mathf.Clamp(transform.position.x, GOManager.waterHeight, 1000), transform.position.z), new Vector3(transform.position.x, Mathf.Clamp(transform.position.x, GOManager.waterHeight, 1000) - 1f, transform.position.z), LayerMask.GetMask("Default")))
                {
                    if (!animator.applyRootMotion)
                        animator.applyRootMotion = true;
                    if (!rippleSpawnerGo.activeSelf)
                        rippleSpawnerGo.SetActive(true);
                    rippleSpawnerTransform.localPosition = new Vector3(0, 0, 1);
                    animator.SetBool("Swimming", true);
                    swim = true;
                    stats.SetBoolAsTrigger("SwimTrigger");
                    if (stats.wpnrdy)
                    {
                        stats.weaponController.UnEquip();
                    }
                }

                else
                {
                    swimDamageUpdateTimer -= deltaTime;
                }
            }
            else
            {
                if (swim && (cc.isGrounded))
                {
                    cc.Move(transform.forward * 0.4f);
                    swim = false;
                    animator.SetBool("Swimming", false);
                }
                if (transform.position.y <= GOManager.waterHeight - 0.3f)
                {
                    if (!inWater)
                    {
                        if (!rippleSpawnerGo.activeSelf)
                            rippleSpawnerGo.SetActive(true);
                        rippleSpawnerTransform.localPosition = new Vector3(0, 0, 0);
                        inWater = true;
                        animator.SetInteger("InWater", 1);

                    }
                }
                else
                {
                    if (transform.position.y <= GOManager.waterHeight)
                    {
                        if (!rippleOn)
                        {
                            rippleOn = true;
                            if (!rippleSpawnerGo.activeSelf)
                                rippleSpawnerGo.SetActive(true);
                            rippleSpawnerTransform.localPosition = new Vector3(0, 0, 0);
                        }
                    }
                    else
                    {
                        if (rippleOn)
                        {
                            rippleOn = false;
                            if (rippleSpawnerGo.activeSelf)
                                rippleSpawnerGo.SetActive(false);
                        }
                    }
                    if (inWater)
                    {
                        inWater = false;
                        animator.SetInteger("InWater", 0);
                    }
                }
            }
            inWaterDelayTimer = 0.1f;
        }
        else
        {
            inWaterDelayTimer -= deltaTime;
        }
        if (swim)
        {
            search.startPositionWS = transform.position;
            waterSurface.ProjectPointOnWaterSurface(search, out searchResult);
            float waterHeightLerp = Mathf.Lerp(searchResult.projectedPositionWS.y, searchResult.projectedPositionWS.y, 0.01f);
            transform.position = new Vector3(transform.position.x, waterHeightLerp - 1f, transform.position.z);
            if (run)
                cc.Move(transform.forward * (1.3f * (1 + moveSpeed)) * Time.deltaTime);

            if (swimDamageUpdateTimer <= 0)
            {
                if (Vector3.Distance(transform.position, new Vector3(-595.446106f, -51.5f, -138.579788f)) > 150 || endurance <= 0)
                {
                    swimDamageCount++;
                    int dmg1 = (stats.maxHealth / 100 + 1) * swimDamageCount * swimDamageCount;
                    int dmg2 = 0;
                    int dmg3 = 0;
                    int dmg4 = 0;
                    stats.CmdTakeDamage(dmg1, dmg2, dmg3, dmg4, true, gameObject.GetComponent<NetworkIdentity>().netId);
                    swimDamageUpdateTimer = 1;
                }
                else
                {
                    swimDamageCount = 0;
                }
            }
        }
        if (!controllBlock)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                GOManager.dissonanceComms.enabled = true;
                // animator.ResetTrigger("Dialogue");
                // animator.SetTrigger("Dialogue");
                // networkAnimator.ResetTrigger("Dialogue");
                //networkAnimator.SetTrigger("Dialogue");
                animator.SetBool("Dialogue", true);
                StartCoroutine(DialogueRnd());
                CmdVoiceOn();
            }
            if (Input.GetKeyUp(KeyCode.G))
            {
                GOManager.dissonanceComms.enabled = false;
                CmdVoiceOff();
                animator.SetBool("Dialogue", false);
                StopCoroutine(DialogueRnd());
            }
            if (!DS.open)
            {
                mouse0 = Input.GetKey(KeyCode.Mouse0);

                w = Input.GetKey(KeyCode.W);
                s = Input.GetKey(KeyCode.S);
                a = Input.GetKey(KeyCode.A);
                d = Input.GetKey(KeyCode.D);
                e = Input.GetKey(KeyCode.E);
                q = Input.GetKey(KeyCode.Q);
                space = Input.GetKeyDown(KeyCode.Space);
            }
            float weight = 1 + stats.weight * 10;
            float enduranceConsume = deltaTime * 10 * weight;
            float swimSpeed = 0.25f / weight;
            float sprintSpeed = 1f / weight;
            if (Input.GetKey(KeyCode.LeftShift) && ((sprint && endurance > enduranceConsume) || (!sprint && endurance > 10)))
            {
                if (!sprint)
                {
                    sprint = true;

                }
                if (swim)
                {
                    if (moveSpeed != swimSpeed)
                    {
                        moveSpeed = swimSpeed;
                        animator.SetFloat("MoveSpeed", 1 + moveSpeed);
                    }
                    endurance -= enduranceConsume * 0.25f;
                }
                else
                {
                    if (moveSpeed != sprintSpeed)
                    {
                        moveSpeed = sprintSpeed;
                        animator.SetFloat("MoveSpeed", 1 + moveSpeed);
                    }
                    endurance -= enduranceConsume;
                }

                if (!enduranceUIActive)
                {
                    enduranceUIActive = true;
                    GOManager.playerEnduranceUIGo.SetActive(true);
                }
                GOManager.playerEnduranceUIRectTransform.sizeDelta = new Vector2(((float)endurance / 100) * 186, 16);
            }
            else
            {
                if (swim && run)
                {
                    endurance -= deltaTime * 1;
                }
                if (sprint)
                {
                    sprint = false;
                    moveSpeed = 0;
                    animator.SetFloat("MoveSpeed", 1);

                }
                if (endurance < 100)
                {
                    if (!swim)
                        endurance += deltaTime * 3;
                    GOManager.playerEnduranceUIRectTransform.sizeDelta = new Vector2(((float)endurance / 100) * 186, 16);

                }
                else
                {
                    endurance = 100;
                    if (enduranceUIActive)
                    {
                        enduranceUIActive = false;
                        GOManager.playerEnduranceUIGo.SetActive(false);
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Z) && !run && !stats.wpnrdy)
            {
                if (sitGround)
                {
                    sitGround = false;
                    animator.SetBool("SitGround", false);
                }
                else
                {
                    sitGround = true;
                    animator.SetBool("SitGround", true);
                    ChatOmegaLul.instance.CmdSendMessageInGlobalChat("/действие Сел отдохнуть");
                }
            }
        }


        float atspd = 1.10f;

        float atspdMult = 0.55f;
        float rota = 0;
        float arot = (q ? 0 : 2f);
        float drot = (e ? 0 : -2f);
        lerpRota = 0;
        attackTimer -= deltaTime;
        if (attackTimer <= 0) attackTimer = 0;
        if (attackTimer <= 0.1f)
        {
            animator.SetBool("Attack", false);
            animator.SetBool("AttackMove", false);
        }

        if (block)
        {
            animator.SetBool("Block", false);
            block = false;
        }
        if (w && Input.GetKeyDown(KeyCode.Mouse0) && attackTimer == 0)
        {
            stats.SetBoolAsTrigger("AttackTrigger");
            animator.SetBool("AttackMove", true);
            animator.SetInteger("AttackRnd", 0);
            attackTimer = atspd * 1.3f;
        }



        if (!controllBlock && !DS.open)
        {


            float mx = Input.GetAxis("Mouse X");
            mouseX += mx * deltaTime * 10;
            if (mouseX > mx) mouseX = mx;
            if (mouseX < mx) mouseX = mx;
            // mouseX = Mathf.Lerp(mouseX, Input.GetAxis("Mouse X"), 0.1f);
            lerpRota = (mouseX + arot + drot) * deltaTime * 70;
        }
        if (!classicControll)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1) && attackTimer <= atspd * 0.8f)
            {

                if (!block)
                {
                    combo = 0;
                    animator.SetBool("Block", true);
                    // CmdAnimatorSetBool("Block", true);
                    animator.SetInteger("BlockRnd", Random.Range(0, 3));
                    block = true;
                    // CmdAnimatorSetInteger("BlockRnd", Random.Range(0, 3));
                }
            }
        }
        if (mouse0)
        {
            //animator.SetBool("Block", false);
            if (attackTimer <= 0)
            {
                combo = 0; failAttack = false;
            }

            if (classicControll)
            {
                if (Input.GetKeyDown(KeyCode.S) && attackTimer <= atspd * 0.8f)
                {

                    if (!block)
                    {
                        animator.SetBool("Block", true);
                        // CmdAnimatorSetBool("Block", true);
                        animator.SetInteger("BlockRnd", Random.Range(0, 3));
                        block = true;
                        // CmdAnimatorSetInteger("BlockRnd", Random.Range(0, 3));
                    }
                }






                if (Input.GetKeyDown(KeyCode.A))
                {

                    if (attackTimer <= atspd * atspdMult)
                    {

                    }
                    else failAttack = true;
                    if (!failAttack)
                    {
                        GetAtkspd();
                        // animator.ResetTrigger("AttackTrigger");
                        // animator.SetTrigger("AttackTrigger");
                        // networkAnimator.ResetTrigger("AttackTrigger");
                        // networkAnimator.SetTrigger("AttackTrigger");
                        stats.SetBoolAsTrigger("AttackTrigger");
                        animator.SetBool("Attack", true);
                        animator.SetInteger("AttackRnd", 2);
                        combo = 0;
                        // CmdAnimatorAttack("AttackTrigger", "Attack", true, "AttackRnd", 2);
                        attackTimer = atspd;
                        //if (stats.wpntype < 2)
                        // combatSystem.hit = true;

                    }
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    if (attackTimer <= atspd * atspdMult)
                    {
                    }
                    else failAttack = true;
                    if (!failAttack)
                    {
                        GetAtkspd();
                        // animator.ResetTrigger("AttackTrigger");
                        // animator.SetTrigger("AttackTrigger");
                        // networkAnimator.ResetTrigger("AttackTrigger");
                        // networkAnimator.SetTrigger("AttackTrigger");
                        stats.SetBoolAsTrigger("AttackTrigger");
                        animator.SetBool("Attack", true);
                        animator.SetInteger("AttackRnd", 1);
                        combo = 0;
                        // CmdAnimatorAttack("AttackTrigger", "Attack", true, "AttackRnd", 1);
                        attackTimer = atspd;
                        //if (stats.wpntype < 2)
                        //  combatSystem.hit = true;
                    }
                }
            }
            if ((classicControll && Input.GetKeyDown(KeyCode.W)) || (!classicControll && Input.GetKeyDown(KeyCode.Mouse0)) && !run)
            {

                if (attackTimer <= atspd * atspdMult)
                {

                }
                else
                {
                    failAttack = true;
                    combo = 0;
                }

                if (!failAttack)
                {
                    if (stats.wpntype == 2)
                    {
                        bool ar = false;
                        for (int i = 0; i < inventorySystem.items.Count; i++)
                        {
                            if (inventorySystem.items[i]._name == "Стрела")
                            {
                                ar = true;
                                break;
                            }
                        }
                        if (ar)
                        {
                            // animator.ResetTrigger("AttackTrigger");
                            // animator.SetTrigger("AttackTrigger");
                            // networkAnimator.ResetTrigger("AttackTrigger");
                            // networkAnimator.SetTrigger("AttackTrigger");
                            stats.SetBoolAsTrigger("AttackTrigger");
                            animator.SetBool("Attack", true);
                            animator.SetInteger("AttackRnd", 0);
                            // CmdAnimatorAttack("AttackTrigger", "Attack", true, "AttackRnd", 0);
                            attackTimer = atspd;
                            weaponController.wpn2.transform.GetChild(0).GetChild(1).GetComponent<Animator>().SetTrigger("Attack");
                        }
                    }
                    else
                    {
                        GetAtkspd();
                        // animator.ResetTrigger("AttackTrigger");
                        // animator.SetTrigger("AttackTrigger");
                        // networkAnimator.ResetTrigger("AttackTrigger");
                        // networkAnimator.SetTrigger("AttackTrigger");
                        stats.SetBoolAsTrigger("AttackTrigger");
                        animator.SetBool("Attack", true);
                        animator.SetInteger("AttackRnd", 0);
                        animator.SetInteger("AttackCombo", combo);
                        combo++;

                        if (combo > maxCombo) combo = 0;
                        // CmdAnimatorAttack("AttackTrigger", "Attack", true, "AttackRnd", 0);
                        attackTimer = atspd;
                        //  if (stats.wpntype < 2)
                        // combatSystem.hit = true;
                    }
                }
            }

            if (!w)
            {
                if (run)
                {
                    attackTimer = 0;
                    run = false;
                    animator.SetBool("Run", false);
                }
            }
            if (!s)
            {

                if (back)
                {
                    back = false;
                    animator.SetBool("Back", false);
                }
            }
            if (!controllBlock)
                rota = (Input.GetAxis("Mouse X")) * deltaTime * 70;
            float mathdeltatime = lerpRota * 0.2f;
            float mathdeltatime2 = deltaTime * 20;
            if (w)
            {


                if (lerpRota != 0)
                {
                    inertX += mathdeltatime;
                    if (inertX > 10) inertX = 10;
                    if (inertX < -10) inertX = -10;
                }
                else
                {
                    if (inertX > 0)
                    {
                        inertX -= mathdeltatime2;
                        if (inertX < 0) inertX = 0;

                    }

                    if (inertX < 0)
                    {
                        inertX += mathdeltatime2;
                        if (inertX > 0) inertX = 0;

                    }
                }

            }
            else
            {
                if (inertX > 0)
                {
                    inertX -= mathdeltatime2;
                    if (inertX < 0) inertX = 0;

                }

                if (inertX < 0)
                {
                    inertX += mathdeltatime2;
                    if (inertX > 0) inertX = 0;

                }

            }

        }
        else
        {


            if (space)
            {
                int JumpIndex;
                jumpTarget = new Vector3(0, 0, 0);
                JumpIndex = 0;
                RaycastHit rh;
                Physics.Raycast(transform.position + new Vector3(0, 3.5f, 0) + transform.TransformDirection(new Vector3(0, 0, 0.5f)), new Vector3(0, -4, 0), out rh, 11);
                if (rh.distance < 3.5f && rh.distance >= 2.2f)
                    JumpIndex = 1;
                if (rh.distance < 2.2f && rh.distance >= 0.9f)
                    JumpIndex = 2;
                if (rh.distance < 0.9f && rh.distance >= 0.2f)
                    JumpIndex = 3;

                JumpDis = rh.distance;
                animator.SetInteger("JumpIndex", JumpIndex);
                animator.SetBool("Jump", true);
                // CmdAnimatorSetBool("Jump", true);
            }
            else
            {
                animator.SetBool("Jump", false);
                // CmdAnimatorSetBool("Jump", false);
            }
            if (s && attackTimer <= atspd * atspdMult)
            {
                if (!back)
                {
                    combo = 0;
                    back = true;
                    animator.SetBool("Back", true);
                    EmotionsInterrupt();
                }

            }
            else
            {
                if (back)
                {
                    back = false;
                    animator.SetBool("Back", false);
                }
            }
            if (d)
            {
                if (!strafeR)
                {
                    animator.SetBool("StrafeRight", true);
                    strafeR = true;
                    EmotionsInterrupt();
                }
            }
            else
            {
                if (strafeR)
                {
                    animator.SetBool("StrafeRight", false);
                    strafeR = false;
                }
            }
            if (a)
            {
                if (!strafeL)
                {
                    animator.SetBool("StrafeLeft", true);
                    strafeL = true;
                    EmotionsInterrupt();
                }
            }
            else
            {
                if (strafeL)
                {
                    animator.SetBool("StrafeLeft", false);
                    strafeL = false;
                }
            }
            if (!controllBlock)
                rota = (Input.GetAxis("Mouse X") + arot + drot) * deltaTime * 70;
            float mathdeltatime = lerpRota * 0.2f;
            float mathdeltatime2 = deltaTime * 20;
            if (w)
            {

                if (!run)
                {
                    run = true;
                    animator.SetBool("Run", true);
                    EmotionsInterrupt();
                }

                if (lerpRota != 0)
                {
                    inertX += mathdeltatime;
                    if (inertX > 10) inertX = 10;
                    if (inertX < -10) inertX = -10;
                }
                else
                {
                    if (inertX > 0)
                    {
                        inertX -= mathdeltatime2;
                        if (inertX < 0) inertX = 0;

                    }

                    if (inertX < 0)
                    {
                        inertX += mathdeltatime2;
                        if (inertX > 0) inertX = 0;

                    }
                }

            }
            else
            {
                if (run)
                {
                    run = false;
                    animator.SetBool("Run", false);

                }
                // CmdAnimatorSetBool("Run", false);
                if (inertX > 0)
                {
                    inertX -= mathdeltatime2;
                    if (inertX < 0) inertX = 0;

                }

                if (inertX < 0)
                {
                    inertX += mathdeltatime2;
                    if (inertX > 0) inertX = 0;

                }

            }

        }



        // CmdAnimatorSetFloat("MoveSpeed", 1.1f + moveSpeed);
        equipTimer -= deltaTime;
        if (equipTimer <= 0) equipTimer = 0;
        if (alpha1 && equipTimer == 0 && !controllBlock && inventorySystem.EquipedWeapon)
        {
            if (stats.wpnrdy && stats.wpntype == 2)
            {
                eqp2();
                Invoke(nameof(eqp1), 1);
            }
            else
            if (stats.wpnrdy && stats.wpntype == 3)
            {
                eqp3();
                Invoke("eqp1", 1);
            }
            else
                eqp1();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && equipTimer == 0 && !controllBlock && inventorySystem.EquipedWeaponBow)
        {
            if (stats.wpnrdy && stats.wpntype < 2)
            {
                eqp1();
                if (inventorySystem.EquipedWeaponBow)
                    Invoke("eqp2", 1);
            }
            else
            if (stats.wpnrdy && stats.wpntype == 3)
            {
                eqp3();
                if (inventorySystem.EquipedWeaponBow)
                    Invoke("eqp2", 1);
            }
            else
                eqp2();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && equipTimer == 0 && !controllBlock && inventorySystem.EquipedWeaponRune)
        {
            if (stats.wpnrdy && stats.wpntype < 2)
            {
                eqp1();
                Invoke("eqp3", 1);
            }
            else
            if (stats.wpnrdy && stats.wpntype == 2)
            {
                eqp2();

                Invoke("eqp3", 1);
            }
            else
                eqp3();
        }















        var state = animator.GetCurrentAnimatorStateInfo(0);
        bool jump = (state.fullPathHash == anim1 | state.fullPathHash == anim2 | state.fullPathHash == anim3 | state.fullPathHash == anim4 | state.fullPathHash == anim5 | state.fullPathHash == anim6 | state.fullPathHash == anim7 | state.fullPathHash == anim8 | state.fullPathHash == anim9);
        if (!swim)
        {
            if (jumpTimer > 0)
            {
                Vector3 xz1 = new Vector3(transform.position.x, 0, transform.position.z);
                Vector3 xz2 = new Vector3(oldPos.x, 0, oldPos.z);
                Vector3 lol = (xz1 - xz2);
                inert = lol * 50;
                jumpTimer -= deltaTime;
                fallTime = 0;
            }
            if (jumpTimer <= 0) jumpTimer = 0;
            cc.Move((transform.TransformDirection(jumpvelocity) + (jumpTimer > 0 ? new Vector3(0, 0, 0) : inert)) * deltaTime);
            if (!jump && jumpTimer == 0)
            {
                cc.Move(new Vector3(0, (-9.8f * fallTime) * deltaTime, 0));
                if (landing)
                    cc.Move(new Vector3(0, -11 * deltaTime, 0));
            }
            landing = cc.isGrounded;

            animator.SetBool("Landing", landing);
            if (!landing)
            {
                if (!jump)
                    fallTime += deltaTime;
                else fallTime = 0;
                if (fallTime > 0.15f)
                {
                    animator.SetBool("Fall", true);
                    if (animator.applyRootMotion)
                        animator.applyRootMotion = false;
                }
                else
                {
                    if (!animator.applyRootMotion)
                        animator.applyRootMotion = true;
                }

            }
            else
            {
                inert = new Vector3(0, 0, 0);
                animator.SetBool("Fall", false);
                if (!animator.applyRootMotion)
                    animator.applyRootMotion = true;
                if (!jump)
                {
                    jumpvelocity = new Vector3(0, 0, 0);
                }
                if (fallTime > 1.2f)
                {
                    int dmg1 = (int)(fallTime * stats.maxHealth / 30);
                    int dmg2 = 0;
                    int dmg3 = 0;
                    int dmg4 = 0;
                    stats.CmdTakeDamage(dmg1, dmg2, dmg3, dmg4, true, gameObject.GetComponent<NetworkIdentity>().netId);
                }
                fallTime = 0;

            }
        }
        if (targetLadder)
        {

            bool strt = (state.fullPathHash == anim10 | state.fullPathHash == anim11);
            animator.SetBool("LadderUpEnd", false);
            // CmdAnimatorSetBool("LadderUpEnd", false);
            if (strt)
            {
                Vector3 veca = targetLadder.transform.position + targetLadder.transform.forward * 0.2f + new Vector3(0, 0, 0);
                transform.position = veca;
            }
            lookTarget = targetLadder.transform.position;
            animator.SetBool("Ladder", true);
            animator.SetInteger("LadderType", targetLadder.type);
            // CmdAnimatorSetBool("Ladder", true);
            // CmdAnimatorSetInteger("LadderType", targetLadder.type);
            if (Input.GetKey(KeyCode.W))
            {
                if (targetLadder.type == 0 && transform.position.y >= targetLadder.transform.position.y + targetLadder.height)
                {
                    targetLadder = null;
                    lookTarget = new Vector3(0, 0, 0);
                    animator.SetBool("Ladder", false);
                    animator.SetBool("LadderUpEnd", true);
                    // CmdAnimatorSetBool("Ladder", false);
                    // CmdAnimatorSetBool("LadderUpEnd", true);
                }
                else
            if (targetLadder.type == 1 && transform.position.y >= targetLadder.transform.position.y)
                {
                    targetLadder = null;
                    lookTarget = new Vector3(0, 0, 0);
                    animator.SetBool("Ladder", false);
                    animator.SetBool("LadderUpEnd", true);
                    // CmdAnimatorSetBool("Ladder", false);
                    // CmdAnimatorSetBool("LadderUpEnd", true);
                }
            }
            if (Input.GetKey(KeyCode.S))
            {
                if (targetLadder.type == 0 && transform.position.y <= targetLadder.transform.position.y + 0.1f)
                {
                    targetLadder = null;
                    lookTarget = new Vector3(0, 0, 0);
                    animator.SetBool("Ladder", false);
                    //CmdAnimatorSetBool("Ladder", false);
                }
                else
                if (targetLadder.type == 1 && transform.position.y <= targetLadder.transform.position.y - targetLadder.height + 0.1f)
                {
                    targetLadder = null;
                    lookTarget = new Vector3(0, 0, 0);
                    animator.SetBool("Ladder", false);
                    //  CmdAnimatorSetBool("Ladder", false);
                }

            }
        }


        if (lookTarget.magnitude > 0)
        {
            rotation = Quaternion.LookRotation(lookTarget - transform.position, Vector3.up).eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, rotation, 0);
        }
        else
        if (lookeTarget.magnitude > 0)
        {
            // rotation = Quaternion.Lerp(_transform.rotation, Quaternion.LookRotation(lookeTarget - _transform.position, Vector3.up), 0.3f).eulerAngles.y;
            // _transform.rotation = Quaternion.Euler(0, rotation, 0);
        }
        else
        if (lookDialogTarget.magnitude > 0)
        {
            rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDialogTarget - transform.position, Vector3.up), 0.3f).eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, rotation, 0);
        }
        else
        {

            if (rota != 0)
            {
                animator.SetBool("TurnL", false);
                animator.SetBool("TurnR", false);
                // CmdAnimatorSetBool("TurnL", false);
                // CmdAnimatorSetBool("TurnR", false);
                rotation += rota * CameraController.instance.sensitive;
                if (!w)
                {
                    if (rota > 1f)
                        //CmdAnimatorSetBool("TurnR", true);
                        animator.SetBool("TurnR", true);
                    if (rota < -1f)
                        // CmdAnimatorSetBool("TurnL", true);
                        animator.SetBool("TurnL", true);
                    animator.SetFloat("TurnSpeed", 0.1f + Mathf.Abs(rota * 0.25f));
                    //CmdAnimatorSetFloat("TurnSpeed", 0.5f + Mathf.Abs(rota * 0.2f));
                }


            }
            else
            {
                animator.SetBool("TurnL", false);
                animator.SetBool("TurnR", false);
                //CmdAnimatorSetBool("TurnL", false);
                //CmdAnimatorSetBool("TurnR", false);
            }
            transform.rotation = Quaternion.Euler(0, rotation, -inertX);
        }


        oldPos = transform.position;
        oldDeltaTime = deltaTime;


        if (timerUpdateInBase >= 1)
        {
            if (Vector3.Distance(transform.position, lastPos) > 1f)
            {
                CmdSetTransformInData(GOManager.AccountName);
                timerUpdateInBase = 0;
                lastPos = transform.position;
            }
        }
        else
            timerUpdateInBase += deltaTime;
    }


    public void Jump()
    {
        jumpvelocity = new Vector3(0, 0.5f, 0);
        jumpTimer = 0.58f;

    }
    public void JumpUpLow()
    {
        jumpvelocity = new Vector3(0, 2.5f - JumpDis, 0.1f);
        jumpTimer = 1f;
    }
    public void JumpUpMid()
    {
        jumpvelocity = new Vector3(0, 1.6f - JumpDis, 0.1f);
        jumpTimer = 1f;
    }

    public void eqp1()
    {
        stats.wpntype = stats.wpnEqpdType[0];
        stats.CmdUpdateWpnType(stats.wpnEqpdType[0]);
        stats.rangeAttack = inventorySystem.EquipedItemWeapon.attackRande;
        stats.Updatewpnstate();
        //weaponSwitcher = !weaponSwitcher;

        if (!stats.wpnrdy)
        {
            CameraController.instance.bowOffset = new Vector3(0, 0, 0);

            //CmdAnimatorResetTrigger("Draw");
            //  CmdAnimatorSetTrigger("Draw");
            stats.SetBoolAsTrigger("Draw");
        }
        if (stats.wpnrdy)
        {
            CameraController.instance.bowOffset = new Vector3(0, 0, 0);
            stats.SetBoolAsTrigger("UnDraw");
            // animator.ResetTrigger("UnDraw");
            // animator.SetTrigger("UnDraw");
            //networkAnimator.ResetTrigger("UnDraw");
            //networkAnimator.SetTrigger("UnDraw");
            // CmdAnimatorResetTrigger("UnDraw");
            // CmdAnimatorSetTrigger("UnDraw");
        }
        equipTimer = 0.8f;
    }
    public void eqp2()
    {
        stats.wpntype = 2;
        stats.CmdUpdateWpnType(2);
        stats.rangeAttack = inventorySystem.EquipedItemWeaponBow.attackRande;
        stats.Updatewpnstate();

        if (!stats.wpnrdy)
        {
            CameraController.instance.bowOffset = new Vector3(1, 0, 0);
            GOManager.aimImage.SetActive(true);
            stats.SetBoolAsTrigger("Draw");
            //animator.ResetTrigger("Draw");
            // animator.SetTrigger("Draw");
            // CmdAnimatorResetTrigger("Draw");
            // CmdAnimatorSetTrigger("Draw");
        }
        if (stats.wpnrdy)
        {
            CameraController.instance.bowOffset = new Vector3(0, 0, 0);
            GOManager.aimImage.SetActive(false);
            // animator.ResetTrigger("UnDraw");
            //animator.SetTrigger("UnDraw");
            stats.SetBoolAsTrigger("UnDraw");
            // CmdAnimatorResetTrigger("UnDraw");
            // CmdAnimatorSetTrigger("UnDraw");
        }
        equipTimer = 0.8f;
    }
    public void eqp3()
    {

        stats.wpntype = 3;

        stats.CmdUpdateWpnType(3);
        stats.rangeAttack = inventorySystem.EquipedItemWeaponRune.attackRande;
        stats.Updatewpnstate();

        if (!stats.wpnrdy)
        {
            CameraController.instance.bowOffset = new Vector3(1, 0, 0);
            CameraController.instance.zScroll = 1;
            // animator.ResetTrigger("Draw");
            //animator.SetTrigger("Draw");
            stats.SetBoolAsTrigger("Draw");
            //CmdAnimatorResetTrigger("Draw");
            // CmdAnimatorSetTrigger("Draw");
        }
        if (stats.wpnrdy)
        {
            CameraController.instance.bowOffset = new Vector3(0, 0, 0);
            // animator.ResetTrigger("UnDraw");
            // animator.SetTrigger("UnDraw");
            stats.SetBoolAsTrigger("UnDraw");

            // CmdAnimatorResetTrigger("UnDraw");
            // CmdAnimatorSetTrigger("UnDraw");
        }
        equipTimer = 0.8f;
    }


    [Command(requiresAuthority = false)]
    public void CmdSetTransformInData(string accname)
    {
        string filepath = "DataBase/Accounts/" + accname + "/Characters/1/Transform.xml";
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
        element = xmlDoc.CreateElement("Transform");
        element.SetAttribute("pos_x", transform.position.x.ToString());
        element.SetAttribute("pos_y", transform.position.y.ToString());
        element.SetAttribute("pos_z", transform.position.z.ToString());
        element.SetAttribute("rot_y", transform.eulerAngles.y.ToString());
        rootNode.AppendChild(element);
        xmlDoc.Save(filepath);
    }
    [Command(requiresAuthority = false)]
    public void CmdGetTransformInData(string accname, NetworkConnectionToClient conn = null)
    {
        string path = "DataBase/Accounts/" + accname + "/Characters/1/Transform.xml";
        if (File.Exists(path))
        {
            XmlTextReader reader = new XmlTextReader(path);
            while (reader.Read())
            {
                if (reader.IsStartElement("Transform"))
                {
                    float posx;
                    float posy;
                    float posz;
                    float roty;
                    float.TryParse(reader.GetAttribute("pos_x"), out posx);
                    float.TryParse(reader.GetAttribute("pos_y"), out posy);
                    float.TryParse(reader.GetAttribute("pos_z"), out posz);
                    float.TryParse(reader.GetAttribute("rot_y"), out roty);
                    TargetRpcGetTransformInData(conn, new Vector3(posx, posy, posz), new Vector3(0, roty, 0));
                    return;
                }
            }


            reader.Close();
        }
        TargetRpcGetTransformInData(conn, new Vector3(-624, -44, 33), new Vector3(0, -90, 0));
    }
    [TargetRpc]
    public void TargetRpcGetTransformInData(NetworkConnection conn, Vector3 pos, Vector3 rot)
    {
        StartCoroutine(EnterWorld(pos, rot));
    }
    IEnumerator EnterWorld(Vector3 pos, Vector3 rot)
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (inventorySystem.itemsLoaded)
            {
                stats.Teleportation(pos, rot.y);
                yield break;
            }
        }
    }
    [Command(requiresAuthority = false)]
    public void CmdVoiceOn()
    {
        RpcVoiceOn();
    }
    [Command(requiresAuthority = false)]
    public void CmdVoiceOff()
    {
        RpcVoiceOff();
    }
    [Command(requiresAuthority = false)]
    public void CmdVoiceOffDelay()
    {
        RpcVoiceOffDelay();
    }
    [ClientRpc]
    public void RpcVoiceOn()
    {

        headEmotions.voice = true;
    }
    [ClientRpc]
    public void RpcVoiceOff()
    {
        headEmotions.voice = false;
    }
    [ClientRpc]
    public void RpcVoiceOffDelay()
    {
        Invoke(nameof(VoiceOnDelay), 4.8f);

    }
    void VoiceOnDelay()
    {
        headEmotions.voice = false;
    }

    void EmotionsInterrupt()
    {
        if (sitGround)
        {
            sitGround = false;
            animator.SetBool("SitGround", false);
        }
        if (dance)
        {
            dance = false;
            animator.SetBool("Dance", false);
        }
    }
}








