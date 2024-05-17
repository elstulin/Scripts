using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstAim : MonoBehaviour
{
    void Awake()
    {
        GOManager.aimImage = gameObject;
        GOManager.aimImage.SetActive(false);
    }

}
