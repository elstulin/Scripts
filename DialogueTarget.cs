using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DialogueTarget : NetworkBehaviour
{
    public int ID;
    private DialogueSystem dialogueSystem;
    public bool AgroDialog;
    public AI ai;
    float dis2;
    Animator playerAnimator;
    Transform playertransform;
    Transform _transform;
   public bool guard;
    bool trig1;
    public bool trig2;
    private void Start()
    {
        dis2 = 3.5f;
        ai = GetComponent<AI>();
        
        
        _transform = transform;
    }
    private void Update()
    {
        if (!NetworkClient.ready) return;

        dialogueSystem = GOManager.playerDS;
        playerAnimator = GOManager.playerAnim;
        playertransform = GOManager.player.transform;
        float dis = Vector3.Distance(playertransform.position, transform.position);
        if (!AgroDialog)
        {
            if (guard&&dis < 3&&!trig1)
            {
                trig1 = true;
                AgroDialog = true;
            }
        }
       else
        if (dis < 3.5f)
        {
            if (!trig2)
            {
                ai.dialogueTarget = playertransform;
                if (!ai.target && dis < 3.5f && Vector3.Dot((transform.position - playertransform.position).normalized, playertransform.forward) > 0)
                {
                    var anim = Animator.StringToHash("Bottom.RunStart");
                    var anim2 = Animator.StringToHash("Bottom.RunLoop");
                    var state = playerAnimator.GetCurrentAnimatorStateInfo(0);
                    bool runstart = (state.fullPathHash == anim || state.fullPathHash == anim2);
                    //if (runstart)
                   // {
                        trig2 = true;
                        trig1 = false;
                        AgroDialog = false;
                        dialogueSystem.GenerageDialogue(this, false);


                   // }
                }
            }
        }
        else
        {
            if (guard)
                QuestSystem.guard10 = false;
            trig1 = false;
            ai.dialogueTarget = null;
        }
    }
}
