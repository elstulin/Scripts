using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstSpriteEnemyMinHp : MonoBehaviour
{
    void Awake()
    {
        GOManager.MinHPSprite = GetComponent<RectTransform>();
    }
}
