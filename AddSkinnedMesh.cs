using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSkinnedMesh : MonoBehaviour
{
    public SkinnedMeshRenderer[] skinnedMesh;
    public SkinnedMeshRenderer[] skinnedMeshLODs;
    public MeshRenderer[] Mesh;
    public static List<GameObject> meshesObj = new List<GameObject>();
    public Transform hip;
    public bool onStart = false;
    public int q;

    void Start()
    {

        if (onStart)
        {
           Invoke(nameof(SpawnSet),0.1f);
        }
    }
    public GameObject SpawnSet()
    {
        GameObject goMain = gameObject;
        goMain.transform.parent = hip.parent;
        goMain.transform.localPosition = new Vector3(0, 0, 0);
        goMain.transform.localRotation = new Quaternion(0, 0, 0, 0);
        for (int q = this.q * 2; q < 2; q++)
        {
            if (q >= skinnedMeshLODs.Length) break;
            string[] bonesName;
            bonesName = new string[skinnedMeshLODs[q].bones.Length];
            for (int i = 0; i < skinnedMeshLODs[q].bones.Length; i++)
            {
                bonesName[i] = skinnedMeshLODs[q].bones[i].name;
            }

            Transform[] bones = new Transform[bonesName.Length];
            for (int i = 0; i < bonesName.Length; i++)
            {
                if (hip.FindChildByRecursive(bonesName[i]))
                    bones[i] = hip.FindChildByRecursive(bonesName[i]);
                else
                {
                    bones[i] = hip;
                    //   Debug.Log(bonesName[i]);
                }
            }

            skinnedMeshLODs[q].bones = bones;
            skinnedMeshLODs[q].rootBone = hip;
        }
        for (int q2 = 0; q2 < skinnedMesh.Length; q2++)
        {
            if (q2 != q)
            {
                if(skinnedMesh[q2].gameObject != skinnedMesh[q].gameObject)
                Destroy(skinnedMesh[q2].gameObject);
                continue;
            }
            string[] bonesName;
            bonesName = new string[skinnedMesh[q].bones.Length];
            for (int i = 0; i < skinnedMesh[q].bones.Length; i++)
            {
                bonesName[i] = skinnedMesh[q].bones[i].name;
            }

            // SkinnedMeshRenderer skinnedMeshRenderer = Instantiate(skinnedMesh[q], goMain.transform.position, goMain.transform.rotation, goMain.transform);
            //skinnedMeshRenderer.gameObject.AddComponent<FatnesMesh>();
            Transform[] bones = new Transform[bonesName.Length];
            for (int i = 0; i < bonesName.Length; i++)
            {
                if (hip.FindChildByRecursive(bonesName[i]))
                    bones[i] = hip.FindChildByRecursive(bonesName[i]);
                else
                {
                    bones[i] = hip;
                    //   Debug.Log(bonesName[i]);
                }
            }

            skinnedMesh[q].bones = bones;
            skinnedMesh[q].rootBone = hip;
        }

        for (int q = 0; q < Mesh.Length; q++)
        {

            Transform parent = hip.FindChildByRecursive(Mesh[q].transform.parent.gameObject.name);
            MeshRenderer MeshRenderer = Instantiate(Mesh[q], parent.position, Quaternion.Euler(0, 0, 0), parent);
            MeshRenderer.transform.localEulerAngles = new Vector3(0, 0, 0);
            meshesObj.Add(MeshRenderer.gameObject);
        }
        Destroy(transform.GetChild(0).gameObject);
        Destroy(this);

        return goMain;

    }
    /*
    public GameObject[] SpawnSet1()
    {
        for (int q = 0; q < Mesh.Length; q++)
        {

            Transform parent = hip.FindChildByRecursive(Mesh[q].transform.parent.gameObject.name);
            MeshRenderer MeshRenderer = Instantiate(Mesh[q], parent.position, Quaternion.Euler(0, 0, 0), parent);
            MeshRenderer.transform.localEulerAngles = new Vector3(0,0,0);
            meshesObj.Add(MeshRenderer.gameObject);
        }
        return meshesObj.ToArray();
    }*/
}
