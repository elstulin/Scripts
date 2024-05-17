using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstChestPanel : MonoBehaviour
{
    void Awake()
    {
        GOManager.chestPanel = gameObject;
        GOManager.chestPanel.SetActive(false);
    }

}
