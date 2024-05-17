using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstSpriteEnemyMaxHp : MonoBehaviour
{
    void Awake()
    {
        GOManager.MaxHPSprite = gameObject;
        GOManager.MinHPSprite = transform.GetChild(0).GetComponent<RectTransform>();
        GOManager.MaxHPSprite.SetActive(false);
    }

}
