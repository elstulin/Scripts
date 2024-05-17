using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveBed : MonoBehaviour
{
    private void Awake()
    {
        GOManager.beds.Add(this);
    }
    private void OnDestroy()
    {
        GOManager.beds.Remove(this);
    }

}
