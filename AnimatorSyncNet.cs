using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AnimatorSyncNet : NetworkBehaviour
{
    public Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        //GetComponent<AI>().animatorSyncNet = this;
    }
    [Command(requiresAuthority = false)]
    public void CmdAnimatorSetBool(string name, bool boolean)
    {
        RpcAnimatorSetBool(name, boolean);
    }
    [ClientRpc]
    public void RpcAnimatorSetBool(string name, bool boolean)
    {
            animator.SetBool(name, boolean);
    }
    [Command(requiresAuthority = false)]
    public void CmdAnimatorSetTrigger(string name)
    {
        RpcAnimatorSetTrigger(name);
    }
    [ClientRpc]
    public void RpcAnimatorSetTrigger(string name)
    {
            animator.ResetTrigger(name);
            animator.SetTrigger(name);
    }
    [Command(requiresAuthority = false)]
    public void CmdAnimatorResetTrigger(string name)
    {
        RpcAnimatorResetTrigger(name);
    }
    [ClientRpc]
    public void RpcAnimatorResetTrigger(string name)
    {
            animator.ResetTrigger(name);
    }
    [Command(requiresAuthority = false)]
    public void CmdAnimatorSetInteger(string name, int value)
    {
        RpcAnimatorSetInteger(name, value);
    }
    [ClientRpc]
    public void RpcAnimatorSetInteger(string name, int value)
    {
            animator.SetInteger(name, value);
    }
    [Command(requiresAuthority = false)]
    public void CmdAnimatorSetFloat(string name, float value)
    {
        RpcAnimatorSetFloat(name, value);
    }
    [ClientRpc]
    public void RpcAnimatorSetFloat(string name, float value)
    {
            animator.SetFloat(name, value);
    }

    [Command(requiresAuthority = false)]
    public void CmdAnimatorAttack(string nameTrig, string nameBool, bool boolean, string nameInt, int value)
    {
        RpcAnimatorAttack(nameTrig, nameBool, boolean, nameInt, value);
    }
    [ClientRpc]
    public void RpcAnimatorAttack(string nameTrig, string nameBool, bool boolean, string nameInt, int value)
    {
            animator.ResetTrigger(nameTrig);
            animator.SetTrigger(nameTrig);
            animator.SetBool(nameBool, boolean);
            animator.SetInteger(nameInt, value);
    }
}
