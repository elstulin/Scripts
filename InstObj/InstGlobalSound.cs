using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstGlobalSound : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        GOManager.globalSource = GetComponent<AudioSource>();
    }
}
