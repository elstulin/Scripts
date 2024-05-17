using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InFlameController : MonoBehaviour
{
    public ParticleSystem[] particleSystem;
    private StatsSystem statsSystem;
    public List<float> flameTime;
    public List<int> flameDamage;
    public float tickDamage;
    bool trig1;
    public AudioSource audioSource;
    private Animator animator;
    public GameObject flamePrefab;
    public bool generate = true;
    void Start()
    {
        flamePrefab = Resources.Load("Flame") as GameObject;
        animator = GetComponent<Animator>();
        statsSystem = GetComponent<StatsSystem>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = Resources.Load("Sounds/SFX/MFX_FIRESPELL_HUMANBURN") as AudioClip;
        if (generate)
        {
            particleSystem = new ParticleSystem[17];
            particleSystem[0] = Instantiate(flamePrefab, animator.GetBoneTransform(HumanBodyBones.Chest)).GetComponent<ParticleSystem>();
            particleSystem[1] = Instantiate(flamePrefab, animator.GetBoneTransform(HumanBodyBones.Hips)).GetComponent<ParticleSystem>();
            particleSystem[2] = Instantiate(flamePrefab, animator.GetBoneTransform(HumanBodyBones.LeftShoulder)).GetComponent<ParticleSystem>();
            particleSystem[3] = Instantiate(flamePrefab, animator.GetBoneTransform(HumanBodyBones.RightShoulder)).GetComponent<ParticleSystem>();
            particleSystem[4] = Instantiate(flamePrefab, animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg)).GetComponent<ParticleSystem>();
            particleSystem[5] = Instantiate(flamePrefab, animator.GetBoneTransform(HumanBodyBones.RightUpperArm)).GetComponent<ParticleSystem>();
            particleSystem[6] = Instantiate(flamePrefab, animator.GetBoneTransform(HumanBodyBones.RightLowerArm)).GetComponent<ParticleSystem>();
            particleSystem[7] = Instantiate(flamePrefab, animator.GetBoneTransform(HumanBodyBones.LeftUpperArm)).GetComponent<ParticleSystem>();
            particleSystem[8] = Instantiate(flamePrefab, animator.GetBoneTransform(HumanBodyBones.LeftLowerArm)).GetComponent<ParticleSystem>();
            particleSystem[9] = Instantiate(flamePrefab, animator.GetBoneTransform(HumanBodyBones.RightUpperLeg)).GetComponent<ParticleSystem>();
            particleSystem[10] = Instantiate(flamePrefab, animator.GetBoneTransform(HumanBodyBones.RightLowerLeg)).GetComponent<ParticleSystem>();
            particleSystem[11] = Instantiate(flamePrefab, animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg)).GetComponent<ParticleSystem>();
            particleSystem[12] = Instantiate(flamePrefab, animator.GetBoneTransform(HumanBodyBones.Head)).GetComponent<ParticleSystem>();
            particleSystem[13] = Instantiate(flamePrefab, animator.GetBoneTransform(HumanBodyBones.LeftFoot)).GetComponent<ParticleSystem>();
            particleSystem[14] = Instantiate(flamePrefab, animator.GetBoneTransform(HumanBodyBones.RightFoot)).GetComponent<ParticleSystem>();
            particleSystem[15] = Instantiate(flamePrefab, animator.GetBoneTransform(HumanBodyBones.LeftHand)).GetComponent<ParticleSystem>();
            particleSystem[16] = Instantiate(flamePrefab, animator.GetBoneTransform(HumanBodyBones.RightHand)).GetComponent<ParticleSystem>();
        }
    }

    
    void Update()
    {

        if (particleSystem[0].particleCount > 0)
        {
            if (!trig1)
            {
                for (int i = 0; i < particleSystem.Length; i++)
                {
                    particleSystem[i].gameObject.SetActive(true);

                }
                audioSource.PlayOneShot(audioSource.clip);
                trig1 = true;

            }

            
        }
        else
        {
            if (trig1)
            {
                for (int i = 0; i < particleSystem.Length; i++)
                {
                    particleSystem[i].gameObject.SetActive(false);

                }
                trig1 = false;
            }
        }
            if (flameTime.Count > 0)
        {
            
            for (int i=0; i< particleSystem.Length; i++)
            {
                particleSystem[i].Emit(flameTime.Count);
                
            }
            for (int i = 0; i < flameTime.Count; i++)
            {
                flameTime[i] -= Time.deltaTime;
                    if (flameTime[i] <= 0)
                {
                    
                    flameTime.RemoveAt(i);
                    flameDamage.RemoveAt(i);
                }
            }
            
        }
        if (flameTime.Count > 0)
        {
            tickDamage += Time.deltaTime;
            if (tickDamage >= 1)
            {
                for (int i =0;i < flameTime.Count;i++) {
                    int dmg1 = 0;
                    int dmg2 = 0;
                    int dmg3 = flameDamage[i];
                    int dmg4 = 0;
                    if(statsSystem.minHealth > 0)
                    statsSystem.CmdTakeDamage( dmg1,  dmg2, dmg3,  dmg4,true, gameObject.GetComponent<Mirror.NetworkIdentity>().netId);
                }
                tickDamage = 0;
            }
        }
    }
}
