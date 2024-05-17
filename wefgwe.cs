using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Water;
public class wefgwe : MonoBehaviour
{
    public Material mat;
    public Transform _transform;
    public int indx;
    public Transform[] tiles;
    Transform playertr;
    Camera cam;
    Vector4[] vecs = new Vector4[100];
    public PlanarReflection planarReflection;
    public Transform waterfall1;
    public Transform waterfall2;
    public RenderTexture[] renderTextures;

    void Start()
    {
        playertr = GOManager.player.transform;
        RaycastHit raycastHit;
        GOManager.planarReflection = planarReflection;
    }

    void Update()
    {/*
         for(int i = 0; i < tiles.Length;i++) 
         {
            Transform tile = tiles[i];
             //tiles[i].gameObject.SetActive(false);
             if ( playertr.position.x <= tile.position.x + 55 * tile.lossyScale.x && playertr.position.z <= tile.position.z + 55 * tile.lossyScale.z
             && playertr.position.x >= tile.position.x - 55*tile.lossyScale.x && playertr.position.z >= tile.position.z - 55 * tile.lossyScale.z)
             {
                 //_transform = tiles[i];
                 //tiles[i].gameObject.SetActive(true);
                 //GOManager.waterRippleCamera = _transform.GetChild(0).GetComponent<Camera>();
                 planarReflection.clipPlaneOffset = tiles[i].localPosition.y/1.827f+0.07f;
                 break;
             }

         }
        
        if (GOManager.waterRippleCamera)
        {
            cam = GOManager.waterRippleCamera;
            //vecs[0] = new Vector4(cam.WorldToScreenPoint(waterfall1.position + new Vector3(0, 0, 0)).x, cam.WorldToScreenPoint(waterfall1.position + new Vector3(0, 0, 0)).y, 1, 0.0f);
            //vecs[1] = new Vector4(cam.WorldToScreenPoint(waterfall2.position + new Vector3(0, 0, 0)).x, cam.WorldToScreenPoint(waterfall2.position + new Vector3(0, 0, 0)).y, 1, 0.0f);
            
            for (int i = 0; i < GOManager.worldChars.Count; i++)
            {
               
                Transform tr = GOManager.worldChars[i].transform;
                if (Vector3.Distance(tr.position, GOManager.player.transform.position) < 50)
                {
                    Vector3 wtosp = cam.WorldToScreenPoint(tr.position + new Vector3(0, 0, 0));
                    vecs[i] = new Vector4(wtosp.x, wtosp.y, 1, 0.0f);
                }
                
            }


            Vector3 wtosp = cam.WorldToScreenPoint(playertr.position + new Vector3(0, 0, 0));
            vecs[0] = new Vector4(wtosp.x, wtosp.y, 1, 0.0f);
            mat.SetFloat("_PositionArray", vecs.Length);
                mat.SetVectorArray("_Positions", vecs);
               
    }
     */
    }
}


