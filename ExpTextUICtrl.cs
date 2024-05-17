using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ExpTextUICtrl : MonoBehaviour
{
    public  RectTransform rectTransform;
    public Text text;
    public float time;
    void Start()
    {
        Destroy(gameObject,3);
    }

    void Update()
    {
        time += Time.smoothDeltaTime;
        rectTransform.anchoredPosition = new Vector2(0,10* time);
        text.color = new Color(1, 1, 1, (3 - time) / 5);
    }
}
