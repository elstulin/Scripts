using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InstSpritePlayerMinMp : MonoBehaviour
{
    void Start()
    {
        GOManager.playerMpUI = GetComponent<RectTransform>();
    }

}
