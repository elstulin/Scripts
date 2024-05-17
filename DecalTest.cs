using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalTest : MonoBehaviour
{
    public MeshFilter meshFilter;
    public float height;
    public LayerMask layerMask;
    Material material;
    float timer;
    public void Start()
    {
            material = GetComponent<MeshRenderer>().material;
        Mesh mesh = new Mesh();

        Vector3[] vertices = meshFilter.sharedMesh.vertices;
        int[] triangles = meshFilter.sharedMesh.triangles;
        Vector3[] normals = meshFilter.sharedMesh.normals;

        Vector2[] uv = meshFilter.sharedMesh.uv;
        for (int i = 0; i < vertices.Length; i++)
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(transform.position + transform.TransformDirection( new Vector3(vertices[i].x * transform.localScale.x, vertices[i].y * transform.localScale.y, vertices[i].z * transform.localScale.z)) + new Vector3(0, height, 0), new Vector3(0, -1, 0), out raycastHit, height * 2, layerMask))
                vertices[i].y = raycastHit.point.y - transform.position.y +0.05f;
            else
                vertices[i].y = 0;
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uv;
        meshFilter.mesh = mesh;
        gameObject.isStatic = true;

    }
    private void FixedUpdate()
    {
        if (Time.frameCount % 2 != 0) return;
        timer += Time.fixedDeltaTime * 2;
            material.SetColor("_Color", new Color(1 - timer / 60, 1 - timer / 60, 1 - timer / 60, 1 - timer / 60));
    }

}
