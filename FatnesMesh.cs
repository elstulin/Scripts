using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatnesMesh : MonoBehaviour
{
    public StatsSystem stats;
   public SkinnedMeshRenderer[] skinnedMesh;
    public MeshFilter[] meshFilter;
    private List<Mesh> mesh = new List<Mesh>();
    private List<Mesh> mesh2 = new List<Mesh>();
    public float[] modify;
    private AddSkinnedMesh addSkinnedMesh;
    void Start()
    {
        addSkinnedMesh = GetComponent<AddSkinnedMesh>();
        if (addSkinnedMesh)
        {
            skinnedMesh = new SkinnedMeshRenderer[] { addSkinnedMesh.skinnedMesh[stats.currentBody] };
        }
        for (int i =0;i< skinnedMesh.Length;i++)
        {
            mesh.Add(Instantiate(skinnedMesh[i].sharedMesh));
        }
        for (int i = 0; i < meshFilter.Length; i++)
        {
            mesh.Add(Instantiate(meshFilter[i].sharedMesh));
        }
        for (int i = 0;i<mesh.Count;i++)
        {
            mesh2.Add(Instantiate(mesh[i]));
        }
        UpdateD();
        if(stats.netIdentity.isLocalPlayer)
        CharacterChanger.OnChangeWeight += UpdateD;
        stats.fatnesMeshes.Add(this);
    }
    private void OnDestroy()
    {
        CharacterChanger.OnChangeWeight -= UpdateD;
        stats.fatnesMeshes.Remove(this);
    }
    public void UpdateD()
    {
        float amount = stats.weight;
        for (int i = 0; i < skinnedMesh.Length; i++)
        {
            
            Vector3[] vertices = mesh[i].vertices;
            Vector3[] normals = mesh[i].normals;

            for (int q = 0; q < vertices.Length; q++)
            {
                if (modify.Length > 0)
                {
                    if (modify[i] < 1f&&amount < 0)
                        amount = 0;
                    vertices[q] += normals[q] * amount * modify[i];
                }
                else
                    vertices[q] += normals[q] * amount;
            }
            mesh2[i].vertices = vertices;
            //mesh.normals = normals;
            skinnedMesh[i].sharedMesh = mesh2[i];
        }
        for (int i = 0; i < meshFilter.Length; i++)
        {
            Vector3[] vertices = mesh[i].vertices;
            Vector3[] normals = mesh[i].normals;

            for (int q = 0; q < vertices.Length; q++)
            {
                if (modify.Length > 0)
                {
                    if(modify[i] < 1f && amount < 0)
                    amount = 0;
                    vertices[q] += normals[q] * amount * modify[i];
                }
                else
                    vertices[q] += normals[q] * amount;
            }
            mesh2[i].vertices = vertices;
            //mesh.normals = normals;
            meshFilter[i].mesh = mesh2[i];

           
        }
        

    }
}
