using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstWaterRippleCam : MonoBehaviour
{
    void Awake()
    {
        GOManager.waterRippleCamera = GetComponent<Camera>();
    }

}
