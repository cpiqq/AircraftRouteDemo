using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CameraController : MonoBehaviour
{
    public Transform aircraft;
    public float smoothTime = 1F;
    public Vector3 velocity = Vector3.zero;
    public Vector3 offset = new Vector3(0, 20f, -20f);

    void Start()
    {
        Vector3 aimPos = aircraft.position + aircraft.right * offset.x + aircraft.up * offset.y + aircraft.forward * offset.z;
        transform.position = aimPos;
        transform.LookAt(aircraft, aircraft.up);

    }

    void LateUpdate ()
    {
        Vector3 aimPos = aircraft.position + aircraft.right * offset.x + aircraft.up * offset.y + aircraft.forward * offset.z;
        transform.position = Vector3.SmoothDamp(transform.position, aimPos, ref velocity, smoothTime);
        transform.LookAt(aircraft, aircraft.up);

    }
    
}
