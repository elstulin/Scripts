using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstItemCams : MonoBehaviour
{
    void Start()
    {
        GOManager.itemCams = gameObject;
       
        Transform _transform = transform;
        for (int i = 0; i < _transform.childCount; i++)
        {
            GameObject go = _transform.GetChild(i).gameObject;
            Camera camera = go.GetComponent<Camera>();
            camera.Render();
            GOManager.iconsList.Add(go);
            go.SetActive(false);

        }
        gameObject.SetActive(false);
    }
}
