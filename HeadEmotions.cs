using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadEmotions : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Mesh meshFrame;
    public Mesh[] meshFrameWiseme;
    public float CloseEyesTime;
    public int curEmotion;
    bool rndWise;
    public AudioSource audioSource;
    public bool coreHead = false;
    public bool voice;
    
    void Start()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        if (coreHead)
        {
            Mesh targetMesh = skinnedMeshRenderer.sharedMesh;
            targetMesh.ClearBlendShapes();
            Vector3[] verts = new Vector3[targetMesh.vertexCount];
            for (int q = 0; q < meshFrameWiseme.Length; q++)
            {
                for (int i = 0; i < verts.Length; i++)
                {
                    if (i < meshFrameWiseme[q].vertexCount)
                        verts[i] = -(targetMesh.vertices[i] - meshFrameWiseme[q].vertices[i]);
                    // if (verts[i].magnitude < 0.011f) verts[i] = new Vector3(0, 0, 0);
                }


                targetMesh.AddBlendShapeFrame("meshFrameWiseme" + q, 1, verts, null, null);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % 2 != 0) return;
        /* float[] asd = new float[64];

             audioSource.GetSpectrumData(asd, 0,FFTWindow.Rectangular);
         float loud = 0;
         for(int i = 0; i < 64; i++)
         {
             loud += asd[i];
         }
         if (loud / 64 > 0.00001f) */
       

        if (voice)
        {
            if (!rndWise)
            {
                curEmotion = Random.Range(0, skinnedMeshRenderer.sharedMesh.blendShapeCount);
                rndWise = true;
            }
        }
        else
        {
            curEmotion = 10;
        }
            CloseEyesTime += Time.deltaTime*2;
            if (CloseEyesTime > 0f)
            {
                
                for (int i = 0; i < skinnedMeshRenderer.sharedMesh.blendShapeCount-1; i++)
                {
                    if (i != curEmotion)
                        skinnedMeshRenderer.SetBlendShapeWeight(i, Mathf.Lerp(skinnedMeshRenderer.GetBlendShapeWeight(i), 0, 0.6f));
                    else
                    {
                        skinnedMeshRenderer.SetBlendShapeWeight(i, Mathf.Lerp(skinnedMeshRenderer.GetBlendShapeWeight(i), 1, 0.6f));
                    }

                }
                if (CloseEyesTime > 0.15f)
                {
                    CloseEyesTime = 0;
                    rndWise = false;
                }
            }
    }

}
