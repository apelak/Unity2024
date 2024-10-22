using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target;       
    public float smoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        Vector3 targetPosition = target.position;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
