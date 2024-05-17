using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMonsters : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] soundWarm;
    public AudioClip[] attackSounds;
    public AudioClip[] hitSounds;
    public AudioClip[] dieSounds;
    public AudioClip[] stepAudioClips;
    public AudioClip[] stepStoneAudioClips;
    public AudioClip[] stepWaterAudioClips;
    public AudioClip[] stepMetalAudioClips;
    public Transform playerTransform;
    public float[] textureValues;
    public Terrain t;

    public int posX;
    public int posZ;
    void Start()
    {
        t = Terrain.activeTerrain;
        playerTransform = gameObject.transform;
    }

    public void GetTerrainTexture()
    {
        ConvertPosition(playerTransform.position);
        CheckTexture();
    }
    void CheckTexture()
    {
        float[,,] aMap = t.terrainData.GetAlphamaps(posX, posZ, 1, 1);
        textureValues[0] = aMap[0, 0, 0];
        textureValues[1] = aMap[0, 0, 1];
        textureValues[2] = aMap[0, 0, 2];
    }
    void ConvertPosition(Vector3 playerPosition)
    {
        Vector3 terrainPosition = playerPosition - t.transform.position;

        Vector3 mapPosition = new Vector3
        (terrainPosition.x / t.terrainData.size.x, 0,
        terrainPosition.z / t.terrainData.size.z);

        float xCoord = mapPosition.x * t.terrainData.alphamapWidth;
        float zCoord = mapPosition.z * t.terrainData.alphamapHeight;

        posX = (int)xCoord;
        posZ = (int)zCoord;
    }
    public void Warm()
    {
        audioSource.PlayOneShot(soundWarm[Random.Range(0, soundWarm.Length)]);
    }
    public void Attack()
    {
        audioSource.PlayOneShot(attackSounds[Random.Range(0, attackSounds.Length)]);
    }
    public void Hit()
    {
        audioSource.PlayOneShot(hitSounds[Random.Range(0, hitSounds.Length)]);
    }
    public void Die()
    {
        audioSource.PlayOneShot(dieSounds[Random.Range(0, dieSounds.Length)]);
    }
    public void Step()
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
            audioSource.PlayOneShot(stepWaterAudioClips[Random.Range(0, stepWaterAudioClips.Length)], 1);
        }
        else
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(playerTransform.position, new Vector3(0, -1, 0), out raycastHit, 0.3f))
            {


                if (raycastHit.transform.name == "Cube")
                {
                    audioSource.PlayOneShot(stepMetalAudioClips[Random.Range(0, stepMetalAudioClips.Length)], 1);
                }
                if (raycastHit.transform.name == "Terrain")
                {
                    GetTerrainTexture();
                    if (textureValues[0] > 0)
                    {
                        audioSource.PlayOneShot(stepAudioClips[Random.Range(0, stepAudioClips.Length)], textureValues[0]);
                    }
                    if (textureValues[1] > 0)
                    {
                        audioSource.PlayOneShot(stepStoneAudioClips[Random.Range(0, stepStoneAudioClips.Length)], textureValues[1]);
                    }
                    if (textureValues[2] > 0)
                    {
                        audioSource.PlayOneShot(stepAudioClips[Random.Range(0, stepAudioClips.Length)], 0.5f);
                        audioSource.PlayOneShot(stepWaterAudioClips[Random.Range(0, stepWaterAudioClips.Length)], 0.25f);
                        audioSource.PlayOneShot(stepStoneAudioClips[Random.Range(0, stepStoneAudioClips.Length)], 0.25f);
                    }
                }
            }
        }
    }
}
