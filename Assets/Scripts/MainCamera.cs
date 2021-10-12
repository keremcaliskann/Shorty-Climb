using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private Transform target;
    private Vector3 offset;

    void Start()
    {
        target = FindObjectOfType<Player>().transform;
        offset = target.position - transform.position;
    }

    void FixedUpdate()
    {
        transform.position = target.position - offset;
    }
}
