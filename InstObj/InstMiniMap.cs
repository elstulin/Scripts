using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstMiniMap : MonoBehaviour
{
    void Start()
    {
        GOManager.minimap = gameObject;
        GOManager.minimap.SetActive(false);
    }

}
