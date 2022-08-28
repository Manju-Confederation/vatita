using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachController : MonoBehaviour
{
    public float approachSpeed = 0.5f;

    void Start()
    {
        
    }

    void Update()
    {
        transform.localScale -= (Vector3) Vector2.one * 0.5f * Time.deltaTime;
    }
}
