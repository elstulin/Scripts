using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstLight : MonoBehaviour
{
    void Awake()
    {
        GOManager.globalight = gameObject;
    }

}
