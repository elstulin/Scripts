using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    void Awake()
    {
        GOManager.inventoryPanel = gameObject;
        gameObject.SetActive(false);
    }
}
