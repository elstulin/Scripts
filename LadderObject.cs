using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderObject : MonoBehaviour
{
    public float height;
    public int type;
    void Awake()
    {
        GOManager.LadderObjs.Add(this);
    }
    void OnDestroy()
    {
        GOManager.LadderObjs.Remove(this);
    }
}
