using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{
    public RawImage rawImage;
    public float timer;
    void Start()
    {
        
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 4)
            rawImage.color -= new Color(0,0,0,Time.deltaTime*0.25f);
    }
}
