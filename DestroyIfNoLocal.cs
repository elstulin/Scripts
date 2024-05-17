using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DestroyIfNoLocal : MonoBehaviour
{
    NetworkIdentity networkIdentity;
    void Start()
    {
        networkIdentity = transform.parent.GetComponent<NetworkIdentity>();
        if (!networkIdentity.isLocalPlayer)
        {
            DestroyImmediate(gameObject);
        }
        else
            Destroy(this);
    }

}
