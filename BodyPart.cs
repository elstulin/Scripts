using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    public StatsSystem Owner;
    public float dmgMultiplayer = 1;
    public bool body = false;
    private void Start()
    {
        Owner.BodyPartColliders.Add(GetComponent<Collider>());
        if (body)
            Owner.rigidbodyPart = GetComponent<Rigidbody>();
    }
}
