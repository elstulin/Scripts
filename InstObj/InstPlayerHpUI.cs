using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstPlayerHpUI : MonoBehaviour
{

    void Awake()
    {
        GOManager.playerHpUIGo = gameObject;
        GOManager.playerHpUIRectTransform = transform.GetChild(0).GetComponent<RectTransform>();
        GOManager.playerHpUIGo.SetActive(false);
        Destroy(this);

    }

}
