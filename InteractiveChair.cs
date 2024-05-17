using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveChair : MonoBehaviour
{
    public string _name;
    private void Awake()
    {
        GOManager.chairs.Add(this);
    }
    private void OnDestroy()
    {
        GOManager.chairs.Remove(this);
    }
}
