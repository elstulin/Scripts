using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;


public class WeaponController : NetworkBehaviour
{
    public GameObject wpn1;
    public Transform wpn1Pos;
    public GameObject wpn2;
    public GameObject wpn3;
    public MeleeWeaponTrail mwt;
    public Transform hip;
    public Transform hip2;
    public Transform hip3;
    public Transform hand;
    public Transform hand2;
    public Transform hand3;
    public GameObject hitpref;
    public LayerMask lm;
    public StatsSystem statsSystem;
    public Animator animator;
    bool fixhand;
    private CombatSystem combatSystem;
    public Vector3 fix;
    public Vector3 weaponOldPos;
    void Start()
    {
        statsSystem = GetComponent<StatsSystem>();
        animator = GetComponent<Animator>();
        combatSystem = GetComponent<CombatSystem>();
        mwt = wpn1.GetComponent<MeleeWeaponTrail>();
        wpn1Pos = wpn1.transform;
    }

    public void Equip()
    {
        if (statsSystem.netIdentity.isLocalPlayer)
        {
            CmdEquip();
        }
    }
    public void EquipL()
    {
        if (statsSystem.netIdentity.isLocalPlayer)
        {
            CmdEquipL();
        }
    }
    public void UnEquip()
    {
        if (statsSystem.netIdentity.isLocalPlayer)
        {
            CmdUnEquip();
        }
    }



    [Command(requiresAuthority = false)]
    public void CmdEquipL()
    {
        RpcEquipL();
    }
    [ClientRpc]
    public void RpcEquipL()
    {

        statsSystem.wpnrdy = true;
        animator.SetBool("Ready", true);
        statsSystem.CmdWpnReadySync(true);
      //  statsSystem.Updatewpnstate();
        fixhand = false;
        if (statsSystem.wpntype == 2)
        {
            wpn2.transform.SetParent(hand3);
            wpn2.transform.localPosition = new Vector3(0, 0, 0);
            wpn2.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            wpn1.transform.SetParent(hand3);
            wpn1.transform.localPosition = new Vector3(0, 0, 0);
            wpn1.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }
    [Command(requiresAuthority = false)]
    public void CmdEquip()
    {
        RpcEquip();
    }
    [ClientRpc]
    public void RpcEquip()
    {
        fixhand = false;
        statsSystem.wpnrdy = true;
        animator.SetBool("Ready", true);
        statsSystem.CmdWpnReadySync(true);
       // statsSystem.Updatewpnstate();
        if (statsSystem.wpntype == 0)
        {
            wpn1.transform.SetParent(hand);
            wpn1.transform.localPosition = new Vector3(0, 0, 0);
            wpn1.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        if (statsSystem.wpntype == 1)
        {

            wpn1.transform.SetParent(hand2);

            if (statsSystem.skill2h >= 30)
                fixhand = true;
            wpn1.transform.localPosition = new Vector3(0, 0, 0);
            wpn1.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        if (statsSystem.wpntype == 3)
        {
            wpn3.transform.SetParent(hand);
            wpn3.transform.localPosition = new Vector3(0, 0, 0);
            wpn3.transform.localEulerAngles = new Vector3(0, 0, 0);
        }

    }
    void OnAnimatorIKa()
    {
        if (fixhand)
        {
            var anim2 = Animator.StringToHash("Up.1h3");
            var anim = Animator.StringToHash("Bottom.1h3idle");
            var state = animator.GetCurrentAnimatorStateInfo(0);
            var state2 = animator.GetCurrentAnimatorStateInfo(7);
            bool idle2h3 = (state.fullPathHash == anim) && (state2.fullPathHash == anim2);
            if (idle2h3 && wpn1.transform.childCount > 2 && statsSystem.skill2h >= 60)
            {
                Transform tr = wpn1.transform.GetChild(2);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, tr.position + tr.right * 0.08f + tr.up * -0.05f + tr.forward * -0.07f);
            }
            else
            if (idle2h3 && wpn1.transform.childCount > 2 && statsSystem.skill2h >= 30)
            {
                Transform tr = wpn1.transform.GetChild(2);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, tr.position + tr.forward * 0.45f + tr.up * -0.06f);
            }
        }
    }
    [Command(requiresAuthority = false)]
    public void CmdUnEquip()
    {
        RpcUnEquip();
    }
    [ClientRpc]
    public void RpcUnEquip()
    {

        statsSystem.wpnrdy = false;
        animator.SetBool("Ready", false);
        statsSystem.CmdWpnReadySync(false);
       // statsSystem.Updatewpnstate();
        if (statsSystem.wpntype == 2)
        {
            wpn2.transform.SetParent(hip3);
            wpn2.transform.localPosition = new Vector3(0, 0, 0);
            wpn2.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            if (statsSystem.wpntype == 0)
                wpn1.transform.SetParent(hip);
            if (statsSystem.wpntype == 1)
                wpn1.transform.SetParent(hip2);
            wpn1.transform.localPosition = new Vector3(0, 0, 0);
            wpn1.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        if (statsSystem.wpntype == 3)
        {
            wpn3.transform.localPosition = new Vector3(0, -1000, 0);
        }
    }
    public void TrailEmitOn()
    {
        mwt.Emit = true;

    }
    public void TrailEmitOff()
    {
        mwt.Emit = false;
    }






}










