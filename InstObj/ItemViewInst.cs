using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemViewInst : MonoBehaviour
{
    void Awake()
    {
        GOManager.inventoryViewPanel = gameObject;
    }
}
