using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowGO : MonoBehaviour
{
    public Transform target;
    void LateUpdate()
    {
        transform.position = target.position;
    }
}
