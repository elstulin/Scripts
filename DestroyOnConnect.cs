using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DestroyOnConnect : NetworkBehaviour
{
    
    private void Start()
    {
       
    }
    void Update()
    {
        if(!gameObject.activeSelf)
        gameObject.SetActive(true);
        if (NetworkClient.ready)
            DestroyImmediate(gameObject);
        
    }
}
