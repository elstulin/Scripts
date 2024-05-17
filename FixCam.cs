using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixCam : MonoBehaviour
{
    public Camera cam;
    public Camera camtarget;
    void Start()
    {
        cam.projectionMatrix = camtarget.projectionMatrix;
    }

}
