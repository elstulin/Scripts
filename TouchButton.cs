using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchButton : MonoBehaviour
{
    public GateObject targetGate;
    public bool touched;
    private void Awake()
    {
        GOManager.buttons.Add(this);
    }
    private void OnDestroy()
    {
        GOManager.buttons.Remove(this);
    }
}
