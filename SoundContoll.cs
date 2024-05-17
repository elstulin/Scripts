using System.Collections;
using System.Collections.Generic;
using Mirror;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class SoundContoll : NetworkBehaviour
{
    public int soundSetup;
    public Transform playerTransform;
    public Transform RFoot;
    public Transform LFoot;
    public int posX;
    public int posZ;
    public float[] textureValues;
    public AudioClip enemyKilledAudio;
    public AudioClip monsterKilledAudio;
    public AudioClip dieMonsterAudio;
    public AudioClip weaponDownAudio;
    public AudioClip weaponDownAudio2;
    public AudioClip willStopAudio;
    public AudioClip wiseMoveAudioClip;
    public AudioClip fallsmallAudioClip;
    public AudioClip fallAudioClip;
    public AudioClip fallwaterAudioClip;
    public AudioSource audioSource;
    public AudioClip[] smallTalksAudioClips;
    public AudioClip[] stepAudioClips;
    public AudioClip[] swimAudioClips;
    public AudioClip[] AmbAudioClips;
    public AudioClip[] stepStoneAudioClips;
    public AudioClip[] stepWaterAudioClips;
    public AudioClip[] stepMetalAudioClips;
    public AudioClip[] stepWoodAudioClips;
    public AudioClip[] woshAudioClips;
    public AudioClip[] bowfireAudioClips;
    public AudioClip[] drawAudioClips;
    public AudioClip[] aarghhSounds;
    public AudioClip[] deadSounds;
    Transform wpn;
    public int[] drawSoundType = new int[3];
    StatsSystem stats;
    public Material[] matsEarth;
    public Material[] matsStone;
    public Material[] matsMEtal;
    public Material[] matsWood;
    public string[] matsEarth1;
    public string[] matsStone1;
    public string[] matsMEtal1;
    public string[] matsWood1;
    // public RippleController rippleController;
    Animator animator;
    int stepCounter;
    int check;
    int savedi;
    int savedMaterial;
    Renderer renderer;
    MeshCollider mc;
    void RemoveDublicates()
    {
        int duplicates = 0;
        for (int i = 0; i < matsEarth.Length; i++)
        {
            for (int q = 0; q < matsEarth.Length; q++)
            {

                if (q != i && matsEarth[i] && matsEarth[q] && matsEarth[i].name == matsEarth[q].name)
                {
                    matsEarth[i] = null;
                    duplicates++;
                }
            }
        }
        for (int i = 0; i < matsEarth.Length; i++)
        {
            for (int q = 0; q < matsStone.Length; q++)
            {

                if (q != i && matsEarth[i] && matsStone[q] && matsEarth[i].name == matsStone[q].name)
                {
                    matsEarth[i] = null;
                    duplicates++;
                }
            }
        }
        for (int i = 0; i < matsEarth.Length; i++)
        {
            for (int q = 0; q < matsWood.Length; q++)
            {

                if (q != i && matsEarth[i] && matsWood[q] && matsEarth[i].name == matsWood[q].name)
                {
                    matsEarth[i] = null;
                    duplicates++;
                }
            }
        }
        for (int i = 0; i < matsEarth.Length; i++)
        {
            for (int q = 0; q < matsMEtal.Length; q++)
            {

                if (q != i && matsEarth[i] && matsMEtal[q] && matsEarth[i].name == matsMEtal[q].name)
                {
                    matsEarth[i] = null;
                    duplicates++;
                }
            }
        }
        for (int i = 0; i < matsStone.Length; i++)
        {
            for (int q = 0; q < matsStone.Length; q++)
            {
                if (q != i && matsStone[i] && matsStone[q] && matsStone[i].name == matsStone[q].name)
                {
                    matsStone[i] = null;
                    duplicates++;
                }
            }
        }
        for (int i = 0; i < matsStone.Length; i++)
        {
            for (int q = 0; q < matsEarth.Length; q++)
            {
                if (q != i && matsStone[i] && matsEarth[q] && matsStone[i].name == matsEarth[q].name)
                {
                    matsStone[i] = null;
                    duplicates++;
                }
            }
        }
        for (int i = 0; i < matsStone.Length; i++)
        {
            for (int q = 0; q < matsWood.Length; q++)
            {
                if (q != i && matsStone[i] && matsWood[q] && matsStone[i].name == matsWood[q].name)
                {
                    matsStone[i] = null;
                    duplicates++;
                }
            }
        }
        for (int i = 0; i < matsStone.Length; i++)
        {
            for (int q = 0; q < matsMEtal.Length; q++)
            {
                if (q != i && matsStone[i] && matsMEtal[q] && matsStone[i].name == matsMEtal[q].name)
                {
                    matsStone[i] = null;
                    duplicates++;
                }
            }
        }
        for (int i = 0; i < matsWood.Length; i++)
        {
            for (int q = 0; q < matsWood.Length; q++)
            {
                if (q != i && matsWood[i] && matsWood[q] && matsWood[i].name == matsWood[q].name)
                {
                    matsWood[i] = null;
                    duplicates++;
                }
            }
        }
        for (int i = 0; i < matsWood.Length; i++)
        {
            for (int q = 0; q < matsWood.Length; q++)
            {
                if (q != i && matsWood[i] && matsWood[q] && matsWood[i].name == matsWood[q].name)
                {
                    matsWood[i] = null;
                    duplicates++;
                }
            }
        }

        for (int i = 0; i < matsWood.Length; i++)
        {
            for (int q = 0; q < matsEarth.Length; q++)
            {
                if (q != i && matsWood[i] && matsEarth[q] && matsWood[i].name == matsEarth[q].name)
                {
                    matsWood[i] = null;
                    duplicates++;
                }
            }
        }

        for (int i = 0; i < matsWood.Length; i++)
        {
            for (int q = 0; q < matsStone.Length; q++)
            {
                if (q != i && matsWood[i] && matsStone[q] && matsWood[i].name == matsStone[q].name)
                {
                    matsWood[i] = null;
                    duplicates++;
                }
            }
        }

        for (int i = 0; i < matsWood.Length; i++)
        {
            for (int q = 0; q < matsMEtal.Length; q++)
            {
                if (q != i && matsWood[i] && matsMEtal[q] && matsWood[i].name == matsMEtal[q].name)
                {
                    matsWood[i] = null;
                    duplicates++;
                }
            }
        }
        for (int i = 0; i < matsMEtal.Length; i++)
        {
            for (int q = 0; q < matsStone.Length; q++)
            {
                if (q != i && matsMEtal[i] && matsStone[q] && matsMEtal[i].name == matsStone[q].name)
                {

                    matsMEtal[i] = null;
                    duplicates++;
                }
            }
        }

        for (int i = 0; i < matsMEtal.Length; i++)
        {
            for (int q = 0; q < matsMEtal.Length; q++)
            {
                if (q != i && matsMEtal[i] && matsMEtal[q] && matsMEtal[i].name == matsMEtal[q].name)
                {

                    matsMEtal[i] = null;
                    duplicates++;
                }
            }
        }

        for (int i = 0; i < matsMEtal.Length; i++)
        {
            for (int q = 0; q < matsWood.Length; q++)
            {
                if (q != i && matsMEtal[i] && matsWood[q] && matsMEtal[i].name == matsWood[q].name)
                {

                    matsMEtal[i] = null;
                    duplicates++;
                }
            }
        }

        for (int i = 0; i < matsMEtal.Length; i++)
        {
            for (int q = 0; q < matsEarth.Length; q++)
            {
                if (q != i && matsMEtal[i] && matsEarth[q] && matsMEtal[i].name == matsEarth[q].name)
                {

                    matsMEtal[i] = null;
                    duplicates++;
                }
            }
        }
        Debug.Log(duplicates + " duplicates removed");
    }
    private void Awake()
    {

        if (soundSetup == -1)
        {
            /*RemoveDublicates();
            matsEarth1 = new string[matsEarth.Length];
            for (int i = 0; i < matsEarth.Length; i++)
            {
                if (matsEarth[i])
                    matsEarth1[i] = matsEarth[i].name;
            }
            matsStone1 = new string[matsStone.Length];
            for (int i = 0; i < matsStone.Length; i++)
            {
                if (matsStone[i])
                    matsStone1[i] = matsStone[i].name;
            }
            matsWood1 = new string[matsWood.Length];
            for (int i = 0; i < matsWood.Length; i++)
            {
                if (matsWood[i])
                    matsWood1[i] = matsWood[i].name;
            }
            matsMEtal1 = new string[matsMEtal.Length];
            for (int i = 0; i < matsMEtal.Length; i++)
            {
                if (matsMEtal[i])
                    matsMEtal1[i] = matsMEtal[i].name;
            }*/
            if (!GOManager.HumanSoundSetup1)
                GOManager.HumanSoundSetup1 = this;
            enabled = false;
        }
    }
    void Start()
    {

        stats = GetComponent<StatsSystem>();
        animator = GetComponent<Animator>();
        playerTransform = transform;
        audioSource = GetComponent<AudioSource>();

        LFoot = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
        RFoot = animator.GetBoneTransform(HumanBodyBones.RightFoot);
        if (soundSetup == 1)
        {
            enemyKilledAudio = GOManager.HumanSoundSetup1.enemyKilledAudio;
            monsterKilledAudio = GOManager.HumanSoundSetup1.monsterKilledAudio;
            dieMonsterAudio = GOManager.HumanSoundSetup1.dieMonsterAudio;
            weaponDownAudio = GOManager.HumanSoundSetup1.weaponDownAudio;
            weaponDownAudio2 = GOManager.HumanSoundSetup1.weaponDownAudio2;
            willStopAudio = GOManager.HumanSoundSetup1.willStopAudio;
            wiseMoveAudioClip = GOManager.HumanSoundSetup1.wiseMoveAudioClip;
            deadSounds = GOManager.HumanSoundSetup1.deadSounds;
            fallsmallAudioClip = GOManager.HumanSoundSetup1.fallsmallAudioClip;
            fallAudioClip = GOManager.HumanSoundSetup1.fallAudioClip;
            fallwaterAudioClip = GOManager.HumanSoundSetup1.fallwaterAudioClip;
            smallTalksAudioClips = GOManager.HumanSoundSetup1.smallTalksAudioClips;
            stepAudioClips = GOManager.HumanSoundSetup1.stepAudioClips;
            stepStoneAudioClips = GOManager.HumanSoundSetup1.stepStoneAudioClips;
            stepWaterAudioClips = GOManager.HumanSoundSetup1.stepWaterAudioClips;
            stepMetalAudioClips = GOManager.HumanSoundSetup1.stepMetalAudioClips;
            stepWoodAudioClips = GOManager.HumanSoundSetup1.stepWoodAudioClips;
            woshAudioClips = GOManager.HumanSoundSetup1.woshAudioClips;
            bowfireAudioClips = GOManager.HumanSoundSetup1.bowfireAudioClips;
            drawAudioClips = GOManager.HumanSoundSetup1.drawAudioClips;
            aarghhSounds = GOManager.HumanSoundSetup1.aarghhSounds;
            deadSounds = GOManager.HumanSoundSetup1.deadSounds;

            matsEarth1 = GOManager.HumanSoundSetup1.matsEarth1;
            matsStone1 = GOManager.HumanSoundSetup1.matsStone1;
            matsWood1 = GOManager.HumanSoundSetup1.matsWood1;
            matsMEtal1 = GOManager.HumanSoundSetup1.matsMEtal1;
            matsEarth = GOManager.HumanSoundSetup1.matsEarth;
            matsStone = GOManager.HumanSoundSetup1.matsStone;
            matsWood = GOManager.HumanSoundSetup1.matsWood;
            matsMEtal = GOManager.HumanSoundSetup1.matsMEtal;

        }

    }
    [ClientCallback]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void StepL()
    {

        RaycastHit hit;
        if (Physics.Linecast(transform.position + new Vector3(0, 1, 0), transform.position - new Vector3(0, 1.1f, 0), out hit, LayerMask.GetMask("Default")))
        {
            if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Water"))
                Instantiate(GOManager.stepParticle, LFoot.position, new Quaternion(0, 0, 0, 0));
        }
    }
    [ClientCallback]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void StepR()
    {

        RaycastHit hit;
        if (Physics.Linecast(transform.position + new Vector3(0, 1, 0), transform.position - new Vector3(0, 1.1f, 0), out hit, LayerMask.GetMask("Default")))
        {
            if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Water"))
                Instantiate(GOManager.stepParticle, RFoot.position, new Quaternion(0, 0, 0, 0));
        }

    }
    public void Amb()
    {
        if (Random.Range(0, 100) > 80)
            audioSource.PlayOneShot(AmbAudioClips[Random.Range(0, AmbAudioClips.Length)], 1);
    }
    [ClientCallback]
    public void Swim()
    {
        audioSource.PlayOneShot(swimAudioClips[Random.Range(0, swimAudioClips.Length)], 1);
    }
    [ClientCallback]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Step()
    {




        RaycastHit hit;
        if (Physics.Linecast(transform.position + new Vector3(0, 1, 0), transform.position - new Vector3(0, 1.1f, 0), out hit, LayerMask.GetMask("Default", "Water")))
        {

            GameObject hitGo = hit.collider.gameObject;
            if (hitGo.layer == LayerMask.NameToLayer("Water"))
            {
                GOManager.playerSS.moveControll.waterSurface = hitGo.GetComponent<WaterSurface>();

                audioSource.PlayOneShot(stepWaterAudioClips[Random.Range(0, stepWaterAudioClips.Length)], 1);
            }
            else
            {
                StartCoroutine(CheckWorldMaterial2(hit));
            }
        }
    }
    IEnumerator CheckWorldMaterial2(RaycastHit hit)
    {
        int materialIdx = -1;
        MeshCollider mcc = hit.collider as MeshCollider;
        if (!mcc) yield break;
        if (mcc != mc)
        {
            mc = mcc;
            renderer = mc.GetComponent<Renderer>();
        }


        if (!mc) yield break;
        Mesh mesh = mc.sharedMesh;
        if (!mesh) yield break;
        int triangleIdx = hit.triangleIndex;
        // int triangleIdx3 = triangleIdx * 3;
        int lookupIdx1 = mesh.triangles[triangleIdx * 3];
        //int lookupIdx2 = mesh.triangles[triangleIdx3 + 1];
        // int lookupIdx3 = mesh.triangles[triangleIdx3 + 2];
        int subMeshesNr = mesh.subMeshCount;
        if (subMeshesNr == 1)
            materialIdx = 0;
        else
        {
            if (savedi >= subMeshesNr)
                savedi = 0;
            yield return new WaitForEndOfFrame();

            for (int i = savedi; i < subMeshesNr; i++)
            {
                int[] tr = mesh.GetTriangles(i);
                int trLength = tr.Length;
                for (int j = 0; j < trLength; j += 3)
                {
                    if (tr[j] == lookupIdx1)
                    {
                        materialIdx = i;
                        savedi = i;
                        break;
                    }
                }
                if (materialIdx != -1) break;
            }

            if (materialIdx == -1)
            {
                yield return new WaitForEndOfFrame();
                for (int i = 0; i < savedi + 1; i++)
                {
                    int[] tr = mesh.GetTriangles(i);
                    int trLength = tr.Length;
                    for (int j = 0; j < trLength; j += 3)
                    {
                        if (tr[j] == lookupIdx1)
                        {
                            materialIdx = i;
                            savedi = i;
                            break;
                        }
                    }
                    if (materialIdx != -1) break;
                }
            }
        }
        yield return new WaitForEndOfFrame();

        if (materialIdx == savedMaterial)
        {
            if (check == 1)
            {
                audioSource.PlayOneShot(stepAudioClips[Random.Range(0, stepAudioClips.Length)], 1);
                yield break;
            }
            else if (check == 2)
            {
                audioSource.PlayOneShot(stepStoneAudioClips[Random.Range(0, stepStoneAudioClips.Length)], 1);
                yield break;
            }
            else if (check == 3)
            {
                audioSource.PlayOneShot(stepWoodAudioClips[Random.Range(0, stepWoodAudioClips.Length)], 1);
                yield break;
            }
            else if (check == 4)
            {
                audioSource.PlayOneShot(stepMetalAudioClips[Random.Range(0, stepMetalAudioClips.Length)], 1);
                yield break;
            }
        }
        else
        {
            string matrstr = renderer.sharedMaterials[materialIdx].name;
            for (int i = 0; i < matsEarth1.Length; i++)
            {
                if (matrstr == matsEarth1[i])
                {
                    check = 1;
                    savedMaterial = materialIdx;
                    audioSource.PlayOneShot(stepAudioClips[Random.Range(0, stepAudioClips.Length)], 1);
                    yield break;
                }
            }
            yield return new WaitForEndOfFrame();
            for (int i = 0; i < matsStone1.Length; i++)
            {
                if (matrstr == matsStone1[i])
                {
                    check = 2;
                    savedMaterial = materialIdx;
                    audioSource.PlayOneShot(stepStoneAudioClips[Random.Range(0, stepStoneAudioClips.Length)], 1);
                    yield break;
                }
            }
            yield return new WaitForEndOfFrame();
            for (int i = 0; i < matsWood1.Length; i++)
            {
                if (matrstr == matsWood1[i])
                {
                    check = 3;
                    savedMaterial = materialIdx;
                    audioSource.PlayOneShot(stepWoodAudioClips[Random.Range(0, stepWoodAudioClips.Length)], 1);
                    yield break;
                }
            }
            yield return new WaitForEndOfFrame();
            for (int i = 0; i < matsMEtal1.Length; i++)
            {
                if (matrstr == matsMEtal1[i])
                {
                    check = 4;
                    savedMaterial = materialIdx;
                    audioSource.PlayOneShot(stepMetalAudioClips[Random.Range(0, stepMetalAudioClips.Length)], 1);
                    yield break;
                }
            }
        }
    }
    public int CheckWorldMaterial(RaycastHit hit)
    {
        int materialIdx = -1;
        MeshCollider mc = hit.collider as MeshCollider;
        if (!mc) return 0;
        Renderer renderer = mc.GetComponent<Renderer>();
        if (!renderer) return 0;
        Mesh mesh = mc.sharedMesh;
        if (!mesh) return 0;
        int triangleIdx = hit.triangleIndex;
        int triangleIdx3 = triangleIdx * 3;
        int lookupIdx1 = mesh.triangles[triangleIdx3];
        //int lookupIdx2 = mesh.triangles[triangleIdx3 + 1];
        // int lookupIdx3 = mesh.triangles[triangleIdx3 + 2];
        int subMeshesNr = mesh.subMeshCount;
        if (savedi >= subMeshesNr)
            savedi = 0;
        for (int i = savedi; i < subMeshesNr; i++)
        {
            int[] tr = mesh.GetTriangles(i);
            for (int j = 0; j < tr.Length; j += 3)
            {
                if (tr[j] == lookupIdx1)
                {
                    materialIdx = i;
                    savedi = i;
                    // savedj = j;
                    break;
                }

            }
            if (materialIdx != -1) break;
        }
        if (materialIdx == -1)
            for (int i = 0; i < savedi + 1; i++)
            {
                int[] tr = mesh.GetTriangles(i);
                for (int j = 0; j < tr.Length; j += 3)
                {
                    if (tr[j] == lookupIdx1)
                    {
                        materialIdx = i;
                        savedi = i;
                        //savedj = j;
                        break;
                    }

                }
                if (materialIdx != -1) break;
            }
        string matrstr = renderer.sharedMaterials[materialIdx].name;
        for (int i = 0; i < matsEarth1.Length; i++)
        {
            if (matsEarth[i] && matrstr == matsEarth[i].name)
            {
                return 1;

            }
        }
        for (int i = 0; i < matsStone1.Length; i++)
        {
            if (matsStone[i] && matrstr == matsStone[i].name)
            {
                return 2;
            }
        }
        for (int i = 0; i < matsWood1.Length; i++)
        {
            if (matsWood[i] && matrstr == matsWood[i].name)
            {
                return 3;
            }
        }
        for (int i = 0; i < matsMEtal1.Length; i++)
        {
            if (matsMEtal[i] && matrstr == matsMEtal[i].name)
            {
                return 4;
            }
        }
        return 0;

    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Fire()
    {
        audioSource.PlayOneShot(bowfireAudioClips[Random.Range(0, bowfireAudioClips.Length)]);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Wosh()
    {
        audioSource.PlayOneShot(woshAudioClips[Random.Range(0, woshAudioClips.Length)]);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Draw()
    {
        if (stats.wpntype < 2)
        {
            if (drawSoundType[0] == 0)
                audioSource.PlayOneShot(drawAudioClips[0]);
            if (drawSoundType[0] == 1)
                audioSource.PlayOneShot(drawAudioClips[2]);
        }
        else
            if (stats.wpntype == 2)
        {
            if (drawSoundType[1] == 0)
                audioSource.PlayOneShot(drawAudioClips[0]);
            if (drawSoundType[1] == 1)
                audioSource.PlayOneShot(drawAudioClips[2]);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void UnDraw()
    {
        if (stats.wpntype < 2)
        {
            if (drawSoundType[0] == 0)
                audioSource.PlayOneShot(drawAudioClips[1]);
            if (drawSoundType[0] == 1)
                audioSource.PlayOneShot(drawAudioClips[3]);
        }
        else if (stats.wpntype == 2)
        {
            if (drawSoundType[1] == 0)
                audioSource.PlayOneShot(drawAudioClips[1]);
            if (drawSoundType[1] == 1)
                audioSource.PlayOneShot(drawAudioClips[3]);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Land()
    {

        bool inwtr = false;
        for (int i = 0; i < GOManager.waters.Count; i++)
        {
            Transform wtr = GOManager.waters[i];
            if (playerTransform.position.y <= wtr.transform.position.y && playerTransform.position.x <= wtr.position.x + 55 / 2 && playerTransform.position.z <= wtr.position.z + 55 / 2
            && playerTransform.position.x >= wtr.position.x - 55 / 2 && playerTransform.position.z >= wtr.position.z - 55 / 2)
                inwtr = true;
        }

        if (inwtr)
        {
            audioSource.PlayOneShot(fallwaterAudioClip, 1);
        }
        else
        {
            audioSource.PlayOneShot(fallsmallAudioClip, 1);
            Instantiate(Resources.Load("ParticleLand"), playerTransform.position + new Vector3(0, 0.2f, 0), Quaternion.Euler(-90, 0, 0));
        }
    }
    public void SmallTalk()
    {
        if (smallTalksAudioClips.Length > 0)
            audioSource.PlayOneShot(smallTalksAudioClips[Random.Range(0, smallTalksAudioClips.Length)]);
    }
    public void Hit()
    {
        audioSource.PlayOneShot(aarghhSounds[Random.Range(0, aarghhSounds.Length)]);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dead()
    {
        audioSource.PlayOneShot(deadSounds[Random.Range(0, deadSounds.Length)]);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WeaponDown()
    {
        RpcWeaponDown();
    }
    [ClientRpc]
    public void RpcWeaponDown()
    {
        audioSource.PlayOneShot(weaponDownAudio, 1);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WeaponDown2()
    {
        RpcWeaponDown2();
    }
    [ClientRpc]
    public void RpcWeaponDown2()
    {
        audioSource.PlayOneShot(weaponDownAudio2, 1);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WiseMove()
    {
        RpcWiseMove();
    }
    [ClientRpc]
    public void RpcWiseMove()
    {
        audioSource.PlayOneShot(wiseMoveAudioClip, 1);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WillStop()
    {
        RpcWillStop();
    }
    [ClientRpc]
    public void RpcWillStop()
    {
        audioSource.PlayOneShot(willStopAudio, 1);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void DieMonster()
    {
        RpcDieMonster();

    }
    [ClientRpc]
    public void RpcDieMonster()
    {
        audioSource.PlayOneShot(dieMonsterAudio, 1);

    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void EnemyKilled()
    {
        RpcEnemyKilled();
    }
    [ClientRpc]
    public void RpcEnemyKilled()
    {
        audioSource.PlayOneShot(enemyKilledAudio, 1);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MonsterKilled()
    {
        RpcMonsterKilled();
    }
    [ClientRpc]
    public void RpcMonsterKilled()
    {
        audioSource.PlayOneShot(monsterKilledAudio, 1);
    }
}

