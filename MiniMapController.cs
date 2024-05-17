using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using static InstPlayer;
public class MiniMapController : MonoBehaviour
{
    public RectTransform rectTransform;
    public RectTransform rectTransform2;
    public Vector2 vector;
    public Vector2 vector2;
    public int[,] fogOfWar = new int[1200,908];

    void Update()
    {

        if (GOManager.player)
        {
            rectTransform.anchoredPosition = new Vector2(GOManager.playerTransform.position.x + vector.x, GOManager.playerTransform.position.z + vector.y);
            rectTransform2.localRotation = Quaternion.Euler(0, 0, -GOManager.playerTransform.eulerAngles.y - 90);
            for (int i = 0; i < GOManager.enemysPosUI.Count; i++)
            {
                GOManager.enemysPosUI[i].anchoredPosition = new Vector2(-(GOManager.enemysPos[i].position.x - GOManager.playerTransform.position.x), -(GOManager.enemysPos[i].position.z - GOManager.playerTransform.position.z));
            }

        }

    }
}

